namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class WhileLoopNode : LoopNode
    {
        private readonly XzaarNode test;
        
        public WhileLoopNode(XzaarNode test, XzaarNode body, int nodeIndex) 
            : base("WHILE", body, nodeIndex)
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