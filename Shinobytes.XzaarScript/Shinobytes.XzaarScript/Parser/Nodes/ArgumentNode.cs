using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ArgumentNode : AstNode
    {
        public ArgumentNode(AstNode item, /*XzaarNode arrayIndex, */int index, int nodeIndex)
            : base(SyntaxKind.ArgumentExpression, null, null, nodeIndex)
        {
            Item = item;
            //ArrayIndex = arrayIndex;
            ArgumentIndex = index;
        }

        public int ArgumentIndex { get; }

        public AstNode Item { get; }

        //public XzaarNode ArrayIndex { get;  }


        public override bool IsEmpty()
        {
            return Item == null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (Item == null)
                return base.ToString().Trim();
            return Item.ToString();
        }
    }
}