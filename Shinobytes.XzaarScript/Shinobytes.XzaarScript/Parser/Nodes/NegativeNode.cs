namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class NegativeNode : AstNode
    {
        public AstNode Expression => this[0];

        public NegativeNode(AstNode expression, int nodeIndex)
            : base(NodeTypes.NEGATIVE, "-", null, nodeIndex)
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