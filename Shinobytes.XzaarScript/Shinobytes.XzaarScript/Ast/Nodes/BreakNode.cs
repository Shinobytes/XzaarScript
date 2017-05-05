namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class BreakNode : ControlFlowNode
    {
        public BreakNode(int nodeIndex)
            : base(XzaarAstNodeTypes.BREAK, "BREAK", nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "break";
        }
    }
  
}