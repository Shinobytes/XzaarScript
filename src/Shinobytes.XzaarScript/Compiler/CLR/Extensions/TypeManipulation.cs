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
    /// Contains extension methods for dealing with the manipulation of the type of objects on the stack
    /// </summary>
    
    public static class TypeManipulation
    {
        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes a reference of the given type if the object is an instance of that type, otherwise the null reference is pushed
        /// </summary>
        /// <typeparam name="T">The type to attempt to cast to</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator IsInstanceOfType<T>(this XsILGenerator generator) => generator.IsInstanceOfType(typeof(T));

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes a reference of the given type if the object is an instance of the given type, otherwise the null reference is pushed
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type to attempt to cast to</param>
        
        public static XsILGenerator IsInstanceOfType(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Isinst, type);

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes a reference of the given type if the object is an instance of that type, otherwise an <see cref="InvalidCastException" /> is thrown
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type to attempt to cast to</param>
        
        public static XsILGenerator CastClass(this XsILGenerator generator, Type type)
        {
            if (type.IsValueType)
            {
                throw new InvalidOperationException("Cannot cast to a value type");
            }

            return generator.FluentEmit(OpCodes.Castclass, type);
        }

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes a reference of the given type if the object is an instance of that type, otherwise an <see cref="InvalidCastException" /> is thrown
        /// </summary>
        /// <typeparam name="T">The type to attempt to cast to</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator CastClass<T>(this XsILGenerator generator) where T : class => generator.CastClass(typeof (T));

        /// <summary>
        /// Pops a value type object from the evaluation stack, and pushes a reference to a new boxed instance of the object onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the value type object</param>
        
        public static XsILGenerator Box(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("Can only box value types");
            }

            return generator.FluentEmit(OpCodes.Box, type);
        }

        /// <summary>
        /// Pops a value type object from the evaluation stack, and pushes a reference to a new boxed instance of the object onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type of the value type object</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Box<T>(this XsILGenerator generator) where T : struct => generator.Box(typeof (T));

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes the address of the boxed value type of the given type
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the boxed value type</param>
        
        public static XsILGenerator Unbox(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("Cannot unbox non-value types");
            }

            return generator.FluentEmit(OpCodes.Unbox, type);
        }

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes the address of the boxed value type of the given type
        /// </summary>
        /// <typeparam name="T">The type of the boxed value type</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Unbox<T>(this XsILGenerator generator) where T : struct => generator.Unbox(typeof (T));

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes the value type object if the object is a boxed value type, or a reference of the given type if the object is a reference type and is an instance of that type
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type to unbox to</param>
        
        public static XsILGenerator UnboxAny(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Unbox_Any, type);

        /// <summary>
        /// Pops a reference from the evaluation stack, and pushes the value type object if the object is a boxed value type, or a reference of the given type if the object is a reference type and is an instance of that type
        /// </summary>
        /// <typeparam name="T">The type to unbox to</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator UnboxAny<T>(this XsILGenerator generator) => generator.UnboxAny(typeof (T));

        /// <summary>
        /// Pushes the number of bytes required to store the given type, for reference types this will always be the size of a reference, not the size of an object of that type itself
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type to get the size of</param>
        
        public static XsILGenerator SizeOf(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Sizeof, type);

        /// <summary>
        /// Pushes the number of bytes required to store the given type, for reference types this will always be the size of a reference, not the size of an object of that type itself
        /// </summary>
        /// <typeparam name="T">The type to get the size of</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator SizeOf<T>(this XsILGenerator generator) => generator.SizeOf(typeof(T));
    }
}
