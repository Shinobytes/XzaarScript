using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ReturnNode : ControlFlowNode
    {
        public ReturnNode(AstNode returnExpression, int nodeIndex)
            : base(SyntaxKind.KeywordReturn, "RETURN", nodeIndex)
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