namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class LoopNode : AstNode
    {
        private readonly AstNode body;

        public LoopNode(string type, AstNode body, int nodeIndex)
            : base(NodeTypes.LOOP, type, null, nodeIndex)
        {
            this.body = body;
        }

        public AstNode Body => body;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return body == null || Body.IsEmpty();
        }

        public override string ToString()
        {
            return "loop { " + this.Body + " }";
        }
    }
}