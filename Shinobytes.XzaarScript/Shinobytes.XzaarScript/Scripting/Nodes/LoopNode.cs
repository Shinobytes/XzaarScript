namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class LoopNode : XzaarNode
    {
        private readonly XzaarNode body;

        public LoopNode(string type = "LOOP", XzaarNode body = null, int nodeIndex = -1)
            : base(XzaarNodeTypes.LOOP, type, null, nodeIndex)
        {
            this.body = body;
        }

        public XzaarNode Body { get { return body; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}