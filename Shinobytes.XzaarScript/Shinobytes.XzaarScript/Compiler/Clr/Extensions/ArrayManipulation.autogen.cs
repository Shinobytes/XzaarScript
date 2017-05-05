using System;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    public static partial class ArrayManipulation
    {
		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Boolean" />), and stores the given <see cref="System.Boolean" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Boolean" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Boolean value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Boolean>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Char" />), and stores the given <see cref="System.Char" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Char" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Char value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Char>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.SByte" />), and stores the given <see cref="System.SByte" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="SByte" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, SByte value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<SByte>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Byte" />), and stores the given <see cref="System.Byte" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Byte" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Byte value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Byte>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Int16" />), and stores the given <see cref="System.Int16" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Int16" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Int16 value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Int16>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.UInt16" />), and stores the given <see cref="System.UInt16" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="UInt16" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, UInt16 value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<UInt16>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Int32" />), and stores the given <see cref="System.Int32" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Int32" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Int32 value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Int32>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.UInt32" />), and stores the given <see cref="System.UInt32" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="UInt32" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, UInt32 value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<UInt32>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Int64" />), and stores the given <see cref="System.Int64" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Int64" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Int64 value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Int64>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.UInt64" />), and stores the given <see cref="System.UInt64" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="UInt64" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, UInt64 value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<UInt64>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Single" />), and stores the given <see cref="System.Single" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Single" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Single value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Single>();
        }

		/// <summary>
        /// Pops an array reference (containing elements of <see cref="System.Double" />), and stores the given <see cref="System.Double" /> value at the given index
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		/// <param name="value">The <see cref="Double" /> value to store in the array</param>
		/// <param name="index">The index to store the value at</param>
		
		public static XsILGenerator StoreElementAtIndex(this XsILGenerator generator, Double value, uint index)
        {
            return generator.LoadConstant(index)
							.LoadConstant(value)
							.StoreElement<Double>();
        }

    }
}
