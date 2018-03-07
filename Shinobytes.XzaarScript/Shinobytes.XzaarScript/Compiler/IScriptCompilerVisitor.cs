using System.Runtime.Serialization;
using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public interface IScriptCompilerVisitor
    {
        object Visit(XzaarExpression expression);
        object Visit(ConditionalExpression expr);
        object Visit(LogicalNotExpression expr);
        object Visit(BinaryExpression binaryOp);
        object Visit(IfElseExpression ifElse);
        object Visit(MemberExpression access);
        object Visit(MemberAccessChainExpression access);
        object Visit(GotoExpression @goto);
        object Visit(SwitchExpression match);
        object Visit(SwitchCaseExpression matchCase);
        object Visit(UnaryExpression unary);
        object Visit(BlockExpression block);
        object Visit(ForExpression @for);
        object Visit(ForEachExpression @foreach);
        object Visit(DoWhileExpression doWhile);
        object Visit(WhileExpression @while);
        object Visit(LoopExpression loop);
        object Visit(DefaultExpression emptyOrNull);
        object Visit(FunctionCallExpression call);
        object Visit(ConstantExpression value);
        object Visit(NegateExpression value);
        object Visit(VariableDefinitionExpression definedVariable);
        object Visit(LabelExpression label);
        object Visit(ParameterExpression parameter);
        object Visit(FunctionExpression function);
        object Visit(StructExpression node);
        object Visit(FieldExpression node);
    }
}