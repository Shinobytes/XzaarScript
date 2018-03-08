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

namespace Shinobytes.XzaarScript.Compiler.Extensions.ExceptionBlock
{
    /// <summary>
    /// Represents a catch block in a protected region
    /// </summary>
    public sealed class CatchBlock : IDisposable
    {
        private readonly XsILGenerator generator;
        private readonly Label endLabel;

        internal CatchBlock(XsILGenerator generator, Label endLabel)
        {
            this.generator = generator;
            this.endLabel = endLabel;
        }

        /// <summary>
        /// Rethrows the exception caught by this catch block
        /// </summary>
        
        public XsILGenerator Rethrow() => generator.FluentEmit(OpCodes.Rethrow);

        /// <summary>
        /// Jumps to the instruction immediately after this protected region (after any finally block executes)
        /// </summary>
        
        public XsILGenerator Leave() => generator.Leave(endLabel);
        
        /// <summary>
        /// End the catch block
        /// </summary>
        public void Dispose()
        {
        }
    }
}