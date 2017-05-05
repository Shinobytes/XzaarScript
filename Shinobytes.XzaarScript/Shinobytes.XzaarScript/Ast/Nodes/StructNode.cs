namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class StructNode : XzaarAstNode
    {
        private readonly string name;
        private readonly XzaarAstNode[] fields;

        public StructNode(string name, XzaarAstNode[] fields, int nodeIndex) 
            : base(XzaarAstNodeTypes.STRUCT, "STRUCT", null, nodeIndex)
        {
            this.fields = fields;
            this.name = name;
        }

        public XzaarAstNode[] Fields { get { return fields; } }

        public string Name { get { return name; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}