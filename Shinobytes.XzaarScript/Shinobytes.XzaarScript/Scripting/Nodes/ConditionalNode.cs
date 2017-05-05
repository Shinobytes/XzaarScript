namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ConditionalNode : XzaarNode
    {
        private readonly XzaarNode condition;
        private XzaarNode ifTrue;
        private XzaarNode ifFalse;

        public ConditionalNode(XzaarNode condition, XzaarNode ifTrue, XzaarNode ifFalse, int nodeIndex) 
            : base(XzaarNodeTypes.BLOCK, "CONDITIONAL", null, nodeIndex)
        {
            if (condition != null) condition.Parent = this;
            if (ifFalse != null) ifFalse.Parent = this;
            if (ifTrue != null) ifTrue.Parent = this;


            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse ?? Empty();

        }

        public XzaarNode GetCondition() => condition;

        public XzaarNode GetTrue() => ifTrue;

        public XzaarNode GetFalse() => ifFalse;        

        public override string ToString()
        {
            return $"if ({condition}) {{{ifTrue}}} else {{{ifFalse}}}";
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public void SetIfTrue(XzaarNode xzaarNode)
        {
            ifTrue = xzaarNode;
        }

        public void SetIfFalse(XzaarNode xzaarNode)
        {
            ifFalse = xzaarNode;
        }
    }
}