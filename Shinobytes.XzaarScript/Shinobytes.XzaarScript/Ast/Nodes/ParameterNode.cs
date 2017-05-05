namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class ParameterNode : XzaarAstNode
    {
        private bool isExtern;

        public ParameterNode(string name, string type, int nodeIndex)
            : base(XzaarAstNodeTypes.PARAMETER, null, null, nodeIndex)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public bool IsExtern
        {
            get { return isExtern; }
            set { isExtern = value; }
        }

        public override string ToString()
        {
            return "Parameter: (Type " + Type + ") " + Name;
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}