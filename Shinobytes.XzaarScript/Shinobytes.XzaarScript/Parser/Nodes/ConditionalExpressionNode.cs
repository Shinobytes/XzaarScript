using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ConditionalExpressionNode : AstNode
    {
        private readonly AstNode condition;
        private AstNode whenTrue;
        private AstNode whenFalse;

        public ConditionalExpressionNode(AstNode condition, AstNode whenTrue, AstNode whenFalse, int nodeIndex)
            : base(SyntaxKind.ConditionalExpression, "CONDITIONAL", null, nodeIndex)
        {
            if (condition != null) condition.Parent = this;
            if (whenTrue != null) whenTrue.Parent = this;
            if (whenFalse != null) whenFalse.Parent = this;
            this.condition = condition;
            this.whenTrue = whenTrue;
            this.whenFalse = whenFalse;
        }

        public AstNode GetCondition() => condition;

        public AstNode GetTrue() => whenTrue;

        public AstNode GetFalse() => whenFalse;


        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return GetCondition() == null && GetTrue() == null && GetFalse() == null;
        }

        public void SetWhenTrue(AstNode node)
        {
            whenTrue = node;
        }

        public void SetWhenFalse(AstNode node)
        {
            whenFalse = node;
        }


        public override string ToString()
        {
            var conditionString = condition.ToString();
            if (condition.Kind != SyntaxKind.Expression)
                conditionString = "(" + conditionString + ")";
            return $"{conditionString} ? {{{whenTrue}}} : {{{whenFalse}}}";
        }

    }
}