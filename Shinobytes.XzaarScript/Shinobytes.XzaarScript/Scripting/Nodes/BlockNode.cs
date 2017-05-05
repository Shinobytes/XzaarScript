using System.Linq;

namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class BlockNode : XzaarNode
    {
        protected BlockNode(XzaarNodeTypes type, int nodeIndex) 
            : base(type, "BLOCK", "BLOCK", nodeIndex) { }

        public BlockNode(int nodeIndex) : this(XzaarNodeTypes.BLOCK, nodeIndex)
        {
        }

        public override string ToString()
        {
            return string.Join(" ", Children.Select(s => s.ToString())); ;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}