namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class CreateStructNode : XzaarAstNode
    {        
        private readonly StructNode structNode;

        public CreateStructNode(StructNode structNode, int nodeIndex)
            : base(XzaarAstNodeTypes.CREATE_STRUCT, "CREATE_STRUCT", null, nodeIndex)
        {
            this.structNode = structNode;
            this.Type = this.structNode.ValueText;
        }

        public CreateStructNode(StructNode structNode, XzaarAstNode[] structFieldInitializers, int nodeIndex)
            : base(XzaarAstNodeTypes.CREATE_STRUCT, "CREATE_STRUCT", null, nodeIndex)
        {
            FieldInitializers = structFieldInitializers;
            this.structNode = structNode;
            this.Type = this.structNode.ValueText;
        }

        public XzaarAstNode[] FieldInitializers { get; set; }

        public StructNode StructNode { get { return structNode; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return "new_struct " + this.structNode.Name;
        }
    }
}