namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class MemberAccessChainNode : XzaarAstNode
    {
        private readonly XzaarAstNode left;
        private readonly XzaarAstNode right;
        private string resultType;

        public MemberAccessChainNode(XzaarAstNode left, XzaarAstNode right, string resultType, int nodeIndex)
            : base(XzaarAstNodeTypes.ACCESSOR_CHAIN, "ACCESSOR_CHAIN", null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            this.resultType = resultType;
        }

        public XzaarAstNode LastAccessor { get { return left; } }
        public XzaarAstNode Accessor { get { return right; } }

        public string ResultType
        {
            get { return resultType; }
        }



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
            return this.LastAccessor + "." + this.Accessor;
        }
    }
}