/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser
{
    public class NullNodeVisitor : INodeVisitor
    {
        public XzaarExpression Visit(LambdaNode lambda)
        {
            return null;
        }

        public virtual XzaarExpression Visit(LogicalConditionalNode logicalConditional)
        {
            return null;
        }

        public virtual XzaarExpression Visit(EqualityOperatorNode equalityOperator)
        {
            return null;
        }

        public virtual XzaarExpression Visit(BinaryOperatorNode bin)
        {
            return null;
        }

        public virtual XzaarExpression Visit(LogicalNotNode node)
        {
            return null;
        }

        public virtual XzaarExpression Visit(IfElseNode ifElse)
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

        public virtual XzaarExpression Visit(LabelNode label)
        {
            return null;
        }

        public virtual XzaarExpression Visit(GotoNode @goto)
        {
            return null;
        }

        public virtual XzaarExpression Visit(MemberAccessChainNode access)
        {
            return null;
        }

        public virtual XzaarExpression Visit(CreateStructNode createStruct)
        {
            return null;
        }

        public virtual XzaarExpression Visit(MemberAccessNode access)
        {
            return null;
        }

        public virtual XzaarExpression Visit(AssignNode assign)
        {
            return null;
        }

        public virtual XzaarExpression Visit(UnaryNode unary)
        {
            return null;
        }

        public virtual XzaarExpression Visit(CaseNode @case)
        {
            return null;
        }

        public virtual XzaarExpression Visit(MatchNode match)
        {
            return null;
        }

        public virtual XzaarExpression Visit(DoWhileLoopNode loop)
        {
            return null;
        }

        public virtual XzaarExpression Visit(WhileLoopNode loop)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ForeachLoopNode loop)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ForLoopNode loop)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ConditionalExpressionNode node)
        {
            return null;
        }

        public virtual XzaarExpression Visit(LoopNode loop)
        {
            return null;
        }

        public virtual XzaarExpression Visit(DefineVariableNode definedVariable)
        {
            return null;
        }

        public virtual XzaarExpression Visit(VariableNode variable)
        {
            return null;
        }

        public virtual XzaarExpression Visit(ParameterNode parameter)
        {
            return null;
        }

        public virtual XzaarExpression Visit(FunctionCallNode call)
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

        public virtual XzaarExpression Visit(StructNode node)
        {
            return null;
        }

        public virtual XzaarExpression Visit(FieldNode node)
        {
            return null;
        }

        public virtual XzaarExpression Visit(NumberNode number)
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

        public XzaarExpression Visit(AstNode node)
        {
            var type = node.GetType();
            // dynamic n = node;

            // return Visit(n);


            if (node is LogicalConditionalNode) return Visit(node as LogicalConditionalNode);
            if (node is EqualityOperatorNode) return Visit(node as EqualityOperatorNode);
            if (node is BinaryOperatorNode) return Visit(node as BinaryOperatorNode);
            if (node is LogicalNotNode) return Visit(node as LogicalNotNode);
            if (node is IfElseNode) return Visit(node as IfElseNode);
            if (node is FunctionParametersNode) return Visit(node as FunctionParametersNode);
            if (node is ReturnNode) return Visit(node as ReturnNode);
            if (node is ContinueNode) return Visit(node as ContinueNode);
            if (node is BreakNode) return Visit(node as BreakNode);
            if (node is LabelNode) return Visit(node as LabelNode);
            if (node is GotoNode) return Visit(node as GotoNode);
            if (node is MemberAccessChainNode) return Visit(node as MemberAccessChainNode);
            if (node is CreateStructNode) return Visit(node as CreateStructNode);
            if (node is MemberAccessNode) return Visit(node as MemberAccessNode);
            if (node is AssignNode) return Visit(node as AssignNode);
            if (node is UnaryNode) return Visit(node as UnaryNode);
            if (node is CaseNode) return Visit(node as CaseNode);
            if (node is MatchNode) return Visit(node as MatchNode);
            if (node is DoWhileLoopNode) return Visit(node as DoWhileLoopNode);
            if (node is WhileLoopNode) return Visit(node as WhileLoopNode);
            if (node is ForeachLoopNode) return Visit(node as ForeachLoopNode);
            if (node is ForLoopNode) return Visit(node as ForLoopNode);
            if (node is LoopNode) return Visit(node as LoopNode);
            if (node is DefineVariableNode) return Visit(node as DefineVariableNode);
            if (node is VariableNode) return Visit(node as VariableNode);
            if (node is ParameterNode) return Visit(node as ParameterNode);
            if (node is FunctionCallNode) return Visit(node as FunctionCallNode);
            if (node is ExpressionNode) return Visit(node as ExpressionNode);
            if (node is ArgumentNode) return Visit(node as ArgumentNode);
            if (node is FunctionNode) return Visit(node as FunctionNode);
            if (node is StructNode) return Visit(node as StructNode);
            if (node is FieldNode) return Visit(node as FieldNode);
            if (node is NumberNode) return Visit(node as NumberNode);
            if (node is LiteralNode) return Visit(node as LiteralNode);
            if (node is BodyNode) return Visit(node as BodyNode);
            if (node is BlockNode) return Visit(node as BlockNode);
            if (node is EmptyNode) return Visit(node as EmptyNode);
            if (node is EntryNode) return Visit(node as EntryNode);

            return Visit(node);
        }
    }
}