namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ControlFlowNode : AstNode
    {
        public ControlFlowNode(NodeTypes nodeType, string name, int nodeIndex)
            : base(nodeType, name, null, nodeIndex) { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}