namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ConditionalOperatorNode : AstNode
    {
        private AstNode left;
        private AstNode right;

        public ConditionalOperatorNode(int operatingOrderWeight, AstNode left, string op, AstNode right, int nodeIndex)
            : base(NodeTypes.CONDITIONAL, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            Op = op;
            OperatingOrder = operatingOrderWeight;
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
            return Op == null & Left == null && Right == null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}