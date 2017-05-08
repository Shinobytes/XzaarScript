using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BreakNode : ControlFlowNode
    {
        public BreakNode(int nodeIndex)
            : base(SyntaxKind.KeywordBreak, "BREAK", nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "break";
        }
    }

}