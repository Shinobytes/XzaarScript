namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class EqualityOperatorNode : AstNode
    {
        private AstNode left;
        private AstNode right;

        public EqualityOperatorNode(AstNode left, string op, AstNode right, int nodeIndex)
            : base(NodeTypes.EQUALITY, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;

            Op = op;
        }

        public AstNode Left => left;

        public string Op { get; }

        public AstNode Right => right;

        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }

        public override bool IsEmpty()
        {
            return Left == null && Op == null && Right == null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public void SetRight(AstNode node)
        {
            this.right = node;
        }
    }
}