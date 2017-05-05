using System;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	
	public static partial class Locals
	{
		
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Boolean" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Boolean value)
		{
			if (local.LocalType != typeof(Boolean))
			{
				throw new ArgumentException("Cannot store a Boolean value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Boolean" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Boolean value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Char" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Char value)
		{
			if (local.LocalType != typeof(Char))
			{
				throw new ArgumentException("Cannot store a Char value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Char" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Char value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="SByte" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, SByte value)
		{
			if (local.LocalType != typeof(SByte))
			{
				throw new ArgumentException("Cannot store a SByte value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="SByte" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, SByte value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Byte" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Byte value)
		{
			if (local.LocalType != typeof(Byte))
			{
				throw new ArgumentException("Cannot store a Byte value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Byte" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Byte value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Int16" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Int16 value)
		{
			if (local.LocalType != typeof(Int16))
			{
				throw new ArgumentException("Cannot store a Int16 value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Int16" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Int16 value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="UInt16" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, UInt16 value)
		{
			if (local.LocalType != typeof(UInt16))
			{
				throw new ArgumentException("Cannot store a UInt16 value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="UInt16" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, UInt16 value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Int32" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Int32 value)
		{
			if (local.LocalType != typeof(Int32))
			{
				throw new ArgumentException("Cannot store a Int32 value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Int32" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Int32 value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="UInt32" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, UInt32 value)
		{
			if (local.LocalType != typeof(UInt32))
			{
				throw new ArgumentException("Cannot store a UInt32 value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="UInt32" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, UInt32 value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Int64" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Int64 value)
		{
			if (local.LocalType != typeof(Int64))
			{
				throw new ArgumentException("Cannot store a Int64 value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Int64" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Int64 value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="UInt64" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, UInt64 value)
		{
			if (local.LocalType != typeof(UInt64))
			{
				throw new ArgumentException("Cannot store a UInt64 value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="UInt64" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, UInt64 value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Single" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Single value)
		{
			if (local.LocalType != typeof(Single))
			{
				throw new ArgumentException("Cannot store a Single value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Single" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Single value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="local">The local to store value in</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Double" /></exception>
		
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, LocalBuilder local, Double value)
		{
			if (local.LocalType != typeof(Double))
			{
				throw new ArgumentException("Cannot store a Double value in a local of type " + local.LocalType.Name);
			}

			return generator.LoadConstant(value)
							.StoreInLocal(local);
		}

		/// <summary>
        /// Stores the given value in the given local
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="localName">The name of the fluently-specified local</param>
		/// <param name="value">The value to store in the local</param>
		/// <exception cref="ArgumentException">Thrown if the local is not of type <see cref="Double" /></exception>
		public static XsILGenerator OverwriteLocalWith(this XsILGenerator generator, string localName, Double value)
			=> generator.OverwriteLocalWith(generator.GetLocal(localName), value);
	}
}