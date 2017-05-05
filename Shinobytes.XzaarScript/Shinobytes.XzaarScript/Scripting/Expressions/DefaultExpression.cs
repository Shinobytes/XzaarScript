namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public sealed class DefaultExpression : XzaarExpression
    {
        private XzaarType type;

        internal DefaultExpression(XzaarType type)
        {
            this.type = type;
        }

        public sealed override XzaarType Type
        {
            get { return type; ; }
        }
        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Default; }
        }
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

    }
}