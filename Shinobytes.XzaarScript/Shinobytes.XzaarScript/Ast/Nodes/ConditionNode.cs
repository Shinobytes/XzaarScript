namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ConditionNode : XzaarAstNode
    {
        public ConditionNode(string type, int nodeIndex)
            : base(XzaarAstNodeTypes.CONDITION, type, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Children.Count == 0;
        }
    }
}