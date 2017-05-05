using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class FunctionParametersNode : XzaarNode
    {
        public FunctionParametersNode(int nodeIndex) : base(XzaarNodeTypes.PARAMETERS, null, null, nodeIndex) { }

        public IReadOnlyList<ParameterNode> Parameters => Children.Cast<ParameterNode>().ToList();

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}