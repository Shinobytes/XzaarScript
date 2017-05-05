namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class LoopNode : XzaarAstNode
    {
        private readonly XzaarAstNode body;

        public LoopNode(string type, XzaarAstNode body, int nodeIndex)
            : base(XzaarAstNodeTypes.LOOP, type, null, nodeIndex)
        {
            this.body = body;
        }

        public XzaarAstNode Body { get { return body; } }

        public override void Accept(IXzaarNodeVisitor visitor)
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