namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ConditionalOperatorNode : XzaarAstNode
    {
        private XzaarAstNode left;
        private XzaarAstNode right;

        public ConditionalOperatorNode(int operatingOrderWeight, XzaarAstNode left, string op, XzaarAstNode right, int nodeIndex)
            : base(XzaarAstNodeTypes.CONDITIONAL, null, null, nodeIndex)
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
            return Op == null & Left == null && Right == null;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}