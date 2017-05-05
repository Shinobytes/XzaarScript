namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class BodyNode : BlockNode
    {
        public BodyNode(int nodeIndex) : base(XzaarNodeTypes.BODY, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}