using Shinobytes.XzaarScript.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser
{
    public interface INodeVisitor
    {
        XzaarExpression Visit(LogicalConditionalNode logicalConditional);
        XzaarExpression Visit(EqualityOperatorNode equalityOperator);
        XzaarExpression Visit(BinaryOperatorNode bin);
        XzaarExpression Visit(LogicalNotNode node);
        XzaarExpression Visit(IfElseNode ifElse);
        XzaarExpression Visit(FunctionParametersNode parameters);
        XzaarExpression Visit(ReturnNode returnNode);
        XzaarExpression Visit(ContinueNode continueNode);
        XzaarExpression Visit(BreakNode breakNode);
        XzaarExpression Visit(LabelNode label);
        XzaarExpression Visit(GotoNode @goto);
        XzaarExpression Visit(MemberAccessChainNode access);
        XzaarExpression Visit(CreateStructNode createStruct);
        XzaarExpression Visit(MemberAccessNode access);
        XzaarExpression Visit(AssignNode assign);
        XzaarExpression Visit(UnaryNode unary);
        XzaarExpression Visit(CaseNode @case);
        XzaarExpression Visit(MatchNode match);
        XzaarExpression Visit(DoWhileLoopNode loop);
        XzaarExpression Visit(WhileLoopNode loop);
        XzaarExpression Visit(ForeachLoopNode loop);
        XzaarExpression Visit(ForLoopNode loop);
        XzaarExpression Visit(ConditionalExpressionNode node);
        XzaarExpression Visit(LoopNode loop);
        XzaarExpression Visit(DefineVariableNode definedVariable);
        XzaarExpression Visit(VariableNode variable);
        XzaarExpression Visit(ParameterNode parameter);
        XzaarExpression Visit(FunctionCallNode call);
        XzaarExpression Visit(ExpressionNode expression);
        XzaarExpression Visit(ArgumentNode argument);
        XzaarExpression Visit(FunctionNode function);
        XzaarExpression Visit(StructNode node);
        XzaarExpression Visit(FieldNode node);
        XzaarExpression Visit(NumberNode number);
        XzaarExpression Visit(LiteralNode literal);
        BlockExpression Visit(BlockNode block);
        XzaarExpression Visit(EmptyNode empty);
        XzaarExpression Visit(EntryNode node);
        BlockExpression Visit(BodyNode body);
        XzaarExpression Visit(AstNode node);
    }
}