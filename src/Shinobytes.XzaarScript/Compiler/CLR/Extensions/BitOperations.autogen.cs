/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

using System;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	/// <summary>
    /// Contains extension methods for perofmring bitwise operations on integers
    /// </summary>
	public static class BitOperations
	{
		#region And

		/// <summary>
        /// Pop two integer values from the evaluation stack and perform a bitwise and operation on them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator And(this XsILGenerator generator) => generator.FluentEmit(OpCodes.And);

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise and operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise and the evaluation stack value with</param>
		
		public static XsILGenerator AndWith(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.And();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise and operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise and the evaluation stack value with</param>
		
		public static XsILGenerator AndWith(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.And();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise and operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise and the evaluation stack value with</param>
		
		public static XsILGenerator AndWith(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.And();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise and operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise and the evaluation stack value with</param>
		
		public static XsILGenerator AndWith(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.And();
		}

		#endregion
		#region Complement

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise complement operation on it
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Complement(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Not);

		#endregion
		#region Not

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise not operation on it
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Not(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Not);

		#endregion
		#region Or

		/// <summary>
        /// Pop two integer values from the evaluation stack and perform a bitwise or operation on them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Or(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Or);

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise or operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise or the evaluation stack value with</param>
		
		public static XsILGenerator OrWith(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Or();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise or operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise or the evaluation stack value with</param>
		
		public static XsILGenerator OrWith(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Or();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise or operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise or the evaluation stack value with</param>
		
		public static XsILGenerator OrWith(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Or();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise or operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise or the evaluation stack value with</param>
		
		public static XsILGenerator OrWith(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Or();
		}

		#endregion
		#region ShiftLeft

		/// <summary>
        /// Pop two integer values from the evaluation stack and perform a bitwise shiftleft operation on them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator ShiftLeft(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Shl);

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftleft operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftleft the evaluation stack value by</param>
		
		public static XsILGenerator ShiftLeftBy(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.ShiftLeft();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftleft operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftleft the evaluation stack value by</param>
		
		public static XsILGenerator ShiftLeftBy(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.ShiftLeft();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftleft operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftleft the evaluation stack value by</param>
		
		public static XsILGenerator ShiftLeftBy(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.ShiftLeft();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftleft operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftleft the evaluation stack value by</param>
		
		public static XsILGenerator ShiftLeftBy(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.ShiftLeft();
		}

		#endregion
		#region ShiftRight

		/// <summary>
        /// Pop two integer values from the evaluation stack and perform a bitwise shiftright operation on them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator ShiftRight(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Shr);

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftright operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftright the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightBy(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.ShiftRight();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftright operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftright the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightBy(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.ShiftRight();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftright operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftright the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightBy(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.ShiftRight();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftright operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftright the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightBy(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.ShiftRight();
		}

		#endregion
		#region ShiftRightUnsigned

		/// <summary>
        /// Pop two integer values from the evaluation stack and perform a bitwise shiftrightunsigned operation on them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator ShiftRightUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Shr_Un);

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftrightunsigned operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftrightunsigned the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightUnsignedBy(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.ShiftRightUnsigned();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftrightunsigned operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftrightunsigned the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightUnsignedBy(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.ShiftRightUnsigned();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftrightunsigned operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftrightunsigned the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightUnsignedBy(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.ShiftRightUnsigned();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise shiftrightunsigned operation by the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise shiftrightunsigned the evaluation stack value by</param>
		
		public static XsILGenerator ShiftRightUnsignedBy(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.ShiftRightUnsigned();
		}

		#endregion
		#region Xor

		/// <summary>
        /// Pop two integer values from the evaluation stack and perform a bitwise xor operation on them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Xor(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Xor);

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise xor operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise xor the evaluation stack value with</param>
		
		public static XsILGenerator XorWith(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Xor();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise xor operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise xor the evaluation stack value with</param>
		
		public static XsILGenerator XorWith(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Xor();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise xor operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise xor the evaluation stack value with</param>
		
		public static XsILGenerator XorWith(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Xor();
		}

		/// <summary>
        /// Pop an integer value from the evaluation stack and perform a bitwise xor operation with the given value
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The value to bitwise xor the evaluation stack value with</param>
		
		public static XsILGenerator XorWith(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Xor();
		}

		#endregion
	}
}
