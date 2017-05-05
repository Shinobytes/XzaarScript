using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsField : XsTypedObject
    {
        private TypeBuilder declaringType;
        private FieldBuilder builder;

        internal XsField(TypeBuilder type, string name, Type fieldType) : base(name, fieldType)
        {
            this.declaringType = type;
            this.builder = declaringType.DefineField(name, fieldType, FieldAttributes.Public);
        }

        public FieldInfo FIeldInfo => builder;
    }
}