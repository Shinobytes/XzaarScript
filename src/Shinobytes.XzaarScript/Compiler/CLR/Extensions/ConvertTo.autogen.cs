/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	/// <summary>
	/// Contains extension methods to convert integer and floating point values to another representation
	/// </summary>
	
	public static class ConvertTo
	{
		#region SByte

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed byte (8 bit integer) with no overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToSByte(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_U1);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed byte (8 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToSByteWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U1);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to a signed byte (8 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToSByteFromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U1_Un);

		#endregion
		#region Byte

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned byte (8 bit integer) with no overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToByte(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_I1);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned byte (8 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToByteWithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I1);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to an unsigned byte (8 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToByteFromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I1_Un);

		#endregion
		#region UInt16

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned short (16 bit integer) with no overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt16(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_U2);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned short (16 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt16WithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U2);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to an unsigned short (16 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt16FromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U2_Un);

		#endregion
		#region Int16

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed short (16 bit integer) with no overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt16(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_I2);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed short (16 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt16WithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I2);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to a signed short (16 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt16FromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I2_Un);

		#endregion
		#region UInt32

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned integer (32 bit integer) with no overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt32(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_U4);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned integer (32 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt32WithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U4);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to an unsigned integer (32 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt32FromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U4_Un);

		#endregion
		#region Int32

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed integer (32 bit integer) with no overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt32(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_I4);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed integer (32 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt32WithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I4);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to a signed integer (32 bit integer) with an overflow check. Pushes an int32 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt32FromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I4_Un);

		#endregion
		#region UInt64

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned long (64 bit integer) with no overflow check. Pushes an int64 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt64(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_U8);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to an unsigned long (64 bit integer) with an overflow check. Pushes an int64 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt64WithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U8);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to an unsigned long (64 bit integer) with an overflow check. Pushes an int64 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToUInt64FromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_U8_Un);

		#endregion
		#region Int64

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed long (64 bit integer) with no overflow check. Pushes an int64 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt64(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_I8);

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed long (64 bit integer) with an overflow check. Pushes an int64 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt64WithOverflowCheck(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I8);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to a signed long (64 bit integer) with an overflow check. Pushes an int64 value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToInt64FromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_Ovf_I8_Un);

		#endregion
		#region Single

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a single floating (8 bit integer) with no overflow check. Pushes an F value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToSingle(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_R4);

		#endregion
		#region Double

		/// <summary>
		/// Converts the signed value on the top of the evaluation stack to a signed byte (8 bit integer) with no overflow check. Pushes an F value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToDouble(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_R8);

		/// <summary>
		/// Converts the unsigned value on the top of the evaluation stack to a signed byte (8 bit integer) with an overflow check. Pushes an F value onto the evaluation stack.
		/// </summary>
		
		public static XsILGenerator ConvertToDoubleFromUnsigned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Conv_R_Un);

		#endregion
	}
}
