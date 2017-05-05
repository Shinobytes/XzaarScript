namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class SwitchExpression : XzaarExpression
    {
        private readonly XzaarExpression value;
        private readonly XzaarExpression[] caseExpressions;

        internal SwitchExpression(XzaarExpression value, XzaarExpression[] caseExpressions)
        {
            this.value = value;
            this.caseExpressions = caseExpressions;
        }

        public XzaarExpression Value => value;
        public XzaarExpression[] Cases => caseExpressions;

        public override ExpressionType NodeType => ExpressionType.Switch;
    }

    public class SwitchCaseExpression : XzaarExpression
    {
        private readonly XzaarExpression matchExpression;
        private readonly XzaarExpression body;

        internal SwitchCaseExpression(XzaarExpression matchExpression, XzaarExpression body)
        {
            this.matchExpression = matchExpression;
            this.body = body;
        }
        public bool IsDefaultCase => matchExpression == null;
        public XzaarExpression Match => matchExpression;
        public XzaarExpression Body => body;

        public override ExpressionType NodeType => ExpressionType.SwitchCase;
    }

    public partial class XzaarExpression
    {
        public static SwitchCaseExpression Case(XzaarExpression test, XzaarExpression body)
        {
            return new SwitchCaseExpression(test, body);
        }

        public static SwitchCaseExpression DefaultCase(XzaarExpression body)
        {
            return new SwitchCaseExpression(null, body);
        }

        public static SwitchExpression Switch(XzaarExpression valueExpression, params XzaarExpression[] caseExpressions)
        {
            return new SwitchExpression(valueExpression, caseExpressions);
        }
    }
}