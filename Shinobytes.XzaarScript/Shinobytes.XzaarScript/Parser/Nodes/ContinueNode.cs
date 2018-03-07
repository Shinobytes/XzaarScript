using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ContinueNode : ControlFlowNode
    {
        public ContinueNode(int nodeIndex)
            : base(SyntaxKind.KeywordContinue, "CONTINUE", nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "continue";
        }
    }
}