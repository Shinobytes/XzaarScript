using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BodyNode : BlockNode
    {
        public BodyNode(int nodeIndex) : base(SyntaxKind.Block, nodeIndex) { }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}