﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for manipulation of type, method and field tokens
    /// </summary>
    
    public static class Tokens
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
