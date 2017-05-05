using System;
using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for loading constant values onto the stack
    /// </summary>
    public static class LoadConstants
    {
        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, Int32 value)
        {
            switch (value)
            {
                case -1:
                    return generator.FluentEmit(OpCodes.Ldc_I4_M1);
                case 0:
                    return generator.FluentEmit(OpCodes.Ldc_I4_0);
                case 1:
                    return generator.FluentEmit(OpCodes.Ldc_I4_1);
                case 2:
                    return generator.FluentEmit(OpCodes.Ldc_I4_2);
                case 3:
                    return generator.FluentEmit(OpCodes.Ldc_I4_3);
                case 4:
                    return generator.FluentEmit(OpCodes.Ldc_I4_4);
                case 5:
                    return generator.FluentEmit(OpCodes.Ldc_I4_5);
                case 6:
                    return generator.FluentEmit(OpCodes.Ldc_I4_6);
                case 7:
                    return generator.FluentEmit(OpCodes.Ldc_I4_7);
                case 8:
                    return generator.FluentEmit(OpCodes.Ldc_I4_8);
                default:
                    return value <= 127 && value >= -128
                        ? generator.FluentEmit(OpCodes.Ldc_I4_S, value)
                        : generator.FluentEmit(OpCodes.Ldc_I4, value);
            }
        }

        public static XsILGenerator LoadConstantOne(this XsILGenerator generator, string type)
        {
            return generator.LoadConstantNumber(type, 1);
        }

        public static XsILGenerator LoadConstantNumber(this XsILGenerator generator, string type, decimal number)
        {
            var itl = type.ToLower();
            if (itl == "f64") return generator.LoadConstant((double)number);
            if (itl == "f32") return generator.LoadConstant((float)number);
            if (itl == "i64") return generator.LoadConstant((long)number);
            if (itl == "i32") return generator.LoadConstant((int)number);
            if (itl == "i16") return generator.LoadConstant((short)number);
            if (itl == "i8") return generator.LoadConstant((sbyte)number);
            if (itl == "u64") return generator.LoadConstant((ulong)number);
            if (itl == "u32") return generator.LoadConstant((uint)number);
            if (itl == "u16") return generator.LoadConstant((ushort)number);
            if (itl == "u8") return generator.LoadConstant((byte)number);
            return generator.LoadConstant((double)number);
        }

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, UInt32 value)
        {
            switch (value)
            {
                case 0:
                    return generator.FluentEmit(OpCodes.Ldc_I4_0);
                case 1:
                    return generator.FluentEmit(OpCodes.Ldc_I4_1);
                case 2:
                    return generator.FluentEmit(OpCodes.Ldc_I4_2);
                case 3:
                    return generator.FluentEmit(OpCodes.Ldc_I4_3);
                case 4:
                    return generator.FluentEmit(OpCodes.Ldc_I4_4);
                case 5:
                    return generator.FluentEmit(OpCodes.Ldc_I4_5);
                case 6:
                    return generator.FluentEmit(OpCodes.Ldc_I4_6);
                case 7:
                    return generator.FluentEmit(OpCodes.Ldc_I4_7);
                case 8:
                    return generator.FluentEmit(OpCodes.Ldc_I4_8);
                default:
                    return value <= 127
                        ? generator.FluentEmit(OpCodes.Ldc_I4_S, value)
                        : generator.FluentEmit(OpCodes.Ldc_I4, value);
            }
        }

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, long value)
        {
            if (value <= int.MaxValue && value >= int.MinValue)
            {
                return generator.LoadConstant((int)value)
                                .ConvertToInt64();
            }
            else
            {
                return generator.FluentEmit(OpCodes.Ldc_I8, value);
            }
        }

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, ulong value)
        {
            if (value <= uint.MaxValue)
            {
                return generator.LoadConstant((uint)value)
                                .ConvertToUInt64();
            }
            else
            {
                return generator.FluentEmit(OpCodes.Ldc_I8, value);
            }
        }

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, float value) => generator.FluentEmit(OpCodes.Ldc_R4, value);

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, double value) => generator.FluentEmit(OpCodes.Ldc_R8, value);

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, bool value) => generator.LoadConstant(value ? 1 : 0);

        /// <summary>
        /// Pushes the given value onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to push onto the evaluation stack</param>

        public static XsILGenerator LoadConstant(this XsILGenerator generator, char value) => generator.LoadConstant((int)value);

        /// <summary>
        /// Pushes the null reference onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator LoadNull(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ldnull);

        /// <summary>
        /// Pushes the given string onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="str">The string to push onto the evaluation stack</param>

        public static XsILGenerator LoadString(this XsILGenerator generator, string str) => generator.FluentEmit(OpCodes.Ldstr, str);
    }
}