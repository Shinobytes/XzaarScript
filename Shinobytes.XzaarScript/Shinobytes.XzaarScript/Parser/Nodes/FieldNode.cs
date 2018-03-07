using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class FieldNode : AstNode
    {
        private readonly string name;
        private readonly string declaringType;

        public FieldNode(string type, string name, string declaringType, int nodeIndex)
            : base(SyntaxKind.FieldDefinitionExpression, "FIELD", null, nodeIndex)
        {
            this.Type = type;
            this.name = name;
            this.declaringType = declaringType;
            this.Value = name;
        }

        public string DeclaringType => declaringType;

        public string Name => name;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "FIELD " + Type + " " + name;
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}