namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ReturnNode : ControlFlowNode
    {
        public ReturnNode(XzaarAstNode returnExpression, int nodeIndex)
            : base(XzaarAstNodeTypes.RETURN, "RETURN", nodeIndex)
        {
            ReturnExpression = returnExpression;
        }

        public XzaarAstNode ReturnExpression { get; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (ReturnExpression != null)
                return "return " + ReturnExpression;
            return "return";
        }
    }
}