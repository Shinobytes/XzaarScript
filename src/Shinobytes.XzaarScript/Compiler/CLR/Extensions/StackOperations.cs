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
 
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for performing stack manipulation
    /// </summary>
    
    public static class StackOperations
    {
        /// <summary>
        /// Pops the top value off the evaluation stack and discards it
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Pop(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Pop);

        /// <summary>
        /// Pops <paramref name="n"/> values off the evaluation stack and discards them
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="n">The number of evaluation stack values to discard</param>
        
        public static XsILGenerator Pop(this XsILGenerator generator, uint n)
        {
            for (int i = 0; i < n; i++)
            {
                generator.FluentEmit(OpCodes.Pop);
            }

            return generator;
        }

        /// <summary>
        /// Duplicates the value on the top of the evaluation stack
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Duplicate(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Dup);

        /// <summary>
        /// Duplicates the value on the top of the evaluation stack <paramref name="n"/> times
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="n">The number of times to duplicate the value</param>
        
        public static XsILGenerator Duplicate(this XsILGenerator generator, uint n)
        {
            for (int i = 0; i < n; i++)
            {
                generator.FluentEmit(OpCodes.Dup);
            }

            return generator;
        }
    }
}
