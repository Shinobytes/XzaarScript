namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class ContinueNode : ControlFlowNode
    {
        public ContinueNode(int nodeIndex)
            : base(XzaarAstNodeTypes.CONTINUE, "CONTINUE", nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "continue";
        }
    }
}