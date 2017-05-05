namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ParameterNode : XzaarNode
    {
        public ParameterNode(string name, string type, int nodeIndex)
            : base(XzaarNodeTypes.PARAMETER, null, null, nodeIndex)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public string Type { get; }

        public override string ToString()
        {
            return "Parameter: (Type " + Type + ") " + Name;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}