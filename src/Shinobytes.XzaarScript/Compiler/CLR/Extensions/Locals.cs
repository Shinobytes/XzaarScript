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
 
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for the manipulation of local variables
    /// </summary>
    public static partial class Locals
    {
        public static XsILGenerator LoadLocal(this XsILGenerator generator, XsVariable local)
            => generator.LoadLocal(local.VariableInfo);

        /// <summary>
        /// Pushes the value of the given local onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to get the value of</param>        
        public static XsILGenerator LoadLocal(this XsILGenerator generator, LocalBuilder local)
        {
            switch (local.LocalIndex)
            {
                case 0:
                    return generator.FluentEmit(OpCodes.Ldloc_0);
                case 1:
                    return generator.FluentEmit(OpCodes.Ldloc_1);
                case 2:
                    return generator.FluentEmit(OpCodes.Ldloc_2);
                default:
                    return local.LocalIndex <= 255
                        ? generator.FluentEmit(OpCodes.Ldloc_S, local)
                        : generator.FluentEmit(OpCodes.Ldloc, local);
            }
        }

        /// <summary>
        /// Pushes the value of the given local onto the evaluation stack
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        public static XsILGenerator LoadLocal(this XsILGenerator generator, string localName)
            => generator.LoadLocal(generator.GetLocal(localName));

        /// <summary>
        /// Pushes the address of the given local onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to get the address of</param>

        public static XsILGenerator LoadLocalAddress(this XsILGenerator generator, LocalBuilder local)
        {
            return local.LocalIndex <= 255
                ? generator.FluentEmit(OpCodes.Ldloca_S, local)
                : generator.FluentEmit(OpCodes.Ldloca, local);
        }

        /// <summary>
        /// Pushes the address of the given local onto the evaluation stack
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        public static XsILGenerator LoadLocalAddress(this XsILGenerator generator, string localName)
            => generator.LoadLocalAddress(generator.GetLocal(localName));

        /// <summary>
        /// Pops a value from the evaluation stack and stores it in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store the evaluation stack value in</param>

        public static XsILGenerator StoreInLocal(this XsILGenerator generator, LocalBuilder local)
        {
            switch (local.LocalIndex)
            {
                case 0:
                    return generator.FluentEmit(OpCodes.Stloc_0);
                case 1:
                    return generator.FluentEmit(OpCodes.Stloc_1);
                case 2:
                    return generator.FluentEmit(OpCodes.Stloc_2);
                default:
                    return (local.LocalIndex <= 255)
                        ? generator.FluentEmit(OpCodes.Stloc_S, local)
                        : generator.FluentEmit(OpCodes.Stloc, local);
            }
        }

        public static XsILGenerator StoreInLocal(this XsILGenerator generator, XsVariable local)
            => generator.StoreInLocal(local.VariableInfo);

        /// <summary>
        /// Pops a value from the evaluation stack and stores it in the given local
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        public static XsILGenerator StoreInLocal(this XsILGenerator generator, string localName)
            => generator.StoreInLocal(generator.GetLocal(localName));
    }
}
