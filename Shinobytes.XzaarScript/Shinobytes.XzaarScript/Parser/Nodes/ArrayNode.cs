using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ArrayNode : LiteralNode
    {
        public ArrayNode(int nodeIndex, List<AstNode> values) : base("ARRAY", null, nodeIndex)
        {
            Values = values;
            this.AddChildren(values);
        }

        public List<AstNode> Values { get; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "[" + string.Join(",", this.Values.Select(c => "" + c).ToArray()) + "]";
        }
    }
}