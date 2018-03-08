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

using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser
{
    public interface INodeVisitor
    {
        XzaarExpression Visit(LogicalConditionalNode logicalConditional);
        XzaarExpression Visit(EqualityOperatorNode equalityOperator);
        XzaarExpression Visit(BinaryOperatorNode bin);
        XzaarExpression Visit(LogicalNotNode node);
        XzaarExpression Visit(IfElseNode ifElse);
        XzaarExpression Visit(FunctionParametersNode parameters);
        XzaarExpression Visit(ReturnNode returnNode);
        XzaarExpression Visit(ContinueNode continueNode);
        XzaarExpression Visit(BreakNode breakNode);
        XzaarExpression Visit(LabelNode label);
        XzaarExpression Visit(GotoNode @goto);
        XzaarExpression Visit(MemberAccessChainNode access);
        XzaarExpression Visit(CreateStructNode createStruct);
        XzaarExpression Visit(MemberAccessNode access);
        XzaarExpression Visit(AssignNode assign);
        XzaarExpression Visit(UnaryNode unary);
        XzaarExpression Visit(CaseNode @case);
        XzaarExpression Visit(MatchNode match);
        XzaarExpression Visit(DoWhileLoopNode loop);
        XzaarExpression Visit(WhileLoopNode loop);
        XzaarExpression Visit(ForeachLoopNode loop);
        XzaarExpression Visit(ForLoopNode loop);
        XzaarExpression Visit(ConditionalExpressionNode node);
        XzaarExpression Visit(LoopNode loop);
        XzaarExpression Visit(DefineVariableNode definedVariable);
        XzaarExpression Visit(VariableNode variable);
        XzaarExpression Visit(ParameterNode parameter);
        XzaarExpression Visit(FunctionCallNode call);
        XzaarExpression Visit(ExpressionNode expression);
        XzaarExpression Visit(ArgumentNode argument);
        XzaarExpression Visit(FunctionNode function);
        XzaarExpression Visit(StructNode node);
        XzaarExpression Visit(FieldNode node);
        XzaarExpression Visit(NumberNode number);
        XzaarExpression Visit(LiteralNode literal);
        BlockExpression Visit(BlockNode block);
        XzaarExpression Visit(EmptyNode empty);
        XzaarExpression Visit(EntryNode node);
        BlockExpression Visit(BodyNode body);
        XzaarExpression Visit(AstNode node);
    }
}