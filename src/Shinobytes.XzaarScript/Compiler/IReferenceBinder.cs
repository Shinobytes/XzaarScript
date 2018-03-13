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
 
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    // TODO(Kalle): Remove, it looks the same as ethe IScriptCompilerVisitor. Completely unecessary
    public interface IReferenceBinder
    {
        object Visit(XzaarExpression expression);
        object Visit(GotoExpression @goto);
        object Visit(MemberExpression member);
        object Visit(BinaryExpression binary);
        object Visit(MemberAccessChainExpression chain);
        object Visit(ForExpression node);
        object Visit(ForEachExpression node);
        object Visit(SwitchCaseExpression node);
        object Visit(DoWhileExpression node);
        object Visit(LoopExpression node);
        object Visit(SwitchExpression node);
        object Visit(CreateStructExpression node);
        object Visit(IfElseExpression node);
        VariableReference Visit(ConditionalExpression node);
        VariableReference Visit(ConstantExpression constant);
        VariableReference Visit(UnaryExpression unary);
        VariableReference Visit(FunctionCallExpression function);
        VariableDefinition Visit(VariableDefinitionExpression definedVariable);
        VariableDefinition Visit(FieldExpression field);
        Label Visit(LabelExpression label);
        ParameterDefinition Visit(ParameterExpression parameter);
        MethodDefinition Visit(FunctionExpression function);
        TypeDefinition Visit(StructExpression node);

    }
}