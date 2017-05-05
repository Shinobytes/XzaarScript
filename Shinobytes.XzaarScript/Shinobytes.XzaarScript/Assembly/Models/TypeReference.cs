using System;

namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class TypeReference : MemberReference
    {
        public TypeVariableCollection Fields { get; internal set; }

        public MethodCollection Methods { get; internal set; }

        public TypeReference(XzaarType type)
        {
            this.Fields = new TypeVariableCollection();
            this.Methods = new MethodCollection();

            if ((object)type == null) throw new ArgumentNullException(nameof(type));
            if ((object)type.BaseType != null)
                this.BaseType = new TypeReference(type.BaseType);
            base.Name = type.Name;
            this.IsArray = type.IsArray;
            if (this.IsArray)
                this.ArrayElementType = type.GetElementType();
        }

        public TypeReference(string name, TypeReference baseType)
        {
            this.Fields = new TypeVariableCollection();
            this.Methods = new MethodCollection();
            base.Name = name;
            this.BaseType = baseType;
        }

        public override MemberTypes MemberType
        {
            get { return IsClass ? MemberTypes.Class : MemberTypes.Struct; }
        }

        public XzaarType ArrayElementType { get; internal set; }

        public bool IsClass { get; internal set; }

        public bool IsStruct { get { return MemberType == MemberTypes.Struct; } }

        public TypeReference BaseType { get; internal set; }

        public bool IsArray { get; internal set; }
    }
}