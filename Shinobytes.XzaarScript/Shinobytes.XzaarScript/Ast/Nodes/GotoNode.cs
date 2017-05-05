namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class GotoNode : ControlFlowNode
    {
        private readonly XzaarAstNode label;

        private readonly string labelName;

        public GotoNode(XzaarAstNode label, int nodeIndex)
            : base(XzaarAstNodeTypes.GOTO, "GOTO", nodeIndex)
        {
            this.label = label;
        }

        public GotoNode(string label, int nodeIndex)
            : base(XzaarAstNodeTypes.GOTO, "GOTO", nodeIndex)
        {
            this.labelName = label;
        }

        public XzaarAstNode Label { get { return label; } }

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