using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class BinaryOperatorNode : AstNode
    {
        private AstNode left;
        private AstNode right;

        public BinaryOperatorNode(int operatingOrderWeight, AstNode left, char op, AstNode right, int nodeIndex)
            : this(operatingOrderWeight, left, op.ToString(), right, nodeIndex)
        {
        }

        public BinaryOperatorNode(int operatingOrderWeight, AstNode left, string op, AstNode right, int nodeIndex)
            : base(SyntaxKind.BinaryExpression, null, null, nodeIndex)
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
            return Op == null && Left == null && Right == null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public void SetLeft(AstNode expr)
        {
            this.left = expr;
        }

        public void SetRight(AstNode expr)
        {
            this.right = expr;
        }
    }
}