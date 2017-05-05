namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class StructNode : XzaarNode
    {
        private readonly string name;
        private readonly XzaarNode[] fields;

        public StructNode(string name, XzaarNode[] fields, int nodeIndex) : base(XzaarNodeTypes.STRUCT, "STRUCT", null, nodeIndex)
        {
            this.fields = fields;
            this.name = name;
        }

        public XzaarNode[] Fields { get { return fields; } }

        public string Name { get { return name; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}