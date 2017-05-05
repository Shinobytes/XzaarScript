using Shinobytes.XzaarScript.Scripting.Expressions;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class NullNodeVisitor : IXzaarNodeVisitor
    {
        public virtual ConditionalExpression Visit(ConditionalOperatorNode conditionalOperator)
        {
            return null;
        }

        public virtual BinaryExpression Visit(EqualityOperatorNode equalityOperator)
        {
            return null;
        }

        public virtual BinaryExpression Visit(BinaryOperatorNode bin)
        {
            return null;
        }

        public virtual ConditionalExpression Visit(ConditionalNode conditional)
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

        public virtual MemberExpression Visit(MemberAccessNode access)
        {
            return null;
        }

        public virtual BinaryExpression Visit(AssignNode assign)
        {
            return null;
        }

        public virtual UnaryExpression Visit(UnaryNode unary)
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

        public virtual FunctionCallExpression Visit(FunctionCallNode call)
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

        public XzaarExpression Visit(XzaarNode node)
        {
            var type = node.GetType();
            dynamic n = node;

            return Visit(n);
        }
    }
}