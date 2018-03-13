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