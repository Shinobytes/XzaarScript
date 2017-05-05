using System.Linq;

namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class FunctionCallNode : XzaarNode
    {
        private readonly XzaarNode instance;
        private readonly XzaarNode function;

        public FunctionCallNode(XzaarNode instance, XzaarNode function, int nodeIndex, params ArgumentNode[] args) 
            : base(XzaarNodeTypes.CALL, "FUNCTION", function.Value, nodeIndex)
        {
            this.instance = instance;
            this.function = function;
            if (args.Length > 0)
                this.AddChildren(args);
        }

        public ArgumentNode[] Arguments => this.Children != null && this.Children.Count > 0 ? this.Children.Cast<ArgumentNode>().ToArray() : new ArgumentNode[0];

        public XzaarNode Function { get { return function; } }

        public XzaarNode Instance { get { return instance; } }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }
    }
}