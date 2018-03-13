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
 
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
	public static partial class FluentInterface
	{

#pragma warning disable 1734

        /// <summary>Puts the specified instruction onto the stream of instructions.</summary>
        /// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode)
        {
            generator.Emit(opcode);
            return generator;
        }

        /// <summary>Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
        /// <param name="arg">The character argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Byte arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
        /// <param name="arg">The character argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.SByte arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="arg">The Int argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Int16 arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
        /// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Int32 arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="meth">A MethodInfo representing a method. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="meth" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// <paramref name="meth" /> is a generic method for which the <see cref="P:System.Reflection.MethodInfo.IsGenericMethodDefinition" /> property is false.</exception>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.MethodInfo meth)
        {
            generator.Emit(opcode, meth);
            return generator;
        }

        /// <summary>Puts the specified instruction and a signature token onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="signature">A helper for constructing a signature token. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="signature" /> is null. </exception>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.SignatureHelper signature)
        {
            generator.Emit(opcode, signature);
            return generator;
        }

        /// <summary>Puts the specified instruction and metadata token for the specified constructor onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="con">A ConstructorInfo representing a constructor. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="con" /> is null. This exception is new in the .NET Framework 4.</exception>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.ConstructorInfo con)
        {
            generator.Emit(opcode, con);
            return generator;
        }

        /// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given type.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
        /// <param name="cls">A Type. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="cls" /> is null. </exception>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Type cls)
        {
            generator.Emit(opcode, cls);
            return generator;
        }

        /// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
        /// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Int64 arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
        /// <param name="arg">The Single argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Single arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream. Defined in the OpCodes enumeration. </param>
        /// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Double arg)
        {
            generator.Emit(opcode, arg);
            return generator;
        }

        /// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="label">The label to which to branch from this location. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.Label label)
        {
            generator.Emit(opcode, label);
            return generator;
        }

        /// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="labels">The array of label objects to which to branch from this location. All of the labels will be used. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="con" /> is null. This exception is new in the .NET Framework 4.</exception>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.Label[] labels)
        {
            generator.Emit(opcode, labels);
            return generator;
        }

        /// <summary>Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="field">A FieldInfo representing a field. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.FieldInfo field)
        {
            generator.Emit(opcode, field);
            return generator;
        }

        /// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given string.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="str">The String to be emitted. </param>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.String str)
        {
            generator.Emit(opcode, str);
            return generator;
        }

        /// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index of the given local variable.</summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
        /// <param name="local">A local variable. </param>
        /// <exception cref="T:System.ArgumentException">The parent method of the <paramref name="local" /> parameter does not match the method associated with this <see cref="T:System.Reflection.Emit.XsILGenerator" />. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="local" /> is null. </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="opcode" /> is a single-byte instruction, and <paramref name="local" /> represents a local variable with an index greater than Byte.MaxValue. </exception>
		/// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
		public static XsILGenerator FluentEmit(this XsILGenerator generator, System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.LocalBuilder local)
        {
            generator.Emit(opcode, local);
            return generator;
        }


#pragma warning restore 1734

    }
}