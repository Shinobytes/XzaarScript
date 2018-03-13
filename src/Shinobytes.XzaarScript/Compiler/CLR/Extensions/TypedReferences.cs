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


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for creating and manipulating typed references
    /// </summary>
    
    public static class TypedReferences
    {
        /// <summary>
        /// Pops a pointer to a piece of data off the evaluation stack and pushes a typed reference (of the given type)
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the resulting reference</param>
        
        public static XsILGenerator MakeTypedReference(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Mkrefany, type);

        /// <summary>
        /// Pops a pointer to a piece of data off the evaluation stack and pushes a typed reference (of the given type)
        /// </summary>
        /// <typeparam name="T">The type of the resulting reference</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator MakeTypedReference<T>(this XsILGenerator generator) => generator.MakeTypedReference(typeof (T));

        /// <summary>
        /// Pops a typed reference from the evaluation stack and pushes the type token of its type
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator GetTypeOfTypedReference(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Refanytype);

        /// <summary>
        /// Pops a typed reference (of the given type) from the evaluation stack and pushes the address of the reference
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the reference</param>
        
        public static XsILGenerator GetAddressOfTypedReference(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Refanyval, type);

        /// <summary>
        /// Pops a typed reference (of the given type) from the evaluation stack and pushes the address of the reference
        /// </summary>
        /// <typeparam name="T">The type of the reference</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator GetAddressOfTypedReference<T>(this XsILGenerator generator) => generator.GetAddressOfTypedReference(typeof (T));
    }
}
