namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class EmptyNode : XzaarNode
    {
        public EmptyNode(int nodeIndex) 
            : base(XzaarNodeTypes.NULL_EMPTY, null, null, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}