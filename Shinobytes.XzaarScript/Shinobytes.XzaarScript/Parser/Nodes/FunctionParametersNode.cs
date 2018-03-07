using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class FunctionParametersNode : AstNode
    {
        public FunctionParametersNode(int nodeIndex)
            : base(SyntaxKind.ParameterListExpression, null, null, nodeIndex) { }

        public IList<ParameterNode> Parameters => Children.Cast<ParameterNode>().ToList();

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return string.Join(", ", Parameters.Select(x => x.ToString()).ToArray());
        }
    }
}