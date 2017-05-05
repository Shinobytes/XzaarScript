namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal sealed class CoalesceConversionBinaryExpression : BinaryExpression
    {
        internal CoalesceConversionBinaryExpression(XzaarExpression left, XzaarExpression right)
            : base(left, right)
        {
        }

        public sealed override ExpressionType NodeType => ExpressionType.Coalesce;

        public sealed override XzaarType Type => Right.Type;
    }
}