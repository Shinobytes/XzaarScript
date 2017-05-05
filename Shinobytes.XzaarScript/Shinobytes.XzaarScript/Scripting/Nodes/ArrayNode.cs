namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ArrayNode : LiteralNode
    {
        public ArrayNode(int nodeIndex) : base("ARRAY", null, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}