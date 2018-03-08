/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

using System;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public class NodeProcessorVisitor : INodeProcessorVisitor
    {
        public virtual AstNode Visit(AstNode node)
        {
            if (node is LogicalConditionalNode conditionalNode) return Visit(conditionalNode);
            if (node is EqualityOperatorNode operatorNode) return Visit(operatorNode);
            if (node is BinaryOperatorNode binaryOperatorNode) return Visit(binaryOperatorNode);
            if (node is IfElseNode elseNode) return Visit(elseNode);
            if (node is FunctionParametersNode parametersNode) return Visit(parametersNode);
            if (node is ReturnNode returnNode) return Visit(returnNode);
            if (node is MemberAccessNode accessNode) return Visit(accessNode);
            if (node is LogicalNotNode notNode) return Visit(notNode);
            if (node is AssignNode assignNode) return Visit(assignNode);
            if (node is DoWhileLoopNode doWhileLoopNode) return Visit(doWhileLoopNode);
            if (node is WhileLoopNode whileLoopNode) return Visit(whileLoopNode);
            if (node is ForeachLoopNode foreachLoopNode) return Visit(foreachLoopNode);
            if (node is ForLoopNode forLoopNode) return Visit(forLoopNode);
            if (node is LoopNode loopNode) return Visit(loopNode);
            if (node is VariableNode variableNode) return Visit(variableNode);
            if (node is ParameterNode parameterNode) return Visit(parameterNode);
            if (node is FunctionCallNode callNode) return Visit(callNode);
            if (node is ExpressionNode expressionNode) return Visit(expressionNode);
            if (node is ArgumentNode argumentNode) return Visit(argumentNode);
            if (node is FunctionNode functionNode) return Visit(functionNode);
            if (node is NumberNode numberNode) return Visit(numberNode);
            if (node is LiteralNode literalNode) return Visit(literalNode);
            if (node is BodyNode bodyNode) return Visit(bodyNode);
            if (node is BlockNode blockNode) return Visit(blockNode);
            if (node is EmptyNode emptyNode) return Visit(emptyNode);
            if (node is EntryNode entryNode) return Visit(entryNode);
            if (node is LabelNode labelNode) return Visit(labelNode);
            if (node is MatchNode matchNode) return Visit(matchNode);
            if (node is CaseNode caseNode) return Visit(caseNode);
            if (node is GotoNode gotoNode) return Visit(gotoNode);
            if (node is BreakNode breakNode) return Visit(breakNode);
            if (node is ContinueNode continueNode) return Visit(continueNode);
            if (node is MemberAccessChainNode chainNode) return Visit(chainNode);
            if (node is StructNode structNode) return Visit(structNode);
            if (node is FieldNode fieldNode) return Visit(fieldNode);

            throw new StackOverflowException();
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