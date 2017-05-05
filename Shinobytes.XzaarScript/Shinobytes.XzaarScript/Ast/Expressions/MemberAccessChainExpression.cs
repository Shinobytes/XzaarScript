namespace Shinobytes.XzaarScript.Ast.Expressions
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
                var fc = Right as FunctionCallExpression;
                if (fc != null)
                {
                    return fc.Type;
                }
                var ma = Right as MemberExpression;
                if (ma != null)
                {
                    return ma.MemberType;
                }
                return null;
            }
        }

        public override XzaarType Type { get { return ResultType;} }

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