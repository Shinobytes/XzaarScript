namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class NegativeNode : XzaarAstNode
    {
        public XzaarAstNode Expression { get { return this[0]; } }

        public NegativeNode(XzaarAstNode expression, int nodeIndex)
            : base(XzaarAstNodeTypes.NEGATIVE, "-", null, nodeIndex)
        {

            this.AddChild(expression);
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Expression == null || Expression.IsEmpty();
        }
    }
}