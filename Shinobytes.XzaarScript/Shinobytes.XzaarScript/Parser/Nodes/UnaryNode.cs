using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class UnaryNode : AstNode
    {
        private readonly bool isPostUnary;
        private readonly bool isIncrementor;
        private readonly AstNode item;

        public UnaryNode(bool isPostUnary, bool isIncrementor, AstNode item, int nodeIndex)
            : base(SyntaxKind.UnaryExpression, "UNARY", null, nodeIndex)
        {
            this.isPostUnary = isPostUnary;
            this.isIncrementor = isIncrementor;
            this.item = item;
        }

        public UnaryNode(bool isPostfix, string op, AstNode item, int nodeIndex)
            : this(isPostfix, false, item, nodeIndex)
        {
            this.Operator = op;
        }

        public AstNode Item => item;

        public bool IsPostUnary => isPostUnary;

        public bool IsIncrementor => isIncrementor;

        public string Operator { get; set; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return Item == null;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Operator))
                return "Unary: " + item;

            if (IsPostUnary)
                return this.item + "" + this.Operator;

            return this.Operator + "" + this.item;
        }
    }
}