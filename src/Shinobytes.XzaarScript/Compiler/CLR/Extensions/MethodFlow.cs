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
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for controlling flow to other methods
    /// </summary>

    public static partial class MethodFlow
    {
        /// <summary>
        /// Exits the current method and jumps immediately to the given method, using the same arguments
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to jump to</param>

        public static XsILGenerator JumpTo(this XsILGenerator generator, MethodInfo method) => generator.FluentEmit(OpCodes.Jmp, method);

        /// <summary>
        /// Calls the given method, popping the requisite number of arguments from the evaluation stack (including the this reference if it is an instance method)
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator Call(this XsILGenerator generator, MethodInfo method) => generator.FluentEmit(OpCodes.Call, method);

        /// <summary>
        /// Calls the given method with virtual semantics, popping a reference (and performing a null check) and the requisite number of arguments from the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to call</param>
        public static XsILGenerator CallVirtual(this XsILGenerator generator, MethodInfo method) => generator.FluentEmit(OpCodes.Callvirt, method);

        public static XsILGenerator CallVirtual(this XsILGenerator generator, XsMethod method) => generator.FluentEmit(OpCodes.Callvirt, method.MethodInfo);

        /// <summary>
        /// Performs a tail call to the given method, popping the requisite number of arguments from the evaluation stack (including the this reference if it is an instance method)
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator TailCall(this XsILGenerator generator, MethodInfo method)
        {
            return generator.FluentEmit(OpCodes.Tailcall)
                            .FluentEmit(OpCodes.Call, method);
        }

        /// <summary>
        /// Performs a tail call to the given method with virtual semantics, popping a reference (and performing a null check) and the requisite number of arguments from the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator TailCallVirtual(this XsILGenerator generator, MethodInfo method)
        {
            return generator.FluentEmit(OpCodes.Tailcall)
                            .FluentEmit(OpCodes.Callvirt, method);
        }

        /// <summary>
        /// Performs a constrained virtual call to the given method, popping an address to storage location of the value or reference (and performing a null check if necessary) and the requisite number of arguments from the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type to constrain the call to</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator ConstrainedCall<T>(this XsILGenerator generator, MethodInfo method)
            => generator.ConstrainedCall(typeof(T), method);

        /// <summary>
        /// Performs a constrained virtual call to the given method, popping an address to storage location of the value or reference (and performing a null check if necessary) and the requisite number of arguments from the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="constrainedType">The type to constrain the call to</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator ConstrainedCall(this XsILGenerator generator, Type constrainedType, MethodInfo method)
        {
            return generator.FluentEmit(OpCodes.Constrained, constrainedType)
                            .FluentEmit(OpCodes.Callvirt, method);
        }

        /// <summary>
        /// Performs a constrained virtual tail call to the given method, popping an address to storage location of the value or reference (and performing a null check if necessary) and the requisite number of arguments from the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type to constrain the call to</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator ConstrainedTailCall<T>(this XsILGenerator generator, MethodInfo method)
            => generator.ConstrainedTailCall(typeof(T), method);

        /// <summary>
        /// Performs a constrained virtual tail call to the given method, popping an address to storage location of the value or reference (and performing a null check if necessary) and the requisite number of arguments from the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="constrainedType">The type to constrain the call to</param>
        /// <param name="method">The method to call</param>

        public static XsILGenerator ConstrainedTailCall(this XsILGenerator generator, Type constrainedType, MethodInfo method)
        {
            return generator.FluentEmit(OpCodes.Constrained, constrainedType)
                            .FluentEmit(OpCodes.Tailcall)
                            .FluentEmit(OpCodes.Callvirt, method);
        }

        /// <summary>
        /// Pops a value from the evaluation stack and returns it to the calling method
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator Return(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ret);
    }
}
