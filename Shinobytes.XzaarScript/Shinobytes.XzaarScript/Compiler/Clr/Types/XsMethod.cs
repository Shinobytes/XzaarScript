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