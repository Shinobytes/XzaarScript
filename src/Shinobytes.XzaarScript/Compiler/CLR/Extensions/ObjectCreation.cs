using System;
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for the creation of objects
    /// </summary>
    
    public static class ObjectCreation
    {
        /// <summary>
        /// Creates a new object or instance of a value type, calling the given constructor (and popping the requisite arguments from the evaluation stack) and pushing the reference or value (respectively) onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="constructor">The constructor to call</param>
        
        public static XsILGenerator NewObject(this XsILGenerator generator, ConstructorInfo constructor) => generator.FluentEmit(OpCodes.Newobj, constructor);

        /// <summary>
        /// Creates a new object or instance of a value type, calling the default constructor and pushing the reference or value (respectively) onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type of object/value type to create</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator NewObject<T>(this XsILGenerator generator) where T : new()
            => generator.FluentEmit(OpCodes.Newobj, typeof (T).GetConstructor(Type.EmptyTypes));

        /// <summary>
        /// Pops the address of the storage location of a value type and initializes each field of the type at that location
        /// </summary>
        /// <param name="type">The type to initialize</param>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator InitializeValueType(this XsILGenerator generator, Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException("Cannot initialize a non-value type");
            }

            return generator.FluentEmit(OpCodes.Initobj, type);
        }

        /// <summary>
        /// Pops the address of the storage location of a value type and initializes each field of the type at that location
        /// </summary>
        /// <typeparam name="T">The type to initialize</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator InitializeValueType<T>(this XsILGenerator generator) where T : struct
            => generator.InitializeValueType(typeof (T));

        /// <summary>
        /// Pops an address, initialization value and number of bytes off the evaluation stack, and initializes the block of memory at the address with the value to that size
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator InitializeBlock(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Initblk);

        /// <summary>
        /// Pops an address off the evaluation stack and initializes the block of memory at the address with the given value to the given size
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The initialization value</param>
        /// <param name="bytes">The number of bytes to initialize</param>
        
        public static XsILGenerator InitializeBlock(this XsILGenerator generator, byte value, uint bytes)
        {
            return generator.LoadConstant(value)
                            .LoadConstant(bytes)
                            .InitializeBlock();
        }

        /// <summary>
        /// Pops an address, initialization value and number of bytes off the evaluation stack, and initializes the block of memory at the address with the value to that size, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator InitializeBlockVolatile(this XsILGenerator generator)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .InitializeBlock();
        }

        /// <summary>
        /// Pops an address off the evaluation stack and initializes the block of memory at the address with the given value to the given size, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="value">The initialization value</param>
        /// <param name="bytes">The number of bytes to initialize</param>
        
        public static XsILGenerator InitializeBlockVolatile(this XsILGenerator generator, byte value, uint bytes)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .InitializeBlock(value, bytes);
        }

        /// <summary>
        /// Pops an integer off the evaluation stack and creates an array of the given type with that length, pushing the reference onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the array</param>
        
        public static XsILGenerator NewArray(this XsILGenerator generator, Type type) => generator.FluentEmit(OpCodes.Newarr);

        /// <summary>
        /// Pops an integer off the evaluation stack and creates an array of the given type with that length, pushing the reference onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type of the array</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator NewArray<T>(this XsILGenerator generator) => generator.NewArray(typeof (T));

        /// <summary>
        /// Creates an array of the given type with the given length, pushing the reference onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type of the array</param>
        /// <param name="length">The length of the array</param>
        
        public static XsILGenerator NewArray(this XsILGenerator generator, Type type, uint length)
        {
            return generator.LoadConstant(length)
                            .NewArray(type);
        }

        /// <summary>
        /// Creates an array of the given type with the given length, pushing the reference onto the evaluation stack
        /// </summary>
        /// <typeparam name="T">The type of the array</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="length">The length of the array</param>
        
        public static XsILGenerator NewArray<T>(this XsILGenerator generator, uint length) => generator.NewArray(typeof(T), length);

        /// <summary>
        /// Pops an unsigned native integer from the evaluation stack, and allocates that number of bytes from the local dynamic memory pool, pushing the address onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator AllocateFromLocalMemoryPool(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Localloc);

        /// <summary>
        /// Allocates the given number of bytes from the local dynamic memory pool, pushing the address onto the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="size"></param>
        
        public static XsILGenerator AllocateFromLocalMemoryPool(this XsILGenerator generator, uint size)
        {
            return generator.LoadConstant(size)
                            .ConvertToUnsignedNativeInteger()
                            .AllocateFromLocalMemoryPool();
        }
    }
}
