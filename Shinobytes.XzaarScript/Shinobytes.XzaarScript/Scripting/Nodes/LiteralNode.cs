namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class LiteralNode : XzaarNode
    {
        public LiteralNode(string nodeName, object value, int nodeIndex) 
            : base(XzaarNodeTypes.LITERAL, nodeName, value, nodeIndex) { }

        public string Type { get; set; }

        public override string ToString()
        {
            return "" + Value;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}