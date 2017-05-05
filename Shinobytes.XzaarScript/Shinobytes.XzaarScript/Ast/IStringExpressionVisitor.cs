using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Ast
{
    public interface IStringExpressionVisitor
    {
        string Visit(XzaarExpression expression);
        string Visit(BinaryExpression binaryOp);
        string Visit(ConditionalExpression conditional);
        string Visit(MemberExpression access);
        string Visit(ParameterExpression parameter);
        string Visit(GotoExpression @goto);
        string Visit(CreateStructExpression node);
        string Visit(LabelExpression label);
        string Visit(SwitchExpression match);
        string Visit(SwitchCaseExpression matchCase);
        string Visit(UnaryExpression unary);
        string Visit(StructExpression node);
        string Visit(FieldExpression node);
        string Visit(LogicalNotExpression node);
        string Visit(MemberAccessChainExpression node);
        string Visit(ForExpression @for);
        string Visit(ForEachExpression @foreach);
        string Visit(DoWhileExpression doWhile);
        string Visit(WhileExpression @while);
        string Visit(LoopExpression loop);
        string Visit(VariableDefinitionExpression definedVariable);
        string Visit(DefaultExpression emptyOrNull);
        string Visit(FunctionCallExpression call);
        string Visit(ConstantExpression value);
        string Visit(BlockExpression block);
        string Visit(FunctionExpression function);
    }
}