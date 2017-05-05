using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class NodeProcessorVisitor : IXzaarNodeProcessorVisitor
    {
        public virtual ConditionalOperatorNode Visit(ConditionalOperatorNode conditionalOperator)
        {
            return conditionalOperator;
        }

        public virtual EqualityOperatorNode Visit(EqualityOperatorNode equalityOperator)
        {
            return equalityOperator;
        }

        public virtual BinaryOperatorNode Visit(BinaryOperatorNode bin)
        {
            return bin;
        }

        public virtual ConditionalNode Visit(ConditionalNode conditional)
        {
            return conditional;
        }

        public virtual FunctionParametersNode Visit(FunctionParametersNode parameters)
        {
            return parameters;
        }

        public virtual ReturnNode Visit(ReturnNode returnNode)
        {
            return returnNode;            
        }

        public virtual MemberAccessNode Visit(MemberAccessNode access)
        {
            return access;
        }

        public virtual AssignNode Visit(AssignNode assign)
        {
            return assign;
        }

        public virtual DoWhileLoopNode Visit(DoWhileLoopNode loop)
        {
            return loop;
        }

        public virtual WhileLoopNode Visit(WhileLoopNode loop)
        {
            return loop;
        }

        public virtual ForeachLoopNode Visit(ForeachLoopNode loop)
        {
            return loop;
        }

        public virtual ForLoopNode Visit(ForLoopNode loop)
        {
            return loop;
        }

        public virtual LoopNode Visit(LoopNode loop)
        {
            return loop;
        }

        public virtual VariableNode Visit(VariableNode variable)
        {
            return variable;
        }

        public virtual ParameterNode Visit(ParameterNode parameter)
        {
            return parameter;
        }

        public virtual FunctionCallNode Visit(FunctionCallNode call)
        {
            return call;
        }

        public virtual ExpressionNode Visit(ExpressionNode expression)
        {
            return expression;
        }

        public virtual ArgumentNode Visit(ArgumentNode argument)
        {
            return argument;
        }

        public virtual FunctionNode Visit(FunctionNode function)
        {
            return function;
        }

        public virtual NumberNode Visit(NumberNode number)
        {
            return number;
        }

        public virtual LiteralNode Visit(LiteralNode literal)
        {
            return literal;
        }

        public virtual BlockNode Visit(BlockNode block)
        {
            return block;
        }

        public virtual EmptyNode Visit(EmptyNode empty)
        {
            return empty;
        }

        public virtual EntryNode Visit(EntryNode node)
        {
            return node;
        }

        public virtual BodyNode Visit(BodyNode body)
        {
            return body;
        }

        public virtual XzaarNode Visit(XzaarNode node)
        {
            return Visit((dynamic)node);
        }

        public virtual LabelNode Visit(LabelNode node)
        {
            return node;
        }

        public virtual MatchNode Visit(MatchNode match)
        {
            return match;
        }

        public virtual CaseNode Visit(CaseNode matchCase)
        {
            return matchCase;
        }

        public virtual ContinueNode Visit(ContinueNode node)
        {
            return node;
        }
        public virtual BreakNode Visit(BreakNode node)
        {
            return node;
        }

        public virtual GotoNode Visit(GotoNode node)
        {
            return node;
        }

        public virtual StructNode Visit(StructNode node)
        {
            return node;
        }

        public virtual FieldNode Visit(FieldNode node)
        {
            return node;
        }
    }
}