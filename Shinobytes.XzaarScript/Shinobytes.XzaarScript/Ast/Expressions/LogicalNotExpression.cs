using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    [Serializable]
    public class LogicalNotExpression : XzaarExpression
    {
        private readonly XzaarExpression expression;

        internal LogicalNotExpression(XzaarExpression expression)
        {
            this.expression = expression;
        }

        public override ExpressionType NodeType => ExpressionType.Not;

        public XzaarExpression Expression => expression;

        public override XzaarType Type => XzaarBaseTypes.Boolean;
    }

    [Serializable]
    public class NegateExpression : XzaarExpression
    {
        private readonly XzaarExpression expression;

        internal NegateExpression(XzaarExpression expression)
        {
            this.expression = expression;
        }

        public override ExpressionType NodeType => ExpressionType.Negate;

        public XzaarExpression Expression => expression;

        public override XzaarType Type => XzaarBaseTypes.Number;
    }

    public partial class XzaarExpression
    {
        public static NegateExpression Negate(XzaarExpression expr)
        {
            return new NegateExpression(expr);
        }

        public static LogicalNotExpression LogicalNot(XzaarExpression expr)
        {
            return new LogicalNotExpression(expr);
        }
    }
}