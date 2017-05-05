using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public interface IXzaarNodeProcessorVisitor
    {
        ConditionalOperatorNode Visit(ConditionalOperatorNode conditionalOperator);
        EqualityOperatorNode Visit(EqualityOperatorNode equalityOperator);
        BinaryOperatorNode Visit(BinaryOperatorNode bin);
        ConditionalNode Visit(ConditionalNode conditional);
        FunctionParametersNode Visit(FunctionParametersNode parameters);
        ReturnNode Visit(ReturnNode returnNode);
        MemberAccessNode Visit(MemberAccessNode access);
        AssignNode Visit(AssignNode assign);
        DoWhileLoopNode Visit(DoWhileLoopNode loop);
        WhileLoopNode Visit(WhileLoopNode loop);
        ForeachLoopNode Visit(ForeachLoopNode loop);
        ForLoopNode Visit(ForLoopNode loop);
        LoopNode Visit(LoopNode loop);
        VariableNode Visit(VariableNode variable);
        ParameterNode Visit(ParameterNode parameter);
        FunctionCallNode Visit(FunctionCallNode call);
        ExpressionNode Visit(ExpressionNode expression);
        ArgumentNode Visit(ArgumentNode argument);
        FunctionNode Visit(FunctionNode function);
        NumberNode Visit(NumberNode number);
        LiteralNode Visit(LiteralNode literal);
        BlockNode Visit(BlockNode block);
        EmptyNode Visit(EmptyNode empty);
        EntryNode Visit(EntryNode node);
        BodyNode Visit(BodyNode body);
        XzaarNode Visit(XzaarNode node);
        LabelNode Visit(LabelNode node);
        MatchNode Visit(MatchNode match);
        CaseNode Visit(CaseNode matchCase);
        GotoNode Visit(GotoNode node);
        BreakNode Visit(BreakNode node);
        ContinueNode Visit(ContinueNode node);

        StructNode Visit(StructNode node);
        FieldNode Visit(FieldNode node);        
    }
}