using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions.ExceptionBlock
{
    /// <summary>
    /// Contains extension methods for creating protected regions
    /// </summary>
    
    public static class ExceptionHandling
    {
        /// <summary>
        /// Starts a protected region
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <returns>An <see cref="ExceptionBlock" /> instance from which the various exception handling blocks can be accessed</returns>
        
        public static ExceptionBlock ExceptionBlock(this XsILGenerator generator) => new ExceptionBlock(generator);
    }
}
