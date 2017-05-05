namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal sealed class LogicalBinaryExpression : BinaryExpression
    {
        private readonly ExpressionType nodeType;

        internal LogicalBinaryExpression(ExpressionType nodeType, XzaarExpression left, XzaarExpression right)
            : base(left, right)
        {
            this.nodeType = nodeType;
        }

        public sealed override XzaarType Type => XzaarBaseTypes.Boolean;

        public sealed override ExpressionType NodeType => nodeType;
    }
}