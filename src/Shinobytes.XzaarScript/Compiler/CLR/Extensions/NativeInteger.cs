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
    /// Contains extension methods for the manipulation of native integer values
    /// </summary>
    
    public static class NativeInteger
    {
        /// <summary>
        /// Pops an address off the evaluation stack, loads a native integer from it and pushes it onto the evaluation stack 
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator LoadNativeIntegerFromAddress(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ldind_I);

        /// <summary>
        /// Pops an address and a native integer off the evaluation stack and stores the native integer at the address
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator StoreNativeIntegerFromStack(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Stind_I);

        /// <summary>
        /// Pops an array reference, an array index and a native integer off the evaluation stack, storing it in the array at the index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator StoreNativeIntegerElement(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Stelem_I);

        /// <summary>
        /// Pops an array reference and an array index, pushing the native integer stored in the array at that index onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator LoadNativeIntegerElement(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ldelem_I);

        /// <summary>
        /// Pops an integer off the evaluation stack and pushes the equivalent native integer
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator ConvertToNativeInteger(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_I);

        /// <summary>
        /// Pops an integer off the evaluation stack and pushes the equivalent unsigned native integer
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator ConvertToUnsignedNativeInteger(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_U);

        /// <summary>
        /// Pops an integer off the evaluation stack and pushes the equivalent native integer, with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator ConvertToNativeIntegerWithOverflow(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I);

        /// <summary>
        /// Pops an integer off the evaluation stack and pushes the equivalent unsigned native integer, with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator ConvertToUnsignedNativeIntegerWithOverflow(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U);
    }
}
