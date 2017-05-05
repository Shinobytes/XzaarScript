namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class BinaryOperatorNode : XzaarAstNode
    {
        private XzaarAstNode left;
        private XzaarAstNode right;

        public BinaryOperatorNode(int operatingOrderWeight, XzaarAstNode left, char op, XzaarAstNode right, int nodeIndex)
            : this(operatingOrderWeight, left, op.ToString(), right, nodeIndex)
        {
        }

        public BinaryOperatorNode(int operatingOrderWeight, XzaarAstNode left, string op, XzaarAstNode right, int nodeIndex)
            : base(XzaarAstNodeTypes.MATH, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;

            Op = op;
            OperatingOrder = operatingOrderWeight;
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
            return Op == null && Left == null && Right == null;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public void SetLeft(XzaarAstNode expr)
        {
            this.left = expr;
        }

        public void SetRight(XzaarAstNode expr)
        {
            this.right = expr;
        }
    }
}