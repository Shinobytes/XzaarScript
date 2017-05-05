using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class FunctionParametersNode : XzaarAstNode
    {
        public FunctionParametersNode(int nodeIndex)
            : base(XzaarAstNodeTypes.PARAMETERS, null, null, nodeIndex) { }

        public IList<ParameterNode> Parameters => Children.Cast<ParameterNode>().ToList();

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
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