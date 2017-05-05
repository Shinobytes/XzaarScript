namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public class MemberAccessChainExpression : XzaarExpression
    {
        internal MemberAccessChainExpression(XzaarExpression left, XzaarExpression right)
        {
            Left = left;
            Right = right;
        }

        public XzaarExpression Left { get; }

        public XzaarExpression Right { get; }

        public XzaarType ResultType
        {
            get
            {
                var ma = Right as MemberExpression;
                if (ma != null)
                {
                    return ma.MemberType;
                }
                return null;
            }
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.MemberAccess; }
        }
    }

    public partial class XzaarExpression
    {
        public static MemberAccessChainExpression AccessChain(XzaarExpression left, XzaarExpression right)
        {
            return new MemberAccessChainExpression(left, right);
        }
    }
}