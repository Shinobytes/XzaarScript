namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class MatchNode : XzaarNode
    {
        private readonly XzaarNode valueExpression;
        private readonly CaseNode[] cases;

        public MatchNode(XzaarNode valueExpression, CaseNode[] cases, int nodeIndex) : base(XzaarNodeTypes.MATCH, "MATCH", null, nodeIndex)
        {
            this.valueExpression = valueExpression;
            this.cases = cases;
        }

        public XzaarNode ValueExpression { get { return valueExpression; } }

        public CaseNode[] Cases { get { return cases; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}