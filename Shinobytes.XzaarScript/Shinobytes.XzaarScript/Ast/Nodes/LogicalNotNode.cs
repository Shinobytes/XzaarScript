namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class LogicalNotNode : XzaarAstNode
    {
        public XzaarAstNode Expression { get { return this[0]; } }

        public LogicalNotNode(XzaarAstNode expression, int nodeIndex)
            : base(XzaarAstNodeTypes.NOT_OPERATOR, "NOT", null, nodeIndex)
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