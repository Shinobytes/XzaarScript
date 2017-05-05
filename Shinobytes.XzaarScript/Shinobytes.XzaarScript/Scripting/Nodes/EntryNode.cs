namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class EntryNode : XzaarNode
    {
        public EntryNode(XzaarNode body = null)
            : base(XzaarNodeTypes.PROGRAM, "PROGRAM", "PROGRAM", -1)
        {
            if (body != null)
                this.AddChild(body);
        }

        public XzaarNode Body => Children.Count > 0 ? this[0] : null;

        public override string ToString()
        {
            return "" + Body;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}