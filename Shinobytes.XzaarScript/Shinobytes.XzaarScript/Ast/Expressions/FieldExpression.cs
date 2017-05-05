using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
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

        public string Name => name;

        public XzaarType FieldType => type;

        public override ExpressionType NodeType => ExpressionType.Field;

        public override XzaarType Type => this.type;
    }


    public partial class XzaarExpression
    {
        public static FieldExpression Field(XzaarType type, string name, XzaarType declaringType)
        {
            return new FieldExpression(type, name, declaringType);
        }
    }
}