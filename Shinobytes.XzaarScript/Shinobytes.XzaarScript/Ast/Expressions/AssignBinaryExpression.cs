namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal sealed class AssignBinaryExpression : BinaryExpression
    {
        internal AssignBinaryExpression(XzaarExpression left, XzaarExpression right)
            : base(left, right)
        {
        }

        public sealed override XzaarType Type => Left.Type;

        public sealed override ExpressionType NodeType => ExpressionType.Assign;
    }
}