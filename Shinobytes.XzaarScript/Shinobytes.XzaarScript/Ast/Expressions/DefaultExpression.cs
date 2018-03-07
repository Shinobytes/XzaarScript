namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public sealed class ErrorExpression : XzaarExpression
    {
        public string ErrorMessage { get; set; }

        public ErrorExpression(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    public sealed class DefaultExpression : XzaarExpression
    {
        private XzaarType type;

        internal DefaultExpression(XzaarType type)
        {
            this.type = type;
        }

        public sealed override XzaarType Type => type;
        public sealed override ExpressionType NodeType => ExpressionType.Default;
    }

    public partial class XzaarExpression
    {
        public static DefaultExpression Empty()
        {
            return new DefaultExpression(XzaarBaseTypes.Void);
        }

        public static DefaultExpression Default(XzaarType type)
        {
            if (type == XzaarBaseTypes.Void)
            {
                return Empty();
            }
            return new DefaultExpression(type);
        }

        public static ErrorExpression Error()
        {
            return new ErrorExpression(null);
        }

        public static ErrorExpression Error(string errorMessage)
        {
            return new ErrorExpression(errorMessage);
        }

    }
}