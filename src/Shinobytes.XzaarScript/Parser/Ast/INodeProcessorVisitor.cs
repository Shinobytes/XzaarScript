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

using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public interface INodeProcessorVisitor
    {
        AstNode Visit(AstNode node);

        LogicalConditionalNode Visit(LogicalConditionalNode logicalConditional);
        EqualityOperatorNode Visit(EqualityOperatorNode equalityOperator);
        BinaryOperatorNode Visit(BinaryOperatorNode bin);
        IfElseNode Visit(IfElseNode ifElse);
        FunctionParametersNode Visit(FunctionParametersNode parameters);
        ReturnNode Visit(ReturnNode returnNode);
        MemberAccessNode Visit(MemberAccessNode access);
        LogicalNotNode Visit(LogicalNotNode node);
        AssignNode Visit(AssignNode assign);
        DoWhileLoopNode Visit(DoWhileLoopNode loop);
        WhileLoopNode Visit(WhileLoopNode loop);
        ForeachLoopNode Visit(ForeachLoopNode loop);
        ForLoopNode Visit(ForLoopNode loop);
        LoopNode Visit(LoopNode loop);
        VariableNode Visit(VariableNode variable);
        ParameterNode Visit(ParameterNode parameter);
        FunctionCallNode Visit(FunctionCallNode call);
        ExpressionNode Visit(ExpressionNode expression);
        ArgumentNode Visit(ArgumentNode argument);
        FunctionNode Visit(FunctionNode function);
        NumberNode Visit(NumberNode number);
        LiteralNode Visit(LiteralNode literal);
        BlockNode Visit(BlockNode block);
        EmptyNode Visit(EmptyNode empty);
        EntryNode Visit(EntryNode node);
        BodyNode Visit(BodyNode body);
        LabelNode Visit(LabelNode node);
        MatchNode Visit(MatchNode match);
        CaseNode Visit(CaseNode matchCase);
        GotoNode Visit(GotoNode node);
        BreakNode Visit(BreakNode node);
        ContinueNode Visit(ContinueNode node);
        MemberAccessChainNode Visit(MemberAccessChainNode node);
        StructNode Visit(StructNode node);
        FieldNode Visit(FieldNode node);        
    }
}