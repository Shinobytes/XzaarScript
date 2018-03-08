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
    /// Contains extension methods that emit instruction prefixes
    /// </summary>
    
    public static class Prefixes
    {
        /// <summary>
        /// Indicates that the next operation should not assume that its arguments are properly aligned
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Unaligned(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Unaligned);
    }
}
