using System.Linq;

namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class ArrayIndexNode : BlockNode
    {
        public ArrayIndexNode(int nodeIndex) : base(XzaarAstNodeTypes.ARRAYINDEX, nodeIndex)
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