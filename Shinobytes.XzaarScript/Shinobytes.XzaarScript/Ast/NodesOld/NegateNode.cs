namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class NegateNode : XzaarAstNode
    {
        public XzaarAstNode Expression { get { return this[0]; } }

        public NegateNode(XzaarAstNode expression, int nodeIndex)
            : base(XzaarAstNodeTypes.NEGATE, "NOT", null, nodeIndex)
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