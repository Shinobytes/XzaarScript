namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ContinueNode : ControlFlowNode
    {
        public ContinueNode(int nodeIndex)
            : base(NodeTypes.CONTINUE, "CONTINUE", nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "continue";
        }
    }
}