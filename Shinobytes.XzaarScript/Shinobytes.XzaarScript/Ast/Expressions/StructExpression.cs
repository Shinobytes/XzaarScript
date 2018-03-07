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

        public string Name => name;
        public XzaarExpression[] Fields => fields;


        public override ExpressionType NodeType => ExpressionType.Struct;
    }

    public partial class XzaarExpression
    {
        public static StructExpression Struct(string name, params XzaarExpression[] fields)
        {
            return new StructExpression(name, fields);
        }
    }
}