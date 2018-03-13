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
 
using System;
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