namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class CaseNode : ControlFlowNode
    {
        private readonly XzaarNode test;
        private readonly XzaarNode body;

        public CaseNode(XzaarNode test, XzaarNode body, int nodeIndex)
            : base(XzaarNodeTypes.CASE, "CASE", nodeIndex)
        {
            this.test = test;
            this.body = body;
        }

        public bool IsDefaultCase
        {
            get { return test == null; }
        }

        public XzaarNode Body { get { return body; } }

        public XzaarNode Test { get { return test; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}