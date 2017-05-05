namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ConditionalOperatorNode : XzaarNode
    {

        public ConditionalOperatorNode(int operatingOrderWeight, XzaarNode left, string op, XzaarNode right, int nodeIndex)
            : base(XzaarNodeTypes.CONDITIONAL, null, null, nodeIndex)
        {
            LeftParams.Add(left);
            RightParams.Add(right);
            Op = op;
            OperatingOrder = operatingOrderWeight;
        }

        public XzaarNode Left => LeftParams[0];
        public string Op { get; }
        public XzaarNode Right => RightParams[0];

        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }        
    }
}