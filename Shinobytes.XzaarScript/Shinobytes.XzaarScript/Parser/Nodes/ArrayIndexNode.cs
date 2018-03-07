using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ArrayIndexNode : BlockNode
    {
        public ArrayIndexNode(int nodeIndex) : base(SyntaxKind.ArrayIndexExpression, nodeIndex)
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