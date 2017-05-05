namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class WhileLoopNode : LoopNode
    {
        private readonly XzaarAstNode test;

        public WhileLoopNode(XzaarAstNode test, XzaarAstNode body, int nodeIndex)
            : base("WHILE", body, nodeIndex)
        {
            this.test = test;
        }

        public XzaarAstNode Test { get { return test; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var testExpr = test.ToString();
            if (test.NodeType != XzaarAstNodeTypes.EXPRESSION)
                testExpr = "(" + testExpr + ")";
            return "while " + testExpr + " { " + Body + " }";
        }
    }
}