using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ControlFlowNode : AstNode
    {
        public ControlFlowNode(SyntaxKind kind, string name, int nodeIndex)
            : base(kind, name, null, nodeIndex) { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}