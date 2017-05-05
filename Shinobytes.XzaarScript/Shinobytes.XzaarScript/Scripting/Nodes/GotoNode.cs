namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class GotoNode : ControlFlowNode
    {
        private readonly XzaarNode label;

        private readonly string labelName;

        public GotoNode(XzaarNode label, int nodeIndex)
            : base(XzaarNodeTypes.GOTO, "GOTO", nodeIndex)
        {
            this.label = label;
        }

        public GotoNode(string label, int nodeIndex)
            : base(XzaarNodeTypes.GOTO, "GOTO", nodeIndex)
        {
            this.labelName = label;
        }

        public XzaarNode Label { get { return label; } }

        public string LabelName
        {
            get
            {
                if (label != null) return label.Value?.ToString();
                return labelName;
            }
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "goto " + labelName;
        }
    }
}