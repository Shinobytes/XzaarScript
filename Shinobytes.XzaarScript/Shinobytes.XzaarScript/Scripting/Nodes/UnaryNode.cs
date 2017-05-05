namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class UnaryNode : XzaarNode
    {
        private readonly bool isPostUnary;
        private readonly bool isIncrementor;
        private readonly XzaarNode item;

        public UnaryNode(bool isPostUnary, bool isIncrementor, XzaarNode item, int nodeIndex) 
            : base(XzaarNodeTypes.UNARY_OPERATOR, "UNARY", null, nodeIndex)
        {
            this.isPostUnary = isPostUnary;
            this.isIncrementor = isIncrementor;
            this.item = item;
        }

        public XzaarNode Item { get { return item; } }

        public bool IsPostUnary { get { return isPostUnary; } }

        public bool IsIncrementor { get { return isIncrementor; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}