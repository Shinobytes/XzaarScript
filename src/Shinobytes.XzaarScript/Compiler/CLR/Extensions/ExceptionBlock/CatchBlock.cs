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
    /// Represents a catch block in a protected region
    /// </summary>
    public sealed class CatchBlock : IDisposable
    {
        private readonly XsILGenerator generator;
        private readonly Label endLabel;

        internal CatchBlock(XsILGenerator generator, Label endLabel)
        {
            this.generator = generator;
            this.endLabel = endLabel;
        }

        /// <summary>
        /// Rethrows the exception caught by this catch block
        /// </summary>
        
        public XsILGenerator Rethrow() => generator.FluentEmit(OpCodes.Rethrow);

        /// <summary>
        /// Jumps to the instruction immediately after this protected region (after any finally block executes)
        /// </summary>
        
        public XsILGenerator Leave() => generator.Leave(endLabel);
        
        /// <summary>
        /// End the catch block
        /// </summary>
        public void Dispose()
        {
        }
    }
}