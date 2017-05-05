using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class ArrayNode : LiteralNode
    {
        public ArrayNode(int nodeIndex, List<XzaarAstNode> values) : base("ARRAY", null, nodeIndex)
        {
            Values = values;
            this.AddChildren(values);
        }

        public List<XzaarAstNode> Values { get; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "[" + string.Join(",", this.Values.Select(c => "" + c)) + "]";
        }
    }
}