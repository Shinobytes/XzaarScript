using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ParameterNode : AstNode
    {
        private bool isExtern;

        public ParameterNode(string name, string type, int nodeIndex)
            : base(SyntaxKind.ParameterDefinitionExpression, null, null, nodeIndex)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public bool IsExtern
        {
            get { return isExtern; }
            set { isExtern = value; }
        }

        public override string ToString()
        {
            return "Parameter: (Type " + Type + ") " + Name;
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}