namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class MemberAccessNode : XzaarAstNode
    {

        public XzaarAstNode ArrayIndex { get; }

        public XzaarAstNode Member { get; }

        public string DeclaringType { get; }

        public string MemberType { get; }

        public MemberAccessNode(XzaarAstNode member, XzaarAstNode arrayIndex, string declaringType, string memberType, int nodeIndex)
            : base(XzaarAstNodeTypes.ACCESS, member.NodeName, member.Value, nodeIndex)
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

        public override bool IsEmpty()
        {
            return false;
        }
    }
}