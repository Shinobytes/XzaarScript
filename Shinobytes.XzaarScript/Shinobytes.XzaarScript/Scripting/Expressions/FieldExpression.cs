using System;

namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    [Serializable]
    public class FieldExpression : XzaarExpression
    {
        private string name;
        private XzaarType type;

        internal FieldExpression(XzaarType type, string name, XzaarType declaringType)
        {
            DeclaringType = declaringType;
            this.type = type;
            this.name = name;
        }

        public XzaarType DeclaringType { get; }

        public string Name { get { return name; } }

        public XzaarType FieldType { get { return type; } }

        public override XzaarExpressionType NodeType { get { return XzaarExpressionType.Field; } }

        public override XzaarType Type { get { return this.type; } }
    }


    public partial class XzaarExpression
    {
        public static FieldExpression Field(XzaarType type, string name, XzaarType declaringType)
        {
            return new FieldExpression(type, name, declaringType);
        }
    }
}