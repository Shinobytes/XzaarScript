namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BreakNode : ControlFlowNode
    {
        public BreakNode(int nodeIndex)
            : base(NodeTypes.BREAK, "BREAK", nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "break";
        }
    }
  
}