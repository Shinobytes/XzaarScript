using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    public static partial class Members
    {
		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, Boolean value)
        {
            if (property.PropertyType != typeof(Boolean))
            {
                throw new InvalidOperationException("Property is not of type Boolean");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, Boolean value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, Boolean value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<Boolean>> expression, Boolean value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, Boolean value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, Char value)
        {
            if (property.PropertyType != typeof(Char))
            {
                throw new InvalidOperationException("Property is not of type Char");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, Char value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, Char value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<Char>> expression, Char value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, Char value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, SByte value)
        {
            if (property.PropertyType != typeof(SByte))
            {
                throw new InvalidOperationException("Property is not of type SByte");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, SByte value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, SByte value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<SByte>> expression, SByte value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, SByte value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, Byte value)
        {
            if (property.PropertyType != typeof(Byte))
            {
                throw new InvalidOperationException("Property is not of type Byte");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, Byte value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, Byte value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<Byte>> expression, Byte value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, Byte value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, Int16 value)
        {
            if (property.PropertyType != typeof(Int16))
            {
                throw new InvalidOperationException("Property is not of type Int16");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, Int16 value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, Int16 value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<Int16>> expression, Int16 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, Int16 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, UInt16 value)
        {
            if (property.PropertyType != typeof(UInt16))
            {
                throw new InvalidOperationException("Property is not of type UInt16");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, UInt16 value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, UInt16 value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<UInt16>> expression, UInt16 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, UInt16 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, Int32 value)
        {
            if (property.PropertyType != typeof(Int32))
            {
                throw new InvalidOperationException("Property is not of type Int32");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, Int32 value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, Int32 value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<Int32>> expression, Int32 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, Int32 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, UInt32 value)
        {
            if (property.PropertyType != typeof(UInt32))
            {
                throw new InvalidOperationException("Property is not of type UInt32");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, UInt32 value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, UInt32 value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<UInt32>> expression, UInt32 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, UInt32 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, Int64 value)
        {
            if (property.PropertyType != typeof(Int64))
            {
                throw new InvalidOperationException("Property is not of type Int64");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, Int64 value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, Int64 value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<Int64>> expression, Int64 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, Int64 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property, UInt64 value)
        {
            if (property.PropertyType != typeof(UInt64))
            {
                throw new InvalidOperationException("Property is not of type UInt64");
            }			

            return generator.LoadConstant(value)
							.SetProperty(property);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName, UInt64 value)
			=> generator.SetProperty(GetPropertyInfo(type, propertyName), value);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the given value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>
		/// <param name="value">The value to set the property to</param>
		
        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName, UInt64 value)
			=> generator.SetProperty(typeof (T), propertyName, value);

		/// <summary>
        /// Calls the setter of the static property represented by the given expression with the given value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty(this XsILGenerator generator, Expression<Func<UInt64>> expression, UInt64 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

		/// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>
		/// <param name="value">The value to set the property to</param>
        
        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression, UInt64 value)
			=> generator.SetProperty(GetPropertyInfo(expression), value);

	}
}