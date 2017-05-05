using System;
using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	
	public static partial class ArithmeticOperations
	{
		#region Add

		/// <summary>
        /// Pops two values from the top of the evaluation stack and adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Add(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Add);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator AddWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Add_Ovf);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator AddUnsignedWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Add_Ovf_Un);

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddTo(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Add();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToWithOverflowCheck(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.AddWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToUnsignedWithOverflowCheck(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.AddUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddTo(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Add();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToWithOverflowCheck(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.AddWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToUnsignedWithOverflowCheck(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.AddUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddTo(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Add();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToWithOverflowCheck(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.AddWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToUnsignedWithOverflowCheck(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.AddUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddTo(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Add();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToWithOverflowCheck(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.AddWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToUnsignedWithOverflowCheck(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.AddUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddTo(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.Add();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToWithOverflowCheck(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.AddWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToUnsignedWithOverflowCheck(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.AddUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddTo(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.Add();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value adds them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToWithOverflowCheck(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.AddWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value adds them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to add the evaluation stack value to</param>
		
		public static XsILGenerator AddToUnsignedWithOverflowCheck(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.AddUnsignedWithOverflowCheck();
		}


		#endregion

		#region Divide

		/// <summary>
        /// Pops two values from the top of the evaluation stack and divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Divide(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Div);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator DivideUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Div_Un);

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideBy(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Divide();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideByUnsigned(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.DivideUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideBy(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Divide();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideByUnsigned(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.DivideUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideBy(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Divide();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideByUnsigned(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.DivideUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideBy(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Divide();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideByUnsigned(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.DivideUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideBy(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.Divide();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideByUnsigned(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.DivideUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideBy(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.Divide();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value divides the first by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator DivideByUnsigned(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.DivideUnsigned();
		}


		#endregion

		#region Multiply

		/// <summary>
        /// Pops two values from the top of the evaluation stack and multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Multiply(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Mul);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator MultiplyWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Mul_Ovf);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator MultiplyUnsignedWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Mul_Ovf_Un);

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyBy(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Multiply();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByWithOverflowCheck(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.MultiplyWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByUnsignedWithOverflowCheck(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.MultiplyUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyBy(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Multiply();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByWithOverflowCheck(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.MultiplyWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByUnsignedWithOverflowCheck(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.MultiplyUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyBy(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Multiply();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByWithOverflowCheck(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.MultiplyWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByUnsignedWithOverflowCheck(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.MultiplyUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyBy(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Multiply();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByWithOverflowCheck(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.MultiplyWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByUnsignedWithOverflowCheck(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.MultiplyUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyBy(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.Multiply();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByWithOverflowCheck(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.MultiplyWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByUnsignedWithOverflowCheck(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.MultiplyUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyBy(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.Multiply();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value multiples them together with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByWithOverflowCheck(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.MultiplyWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value multiples them together without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to multiply with the evaluation stack value</param>
		
		public static XsILGenerator MultiplyByUnsignedWithOverflowCheck(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.MultiplyUnsignedWithOverflowCheck();
		}


		#endregion

		#region Remainder

		/// <summary>
        /// Pops two values from the top of the evaluation stack and finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Remainder(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Rem);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator RemainderUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Rem_Un);

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFrom(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Remainder();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFromUnsigned(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.RemainderUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFrom(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Remainder();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFromUnsigned(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.RemainderUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFrom(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Remainder();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFromUnsigned(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.RemainderUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFrom(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Remainder();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFromUnsigned(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.RemainderUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFrom(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.Remainder();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFromUnsigned(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.RemainderUnsigned();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFrom(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.Remainder();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value finds the remainder when the first is divided by the second without regard for sign
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to divide the evaluation stack value by</param>
		
		public static XsILGenerator RemainderFromUnsigned(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.RemainderUnsigned();
		}


		#endregion

		#region Subtract

		/// <summary>
        /// Pops two values from the top of the evaluation stack and subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Sub);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Sub_Ovf);

		/// <summary>
        /// Pops two values from the top of the evaluation stack and subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Sub_Ovf_Un);

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.Subtract();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.SubtractWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator, Int32 value)
		{
			return generator.LoadConstant(value)
							.SubtractUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.Subtract();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.SubtractWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator, UInt32 value)
		{
			return generator.LoadConstant(value)
							.SubtractUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.Subtract();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.SubtractWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator, Int64 value)
		{
			return generator.LoadConstant(value)
							.SubtractUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.Subtract();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.SubtractWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator, UInt64 value)
		{
			return generator.LoadConstant(value)
							.SubtractUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.Subtract();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.SubtractWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator, Single value)
		{
			return generator.LoadConstant(value)
							.SubtractUnsignedWithOverflowCheck();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator Subtract(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.Subtract();
		}

		/// <summary>
        /// Pops a value from the top of the evaluation stack, and with the given value subtracts the second from the first with a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractWithOverflowCheck(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.SubtractWithOverflowCheck();
		}

		/// <summary>
        /// Pop a value from the top of the evaluation stack, and with the given value subtracts the second from the first without regard for sign, and a check for overflow
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The value to subtract from the evaluation stack value</param>
		
		public static XsILGenerator SubtractUnsignedWithOverflowCheck(this XsILGenerator generator, Double value)
		{
			return generator.LoadConstant(value)
							.SubtractUnsignedWithOverflowCheck();
		}


		#endregion

	}
}
