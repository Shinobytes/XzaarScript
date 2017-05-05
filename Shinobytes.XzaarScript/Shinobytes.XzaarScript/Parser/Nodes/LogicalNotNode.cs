namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class LogicalNotNode : AstNode
    {
        public AstNode Expression => this[0];

        public LogicalNotNode(AstNode expression, int nodeIndex)
            : base(NodeTypes.NOT_OPERATOR, "NOT", null, nodeIndex)
        {

            this.AddChild(expression);
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Expression == null || Expression.IsEmpty();
        }
    }
}