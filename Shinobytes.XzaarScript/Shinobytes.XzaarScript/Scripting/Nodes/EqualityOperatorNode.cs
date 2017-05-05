namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class EqualityOperatorNode : XzaarNode
    {

        public EqualityOperatorNode(XzaarNode left, string op, XzaarNode right, int nodeIndex)
            : base(XzaarNodeTypes.EQUALITY, null, null, nodeIndex)
        {
            LeftParams.Add(left);
            RightParams.Add(right);
            Op = op;
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