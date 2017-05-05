namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class AnyNode : XzaarNode
    {
        public AnyNode(XzaarNodeTypes nodeType, string nodeName, object value, int nodeIndex) 
            : base(nodeType, nodeName, value, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}