using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class EmptyNode : AstNode
    {
        public EmptyNode(int nodeIndex) 
            : base(SyntaxKind.Empty, null, null, nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return "";            
        }
    }
}