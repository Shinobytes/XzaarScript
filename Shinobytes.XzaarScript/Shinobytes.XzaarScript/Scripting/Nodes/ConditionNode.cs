namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ConditionNode : XzaarNode
    {
        public ConditionNode(string type, int nodeIndex) : base(XzaarNodeTypes.CONDITION, type, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}