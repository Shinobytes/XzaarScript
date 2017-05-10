using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class NodeProcessorVisitor : INodeProcessorVisitor
    {
        public virtual AstNode Visit(AstNode node)
        {
            if (node is LogicalConditionalNode) return Visit(node as LogicalConditionalNode);
            if (node is EqualityOperatorNode) return Visit(node as EqualityOperatorNode);
            if (node is BinaryOperatorNode) return Visit(node as BinaryOperatorNode);
            if (node is IfElseNode) return Visit(node as IfElseNode);
            if (node is FunctionParametersNode) return Visit(node as FunctionParametersNode);
            if (node is ReturnNode) return Visit(node as ReturnNode);
            if (node is MemberAccessNode) return Visit(node as MemberAccessNode);
            if (node is LogicalNotNode) return Visit(node as LogicalNotNode);
            if (node is AssignNode) return Visit(node as AssignNode);
            if (node is DoWhileLoopNode) return Visit(node as DoWhileLoopNode);
            if (node is WhileLoopNode) return Visit(node as WhileLoopNode);
            if (node is ForeachLoopNode) return Visit(node as ForeachLoopNode);
            if (node is ForLoopNode) return Visit(node as ForLoopNode);
            if (node is LoopNode) return Visit(node as LoopNode);
            if (node is VariableNode) return Visit(node as VariableNode);
            if (node is ParameterNode) return Visit(node as ParameterNode);
            if (node is FunctionCallNode) return Visit(node as FunctionCallNode);
            if (node is ExpressionNode) return Visit(node as ExpressionNode);
            if (node is ArgumentNode) return Visit(node as ArgumentNode);
            if (node is FunctionNode) return Visit(node as FunctionNode);
            if (node is NumberNode) return Visit(node as NumberNode);
            if (node is LiteralNode) return Visit(node as LiteralNode);
            if (node is BodyNode) return Visit(node as BodyNode);
            if (node is BlockNode) return Visit(node as BlockNode);
            if (node is EmptyNode) return Visit(node as EmptyNode);
            if (node is EntryNode) return Visit(node as EntryNode);
            if (node is LabelNode) return Visit(node as LabelNode);
            if (node is MatchNode) return Visit(node as MatchNode);
            if (node is CaseNode) return Visit(node as CaseNode);
            if (node is GotoNode) return Visit(node as GotoNode);
            if (node is BreakNode) return Visit(node as BreakNode);
            if (node is ContinueNode) return Visit(node as ContinueNode);
            if (node is MemberAccessChainNode) return Visit(node as MemberAccessChainNode);
            if (node is StructNode) return Visit(node as StructNode);
            if (node is FieldNode) return Visit(node as FieldNode);
            return Visit(node);
        }


        public virtual LogicalConditionalNode Visit(LogicalConditionalNode logicalConditional)
        {
            return logicalConditional;
        }

        public virtual EqualityOperatorNode Visit(EqualityOperatorNode equalityOperator)
        {
            return equalityOperator;
        }

        public virtual BinaryOperatorNode Visit(BinaryOperatorNode bin)
        {
            return bin;
        }

        public virtual IfElseNode Visit(IfElseNode ifElse)
        {
            return ifElse;
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

        public virtual LogicalNotNode Visit(LogicalNotNode node)
        {
            return node;
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

        public virtual MemberAccessChainNode Visit(MemberAccessChainNode node)
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