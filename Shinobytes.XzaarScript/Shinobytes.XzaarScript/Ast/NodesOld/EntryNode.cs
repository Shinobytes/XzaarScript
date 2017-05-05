namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class EntryNode : XzaarAstNode
    {
        public EntryNode(XzaarAstNode body)
            : base(XzaarAstNodeTypes.PROGRAM, "PROGRAM", "PROGRAM", -1)
        {
            if (body != null)
                this.AddChild(body);
        }

        public XzaarAstNode Body => Children.Count > 0 ? this[0] : null;

        public override string ToString()
        {
            return "" + Body;
        }

        public override bool IsEmpty()
        {
            return Body != null;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}