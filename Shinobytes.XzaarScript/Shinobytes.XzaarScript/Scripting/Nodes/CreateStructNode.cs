namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class CreateStructNode : XzaarNode
    {
        private readonly StructNode structNode;

        public CreateStructNode(StructNode structNode, int nodeIndex)
            : base(XzaarNodeTypes.CREATE_STRUCT, "CREATE_STRUCT", null, nodeIndex)
        {
            this.structNode = structNode;
        }

        public StructNode StructNode { get { return structNode; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}