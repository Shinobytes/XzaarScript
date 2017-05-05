using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    [Serializable]
    public class UnaryExpression : XzaarExpression
    {
        private readonly XzaarExpression item;
        private XzaarExpressionType nodeType;
        private XzaarType type;

        internal UnaryExpression(XzaarExpression item, XzaarExpressionType nodeType)
        {
            this.item = item;
            this.nodeType = nodeType;
        }

        public XzaarExpression Item
        {
            get { return item; }
        }

        public override XzaarExpressionType NodeType
        {
            get { return nodeType; }
        }

        public override XzaarType Type
        {
            get { return XzaarBaseTypes.Number; }
        }
    }

    public partial class XzaarExpression
    {
        public static UnaryExpression UnaryOperation(XzaarExpression item, XzaarExpressionType type)
        {
            return new UnaryExpression(item, type);
        }

        public static UnaryExpression PostIncrementor(XzaarExpression item)
        {
            return UnaryOperation(item, XzaarExpressionType.PostIncrementAssign);
        }

        public static UnaryExpression Incrementor(XzaarExpression item)
        {
            return UnaryOperation(item, XzaarExpressionType.Increment);
        }

        public static UnaryExpression PostDecrementor(XzaarExpression item)
        {
            return UnaryOperation(item, XzaarExpressionType.PostDecrementAssign);
        }

        public static UnaryExpression Decrementor(XzaarExpression item)
        {
            return UnaryOperation(item, XzaarExpressionType.Decrement);
        }
    }
}