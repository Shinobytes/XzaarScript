namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class DoWhileLoopNode : LoopNode
    {
        private readonly XzaarAstNode test;

        public DoWhileLoopNode(XzaarAstNode test, XzaarAstNode body, int nodeIndex)
            : base("DO", body, nodeIndex)
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
            return "do { " + Body + " } while " + testExpr;
        }
    }
}