namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal sealed class LogicalBinaryExpression : BinaryExpression
    {
        private readonly XzaarExpressionType nodeType;

        internal LogicalBinaryExpression(XzaarExpressionType nodeType, XzaarExpression left, XzaarExpression right)
            : base(left, right)
        {
            this.nodeType = nodeType;
        }

        public sealed override XzaarType Type
        {
            get { return XzaarBaseTypes.Boolean; }
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return nodeType; }
        }
    }
}