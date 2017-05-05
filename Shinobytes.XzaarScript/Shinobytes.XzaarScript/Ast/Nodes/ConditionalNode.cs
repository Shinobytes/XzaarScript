namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ConditionalNode : XzaarAstNode
    {
        private readonly XzaarAstNode condition;
        private XzaarAstNode ifTrue;
        private XzaarAstNode ifFalse;

        public ConditionalNode(XzaarAstNode condition, XzaarAstNode ifTrue, XzaarAstNode ifFalse, int nodeIndex)
            : base(XzaarAstNodeTypes.BLOCK, "CONDITIONAL", null, nodeIndex)
        {
            if (condition != null) condition.Parent = this;
            if (ifFalse != null) ifFalse.Parent = this;
            if (ifTrue != null) ifTrue.Parent = this;


            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse ?? Empty();

        }

        public XzaarAstNode GetCondition() => condition;

        public XzaarAstNode GetTrue() => ifTrue;

        public XzaarAstNode GetFalse() => ifFalse;

        public override bool IsEmpty()
        {
            return GetCondition() == null && GetTrue() == null && GetFalse() == null;
        }

        public override string ToString()
        {
            var conditionString = condition.ToString();
            if (condition.NodeType != XzaarAstNodeTypes.EXPRESSION)
                conditionString = "(" + conditionString + ")";
            return $"if {conditionString} {{{ifTrue}}} else {{{ifFalse}}}";
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public void SetIfTrue(XzaarAstNode xzaarNode)
        {
            ifTrue = xzaarNode;
        }

        public void SetIfFalse(XzaarAstNode xzaarNode)
        {
            ifFalse = xzaarNode;
        }
    }
}