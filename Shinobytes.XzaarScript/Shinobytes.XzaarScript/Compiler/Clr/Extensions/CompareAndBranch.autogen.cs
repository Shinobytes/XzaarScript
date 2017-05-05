using System;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{

    public static partial class CompareAndBranch
    {

        #region Equal

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqual(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Beq, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqual(this XsILGenerator generator, string labelName)
            => generator.BranchIfEqual(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Beq_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfEqualShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualTo(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfEqualToShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops two integer values from the evaluation stack and pushes the result of comparing whether the first is equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator CompareEqual(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ceq);

        public static XsILGenerator CompareNotEqual(this XsILGenerator generator)
        {
            return generator.CompareEqual().CompareEqualTo(0);
        }


        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, Char value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, Int32 value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, UInt32 value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, Int64 value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, UInt64 value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, Single value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareEqualTo(this XsILGenerator generator, Double value)
        {
            return generator.LoadConstant(value)
                            .CompareEqual();
        }

        #endregion

        #region NotEqual
        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is not equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualUnsigned(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bne_Un, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is not equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualUnsigned(this XsILGenerator generator, string labelName)
            => generator.BranchIfNotEqualUnsigned(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is not equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualUnsignedShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bne_Un_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is not equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualUnsignedShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfNotEqualUnsignedShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsigned(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfNotEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfNotEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is not equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfNotEqualToUnsignedShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfNotEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));

        #endregion

        #region GreaterThanOrEqual

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqual(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bge, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqual(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterThanOrEqual(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bge_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterThanOrEqualShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualTo(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualUnsigned(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bge_Un, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualUnsigned(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterThanOrEqualUnsigned(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualUnsignedShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bge_Un_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualUnsignedShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterThanOrEqualUnsignedShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsigned(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));

        #endregion

        #region Greater

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreater(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bgt, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreater(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreater(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bgt_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreater(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThan(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterUnsigned(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bgt_Un, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterUnsigned(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterUnsigned(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterUnsignedShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Bgt_Un_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is greater than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterUnsignedShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfGreaterUnsignedShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsigned(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfGreaterUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is greater than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfGreaterThanUnsignedShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfGreaterThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and pushes the result of comparing whether the first is greater than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator CompareGreater(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Cgt);

        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, Char value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, Int32 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, UInt32 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, Int64 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, UInt64 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, Single value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThan(this XsILGenerator generator, Double value)
        {
            return generator.LoadConstant(value)
                            .CompareGreater();
        }

        /// <summary>
        /// Pops two integer values from the evaluation stack and pushes the result of comparing whether the first is greater than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator CompareGreaterUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Cgt_Un);

        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, Char value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, Int32 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, UInt32 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, Int64 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, UInt64 value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, Single value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is greater than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareGreaterThanUnsigned(this XsILGenerator generator, Double value)
        {
            return generator.LoadConstant(value)
                            .CompareGreaterUnsigned();
        }

        #endregion

        #region LessThanOrEqual

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqual(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Ble, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqual(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessThanOrEqual(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Ble_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessThanOrEqualShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqual(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualTo(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanOrEqualTo(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanOrEqualToShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualUnsigned(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Ble_Un, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualUnsigned(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessThanOrEqualUnsigned(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualUnsignedShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Ble_Un_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than or equal to the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualUnsignedShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessThanOrEqualUnsignedShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsigned(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessThanOrEqualUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than or equal to the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanOrEqualToUnsignedShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanOrEqualToUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));

        #endregion

        #region Less

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLess(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Blt, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLess(this XsILGenerator generator, string labelName)
            => generator.BranchIfLess(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Blt_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLess(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThan(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThan(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessUnsigned(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Blt_Un, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessUnsigned(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessUnsigned(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessUnsignedShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Blt_Un_S, label);

        /// <summary>
        /// Pops two integer values from the evaluation stack and branches to the given label if the first is less than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessUnsignedShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchIfLessUnsignedShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Char value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Char value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Int32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Int32 value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, UInt32 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, UInt32 value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Int64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Int64 value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, UInt64 value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, UInt64 value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Single value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Single value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));
        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsigned(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsigned(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanUnsigned(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="label">The label to branch to</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Double value, Label label)
        {
            return generator.LoadConstant(value)
                            .BranchIfLessUnsignedShortForm(label);
        }

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the given label if it is less than the given value, with regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>
        /// <param name="labelName">The name of the fluently-specified label</param>

        public static XsILGenerator BranchIfLessThanUnsignedShortForm(this XsILGenerator generator, Double value, string labelName)
            => generator.BranchIfLessThanUnsignedShortForm(value, generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops two integer values from the evaluation stack and pushes the result of comparing whether the first is less than the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator CompareLess(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Clt);

        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, Char value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, Int32 value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, UInt32 value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, Int64 value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, UInt64 value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, Single value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThan(this XsILGenerator generator, Double value)
        {
            return generator.LoadConstant(value)
                            .CompareLess();
        }

        /// <summary>
        /// Pops two integer values from the evaluation stack and pushes the result of comparing whether the first is less than the second, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator CompareLessUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Clt_Un);

        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, Char value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, Int32 value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, UInt32 value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, Int64 value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, UInt64 value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, Single value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }
        /// <summary>
        /// Pops an integer value from the evaluation stack and pushes the result of comparing whether it is less than the given value, without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to compare to the evaluation stack value</param>

        public static XsILGenerator CompareLessThanUnsigned(this XsILGenerator generator, Double value)
        {
            return generator.LoadConstant(value)
                            .CompareLessUnsigned();
        }

        #endregion

    }
}
