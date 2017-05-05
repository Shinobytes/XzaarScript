namespace Shinobytes.XzaarScript.Ast.Expressions
{
    internal class MethodBinaryExpression : SimpleBinaryExpression
    {
        private readonly XzaarMethodInfo _method;

        internal MethodBinaryExpression(XzaarExpressionType nodeType, XzaarExpression left, XzaarExpression right, XzaarType type, XzaarMethodInfo method)
            : base(nodeType, left, right, type)
        {
            _method = method;
        }

        internal override XzaarMethodInfo GetMethod()
        {
            return _method;
        }

    }
}