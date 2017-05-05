namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ArgumentNode : XzaarNode
    {
        public ArgumentNode(XzaarNode item, int index, int nodeIndex) 
            : base(XzaarNodeTypes.ARGUMENT, null, null, nodeIndex)
        {
            Item = item;
            ArgumentIndex = index;
        }

        public int ArgumentIndex { get; }

        public XzaarNode Item { get; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}