using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for operations useful to debuggers
    /// </summary>

    public static class DebugOperations
    {
        /// <summary>
        /// Performs no operation
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator NoOp(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Nop);

        public static XsILGenerator Nop(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Nop);

        /// <summary>
        /// Performs no operation
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator NoOperation(this XsILGenerator generator) => generator.NoOp();

        /// <summary>
        /// Signals an attached debugger to break execution
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator Break(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Break);

        /// <summary>
        /// Signals an attached debugger to break execution
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator BreakInDebugger(this XsILGenerator generator) => generator.Break();
    }
}
