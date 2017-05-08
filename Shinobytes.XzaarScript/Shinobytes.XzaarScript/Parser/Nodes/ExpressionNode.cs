using System;
using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ExpressionNode : AstNode
    {
        public ExpressionNode(int nodeIndex)
            : base(SyntaxKind.Expression, null, null, nodeIndex) { }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
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