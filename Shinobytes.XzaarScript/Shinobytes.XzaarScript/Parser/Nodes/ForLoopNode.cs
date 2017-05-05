namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ForLoopNode : LoopNode
    {
        private readonly AstNode initiator;
        private readonly AstNode test;
        private readonly AstNode incrementor;

        public ForLoopNode(AstNode initiator, AstNode test, AstNode incrementor, AstNode body, int nodeIndex)
            : base("FOR", body, nodeIndex)
        {
            this.initiator = initiator;
            this.test = test;
            this.incrementor = incrementor;
        }

        public AstNode Initiator => initiator;

        public AstNode Incrementor => incrementor;

        public AstNode Test => test;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "for (" + this.initiator + "; " + this.test + "; " + this.incrementor + ") { " + this.Body + " }";
        }
    }
}