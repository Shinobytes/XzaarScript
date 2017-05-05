namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class AssignNode : XzaarAstNode
    {
        private readonly XzaarAstNode left;
        private readonly XzaarAstNode right;

        public AssignNode(XzaarAstNode left, XzaarAstNode right, int nodeIndex)
            : base(XzaarAstNodeTypes.ASSIGN, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            //Children.Add(left);
            //Children.Add(right);
        }

        public XzaarAstNode Left => left;
        public XzaarAstNode Right => right;

        public override string ToString()
        {
            return $"{Left} = {Right}";
        }

        public override bool IsEmpty()
        {
            return Left == null && Right == null;
        }


        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}