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
