namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ReturnNode : ControlFlowNode
    {
        public ReturnNode(AstNode returnExpression, int nodeIndex)
            : base(NodeTypes.RETURN, "RETURN", nodeIndex)
        {
            ReturnExpression = returnExpression;
        }

        public AstNode ReturnExpression { get; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (ReturnExpression != null)
                return "return " + ReturnExpression;
            return "return";
        }
    }
}