namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BodyNode : BlockNode
    {
        public BodyNode(int nodeIndex) : base(NodeTypes.BODY, nodeIndex) { }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}