using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BlockNode : AstNode
    {
        protected BlockNode(SyntaxKind type, int nodeIndex)
            : base(type, "BLOCK", "BLOCK", nodeIndex) { }

        public BlockNode(int nodeIndex) : this(SyntaxKind.Block, nodeIndex)
        {
        }

        public override string ToString()
        {
            return string.Join(" ", Children.Select(s => s.ToString()).ToArray()); ;
        }

        public override bool IsEmpty()
        {
            return Children == null || Children.Count == 0;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}