namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class FieldNode : XzaarAstNode
    {
        private readonly string type;
        private readonly string name;
        private readonly string declaringType;

        public FieldNode(string type, string name, string declaringType, int nodeIndex) 
            : base(XzaarAstNodeTypes.FIELD, "FIELD", null, nodeIndex)
        {
            this.type = type;
            this.name = name;
            this.declaringType = declaringType;
        }

        public string DeclaringType { get { return declaringType; } }

        public string Type { get { return type; } }

        public string Name { get { return name; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "FIELD " + type + " " + name;
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}