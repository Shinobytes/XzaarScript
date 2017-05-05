using System;
using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	public static partial class Arguments
	{
		/// <summary>
        /// Overwrite the specified argument with the given value.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>
		/// <param name="value">The value to store in the argument</param>
		
		public static XsILGenerator OverwriteArgument(this XsILGenerator generator, ushort argNum, Char value)
		{
			return generator.LoadConstant(value)
							.StoreInArgument(argNum);
		}
		
		/// <summary>
        /// Overwrite the specified argument with the given value.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>
		/// <param name="value">The value to store in the argument</param>
		
		public static XsILGenerator OverwriteArgument(this XsILGenerator generator, ushort argNum, Boolean value)
		{
			return generator.LoadConstant(value)
							.StoreInArgument(argNum);
		}
		
		/// <summary>
        /// Overwrite the specified argument with the given value.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>
		/// <param name="value">The value to store in the argument</param>
		
		public static XsILGenerator OverwriteArgument(this XsILGenerator generator, ushort argNum, Int32 value)
		{
			return generator.LoadConstant(value)
							.StoreInArgument(argNum);
		}
		
		/// <summary>
        /// Overwrite the specified argument with the given value.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>
		/// <param name="value">The value to store in the argument</param>
		
		public static XsILGenerator OverwriteArgument(this XsILGenerator generator, ushort argNum, UInt32 value)
		{
			return generator.LoadConstant(value)
							.StoreInArgument(argNum);
		}
		
		/// <summary>
        /// Overwrite the specified argument with the given value.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>
		/// <param name="value">The value to store in the argument</param>
		
		public static XsILGenerator OverwriteArgument(this XsILGenerator generator, ushort argNum, Int64 value)
		{
			return generator.LoadConstant(value)
							.StoreInArgument(argNum);
		}
		
		/// <summary>
        /// Overwrite the specified argument with the given value.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="argNum">The index of the argument to store the value in</param>
		/// <param name="value">The value to store in the argument</param>
		
		public static XsILGenerator OverwriteArgument(this XsILGenerator generator, ushort argNum, UInt64 value)
		{
			return generator.LoadConstant(value)
							.StoreInArgument(argNum);
		}
		
	}
}