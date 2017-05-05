using System;
using System.Linq;

namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class MatchNode : XzaarAstNode
    {
        private readonly XzaarAstNode valueExpression;
        private readonly CaseNode[] cases;

        public MatchNode(XzaarAstNode valueExpression, CaseNode[] cases, int nodeIndex)
            : base(XzaarAstNodeTypes.MATCH, "MATCH", null, nodeIndex)
        {
            this.valueExpression = valueExpression;
            this.cases = cases;
        }

        public XzaarAstNode ValueExpression { get { return valueExpression; } }

        public CaseNode[] Cases { get { return cases; } }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return "switch (" + this.ValueExpression + ") { " + String.Join(" ", Cases.Select(c => c.ToString())) + " }";
        }
    }
}