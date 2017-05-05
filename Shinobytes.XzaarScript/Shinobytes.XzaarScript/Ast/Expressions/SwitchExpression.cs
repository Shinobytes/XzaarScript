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

        public XzaarExpression Value { get { return value; } }
        public XzaarExpression[] Cases { get { return caseExpressions; } }

        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Switch; }
        }
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
        public bool IsDefaultCase { get { return matchExpression == null; } }
        public XzaarExpression Match { get { return matchExpression; } }
        public XzaarExpression Body { get { return body; } }

        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.SwitchCase; }
        }
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