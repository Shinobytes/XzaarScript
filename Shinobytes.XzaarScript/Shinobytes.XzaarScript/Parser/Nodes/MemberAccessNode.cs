using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class MemberAccessNode : AstNode
    {

        public AstNode ArrayIndex { get; }

        public AstNode Member { get; }

        public string DeclaringType { get; set; }

        public string MemberType { get; set; }

        public MemberAccessNode(AstNode member, AstNode arrayIndex, string declaringType, string memberType, int nodeIndex)
            : base(SyntaxKind.MemberAccess, member.NodeName, member.Value, nodeIndex)
        {
            this.DeclaringType = declaringType;
            this.ArrayIndex = arrayIndex;
            MemberType = memberType;
            Member = member;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
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