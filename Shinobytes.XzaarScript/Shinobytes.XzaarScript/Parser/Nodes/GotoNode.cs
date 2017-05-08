using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class GotoNode : ControlFlowNode
    {
        private readonly AstNode label;

        private readonly string labelName;

        public GotoNode(AstNode label, int nodeIndex)
            : base(SyntaxKind.KeywordGoto, "GOTO", nodeIndex)
        {
            this.label = label;
        }

        public GotoNode(string label, int nodeIndex)
            : base(SyntaxKind.KeywordGoto, "GOTO", nodeIndex)
        {
            this.labelName = label;
        }

        public AstNode Label => label;

        public string LabelName
        {
            get
            {
                if (label != null) return label.Value?.ToString();
                return labelName;
            }
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "goto " + labelName;
        }
    }
}