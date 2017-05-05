using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    [Serializable]
    public class StructExpression : XzaarExpression
    {
        private readonly string name;
        private readonly XzaarExpression[] fields;

        internal StructExpression(string name, XzaarExpression[] fields)
        {
            this.name = name;
            this.fields = fields;
        }

        public string Name { get { return name; } }
        public XzaarExpression[] Fields { get { return fields; } }


        public override XzaarExpressionType NodeType { get { return XzaarExpressionType.Struct; } }

    }

    public partial class XzaarExpression
    {
        public static StructExpression Struct(string name, params XzaarExpression[] fields)
        {
            return new StructExpression(name, fields);
        }
    }
}