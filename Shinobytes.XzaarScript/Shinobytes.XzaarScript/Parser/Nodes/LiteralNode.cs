namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class LiteralNode : AstNode
    {
        public LiteralNode(string nodeName, object value, int nodeIndex)
            : base(NodeTypes.LITERAL, nodeName, value, nodeIndex)
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

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}