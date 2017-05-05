using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;
using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public interface IXzaarDiscoveryVisitor
    {
        object Visit(XzaarExpression expression);
        object Visit(GotoExpression @goto);
        object Visit(MemberExpression member);
        object Visit(BinaryExpression binary);
        object Visit(MemberAccessChainExpression chain);
        object Visit(ForExpression node);
        object Visit(ForEachExpression node);
        object Visit(SwitchCaseExpression node);
        object Visit(DoWhileExpression node);
        object Visit(LoopExpression node);
        object Visit(SwitchExpression node);
        object Visit(CreateStructExpression node);
        object Visit(ConditionalExpression node);
        VariableReference Visit(ConstantExpression constant);
        VariableReference Visit(UnaryExpression unary);
        VariableReference Visit(FunctionCallExpression function);
        VariableDefinition Visit(VariableDefinitionExpression definedVariable);
        VariableDefinition Visit(FieldExpression field);
        Label Visit(LabelExpression label);
        ParameterDefinition Visit(ParameterExpression parameter);
        MethodDefinition Visit(FunctionExpression function);
        TypeDefinition Visit(StructExpression node);

    }
}