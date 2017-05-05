namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class VariableNode : XzaarNode
    {
        private bool isDeclared;
        public VariableNode(string type, string name, object value, bool isVariableDefinition = false, int nodeIndex = -1)
            : base(XzaarNodeTypes.VARIABLE, isVariableDefinition ? "DEFINITION" : null, null, nodeIndex)
        {
            Value = value;
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public string Type { get; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
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