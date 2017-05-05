namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ForeachLoopNode : LoopNode
    {
        private readonly XzaarAstNode variableDefinition;
        private readonly XzaarAstNode sourceExpression;

        public ForeachLoopNode(XzaarAstNode variableDefinition, XzaarAstNode sourceExpression, XzaarAstNode body, int nodeIndex)
            : base("FOREACH", body, nodeIndex)
        {
            this.variableDefinition = variableDefinition;
            this.sourceExpression = sourceExpression;
        }

        public XzaarAstNode Source { get { return sourceExpression; } }

        public XzaarAstNode Variable { get { return variableDefinition; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "foreach (" + this.Variable + " in " + this.Source + ") { " + this.Body + " }";
        }
    }
}