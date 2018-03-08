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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsMethod : XsObject
    {
        private readonly List<XsVariable> variables = new List<XsVariable>();
        private readonly TypeBuilder declaringTypeBuilder;
        private readonly MethodAttributes attributes;
        private readonly XsParameter[] parameters;
        private readonly Type returnType;
        private readonly MethodBuilder builder;
        private XsILGenerator il;
        private XsStruct declaringType;

        public XsMethod(XsStruct declaringType, string name, MethodAttributes attributes, Type returnType, XsParameter[] parameters) : base(name)
        {
            this.declaringType = declaringType;
            this.declaringTypeBuilder = declaringType.TypeBuilder;

            this.attributes = attributes;
            this.parameters = parameters;
            this.returnType = returnType;

            this.builder = declaringTypeBuilder.DefineMethod(name, attributes, returnType, parameters.Select(i => i.Type).ToArray());
            this.il = new XsILGenerator(this.builder.GetILGenerator());
        }

        public XsILGenerator GetILGenerator() => il;

        public XsVariable[] Variables => variables.ToArray();

        public XsParameter[] Parameters => parameters;

        public MethodInfo MethodInfo => builder;

        public string FullName => $"{declaringType.FullName}.{Name}";

        public XsVariable DefineVariable(Type type)
        {
            var variable = new XsVariable(this.il, null, type);
            this.variables.Add(variable);
            return variable;
        }

        public XsVariable DefineVariable(string name, Type type)
        {
            var variable = new XsVariable(this.il, name, type);
            this.variables.Add(variable);
            return variable;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(XsMethod other)
        {
            if (other == null)
                return false;
            return other.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + declaringType.GetHashCode();
                hash = hash * 23 + parameters.Length.GetHashCode();
                return hash;
            }
        }
    }
}