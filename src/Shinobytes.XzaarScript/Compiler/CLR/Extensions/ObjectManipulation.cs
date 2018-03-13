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
