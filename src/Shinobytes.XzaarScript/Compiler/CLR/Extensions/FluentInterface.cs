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
using System.Collections.Generic;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods that support fluent use of the XsILGenerator API
    /// </summary>
    


    public static partial class FluentInterface
    {
        private static readonly ConditionalWeakTable<XsILGenerator, GeneratorData> GeneratorExtraData = new ConditionalWeakTable<XsILGenerator, GeneratorData>();

        /// <summary>
        /// Mark the fluently-specified label
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="labelName">The name of the fluently-specified label</param>
        
        public static XsILGenerator MarkLabel(this XsILGenerator generator, string labelName)
        {
            var data = GeneratorExtraData.GetOrCreateValue(generator);
            Label label;
            if (!data.Labels.TryGetValue(labelName, out label))
            {
                throw new InvalidOperationException("No label with the name `" + labelName + "` declared");
            }

            generator.MarkLabel(label);
            data.Labels.Remove(labelName);
            
            return generator;
        }

        internal static Label GetOrCreateLabel(this XsILGenerator generator, string labelName)
        {
            var data = GeneratorExtraData.GetOrCreateValue(generator);

            Label label;
            if (data.Labels.TryGetValue(labelName, out label))
            {
                return label;
            }
            else
            {
                label = generator.DefineLabel();
                data.Labels.Add(labelName, label);
                return label;
            }
        }

        private static XsILGenerator CreateLocal(this XsILGenerator generator, string localName, Type localType, bool pinned)
        {
            var data = GeneratorExtraData.GetOrCreateValue(generator);

            if (data.Locals.ContainsKey(localName))
            {
                throw new InvalidOperationException("Local with the name `" + localName + "` already declared");
            }

            var local = generator.DeclareLocal(localType, pinned);
            data.Locals.Add(localName, local);

            return generator;
        }

        /// <summary>
        /// Fluently specify a new local with the given name and type
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        /// <param name="localType">The type of the fluently-specified local</param>
        
        public static XsILGenerator CreateLocal(this XsILGenerator generator, string localName, Type localType)
            => generator.CreateLocal(localName, localType, false);

        /// <summary>
        /// Fluently specify a new local with the given name and type
        /// </summary>
        /// <typeparam name="T">The type of the fluently-specified local</typeparam>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        
        public static XsILGenerator CreateLocal<T>(this XsILGenerator generator, string localName)
            => generator.CreateLocal(localName, typeof (T));

        /// <summary>
        /// Fluently specify a new local with the given name and type whose contents are pinned in memory
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        /// <param name="localType">The type of the fluently-specified local</param>
        
        public static XsILGenerator CreatePinnedLocal(this XsILGenerator generator, string localName, Type localType)
            => generator.CreateLocal(localName, localType, true);

        /// <summary>
        /// Fluently specify a new local with the given name and type whose contents are pinned in memory
        /// </summary>
        /// <typeparam name="T">The type of the fluently-specified local</typeparam>
        /// <param name="generator"></param>
        /// <param name="localName">The name of the fluently-specified local</param>
        
        public static XsILGenerator CreatePinnedLocal<T>(this XsILGenerator generator, string localName)
            => generator.CreateLocal(localName, typeof(T), true);

        internal static LocalBuilder GetLocal(this XsILGenerator generator, string localName)
        {
            LocalBuilder local;
            if (!GeneratorExtraData.GetOrCreateValue(generator).Locals.TryGetValue(localName, out local))
            {
                throw new InvalidOperationException("No local with the name `" + localName + "` declared");
            }

            return local;
        }

        private sealed class GeneratorData
        {
            public IDictionary<string, Label> Labels { get; }  = new Dictionary<string, Label>();

            public IDictionary<string, LocalBuilder> Locals { get; } = new Dictionary<string, LocalBuilder>();
        }
    }
}
