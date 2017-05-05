using System.Linq;

namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class FunctionCallNode : XzaarAstNode
    {
        private readonly XzaarAstNode function;

        public FunctionCallNode(XzaarAstNode instance, XzaarAstNode function, int nodeIndex, ArgumentNode[] args)
            : base(XzaarAstNodeTypes.CALL, "FUNCTION", function.Value, nodeIndex)
        {
            this.Instance = instance;
            this.function = function;
            if (args.Length > 0)
                this.AddChildren(args);
        }

        public ArgumentNode[] Arguments => this.Children != null && this.Children.Count > 0 ? this.Children.Cast<ArgumentNode>().ToArray() : new ArgumentNode[0];

        public XzaarAstNode Function { get { return function; } }

        public XzaarAstNode Instance { get; set; }

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            //if (this.Instance != null)
            //{
            //    return this.Instance + "." + this.function + "(" +
            //           string.Join(", ", this.Arguments.Select(i => i.ToString())) + ")";
            //}
            return this.function + "(" +
                   string.Join(", ", this.Arguments.Select(x => x.ToString()).ToArray()) + ")";
        }        
    }
}