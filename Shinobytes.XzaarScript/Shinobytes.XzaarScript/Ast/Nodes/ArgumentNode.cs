namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ArgumentNode : XzaarAstNode
    {
        public ArgumentNode(XzaarAstNode item, /*XzaarNode arrayIndex, */int index, int nodeIndex)
            : base(XzaarAstNodeTypes.ARGUMENT, null, null, nodeIndex)
        {
            Item = item;
            //ArrayIndex = arrayIndex;
            ArgumentIndex = index;
        }

        public int ArgumentIndex { get; }

        public XzaarAstNode Item { get; }

        //public XzaarNode ArrayIndex { get;  }


        public override bool IsEmpty()
        {
            return Item == null;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (Item == null)
                return base.ToString().Trim();
            return Item.ToString();
        }
    }
}