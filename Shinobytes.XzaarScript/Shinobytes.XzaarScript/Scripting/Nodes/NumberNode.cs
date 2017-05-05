namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class NumberNode : LiteralNode
    {
        public NumberNode(object number, int nodeIndex) : base("NUMBER", number, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}