using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	
    public static partial class Fields
    {
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Boolean" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Boolean value)
		{
			if (field.FieldType != typeof(Boolean))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Boolean" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Boolean value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Boolean" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Boolean value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Boolean>> fieldExpression, Boolean value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Boolean>> fieldExpression, Boolean value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Boolean" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Boolean value)
		{
			if (field.FieldType != typeof(Boolean))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Boolean" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Boolean value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Boolean" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Boolean value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Boolean>> fieldExpression, Boolean value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Boolean>> fieldExpression, Boolean value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Char" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Char value)
		{
			if (field.FieldType != typeof(Char))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Char" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Char value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Char" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Char value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Char>> fieldExpression, Char value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Char>> fieldExpression, Char value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Char" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Char value)
		{
			if (field.FieldType != typeof(Char))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Char" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Char value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Char" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Char value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Char>> fieldExpression, Char value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Char>> fieldExpression, Char value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="SByte" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, SByte value)
		{
			if (field.FieldType != typeof(SByte))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="SByte" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, SByte value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="SByte" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, SByte value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<SByte>> fieldExpression, SByte value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, SByte>> fieldExpression, SByte value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="SByte" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, SByte value)
		{
			if (field.FieldType != typeof(SByte))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="SByte" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, SByte value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="SByte" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, SByte value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<SByte>> fieldExpression, SByte value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, SByte>> fieldExpression, SByte value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Byte" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Byte value)
		{
			if (field.FieldType != typeof(Byte))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Byte" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Byte value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Byte" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Byte value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Byte>> fieldExpression, Byte value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Byte>> fieldExpression, Byte value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Byte" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Byte value)
		{
			if (field.FieldType != typeof(Byte))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Byte" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Byte value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Byte" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Byte value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Byte>> fieldExpression, Byte value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Byte>> fieldExpression, Byte value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int16" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Int16 value)
		{
			if (field.FieldType != typeof(Int16))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int16" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Int16 value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int16" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Int16 value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Int16>> fieldExpression, Int16 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Int16>> fieldExpression, Int16 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int16" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Int16 value)
		{
			if (field.FieldType != typeof(Int16))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int16" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Int16 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int16" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Int16 value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Int16>> fieldExpression, Int16 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Int16>> fieldExpression, Int16 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt16" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, UInt16 value)
		{
			if (field.FieldType != typeof(UInt16))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt16" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, UInt16 value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt16" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, UInt16 value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<UInt16>> fieldExpression, UInt16 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, UInt16>> fieldExpression, UInt16 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt16" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, UInt16 value)
		{
			if (field.FieldType != typeof(UInt16))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt16" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, UInt16 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt16" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, UInt16 value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<UInt16>> fieldExpression, UInt16 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, UInt16>> fieldExpression, UInt16 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int32" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Int32 value)
		{
			if (field.FieldType != typeof(Int32))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int32" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Int32 value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int32" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Int32 value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Int32>> fieldExpression, Int32 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Int32>> fieldExpression, Int32 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int32" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Int32 value)
		{
			if (field.FieldType != typeof(Int32))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int32" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Int32 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int32" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Int32 value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Int32>> fieldExpression, Int32 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Int32>> fieldExpression, Int32 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt32" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, UInt32 value)
		{
			if (field.FieldType != typeof(UInt32))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt32" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, UInt32 value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt32" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, UInt32 value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<UInt32>> fieldExpression, UInt32 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, UInt32>> fieldExpression, UInt32 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt32" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, UInt32 value)
		{
			if (field.FieldType != typeof(UInt32))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt32" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, UInt32 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt32" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, UInt32 value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<UInt32>> fieldExpression, UInt32 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, UInt32>> fieldExpression, UInt32 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int64" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Int64 value)
		{
			if (field.FieldType != typeof(Int64))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int64" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Int64 value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int64" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Int64 value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Int64>> fieldExpression, Int64 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Int64>> fieldExpression, Int64 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int64" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Int64 value)
		{
			if (field.FieldType != typeof(Int64))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int64" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Int64 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Int64" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Int64 value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Int64>> fieldExpression, Int64 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Int64>> fieldExpression, Int64 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt64" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, UInt64 value)
		{
			if (field.FieldType != typeof(UInt64))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt64" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, UInt64 value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt64" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, UInt64 value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<UInt64>> fieldExpression, UInt64 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, UInt64>> fieldExpression, UInt64 value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt64" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, UInt64 value)
		{
			if (field.FieldType != typeof(UInt64))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt64" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, UInt64 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="UInt64" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, UInt64 value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<UInt64>> fieldExpression, UInt64 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, UInt64>> fieldExpression, UInt64 value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Single" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Single value)
		{
			if (field.FieldType != typeof(Single))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Single" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Single value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Single" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Single value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Single>> fieldExpression, Single value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Single>> fieldExpression, Single value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Single" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Single value)
		{
			if (field.FieldType != typeof(Single))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Single" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Single value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Single" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Single value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Single>> fieldExpression, Single value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Single>> fieldExpression, Single value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Double" /></exception>
		
		public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, FieldInfo field, Double value)
		{
			if (field.FieldType != typeof(Double))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Double" /></exception>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Type type, string fieldName, Double value)
            => generator.OverwriteFieldWith(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Double" /></exception>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, string fieldName, Double value)
            => generator.OverwriteFieldWith(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith(this XsILGenerator generator, Expression<Func<Double>> fieldExpression, Double value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWith<T>(this XsILGenerator generator, Expression<Func<T, Double>> fieldExpression, Double value)
            => generator.OverwriteFieldWith(GetFieldInfo(fieldExpression), value);

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Double" /></exception>
		
		public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, FieldInfo field, Double value)
		{
			if (field.FieldType != typeof(Double))
			{
				throw new InvalidOperationException("Type mismatch - field is of type " + field.FieldType);
			}

			return generator.LoadConstant(value)
							.FluentEmit(OpCodes.Volatile)
							.StoreInField(field);
		}

		/// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Double" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Type type, string fieldName, Double value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(type, fieldName), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>
		/// <param name="value">The value to overwrite the field with</param>
		/// <exception cref="InvalidOperationException">Thrown if the field is not of type <see cref="Double" /></exception>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, string fieldName, Double value)
            => generator.OverwriteFieldWithVolatile(typeof(T), fieldName, value);

        /// <summary>
        /// Stores the given value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile(this XsILGenerator generator, Expression<Func<Double>> fieldExpression, Double value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);

        /// <summary>
        /// Pops a reference from the evaluation stack and stores the given value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>
		/// <param name="value">The value to overwrite the field with</param>
        
        public static XsILGenerator OverwriteFieldWithVolatile<T>(this XsILGenerator generator, Expression<Func<T, Double>> fieldExpression, Double value)
            => generator.OverwriteFieldWithVolatile(GetFieldInfo(fieldExpression), value);
	}
}