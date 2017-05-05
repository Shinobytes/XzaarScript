namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class BodyNode : BlockNode
    {
        public BodyNode(int nodeIndex) : base(XzaarAstNodeTypes.BODY, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}