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
    /// Contains extension methods for operations useful to debuggers
    /// </summary>

    public static class DebugOperations
    {
        /// <summary>
        /// Performs no operation
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator NoOp(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Nop);

        public static XsILGenerator Nop(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Nop);

        /// <summary>
        /// Performs no operation
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator NoOperation(this XsILGenerator generator) => generator.NoOp();

        /// <summary>
        /// Signals an attached debugger to break execution
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator Break(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Break);

        /// <summary>
        /// Signals an attached debugger to break execution
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>

        public static XsILGenerator BreakInDebugger(this XsILGenerator generator) => generator.Break();
    }
}
