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
            get { return XzaarExpressionType.Negate; }
        }

        public XzaarExpression Expression
        {
            get { return expression; }
        }

        public override XzaarType Type
        {
            get { return XzaarBaseTypes.Number; }
        }
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