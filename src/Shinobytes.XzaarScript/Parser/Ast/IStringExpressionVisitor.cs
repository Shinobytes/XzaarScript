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

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public interface IStringExpressionVisitor
    {
        string Visit(LambdaExpression lambda);
        string Visit(XzaarExpression expression);
        string Visit(ConditionalExpression expr);
        string Visit(BinaryExpression binaryOp);
        string Visit(IfElseExpression ifElse);
        string Visit(MemberExpression access);
        string Visit(ParameterExpression parameter);
        string Visit(GotoExpression @goto);
        string Visit(CreateStructExpression node);
        string Visit(LabelExpression label);
        string Visit(SwitchExpression match);
        string Visit(SwitchCaseExpression matchCase);
        string Visit(UnaryExpression unary);
        string Visit(StructExpression node);
        string Visit(FieldExpression node);
        string Visit(LogicalNotExpression node);
        string Visit(MemberAccessChainExpression node);
        string Visit(ForExpression @for);
        string Visit(ForEachExpression @foreach);
        string Visit(DoWhileExpression doWhile);
        string Visit(WhileExpression @while);
        string Visit(LoopExpression loop);
        string Visit(VariableDefinitionExpression definedVariable);
        string Visit(DefaultExpression emptyOrNull);
        string Visit(FunctionCallExpression call);
        string Visit(ConstantExpression value);
        string Visit(BlockExpression block);
        string Visit(FunctionExpression function);
    }
}