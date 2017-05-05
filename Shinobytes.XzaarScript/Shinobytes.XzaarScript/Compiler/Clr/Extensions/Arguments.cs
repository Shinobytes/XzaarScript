using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for dealing with arguments to the method
    /// </summary>

    public static partial class Arguments
    {

        public static XsILGenerator LoadArgument(this XsILGenerator generator, XsParameter param)
            => generator.LoadArgument((ushort)param.Index);

        /// <summary>
        /// Loads the specified argument onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to load</param>
        public static XsILGenerator LoadArgument(this XsILGenerator generator, ushort argNum)
        {
            switch (argNum)
            {
                case 0:
                    return generator.FluentEmit(OpCodes.Ldarg_0);
                case 1:
                    return generator.FluentEmit(OpCodes.Ldarg_1);
                case 2:
                    return generator.FluentEmit(OpCodes.Ldarg_2);
                case 3:
                    return generator.FluentEmit(OpCodes.Ldarg_3);
                default:
                    return argNum <= 255
                        ? generator.FluentEmit(OpCodes.Ldarg_S, (byte)argNum)
                        : generator.FluentEmit(OpCodes.Ldarg, argNum);
            }
        }

        /// <summary>
        /// Short-cut to load the first argument - which is the this reference in instance methods
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator LoadThis(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ldarg_0);

        /// <summary>
        /// Loads the address of the specified argument onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum"></param>

        public static XsILGenerator LoadArgumentAddress(this XsILGenerator generator, ushort argNum)
        {
            return argNum <= 255
                ? generator.FluentEmit(OpCodes.Ldarga_S, (byte)argNum)
                : generator.FluentEmit(OpCodes.Ldarga, argNum);
        }

        /// <summary>
        /// Pop the value on the top of the evaluation stack and stores it in the specified argument
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>

        public static XsILGenerator StoreInArgument(this XsILGenerator generator, ushort argNum)
        {
            return argNum <= 255
                ? generator.FluentEmit(OpCodes.Starg_S, (byte)argNum)
                : generator.FluentEmit(OpCodes.Starg, argNum);
        }

        public static XsILGenerator StoreInArgument(this XsILGenerator generator, XsParameter param)
            => generator.StoreInArgument((ushort)param.Index);

        /// <summary>
        /// Pushes an unmanaged pointer to the argument list of the current method
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator LoadArgumentList(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Arglist);
    }
}
