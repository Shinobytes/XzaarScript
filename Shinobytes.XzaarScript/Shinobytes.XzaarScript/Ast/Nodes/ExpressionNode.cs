using System;
using System.Linq;

namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ExpressionNode : XzaarAstNode
    {
        public ExpressionNode(int nodeIndex)
            : base(XzaarAstNodeTypes.EXPRESSION, null, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Children.Count == 0;
        }

        public override string ToString()
        {
            if (!IsEmpty()) return "(" + String.Join(" ", this.Children.Select(c => c.ToString()).ToArray()) + ")";
            return "()";
        }
    }
}