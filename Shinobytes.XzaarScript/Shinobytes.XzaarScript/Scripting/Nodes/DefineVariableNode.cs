namespace Shinobytes.XzaarScript.Scripting.Nodes
{   
    public class DefineVariableNode : VariableNode
    {
        public DefineVariableNode(string type, string name, XzaarNode value, int nodeIndex)
            : base(type, name, value, true, nodeIndex)
        {
        }

        public XzaarNode AssignmentExpression
        {
            get { return Value as XzaarNode; }
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
                    return "var " + Name + " : " + Type + " = []";
                }
                return "var " + Name + " : " + Type + " = " + Value;
            }
            return "var " + Name;
        }

    }
}