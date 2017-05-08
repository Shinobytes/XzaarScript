using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class StructNode : AstNode
    {
        private readonly string name;
        private readonly AstNode[] fields;

        public StructNode(string name, AstNode[] fields, int nodeIndex)
            : base(SyntaxKind.StructDefinitionExpression, "STRUCT", null, nodeIndex)
        {
            this.fields = fields;
            this.name = name;
        }

        public AstNode[] Fields => fields;

        public string Name => name;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}