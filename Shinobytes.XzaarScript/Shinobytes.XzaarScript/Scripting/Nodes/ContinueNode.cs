namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ContinueNode : ControlFlowNode
    {
        public ContinueNode(int nodeIndex)
            : base(XzaarNodeTypes.CONTINUE, "CONTINUE", nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}