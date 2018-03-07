using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class MemberAccessChainNode : AstNode
    {
        private readonly AstNode left;
        private readonly AstNode right;
        private string resultType;

        public MemberAccessChainNode(AstNode left, AstNode right, string resultType, int nodeIndex)
            : base(SyntaxKind.MemberAccess, "ACCESSOR_CHAIN", null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            this.resultType = resultType;
        }

        public AstNode LastAccessor => left;
        public AstNode Accessor => right;

        public string ResultType
        {
            get { return resultType; }
            set { resultType = value; }
        }



        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
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