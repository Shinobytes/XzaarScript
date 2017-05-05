using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class FunctionCallNode : AstNode
    {
        private readonly AstNode function;

        public FunctionCallNode(AstNode instance, AstNode function, int nodeIndex, ArgumentNode[] args)
            : base(NodeTypes.CALL, "FUNCTION", function.Value, nodeIndex)
        {
            this.Instance = instance;
            this.function = function;
            if (args.Length > 0)
                this.AddChildren(args);
        }

        public ArgumentNode[] Arguments => this.Children != null && this.Children.Count > 0 ? this.Children.Cast<ArgumentNode>().ToArray() : new ArgumentNode[0];

        public AstNode Function => function;

        public AstNode Instance { get; set; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
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