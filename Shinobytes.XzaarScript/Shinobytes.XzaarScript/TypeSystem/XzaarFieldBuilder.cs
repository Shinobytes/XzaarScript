using System;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public class XzaarFieldBuilder : XzaarFieldInfo
    {
        private string name;
        private XzaarType declaringType;
        private XzaarType fieldType;

        public XzaarFieldBuilder(string name, XzaarType fieldType, XzaarType declaringType)
        {
            this.name = name;
            this.fieldType = fieldType;
            this.declaringType = declaringType;
        }

        public override string Name
        {
            get { return name; }
        }

        public override XzaarType GetXzaarType()
        {
            throw new NotImplementedException();
        }

        public override XzaarMemberTypes MemberType
        {
            get { return XzaarMemberTypes.Field; }
        }

        public override XzaarType DeclaringType
        {
            get { return declaringType; }
        }

        public override XzaarType FieldType
        {
            get { return fieldType; }
        }

        public override object GetValue(object obj)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object obj, object value)
        {
            throw new NotImplementedException();
        }
    }
}