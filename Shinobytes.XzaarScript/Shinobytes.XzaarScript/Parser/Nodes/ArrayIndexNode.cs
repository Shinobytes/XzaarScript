using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ArrayIndexNode : BlockNode
    {
        public ArrayIndexNode(int nodeIndex) : base(NodeTypes.ARRAYINDEX, nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "[" + string.Join(" ", Children.Select(s => s.ToString()).ToArray()) + "]";
        }
    }
}