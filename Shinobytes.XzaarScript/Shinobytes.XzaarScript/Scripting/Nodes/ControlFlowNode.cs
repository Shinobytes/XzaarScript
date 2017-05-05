namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ControlFlowNode : XzaarNode
    {
        public ControlFlowNode(XzaarNodeTypes nodeType, string name, int nodeIndex) : base(nodeType, name, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}