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
    public interface INodeVisitor
    {
        XzaarExpression Visit(LambdaNode lambda);
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