namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class DefineVariableNode : VariableNode
    {
        public DefineVariableNode(string type, string name, XzaarAstNode value, int nodeIndex)
            : base(type, name, value, true, nodeIndex)
        {
        }

        public XzaarAstNode AssignmentExpression
        {
            get { return Value as XzaarAstNode; }
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (Value != null)
            {
                if (Value is ArrayNode)
                {
                    var aNode = Value as ArrayNode;
                    return "var " + Name + " : " + Type + " = " + aNode;
                }
                return "var " + Name + " : " + Type + " = " + Value;
            }
            return "var " + Name;
        }

        public void SetType(string any)
        {
            this.Type = any;
        }

        public void SetValue(XzaarAstNode val)
        {
            this.Value = val;
        }
    }
}