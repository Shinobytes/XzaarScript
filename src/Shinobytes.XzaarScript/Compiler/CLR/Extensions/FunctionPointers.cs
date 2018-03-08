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

using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods that create function pointers
    /// </summary>
    public static class FunctionPointers
    {
        /// <summary>
        /// Pushes an unmanaged pointer (type native int) to the native code implementing the given method onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to load the pointer for</param>
        
        public static XsILGenerator LoadFunctionPointer(this XsILGenerator generator, MethodInfo method)
            => generator.FluentEmit(OpCodes.Ldftn, method);

        /// <summary>
        /// Pops a reference off the evaluation stack, and pushes an unmanaged pointer (type native int) to the native code implementing the given virtual method for that object onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="method">The method to load the pointer for</param>
        
        public static XsILGenerator LoadVirtualFunctionPointer(this XsILGenerator generator, MethodInfo method)
            => generator.FluentEmit(OpCodes.Ldvirtftn, method);
    }
}
