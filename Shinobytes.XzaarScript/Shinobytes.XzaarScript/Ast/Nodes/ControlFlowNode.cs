namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ControlFlowNode : XzaarAstNode
    {
        public ControlFlowNode(XzaarAstNodeTypes nodeType, string name, int nodeIndex)
            : base(nodeType, name, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}