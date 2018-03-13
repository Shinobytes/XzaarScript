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
    /// Contains extension methods for manipulation of type, method and field tokens
    /// </summary>
    
    public static class TokenExtensions
    {
        /// <summary>
        /// Pushes the token for the given type onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type to load the token of</param>
        
        public static XsILGenerator LoadTokenFor(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Ldtoken, type);

        /// <summary>
        /// Pushes the token for the given type onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type to load the token of</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator LoadTokenFor<T>(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ldtoken, typeof (T));

        private static readonly MethodInfo GetTypeFromHandle = typeof (Type).GetMethod("GetTypeFromHandle");

        /// <summary>
        /// Pushes the <see cref="Type" /> of the given type onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type to load the <see cref="Type" /> of</param>
        
        public static XsILGenerator TypeOf(this XsILGenerator generator, Type type)
        {
            return generator.LoadTokenFor(type)
                            .Call(GetTypeFromHandle);
        }

        /// <summary>
        /// Pushes the <see cref="Type" /> of the given type onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type to load the <see cref="Type" /> of</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator TypeOf<T>(this XsILGenerator generator) => generator.TypeOf(typeof (T));

        /// <summary>
        /// Pushes the token for the given method onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to load the token of</param>
        
        public static XsILGenerator LoadTokenFor(this XsILGenerator generator, MethodInfo method) => generator.FluentEmit(OpCodes.Ldtoken, method);
        
        private static readonly MethodInfo GetMethodFromHandle = typeof(MethodBase).GetMethod("GetMethodFromHandle", new [] { typeof(RuntimeMethodHandle) });

        /// <summary>
        /// Pushes the <see cref="MethodInfo" /> of the given method onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to load the <see cref="MethodInfo" /> of</param>
        
        public static XsILGenerator MethodInfoFor(this XsILGenerator generator, MethodInfo method)
        {
            return generator.LoadTokenFor(method)
                            .Call(GetMethodFromHandle);
        }

        /// <summary>
        /// Pushes the token for the given field onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to load the token of</param>
        
        public static XsILGenerator LoadTokenFor(this XsILGenerator generator, FieldInfo field) => generator.FluentEmit(OpCodes.Ldtoken, field);

        private static readonly MethodInfo GetFieldFromHandle = typeof (FieldInfo).GetMethod("GetFieldFromHandle", new [] { typeof (RuntimeFieldHandle) });

        /// <summary>
        /// Pushes the <see cref="FieldInfo" /> of the given field onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to load the <see cref="FieldInfo" /> of</param>
        
        public static XsILGenerator FieldInfoFor(this XsILGenerator generator, FieldInfo field)
        {
            return generator.LoadTokenFor(field)
                            .Call(GetFieldFromHandle);
        }
    }
}
