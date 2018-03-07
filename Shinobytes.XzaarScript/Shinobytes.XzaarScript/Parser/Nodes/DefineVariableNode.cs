namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class DefineVariableNode : VariableNode
    {
        public DefineVariableNode(string type, string name, AstNode value, int nodeIndex)
            : base(type, name, value, true, nodeIndex)
        {
        }

        public AstNode AssignmentExpression => Value as AstNode;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
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

        public void SetValue(AstNode val)
        {
            this.Value = val;
        }
    }
}