using Shinobytes.XzaarScript.Ast.Expressions;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class NullNodeVisitor : IXzaarNodeVisitor
    {
        public virtual XzaarExpression Visit(ConditionalOperatorNode conditionalOperator)
        {
            return null;
        }

        public virtual XzaarExpression Visit(EqualityOperatorNode equalityOperator)
        {
            return null;
        }

        public virtual XzaarExpression Visit(BinaryOperatorNode bin)
        {
            return null;
        }

        public virtual XzaarExpression Visit(LogicalNotNode node)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ConditionalNode conditional)
        {
            return null;
        }

        public virtual XzaarExpression Visit(FunctionParametersNode parameters)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ReturnNode returnNode)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ContinueNode continueNode)
        {
            return null;
        }

        public virtual XzaarExpression Visit(BreakNode breakNode)
        {
            return null;
        }

        public virtual LabelExpression Visit(LabelNode label)
        {
            return null;
        }

        public virtual GotoExpression Visit(GotoNode @goto)
        {
            return null;
        }

        public virtual MemberAccessChainExpression Visit(MemberAccessChainNode access)
        {
            return null;
        }

        public virtual CreateStructExpression Visit(CreateStructNode createStruct)
        {
            return null;
        }

        public virtual XzaarExpression Visit(MemberAccessNode access)
        {
            return null;
        }

        public virtual BinaryExpression Visit(AssignNode assign)
        {
            return null;
        }

        public virtual XzaarExpression Visit(UnaryNode unary)
        {
            return null;
        }

        public virtual SwitchCaseExpression Visit(CaseNode matchCase)
        {
            return null;
        }

        public virtual SwitchExpression Visit(MatchNode match)
        {
            return null;
        }

        public virtual DoWhileExpression Visit(DoWhileLoopNode loop)
        {
            return null;
        }

        public virtual WhileExpression Visit(WhileLoopNode loop)
        {
            return null;
        }

        public virtual ForEachExpression Visit(ForeachLoopNode loop)
        {
            return null;
        }

        public virtual ForExpression Visit(ForLoopNode loop)
        {
            return null;
        }

        public virtual LoopExpression Visit(LoopNode loop)
        {
            return null;
        }

        public virtual VariableDefinitionExpression Visit(DefineVariableNode definedVariable)
        {
            return null;
        }

        public virtual ParameterExpression Visit(VariableNode variable)
        {
            return null;
        }

        public virtual ParameterExpression Visit(ParameterNode parameter)
        {
            return null;
        }

        public virtual XzaarExpression Visit(FunctionCallNode call)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ExpressionNode expression)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ArgumentNode argument)
        {
            return null;
        }

        public virtual XzaarExpression Visit(FunctionNode function)
        {
            return null;
        }

        public virtual StructExpression Visit(StructNode node)
        {
            return null;
        }

        public virtual FieldExpression Visit(FieldNode node)
        {
            return null;
        }

        public virtual ConstantExpression Visit(NumberNode number)
        {
            return null;
        }

        public virtual XzaarExpression Visit(LiteralNode literal)
        {
            return null;
        }

        public virtual BlockExpression Visit(BlockNode block)
        {
            return null;
        }

        public virtual XzaarExpression Visit(EmptyNode empty)
        {
            return null;
        }

        public virtual XzaarExpression Visit(EntryNode node)
        {
            return null;
        }

        public virtual BlockExpression Visit(BodyNode body)
        {
            return null;
        }

        public XzaarExpression Visit(XzaarAstNode node)
        {
            var type = node.GetType();
            // dynamic n = node;

            // return Visit(n);


            if (node is ConditionalOperatorNode) return Visit(node as ConditionalOperatorNode);
            //XzaarExpression Visit(ConditionalOperatorNode conditionalOperator);
            if (node is EqualityOperatorNode) return Visit(node as EqualityOperatorNode);
            //XzaarExpression Visit(EqualityOperatorNode equalityOperator);
            if (node is BinaryOperatorNode) return Visit(node as BinaryOperatorNode);
            //XzaarExpression Visit(BinaryOperatorNode bin);
            if (node is LogicalNotNode) return Visit(node as LogicalNotNode);
            //XzaarExpression Visit(LogicalNotNode node);
            if (node is ConditionalNode) return Visit(node as ConditionalNode);
            //XzaarExpression Visit(ConditionalNode conditional);
            if (node is FunctionParametersNode) return Visit(node as FunctionParametersNode);
            //XzaarExpression Visit(FunctionParametersNode parameters);
            if (node is ReturnNode) return Visit(node as ReturnNode);
            //XzaarExpression Visit(ReturnNode returnNode);
            if (node is ContinueNode) return Visit(node as ContinueNode);
            //XzaarExpression Visit(ContinueNode continueNode);
            if (node is BreakNode) return Visit(node as BreakNode);
            //XzaarExpression Visit(BreakNode breakNode);
            if (node is LabelNode) return Visit(node as LabelNode);
            //LabelExpression Visit(LabelNode label);
            if (node is GotoNode) return Visit(node as GotoNode);
            //GotoExpression Visit(GotoNode @goto);
            if (node is MemberAccessChainNode) return Visit(node as MemberAccessChainNode);
            //MemberAccessChainExpression Visit(MemberAccessChainNode access);
            if (node is CreateStructNode) return Visit(node as CreateStructNode);
            //CreateStructExpression Visit(CreateStructNode createStruct);
            if (node is MemberAccessNode) return Visit(node as MemberAccessNode);
            //XzaarExpression Visit(MemberAccessNode access);
            if (node is AssignNode) return Visit(node as AssignNode);
            //BinaryExpression Visit(AssignNode assign);
            if (node is UnaryNode) return Visit(node as UnaryNode);
            //XzaarExpression Visit(UnaryNode unary);
            if (node is CaseNode) return Visit(node as CaseNode);
            //SwitchCaseExpression Visit(CaseNode matchCase);
            if (node is MatchNode) return Visit(node as MatchNode);
            //SwitchExpression Visit(MatchNode match);
            if (node is DoWhileLoopNode) return Visit(node as DoWhileLoopNode);
            //DoWhileExpression Visit(DoWhileLoopNode loop);
            if (node is WhileLoopNode) return Visit(node as WhileLoopNode);
            //WhileExpression Visit(WhileLoopNode loop);
            if (node is ForeachLoopNode) return Visit(node as ForeachLoopNode);
            //ForEachExpression Visit(ForeachLoopNode loop);
            if (node is ForLoopNode) return Visit(node as ForLoopNode);
            //ForExpression Visit(ForLoopNode loop);
            if (node is LoopNode) return Visit(node as LoopNode);
            //LoopExpression Visit(LoopNode loop);
            if (node is DefineVariableNode) return Visit(node as DefineVariableNode);
            //VariableDefinitionExpression Visit(DefineVariableNode definedVariable);
            if (node is VariableNode) return Visit(node as VariableNode);
            //ParameterExpression Visit(VariableNode variable);
            if (node is ParameterNode) return Visit(node as ParameterNode);
            //ParameterExpression Visit(ParameterNode parameter);
            if (node is FunctionCallNode) return Visit(node as FunctionCallNode);
            //XzaarExpression Visit(FunctionCallNode call);
            if (node is ExpressionNode) return Visit(node as ExpressionNode);
            //XzaarExpression Visit(ExpressionNode expression);
            if (node is ArgumentNode) return Visit(node as ArgumentNode);
            //XzaarExpression Visit(ArgumentNode argument);
            if (node is FunctionNode) return Visit(node as FunctionNode);
            //XzaarExpression Visit(FunctionNode function);
            if (node is StructNode) return Visit(node as StructNode);
            //StructExpression Visit(StructNode node);
            if (node is FieldNode) return Visit(node as FieldNode);
            //FieldExpression Visit(FieldNode node);
            if (node is NumberNode) return Visit(node as NumberNode);
            //ConstantExpression Visit(NumberNode number);
            if (node is LiteralNode) return Visit(node as LiteralNode);
            //XzaarExpression Visit(LiteralNode literal);
            if (node is BodyNode) return Visit(node as BodyNode);
            //BlockExpression Visit(BodyNode body);
            if (node is BlockNode) return Visit(node as BlockNode);
            //BlockExpression Visit(BlockNode block);
            if (node is EmptyNode) return Visit(node as EmptyNode);
            //XzaarExpression Visit(EmptyNode empty);
            if (node is EntryNode) return Visit(node as EntryNode);

            return Visit(node);
        }
    }
}