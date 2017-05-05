namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class NumberNode : LiteralNode
    {
        public NumberNode(object number, int nodeIndex) 
            : base("NUMBER", number, nodeIndex)
        {
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}