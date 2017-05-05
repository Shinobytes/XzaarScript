namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    internal sealed class AssignBinaryExpression : BinaryExpression
    {
        internal AssignBinaryExpression(XzaarExpression left, XzaarExpression right)
            : base(left, right)
        {
        }

        public sealed override XzaarType Type
        {
            get { return Left.Type; }
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Assign; }
        }
    }
}