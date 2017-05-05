namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class DoWhileLoopNode : LoopNode
    {
        private readonly AstNode test;

        public DoWhileLoopNode(AstNode test, AstNode body, int nodeIndex)
            : base("DO", body, nodeIndex)
        {
            this.test = test;
        }

        public AstNode Test => test;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }


        public override string ToString()
        {
            var testExpr = test.ToString();
            if (test.NodeType != NodeTypes.EXPRESSION)
                testExpr = "(" + testExpr + ")";
            return "do { " + Body + " } while " + testExpr;
        }
    }
}