using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    [Serializable]
    public class NegateExpression : XzaarExpression
    {
        private readonly XzaarExpression expression;

        internal NegateExpression(XzaarExpression expression)
        {
            this.expression = expression;
        }

        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Not; }
        }

        public XzaarExpression Expression
        {
            get { return expression; }
        }

        public override XzaarType Type
        {
            get { return XzaarBaseTypes.Boolean; }
        }
    }

    public partial class XzaarExpression
    {
        public static NegateExpression Negate(XzaarExpression expr)
        {
            return new NegateExpression(expr);
        }
    }
}