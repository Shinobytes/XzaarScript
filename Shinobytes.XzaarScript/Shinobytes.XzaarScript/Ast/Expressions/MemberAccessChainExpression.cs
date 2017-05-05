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

        public override XzaarType Type => ResultType;

        public sealed override ExpressionType NodeType => ExpressionType.MemberAccess;
    }

    public partial class XzaarExpression
    {
        public static MemberAccessChainExpression AccessChain(XzaarExpression left, XzaarExpression right)
        {
            return new MemberAccessChainExpression(left, right);
        }
    }
}