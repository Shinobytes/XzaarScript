namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    internal class SimpleBinaryExpression : BinaryExpression
    {
        private readonly XzaarExpressionType _nodeType;
        private readonly XzaarType _type;

        internal SimpleBinaryExpression(XzaarExpressionType nodeType, XzaarExpression left, XzaarExpression right, XzaarType type)
            : base(left, right)
        {
            _nodeType = nodeType;
            _type = type;
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return _nodeType; }
        }

        public sealed override XzaarType Type
        {
            get { return _type; }
        }
    }
}