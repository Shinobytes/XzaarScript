using System;
using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class MatchNode : AstNode
    {
        private readonly AstNode valueExpression;
        private readonly CaseNode[] cases;

        public MatchNode(AstNode valueExpression, CaseNode[] cases, int nodeIndex)
            : base(NodeTypes.MATCH, "MATCH", null, nodeIndex)
        {
            this.valueExpression = valueExpression;
            this.cases = cases;
        }

        public AstNode ValueExpression => valueExpression;

        public CaseNode[] Cases => cases;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return "switch (" + this.ValueExpression + ") { " + String.Join(" ", 
                Cases.Select(c => c.ToString()).ToArray()) + " }";
        }
    }
}