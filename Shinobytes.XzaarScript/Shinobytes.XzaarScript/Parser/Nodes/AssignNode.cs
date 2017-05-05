namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class AssignNode : AstNode
    {
        private readonly AstNode left;
        private AstNode right;

        public AssignNode(AstNode left, AstNode right, int nodeIndex)
            : base(NodeTypes.ASSIGN, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            //Children.Add(left);
            //Children.Add(right);
        }

        public AstNode Left => left;
        public AstNode Right => right;

        public override string ToString()
        {
            return $"{Left} = {Right}";
        }

        public override bool IsEmpty()
        {
            return Left == null && Right == null;
        }


        public void SetAssignmentValue(AstNode value)
        {
            this.right = value;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}