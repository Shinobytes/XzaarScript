namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class EqualityOperatorNode : XzaarAstNode
    {
        private XzaarAstNode left;
        private XzaarAstNode right;

        public EqualityOperatorNode(XzaarAstNode left, string op, XzaarAstNode right, int nodeIndex)
            : base(XzaarAstNodeTypes.EQUALITY, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;

            Op = op;
        }

        public XzaarAstNode Left => left;

        public string Op { get; }

        public XzaarAstNode Right => right;

        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }

        public override bool IsEmpty()
        {
            return Left == null && Op == null && Right == null;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public void SetRight(XzaarAstNode xzaarNode)
        {
            this.right = xzaarNode;
        }
    }
}