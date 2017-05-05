using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BlockNode : AstNode
    {
        protected BlockNode(NodeTypes type, int nodeIndex)
            : base(type, "BLOCK", "BLOCK", nodeIndex) { }

        public BlockNode(int nodeIndex) : this(NodeTypes.BLOCK, nodeIndex)
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