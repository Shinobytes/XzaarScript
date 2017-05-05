namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class MemberAccessChainNode : XzaarNode
    {
        private readonly XzaarNode left;
        private readonly XzaarNode right;
        private string resultType;

        public MemberAccessChainNode(XzaarNode left, XzaarNode right, string resultType, int nodeIndex)
            : base(XzaarNodeTypes.ACCESSOR_CHAIN, "ACCESSOR_CHAIN", null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            this.resultType = resultType;
        }

        public XzaarNode LastAccessor { get { return left; } }
        public XzaarNode Accessor { get { return right; } }

        public string ResultType
        {
            get { return resultType; }
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }

    public class MemberAccessNode : XzaarNode
    {

        public XzaarNode ArrayIndex { get; }

        public XzaarNode Member { get; }

        public string DeclaringType { get; }

        public string MemberType { get; }

        public MemberAccessNode(XzaarNode member, XzaarNode arrayIndex, string declaringType, string memberType, int nodeIndex)
            : base(XzaarNodeTypes.ACCESS, member.NodeName, member.Value, nodeIndex)
        {
            this.DeclaringType = declaringType;
            this.ArrayIndex = arrayIndex;
            MemberType = memberType;
            Member = member;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }


        public override string ToString()
        {
            if (Member != null)
            {
                if (ArrayIndex != null)
                {
                    return Member + "[" + ArrayIndex + "]";
                }
                return Member.ToString();
            }
            return base.ToString();
        }
    }
}