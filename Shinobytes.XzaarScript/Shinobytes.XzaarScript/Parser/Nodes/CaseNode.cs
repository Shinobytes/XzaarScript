using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class CaseNode : ControlFlowNode
    {
        private readonly AstNode test;
        private readonly AstNode body;

        public CaseNode(AstNode test, AstNode body, int nodeIndex)
            : base(SyntaxKind.KeywordCase, "CASE", nodeIndex)
        {
            this.test = test;
            this.body = body;
        }

        public bool IsDefaultCase => test == null;

        public AstNode Body => body;

        public AstNode Test => test;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (test == null)
                return "default: " + body;
            return "case " + test + ": " + body;
        }
    }
}