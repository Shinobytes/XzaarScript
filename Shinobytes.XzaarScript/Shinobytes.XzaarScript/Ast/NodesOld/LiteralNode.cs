namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class LiteralNode : XzaarAstNode
    {
        public LiteralNode(string nodeName, object value, int nodeIndex) 
            : base(XzaarAstNodeTypes.LITERAL, nodeName, value, nodeIndex) { }

        public string Type { get; set; }

        public override string ToString()
        {
            return "" + Value;
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}