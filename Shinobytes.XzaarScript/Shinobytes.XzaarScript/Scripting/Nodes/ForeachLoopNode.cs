namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class ForeachLoopNode : LoopNode
    {
        private readonly XzaarNode variableDefinition;
        private readonly XzaarNode sourceExpression;

        public ForeachLoopNode(XzaarNode variableDefinition, XzaarNode sourceExpression, XzaarNode body, int nodeIndex)
            : base("FOREACH", body, nodeIndex)
        {
            this.variableDefinition = variableDefinition;
            this.sourceExpression = sourceExpression;
        }

        public XzaarNode Source { get { return sourceExpression; } }

        public XzaarNode Variable { get { return variableDefinition; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}