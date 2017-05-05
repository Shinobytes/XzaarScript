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
