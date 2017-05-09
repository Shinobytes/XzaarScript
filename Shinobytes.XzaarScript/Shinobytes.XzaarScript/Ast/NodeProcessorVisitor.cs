using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class NodeProcessorVisitor : INodeProcessorVisitor
    {
        public virtual AstNode Visit(AstNode node)
        {
            if (node is LogicalConditionalNode) return Visit(node as LogicalConditionalNode);
            //ConditionalOperatorNode Visit(ConditionalOperatorNode conditionalOperator);
            if (node is EqualityOperatorNode) return Visit(node as EqualityOperatorNode);
            //EqualityOperatorNode Visit(EqualityOperatorNode equalityOperator);
            if (node is BinaryOperatorNode) return Visit(node as BinaryOperatorNode);
            //BinaryOperatorNode Visit(BinaryOperatorNode bin);
            if (node is IfElseNode) return Visit(node as IfElseNode);
            //IfElseNode Visit(IfElseNode ifElse);
            if (node is FunctionParametersNode) return Visit(node as FunctionParametersNode);
            //FunctionParametersNode Visit(FunctionParametersNode parameters);
            if (node is ReturnNode) return Visit(node as ReturnNode);
            //ReturnNode Visit(ReturnNode returnNode);
            if (node is MemberAccessNode) return Visit(node as MemberAccessNode);
            //MemberAccessNode Visit(MemberAccessNode access);
            if (node is LogicalNotNode) return Visit(node as LogicalNotNode);
            //LogicalNotNode Visit(LogicalNotNode node);
            if (node is AssignNode) return Visit(node as AssignNode);
            //AssignNode Visit(AssignNode assign);
            if (node is DoWhileLoopNode) return Visit(node as DoWhileLoopNode);
            //DoWhileLoopNode Visit(DoWhileLoopNode loop);
            if (node is WhileLoopNode) return Visit(node as WhileLoopNode);
            //WhileLoopNode Visit(WhileLoopNode loop);
            if (node is ForeachLoopNode) return Visit(node as ForeachLoopNode);
            //ForeachLoopNode Visit(ForeachLoopNode loop);
            if (node is ForLoopNode) return Visit(node as ForLoopNode);
            //ForLoopNode Visit(ForLoopNode loop);
            if (node is LoopNode) return Visit(node as LoopNode);
            //LoopNode Visit(LoopNode loop);
            if (node is VariableNode) return Visit(node as VariableNode);
            //VariableNode Visit(VariableNode variable);
            if (node is ParameterNode) return Visit(node as ParameterNode);
            //ParameterNode Visit(ParameterNode parameter);
            if (node is FunctionCallNode) return Visit(node as FunctionCallNode);
            //FunctionCallNode Visit(FunctionCallNode call);
            if (node is ExpressionNode) return Visit(node as ExpressionNode);
            //ExpressionNode Visit(ExpressionNode expression);
            if (node is ArgumentNode) return Visit(node as ArgumentNode);
            //ArgumentNode Visit(ArgumentNode argument);
            if (node is FunctionNode) return Visit(node as FunctionNode);
            //FunctionNode Visit(FunctionNode function);
            if (node is NumberNode) return Visit(node as NumberNode);
            //NumberNode Visit(NumberNode number);
            if (node is LiteralNode) return Visit(node as LiteralNode);
            //LiteralNode Visit(LiteralNode literal);
            if (node is BodyNode) return Visit(node as BodyNode);
            //BodyNode Visit(BodyNode body);
            if (node is BlockNode) return Visit(node as BlockNode);
            //BlockNode Visit(BlockNode block);
            if (node is EmptyNode) return Visit(node as EmptyNode);
            //EmptyNode Visit(EmptyNode empty);
            if (node is EntryNode) return Visit(node as EntryNode);
            //EntryNode Visit(EntryNode node);         
            if (node is LabelNode) return Visit(node as LabelNode);
            //LabelNode Visit(LabelNode node);
            if (node is MatchNode) return Visit(node as MatchNode);
            //MatchNode Visit(MatchNode match);
            if (node is CaseNode) return Visit(node as CaseNode);
            //CaseNode Visit(CaseNode matchCase);
            if (node is GotoNode) return Visit(node as GotoNode);
            //GotoNode Visit(GotoNode node);
            if (node is BreakNode) return Visit(node as BreakNode);
            //BreakNode Visit(BreakNode node);
            if (node is ContinueNode) return Visit(node as ContinueNode);
            //ContinueNode Visit(ContinueNode node);
            if (node is MemberAccessChainNode) return Visit(node as MemberAccessChainNode);
            //MemberAccessChainNode Visit(MemberAccessChainNode node);
            if (node is StructNode) return Visit(node as StructNode);
            //StructNode Visit(StructNode node);
            if (node is FieldNode) return Visit(node as FieldNode);
            //FieldNode Visit(FieldNode node);
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