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
