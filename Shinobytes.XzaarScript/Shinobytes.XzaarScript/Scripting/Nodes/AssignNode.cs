namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class AssignNode : XzaarNode
    {
        public AssignNode(XzaarNode left, XzaarNode right, int nodeIndex) 
            : base(XzaarNodeTypes.ASSIGN, null, null, nodeIndex)
        {
            LeftParams.Add(left);
            RightParams.Add(right);
        }

        public XzaarNode Left => LeftParams[0];
        public XzaarNode Right => RightParams[0];

        public override string ToString()
        {
            return $"{Left} = {Right}";
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}