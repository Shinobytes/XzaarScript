namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal class SimpleBinaryExpression : BinaryExpression
    {
        private readonly ExpressionType _nodeType;
        private readonly XzaarType _type;

        internal SimpleBinaryExpression(ExpressionType nodeType, XzaarExpression left, XzaarExpression right, XzaarType type)
            : base(left, right)
        {
            _nodeType = nodeType;
            _type = type;
        }

        public sealed override ExpressionType NodeType => _nodeType;

        public sealed override XzaarType Type => _type;
    }
}