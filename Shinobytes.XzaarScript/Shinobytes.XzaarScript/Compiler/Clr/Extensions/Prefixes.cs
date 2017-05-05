using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods that emit instruction prefixes
    /// </summary>
    
    public static class Prefixes
    {
        /// <summary>
        /// Indicates that the next operation should not assume that its arguments are properly aligned
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Unaligned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Unaligned);
    }
}
