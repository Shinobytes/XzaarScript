using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class CreateStructNode : AstNode
    {        
        private readonly StructNode structNode;

        public CreateStructNode(StructNode structNode, int nodeIndex)
            : base(SyntaxKind.TypeInstantiation, "CREATE_STRUCT", null, nodeIndex)
        {
            this.structNode = structNode;
            this.Type = this.structNode.ValueText;
        }

        public CreateStructNode(StructNode structNode, AstNode[] structFieldInitializers, int nodeIndex)
            : base(SyntaxKind.TypeInstantiation, "CREATE_STRUCT", null, nodeIndex)
        {
            FieldInitializers = structFieldInitializers;
            this.structNode = structNode;
            this.Type = this.structNode.ValueText;
        }

        public AstNode[] FieldInitializers { get; set; }

        public StructNode StructNode => structNode;

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
            return "new_struct " + this.structNode.Name;
        }
    }
}