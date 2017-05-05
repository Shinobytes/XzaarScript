namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ForLoopNode : LoopNode
    {
        private readonly XzaarAstNode initiator;
        private readonly XzaarAstNode test;
        private readonly XzaarAstNode incrementor;

        public ForLoopNode(XzaarAstNode initiator, XzaarAstNode test, XzaarAstNode incrementor, XzaarAstNode body, int nodeIndex)
            : base("FOR", body, nodeIndex)
        {
            this.initiator = initiator;
            this.test = test;
            this.incrementor = incrementor;
        }

        public XzaarAstNode Initiator { get { return initiator; } }

        public XzaarAstNode Incrementor { get { return incrementor; } }

        public XzaarAstNode Test { get { return test; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "for (" + this.initiator + "; " + this.test + "; " + this.incrementor + ") { " + this.Body + " }";
        }
    }
}