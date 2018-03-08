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