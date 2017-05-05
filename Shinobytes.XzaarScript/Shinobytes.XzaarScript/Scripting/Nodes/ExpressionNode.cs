namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ExpressionNode : XzaarNode
    {
        public ExpressionNode(int nodeIndex) : base(XzaarNodeTypes.EXPRESSION, null, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}