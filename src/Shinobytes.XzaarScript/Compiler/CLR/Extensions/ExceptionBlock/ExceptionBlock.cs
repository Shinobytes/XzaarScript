/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
using System;
using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions.ExceptionBlock
{
    /// <summary>
    /// Represents a protected region
    /// </summary>
    public sealed class ExceptionBlock : IDisposable
    {
        private readonly XsILGenerator generator;
        private readonly Label endLabel;

        private bool tryBlockEnded;
        private bool hasCatchBlocks;
        private bool hasFinallyBlock;
        private bool hasFaultBlock;

        internal ExceptionBlock(XsILGenerator generator)
        {
            this.generator = generator;
            endLabel = generator.DefineLabel();
            generator.BeginExceptionBlock();
        }

        /// <summary>
        /// Jumps to the instruction immediately after this protected region (after any finally block executes)
        /// </summary>
        
        public XsILGenerator Leave() => generator.Leave(endLabel);

        /// <summary>
        /// Jumps to the instruction immediately after this protected region (after any finally block executes)
        /// </summary>
        
        public XsILGenerator LeaveShortForm() => generator.LeaveShortForm(endLabel);

        private void EnsureTryBlockEnded()
        {
            if (tryBlockEnded)
            {
                return;
            }

            Leave();
            tryBlockEnded = true;
        }

        /// <summary>
        /// Starts a new catch block which handles exceptions of any type
        /// </summary>
        
        public CatchBlock CatchBlock()
        {
            return CatchBlock<object>();
        }

        /// <summary>
        /// Starts a new catch block which handles exceptions of the given type
        /// </summary>
        /// <typeparam name="T">The type of exception this catch block should handle</typeparam>
        
        public CatchBlock CatchBlock<T>()
        {
            return CatchBlock(typeof (T));
        }

        /// <summary>
        /// Starts a new catch block which handles exceptions of the given type
        /// </summary>
        /// <param name="exceptionType">The type of exception this catch block should handle</param>
        
        public CatchBlock CatchBlock(Type exceptionType)
        {
            return CatchBlock(exceptionType, null);
        }

        /// <summary>
        /// Starts a new catch block which handles exceptions based on the filter created by the given action
        /// </summary>
        /// <param name="filter">An action that writes the IL comprising the filter block</param>
        
        public CatchBlock CatchBlock(Action<XsILGenerator> filter)
        {
            return CatchBlock(null, filter);
        }

        /// <summary>
        /// Starts a new catch block which handles exceptions of the given type based on the filter created by the given action
        /// </summary>
        /// <typeparam name="T">The type of exception this catch block should handle</typeparam>
        /// <param name="filter">An action that writes the IL comprising the filter block</param>
        
        public CatchBlock CatchBlock<T>(Action<XsILGenerator> filter)
        {
            return CatchBlock(typeof (T), filter);
        }

        /// <summary>
        /// Starts a new catch block which handles exceptions of the given type based on the filter created by the given action
        /// </summary>
        /// <param name="exceptionType">The type of exception this catch block should handle</param>
        /// <param name="filter">An action that writes the IL comprising the filter block</param>
        
        public CatchBlock CatchBlock(Type exceptionType, Action<XsILGenerator> filter)
        {
            EnsureTryBlockEnded();

            if (hasFinallyBlock)
            {
                throw new InvalidOperationException(
                    "Exception block already has a finally block - cannot start new catch block");
            }

            if (hasFaultBlock)
            {
                throw new InvalidOperationException(
                    "Exception block already has a fault block - cannot have both catch and fault blocks");
            }

            if (filter != null)
            {
                generator.BeginExceptFilterBlock();

                if (exceptionType != null)
                {
                    var filterBlockEnd = generator.DefineLabel();
                    var customFilter = generator.DefineLabel();

                    generator
                        .Duplicate()
                        .IsInstanceOfType(exceptionType)
                        .BranchIfTrue(customFilter)

                        .Pop()
                        .LoadConstant(false)
                        .BranchTo(filterBlockEnd);

                    generator.MarkLabel(customFilter);
                    filter(generator);

                    generator.MarkLabel(filterBlockEnd);
                }
                else
                {
                    filter(generator);
                }

                generator.BeginCatchBlock(null);
            }
            else
            {
                generator.BeginCatchBlock(exceptionType);
            }

            hasCatchBlocks = true;

            return new CatchBlock(generator, endLabel);
        }

        /// <summary>
        /// Starts a fault block
        /// </summary>
        
        public FaultBlock FaultBlock()
        {
            if (hasCatchBlocks)
            {
                throw new InvalidOperationException("Exception block already has catch blocks - cannot use both catch and fault block");
            }

            if (hasFaultBlock)
            {
                throw new InvalidOperationException("Exception block already has fault block");
            }

            if (hasFinallyBlock)
            {
                throw new InvalidOperationException(
                    "Exception block already has a finally block - cannot have both fault and finally - try nesting this exception block in another that has its own fault block");
            }

            EnsureTryBlockEnded();

            hasFaultBlock = true;

            generator.BeginFaultBlock();
            return new FaultBlock();
        }

        /// <summary>
        /// Starts a finally block
        /// </summary>
        
        public FinallyBlock FinallyBlock()
        {
            if (hasFinallyBlock)
            {
                throw new InvalidOperationException("Exception block already has finally block");
            }

            if (hasFaultBlock)
            {
                throw new InvalidOperationException(
                    "Exception block already has a fault block - cannot have both fault and finally - try nesting this exception block in another that has its own finally block");
            }

            EnsureTryBlockEnded();

            hasFinallyBlock = true;

            generator.BeginFinallyBlock();
            return new FinallyBlock();
        }

        /// <summary>
        /// Ends the current protected region - if no exception handling blocks have been specified, a catch block that suppresses all exceptions is emitted
        /// </summary>
        public void Dispose()
        {
            if (!tryBlockEnded)
            {
                EnsureTryBlockEnded();
                CatchBlock().Dispose(); // Suppress all errors and do nothing
            }

            generator.EndExceptionBlock();
            generator.MarkLabel(endLabel);
        }
    }
}
