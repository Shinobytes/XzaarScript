using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    [Serializable]
    public class UnaryExpression : XzaarExpression
    {
        private readonly XzaarExpression item;
        private ExpressionType nodeType;
        private XzaarType type;

        internal UnaryExpression(XzaarExpression item, ExpressionType nodeType)
        {
            this.item = item;
            this.nodeType = nodeType;
        }

        public XzaarExpression Item => item;

        public override ExpressionType NodeType => nodeType;

        public override XzaarType Type => XzaarBaseTypes.Number;
    }

    public partial class XzaarExpression
    {
        public static UnaryExpression UnaryOperation(XzaarExpression item, ExpressionType type)
        {
            return new UnaryExpression(item, type);
        }

        public static UnaryExpression PostIncrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.PostIncrementAssign);
        }

        public static UnaryExpression Incrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.Increment);
        }

        public static UnaryExpression PostDecrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.PostDecrementAssign);
        }

        public static UnaryExpression Decrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.Decrement);
        }
    }
}