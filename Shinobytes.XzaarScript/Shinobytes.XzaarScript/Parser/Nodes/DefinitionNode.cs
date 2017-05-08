using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class DefinitionNode : AstNode
    {
        public DefinitionNode(string type, int nodeIndex) 
            : base(SyntaxKind.MemberDefinitionExpression, type, null, nodeIndex) { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}