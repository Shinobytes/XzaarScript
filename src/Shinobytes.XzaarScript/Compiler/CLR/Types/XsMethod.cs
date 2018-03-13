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