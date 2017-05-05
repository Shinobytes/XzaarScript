namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class DefinitionNode : XzaarAstNode
    {
        public DefinitionNode(string type, int nodeIndex) 
            : base(XzaarAstNodeTypes.DECLARATION, type, null, nodeIndex) { }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}