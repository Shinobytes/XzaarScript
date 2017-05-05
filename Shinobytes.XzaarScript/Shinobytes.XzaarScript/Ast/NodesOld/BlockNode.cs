using System.Linq;

namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class BlockNode : XzaarAstNode
    {
        protected BlockNode(XzaarAstNodeTypes type, int nodeIndex)
            : base(type, "BLOCK", "BLOCK", nodeIndex) { }

        public BlockNode(int nodeIndex) : this(XzaarAstNodeTypes.BLOCK, nodeIndex)
        {
        }

        public override string ToString()
        {
            return string.Join(" ", Children.Select(s => s.ToString())); ;
        }

        public override bool IsEmpty()
        {
            return Children == null || Children.Count == 0;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}