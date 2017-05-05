using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for comparing objects on the stack
    /// </summary>
    public static partial class CompareAndBranch
    {
        /// <summary>
        /// Pops a reference from the evaluation stack and branches to the given label if it is the null reference
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchIfNull(this XsILGenerator generator, Label label)
        {
            return generator.LoadNull()
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops a reference from the evaluation stack and branches to the given label if it is the null reference
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchIfNull(this XsILGenerator generator, string labelName)
            => generator.BranchIfNull(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops a reference from the evaluation stack and branches to the given label if it is the null reference
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchIfNullShortForm(this XsILGenerator generator, Label label)
        {
            return generator.LoadNull()
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops a reference from the evaluation stack and branches to the given label if it is the null reference
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchIfNullShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfNullShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as true
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchIfTrue(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Brtrue, label);

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as true
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchIfTrue(this XsILGenerator generator, string labelName)
            => generator.BranchIfTrue(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as true
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchIfTrueShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Brtrue_S, label);

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as true
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchIfTrueShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfTrueShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as false
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchIfFalse(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Brfalse, label);

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as false
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchIfFalse(this XsILGenerator generator, string labelName)
            => generator.BranchIfFalse(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as false
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchIfFalseShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Brfalse_S, label);

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it interprets as false
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchIfFalseShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfFalseShortForm(generator.GetOrCreateLabel(labelName));
    }
}
