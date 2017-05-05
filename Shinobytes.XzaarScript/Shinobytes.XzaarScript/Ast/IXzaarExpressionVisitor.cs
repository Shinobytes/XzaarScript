using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript.Scripting
{
    public interface IXzaarExpressionVisitor
    {
        XzaarExpression Visit(XzaarExpression expression);
        BinaryExpression Visit(BinaryExpression binaryOp);
        ConditionalExpression Visit(ConditionalExpression conditional);
        MemberExpression Visit(MemberExpression access);
        ParameterExpression Visit(ParameterExpression parameter);

        GotoExpression Visit(GotoExpression @goto);

        LabelExpression Visit(LabelExpression label);
        SwitchExpression Visit(SwitchExpression match);
        SwitchCaseExpression Visit(SwitchCaseExpression matchCase);

        UnaryExpression Visit(UnaryExpression unary);

        StructExpression Visit(StructExpression node);
        FieldExpression Visit(FieldExpression node);

        ForExpression Visit(ForExpression @for);
        ForEachExpression Visit(ForEachExpression @foreach);
        DoWhileExpression Visit(DoWhileExpression doWhile);
        WhileExpression Visit(WhileExpression @while);
        LoopExpression Visit(LoopExpression loop);
        VariableDefinitionExpression Visit(VariableDefinitionExpression definedVariable);

        DefaultExpression Visit(DefaultExpression emptyOrNull);

        FunctionCallExpression Visit(FunctionCallExpression call);
        ConstantExpression Visit(ConstantExpression value);
        BlockExpression Visit(BlockExpression block);
        FunctionExpression Visit(FunctionExpression function);
    }
}