namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ConditionNode : AstNode
    {
        public ConditionNode(string type, int nodeIndex)
            : base(NodeTypes.CONDITION, type, null, nodeIndex) { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Children.Count == 0;
        }
    }
}