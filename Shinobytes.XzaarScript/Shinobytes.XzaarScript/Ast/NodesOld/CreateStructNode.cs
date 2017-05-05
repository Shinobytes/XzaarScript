namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class CreateStructNode : XzaarAstNode
    {
        private readonly StructNode structNode;

        public CreateStructNode(StructNode structNode, int nodeIndex)
            : base(XzaarAstNodeTypes.CREATE_STRUCT, "CREATE_STRUCT", null, nodeIndex)
        {
            this.structNode = structNode;
        }

        public StructNode StructNode { get { return structNode; } }

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