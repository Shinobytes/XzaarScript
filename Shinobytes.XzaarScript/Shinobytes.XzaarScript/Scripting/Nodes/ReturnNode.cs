namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ReturnNode : ControlFlowNode
    {
        public ReturnNode(XzaarNode returnExpression, int nodeIndex)
            : base(XzaarNodeTypes.RETURN, "RETURN", nodeIndex)
        {
            ReturnExpression = returnExpression;
        }

        public XzaarNode ReturnExpression { get; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}