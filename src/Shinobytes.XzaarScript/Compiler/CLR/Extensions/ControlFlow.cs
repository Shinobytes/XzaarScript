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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for controlling the flow of the method
    /// </summary>
    public static class ControlFlow
    {
        /// <summary>
        /// Branch unconditionally to the given label
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchTo(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Br, label);

        /// <summary>
        /// Branch unconditionally to the given label
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchTo(this XsILGenerator generator, string labelName)
            => generator.BranchTo(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Branch unconditionally to the given label
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator BranchToShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Br_S, label);

        /// <summary>
        /// Branch unconditionally to the given label
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator BranchToShortForm(this XsILGenerator generator, string labelName)
            => generator.BranchToShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the corresponding zero-indexed label in the provided list, continuing to the next instruction if the value is outside the valid range
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labels">The labels to form a jump table from</param>
        
        public static XsILGenerator Switch(this XsILGenerator generator, params Label[] labels) => generator.FluentEmit(OpCodes.Switch, labels);

        /// <summary>
        /// Pops an integer value from the evaluation stack and branches to the corresponding zero-indexed label in the provided list, continuing to the next instruction if the value is outside the valid range
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelNames">The names of the fluently-specified labels to form a jump table from</param>
        
        public static XsILGenerator Switch(this XsILGenerator generator, params string[] labelNames)
            => generator.Switch(labelNames.Select(generator.GetOrCreateLabel).ToArray());

        /// <summary>
        /// Branch to the given label, clearing the evaluation stack; can be used to leave a protected region
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator Leave(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Leave, label);

        /// <summary>
        /// Branch to the given label, clearing the evaluation stack; can be used to leave a protected region
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator Leave(this XsILGenerator generator, string labelName)
            => generator.Leave(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Branch to the given label, clearing the evaluation stack; can be used to leave a protected region
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="label">The label to branch to</param>
        
        public static XsILGenerator LeaveShortForm(this XsILGenerator generator, Label label) => generator.FluentEmit(OpCodes.Leave_S, label);

        /// <summary>
        /// Branch to the given label, clearing the evaluation stack; can be used to leave a protected region
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator LeaveShortForm(this XsILGenerator generator, string labelName)
            => generator.LeaveShortForm(generator.GetOrCreateLabel(labelName));

        /// <summary>
        /// Pops a reference to an exception off the evaluation stack and throws it
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Throw(this XsILGenerator generator) => generator.FluentEmit(OpCodes.Throw);

        /// <summary>
        /// Throws an exception of the given type, using the default constructor
        /// </summary>
        /// <typeparam name="T">The type of the exception to throw</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        
        public static XsILGenerator Throw<T>(this XsILGenerator generator) where T : Exception, new()
        {
            generator.ThrowException(typeof (T));
            return generator;
        }

        
        private static readonly Type[] StringTypeArray = { typeof (string) };

        /// <summary>
        /// Throws an exception of the given type with the given message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="message">The message to give the exception</param>
        /// <exception cref="InvalidOperationException">Exception type <typeparamref name="T"/> does not have a public constructor taking only a string</exception>
        
        public static XsILGenerator Throw<T>(this XsILGenerator generator, string message) where T : Exception
        {
            var constructor = typeof (T).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, StringTypeArray, null);

            if (constructor == null)
            {
                throw new InvalidOperationException("Exception type " + typeof (T).Name + " does not have a public constructor taking only a string");
            }

            return generator.LoadString(message)
                            .NewObject(constructor)
                            .Throw();
        }
    }
}
