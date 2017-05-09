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
        LabelExpression Visit(LabelNode label);
        GotoExpression Visit(GotoNode @goto);
        MemberAccessChainExpression Visit(MemberAccessChainNode access);
        CreateStructExpression Visit(CreateStructNode createStruct);
        XzaarExpression Visit(MemberAccessNode access);
        BinaryExpression Visit(AssignNode assign);
        XzaarExpression Visit(UnaryNode unary);
        SwitchCaseExpression Visit(CaseNode @case);
        SwitchExpression Visit(MatchNode match);
        DoWhileExpression Visit(DoWhileLoopNode loop);
        WhileExpression Visit(WhileLoopNode loop);
        ForEachExpression Visit(ForeachLoopNode loop);
        ForExpression Visit(ForLoopNode loop);
        ConditionalExpression Visit(ConditionalExpressionNode node);
        LoopExpression Visit(LoopNode loop);
        VariableDefinitionExpression Visit(DefineVariableNode definedVariable);
        ParameterExpression Visit(VariableNode variable);
        ParameterExpression Visit(ParameterNode parameter);
        XzaarExpression Visit(FunctionCallNode call);
        XzaarExpression Visit(ExpressionNode expression);
        XzaarExpression Visit(ArgumentNode argument);
        XzaarExpression Visit(FunctionNode function);
        StructExpression Visit(StructNode node);
        FieldExpression Visit(FieldNode node);
        ConstantExpression Visit(NumberNode number);
        XzaarExpression Visit(LiteralNode literal);
        BlockExpression Visit(BlockNode block);
        XzaarExpression Visit(EmptyNode empty);
        XzaarExpression Visit(EntryNode node);
        BlockExpression Visit(BodyNode body);
        XzaarExpression Visit(AstNode node);
    }
}