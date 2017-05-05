namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class EmptyNode : XzaarAstNode
    {
        public EmptyNode(int nodeIndex) 
            : base(XzaarAstNodeTypes.NULL_EMPTY, null, null, nodeIndex)
        {
        }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return "";            
        }
    }
}