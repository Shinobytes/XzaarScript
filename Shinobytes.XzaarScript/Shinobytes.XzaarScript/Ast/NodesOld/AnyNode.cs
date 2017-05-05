namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class AnyNode : XzaarAstNode
    {
        public AnyNode(XzaarAstNodeTypes nodeType, string nodeName, object value, int nodeIndex)
            : base(nodeType, nodeName, value, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return (this.Children == null || this.Children.Count == 0) && this.Value == null && NodeName == null;
        }
    }
}