namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal sealed class CoalesceConversionBinaryExpression : BinaryExpression
    {
        internal CoalesceConversionBinaryExpression(XzaarExpression left, XzaarExpression right)
            : base(left, right)
        {
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Coalesce; }
        }

        public sealed override XzaarType Type
        {
            get { return Right.Type; }
        }
    }
}