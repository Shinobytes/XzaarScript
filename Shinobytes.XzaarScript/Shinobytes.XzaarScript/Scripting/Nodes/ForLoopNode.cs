namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ForLoopNode : LoopNode
    {
        private readonly XzaarNode initiator;
        private readonly XzaarNode test;
        private readonly XzaarNode incrementor;        

        public ForLoopNode(XzaarNode initiator, XzaarNode test, XzaarNode incrementor, XzaarNode body, int nodeIndex) 
            : base("FOR", body, nodeIndex)
        {
            this.initiator = initiator;
            this.test = test;
            this.incrementor = incrementor;            
        }
        
        public XzaarNode Initiator { get { return initiator; } }

        public XzaarNode Incrementor { get { return incrementor; } }

        public XzaarNode Test { get { return test; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}