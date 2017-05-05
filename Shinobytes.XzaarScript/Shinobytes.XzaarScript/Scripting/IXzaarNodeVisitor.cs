using Shinobytes.XzaarScript.Scripting.Expressions;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public interface IXzaarNodeVisitor
    {
        ConditionalExpression Visit(ConditionalOperatorNode conditionalOperator);
        BinaryExpression Visit(EqualityOperatorNode equalityOperator);
        BinaryExpression Visit(BinaryOperatorNode bin);

        ConditionalExpression Visit(ConditionalNode conditional);
        XzaarExpression Visit(FunctionParametersNode parameters);
        XzaarExpression Visit(ReturnNode returnNode);
        XzaarExpression Visit(ContinueNode continueNode);
        XzaarExpression Visit(BreakNode breakNode);
        
        LabelExpression Visit(LabelNode label);
        GotoExpression Visit(GotoNode @goto);

        MemberAccessChainExpression Visit(MemberAccessChainNode access);

        CreateStructExpression Visit(CreateStructNode createStruct);

        MemberExpression Visit(MemberAccessNode access);
        BinaryExpression Visit(AssignNode assign);

        UnaryExpression Visit(UnaryNode unary);

        SwitchCaseExpression Visit(CaseNode matchCase);
        SwitchExpression Visit(MatchNode match);

        DoWhileExpression Visit(DoWhileLoopNode loop);
        WhileExpression Visit(WhileLoopNode loop);
        ForEachExpression Visit(ForeachLoopNode loop);
        ForExpression Visit(ForLoopNode loop);
        LoopExpression Visit(LoopNode loop);

        VariableDefinitionExpression Visit(DefineVariableNode definedVariable);

        ParameterExpression Visit(VariableNode variable);
        ParameterExpression Visit(ParameterNode parameter);
        FunctionCallExpression Visit(FunctionCallNode call);
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

        XzaarExpression Visit(XzaarNode node);
    }
}