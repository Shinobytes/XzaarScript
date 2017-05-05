namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class LiteralNode : XzaarAstNode
    {
        public LiteralNode(string nodeName, object value, int nodeIndex)
            : base(XzaarAstNodeTypes.LITERAL, nodeName, value, nodeIndex)
        {
            if (this.Type == null)
            {
                this.Type = "any";
            }

            if (this.Type.ToLower() == "any")
            {
                if (nodeName.ToLower() == "string")
                {
                    this.Type = "string";
                }
                else if (nodeName.ToLower() == "number")
                {
                    this.Type = "number";
                }
            }
        }

        public override string ToString()
        {
            if (this.NodeName == "STRING")
            {
                return "\"" + Value + "\"";
            }
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