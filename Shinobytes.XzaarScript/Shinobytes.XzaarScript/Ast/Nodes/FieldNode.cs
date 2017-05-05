namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class FieldNode : XzaarAstNode
    {
        private readonly string name;
        private readonly string declaringType;

        public FieldNode(string type, string name, string declaringType, int nodeIndex)
            : base(XzaarAstNodeTypes.FIELD, "FIELD", null, nodeIndex)
        {
            this.Type = type;
            this.name = name;
            this.declaringType = declaringType;
            this.Value = name;
        }

        public string DeclaringType { get { return declaringType; } }

        public string Name { get { return name; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
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