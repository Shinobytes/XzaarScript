using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript.Scripting
{
    public class ExpressionVisitor : IXzaarExpressionVisitor
    {
        public virtual XzaarExpression Visit(XzaarExpression expression)
        {
            return Visit((dynamic)expression);
        }

        public virtual BinaryExpression Visit(BinaryExpression binaryOp)
        {
            return binaryOp;
        }

        public virtual ConditionalExpression Visit(ConditionalExpression conditional)
        {
            return conditional;
        }

        public virtual MemberExpression Visit(MemberExpression access)
        {
            return access;
        }

        public virtual ParameterExpression Visit(ParameterExpression parameter)
        {
            return parameter;
        }

        public virtual GotoExpression Visit(GotoExpression @goto)
        {
            return @goto;
        }

        public virtual LabelExpression Visit(LabelExpression label)
        {
            return label;
        }

        public virtual SwitchExpression Visit(SwitchExpression match)
        {
            return match;
        }

        public virtual SwitchCaseExpression Visit(SwitchCaseExpression matchCase)
        {
            return matchCase;
        }

        public virtual UnaryExpression Visit(UnaryExpression unary)
        {
            return unary;
        }

        public virtual StructExpression Visit(StructExpression node)
        {
            return node;
        }

        public virtual FieldExpression Visit(FieldExpression node)
        {
            return node;
        }

        public virtual ForExpression Visit(ForExpression @for)
        {
            return @for;
        }

        public virtual ForEachExpression Visit(ForEachExpression @foreach)
        {
            return @foreach;
        }

        public virtual DoWhileExpression Visit(DoWhileExpression doWhile)
        {
            return doWhile;
        }

        public virtual WhileExpression Visit(WhileExpression @while)
        {
            return @while;
        }

        public virtual LoopExpression Visit(LoopExpression loop)
        {
            return loop;
        }

        public virtual VariableDefinitionExpression Visit(VariableDefinitionExpression definedVariable)
        {
            return definedVariable;
        }

        public virtual DefaultExpression Visit(DefaultExpression emptyOrNull)
        {
            return emptyOrNull;
        }

        public virtual FunctionCallExpression Visit(FunctionCallExpression call)
        {
            return call;
        }

        public virtual ConstantExpression Visit(ConstantExpression value)
        {
            return value;
        }

        public virtual BlockExpression Visit(BlockExpression block)
        {
            return block;
        }

        public virtual FunctionExpression Visit(FunctionExpression function)
        {
            return function;
        }
    }
}