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
    /// Contains extension methods for the manipulation of objects
    /// </summary>
    
    public static class ObjectManipulation
    {
        /// <summary>
        /// Pops two addresses from the evaluation stack and copies the value type object (of the given type) in the first into the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the value type object</param>
        
        public static XsILGenerator CopyObject(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("Copying a non-value-type results in unspecified runtime behaviour");
            }

            return generator.FluentEmit(OpCodes.Cpobj, type);
        }

        /// <summary>
        /// Pops two addresses from the evaluation stack and copies the value type object (of the given type) in the first into the second
        /// </summary>
        /// <typeparam name="T">The type of the value type object</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator CopyObject<T>(this XsILGenerator generator) where T : struct => generator.CopyObject(typeof (T));
        
        /// <summary>
        /// Pops two addresses and an integer from the evaluation stack, and copies that number of bytes from the first address to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator CopyBlock(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Cpblk);

        /// <summary>
        /// Pops two address from the evaluation stack and copies the given number of bytes from the first to the second
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="bytes">The number of bytes to copy</param>
        
        public static XsILGenerator CopyBlock(this XsILGenerator generator, uint bytes)
        {
            return generator.LoadConstant(bytes)
                            .CopyBlock();
        }

        /// <summary>
        /// Pops two addresses and an integer from the evaluation stack, and copies that number of bytes from the first address to the second, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator CopyBlockVolatile(this XsILGenerator generator)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .CopyBlock();
        }

        /// <summary>
        /// Pops two address from the evaluation stack and copies the given number of bytes from the first to the second, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="bytes">The number of bytes to copy</param>
        
        public static XsILGenerator CopyBlockVolatile(this XsILGenerator generator, uint bytes)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .CopyBlock(bytes);
        }

        /// <summary>
        /// Pops an address from the evaluation stack and pushes the value type object (of the given type) at that location onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the value type object</param>
        
        public static XsILGenerator LoadValueTypeOntoStack(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("This operation is not valid on reference types");
            }

            if (type == typeof (sbyte))
            {
                return generator.FluentEmit(OpCodes.Ldind_I1);
            }
            else if (type == typeof (byte))
            {
                return generator.FluentEmit(OpCodes.Ldind_U1);
            }
            else if (type == typeof (short))
            {
                return generator.FluentEmit(OpCodes.Ldind_I2);
            }
            else if (type == typeof (ushort))
            {
                return generator.FluentEmit(OpCodes.Ldind_U2);
            }
            else if (type == typeof (int))
            {
                return generator.FluentEmit(OpCodes.Ldind_I4);
            }
            else if (type == typeof (uint))
            {
                return generator.FluentEmit(OpCodes.Ldind_U4);
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                return generator.FluentEmit(OpCodes.Ldind_I8);
            }
            else if (type == typeof (float))
            {
                return generator.FluentEmit(OpCodes.Ldind_R4);
            }
            else if (type == typeof (double))
            {
                return generator.FluentEmit(OpCodes.Ldind_R8);
            }
            else
            {
                return generator.FluentEmit(OpCodes.Ldobj, type);
            }
        }

        /// <summary>
        /// Pops an address from the evaluation stack and pushes the value type object (of the given type) at that location onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type of the value type object</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator LoadValueTypeOntoStack<T>(this XsILGenerator generator) where T : struct
            => generator.LoadValueTypeOntoStack(typeof (T));

        /// <summary>
        /// Pops an address from the evaluation stack and pushes the value type object (of the given type) at that location onto the evaluation stack, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the value type object</param>
        
        public static XsILGenerator LoadValueTypeOntoStackVolatile(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("This operation is not valid on reference types");
            }

            return generator.FluentEmit(OpCodes.Volatile)
                            .LoadValueTypeOntoStack(type);
        }

        /// <summary>
        /// Pops an address from the evaluation stack and pushes the value type object (of the given type) at that location onto the evaluation stack, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type of the value type object</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator LoadValueTypeOntoStackVolatile<T>(this XsILGenerator generator) where T : struct
            => generator.LoadValueTypeOntoStackVolatile(typeof(T));

        /// <summary>
        /// Pops an address and a value type object (of the given type) from the evaluation stack, and copies the object into the address
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the value type object</param>
        
        public static XsILGenerator StoreValueTypeFromStack(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("This operation is not valid on reference types");
            }

            if (type == typeof(sbyte) || type == typeof(byte))
            {
                return generator.FluentEmit(OpCodes.Ldind_I1);
            }
            else if (type == typeof(short) || type == typeof(ushort))
            {
                return generator.FluentEmit(OpCodes.Ldind_I2);
            }
            else if (type == typeof(int) || type == typeof(uint))
            {
                return generator.FluentEmit(OpCodes.Ldind_I4);
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                return generator.FluentEmit(OpCodes.Ldind_I8);
            }
            else if (type == typeof(float))
            {
                return generator.FluentEmit(OpCodes.Ldind_R4);
            }
            else if (type == typeof(double))
            {
                return generator.FluentEmit(OpCodes.Ldind_R8);
            }
            else
            {
                return generator.FluentEmit(OpCodes.Ldobj, type);
            }
        }

        /// <summary>
        /// Pops an address and a value type object (of the given type) from the evaluation stack, and copies the object into the address
        /// </summary>
        /// <typeparam name="T">The type of the value type object</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator StoreValueTypeFromStack<T>(this XsILGenerator generator) where T : struct
            => generator.StoreValueTypeFromStack(typeof (T));

        /// <summary>
        /// Pops an address and a value type object (of the given type) from the evaluation stack, and copies the object into the address, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the value type object</param>
        
        public static XsILGenerator StoreValueTypeFromStackVolatile(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("This operation is not valid on a reference type");
            }

            return generator.FluentEmit(OpCodes.Volatile)
                            .FluentEmit(OpCodes.Stobj, type);
        }

        /// <summary>
        /// Pops an address and a value type object (of the given type) from the evaluation stack, and copies the object into the address, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type of the value type object</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator StoreValueTypeFromStackVolatile<T>(this XsILGenerator generator) where T : struct
            => generator.StoreValueTypeFromStack(typeof(T));

        /// <summary>
        /// Pops an address from the evaluation stack, and pushes the object reference located at that address
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator LoadReferenceFromAddress(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Ldind_Ref);
    }
}
