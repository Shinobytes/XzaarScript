namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class BreakNode : ControlFlowNode
    {
        public BreakNode(int nodeIndex)
            : base(XzaarNodeTypes.BREAK, "BREAK", nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
  
}