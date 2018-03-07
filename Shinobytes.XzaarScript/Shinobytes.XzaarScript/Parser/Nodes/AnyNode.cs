using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class AnyNode : AstNode
    {
        public AnyNode(SyntaxKind kind, string nodeName, object value, int nodeIndex)
            : base(kind, nodeName, value, nodeIndex)
        {
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return (this.Children == null || this.Children.Count == 0) && this.Value == null && NodeName == null;
        }
    }
}