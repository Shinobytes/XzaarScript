namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class LabelNode : AstNode
    {
        private readonly string name;

        public LabelNode(string name, int nodeIndex)
            : base(NodeTypes.LABEL, null, null, nodeIndex)
        {
            if (name.EndsWith(":")) name = name.Remove(name.Length - 1);
            this.name = name;
        }

        public string Name => name;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
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