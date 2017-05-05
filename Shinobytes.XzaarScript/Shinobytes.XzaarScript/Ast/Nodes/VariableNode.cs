namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class VariableNode : XzaarAstNode
    {
        private bool isDeclared;

        public bool IsExtern;

        public VariableNode(string type, string name, object value, bool isVariableDefinition, int nodeIndex)
            : base(XzaarAstNodeTypes.VARIABLE, isVariableDefinition ? "DEFINITION" : null, null, nodeIndex)
        {
            Value = value;
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
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