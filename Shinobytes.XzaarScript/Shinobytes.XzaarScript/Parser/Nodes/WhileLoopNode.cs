using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class WhileLoopNode : LoopNode
    {
        private readonly AstNode test;

        public WhileLoopNode(AstNode test, AstNode body, int nodeIndex)
            : base("WHILE", body, nodeIndex)
        {
            this.test = test;
        }

        public AstNode Test => test;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var testExpr = test.ToString();
            if (test.Kind != SyntaxKind.Expression)
                testExpr = "(" + testExpr + ")";
            return "while " + testExpr + " { " + Body + " }";
        }
    }
}