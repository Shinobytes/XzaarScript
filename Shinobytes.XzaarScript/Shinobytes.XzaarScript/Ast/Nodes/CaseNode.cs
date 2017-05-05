namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class CaseNode : ControlFlowNode
    {
        private readonly XzaarAstNode test;
        private readonly XzaarAstNode body;

        public CaseNode(XzaarAstNode test, XzaarAstNode body, int nodeIndex)
            : base(XzaarAstNodeTypes.CASE, "CASE", nodeIndex)
        {
            this.test = test;
            this.body = body;
        }

        public bool IsDefaultCase
        {
            get { return test == null; }
        }

        public XzaarAstNode Body { get { return body; } }

        public XzaarAstNode Test { get { return test; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (test == null)
                return "default: " + body;
            return "case " + test + ": " + body;
        }
    }
}