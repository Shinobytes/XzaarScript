namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class UnaryNode : XzaarAstNode
    {
        private readonly bool isPostUnary;
        private readonly bool isIncrementor;
        private readonly XzaarAstNode item;

        public UnaryNode(bool isPostUnary, bool isIncrementor, XzaarAstNode item, int nodeIndex)
            : base(XzaarAstNodeTypes.UNARY_OPERATOR, "UNARY", null, nodeIndex)
        {
            this.isPostUnary = isPostUnary;
            this.isIncrementor = isIncrementor;
            this.item = item;
        }

        public UnaryNode(bool isPostfix, string op, XzaarAstNode item, int nodeIndex)
            : this(isPostfix, false, item, nodeIndex)
        {
            this.Operator = op;
        }

        public XzaarAstNode Item { get { return item; } }

        public bool IsPostUnary { get { return isPostUnary; } }

        public bool IsIncrementor { get { return isIncrementor; } }

        public string Operator { get; set; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
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