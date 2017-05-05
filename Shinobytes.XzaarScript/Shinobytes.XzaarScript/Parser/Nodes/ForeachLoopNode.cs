namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ForeachLoopNode : LoopNode
    {
        private readonly AstNode variableDefinition;
        private readonly AstNode sourceExpression;

        public ForeachLoopNode(AstNode variableDefinition, AstNode sourceExpression, AstNode body, int nodeIndex)
            : base("FOREACH", body, nodeIndex)
        {
            this.variableDefinition = variableDefinition;
            this.sourceExpression = sourceExpression;
        }

        public AstNode Source => sourceExpression;

        public AstNode Variable => variableDefinition;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "foreach (" + this.Variable + " in " + this.Source + ") { " + this.Body + " }";
        }
    }
}