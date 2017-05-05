namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class EntryNode : AstNode
    {
        public EntryNode(AstNode body)
            : base(NodeTypes.PROGRAM, "PROGRAM", "PROGRAM", -1)
        {
            if (body != null)
                this.AddChild(body);
        }

        public AstNode Body => Children.Count > 0 ? this[0] : null;

        public override string ToString()
        {
            return "" + Body;
        }

        public override bool IsEmpty()
        {
            return Body != null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}