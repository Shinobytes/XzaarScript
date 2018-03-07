using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class VariableNode : AstNode
    {
        private bool isDeclared;

        public bool IsExtern;

        public VariableNode(string type, string name, object value, bool isVariableDefinition, int nodeIndex)
            : base(SyntaxKind.VariableDefinitionExpression, isVariableDefinition ? "DEFINITION" : null, null, nodeIndex)
        {
            Value = value;
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public void Declare()
        {
            this.isDeclared = true;
        }

        public bool IsDeclared()
        {
            return this.isDeclared;
        }
    }
}