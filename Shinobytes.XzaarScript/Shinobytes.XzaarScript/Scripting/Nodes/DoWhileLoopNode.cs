namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class DoWhileLoopNode : LoopNode
    {
        private readonly XzaarNode test;
        
        public DoWhileLoopNode(XzaarNode test, XzaarNode body, int nodeIndex) 
            : base("DO", body, nodeIndex)
        {
            this.test = test;        
        }
        
        public XzaarNode Test { get { return test; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}