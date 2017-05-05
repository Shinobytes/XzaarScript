using System.Linq;

namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ArrayIndexNode : BlockNode
    {
        public ArrayIndexNode(int nodeIndex) : base(XzaarNodeTypes.ARRAYINDEX, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "[" + string.Join(" ", Children.Select(s => s.ToString())) + "]";
        }
    }
}