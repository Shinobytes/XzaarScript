namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class LabelNode : XzaarAstNode
    {
        private readonly string name;

        public LabelNode(string name, int nodeIndex)
            : base(XzaarAstNodeTypes.LABEL, null, null, nodeIndex)
        {
            if (name.EndsWith(":")) name = name.Remove(name.Length - 1);
            this.name = name;
        }

        public string Name { get { return name; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "l_" + name + ":";
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}