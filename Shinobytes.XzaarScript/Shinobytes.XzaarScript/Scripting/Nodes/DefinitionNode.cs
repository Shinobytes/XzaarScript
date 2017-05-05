namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class DefinitionNode : XzaarNode
    {
        public DefinitionNode(string type, int nodeIndex) 
            : base(XzaarNodeTypes.DECLARATION, type, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}