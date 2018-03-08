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

namespace Shinobytes.XzaarScript.Compiler
{
    public interface IScriptCompilerVisitor
    {
        object Visit(XzaarExpression expression);
        object Visit(ConditionalExpression expr);
        object Visit(LogicalNotExpression expr);
        object Visit(BinaryExpression binaryOp);
        object Visit(IfElseExpression ifElse);
        object Visit(MemberExpression access);
        object Visit(MemberAccessChainExpression access);
        object Visit(GotoExpression @goto);
        object Visit(SwitchExpression match);
        object Visit(SwitchCaseExpression matchCase);
        object Visit(UnaryExpression unary);
        object Visit(BlockExpression block);
        object Visit(ForExpression @for);
        object Visit(ForEachExpression @foreach);
        object Visit(DoWhileExpression doWhile);
        object Visit(WhileExpression @while);
        object Visit(LoopExpression loop);
        object Visit(DefaultExpression emptyOrNull);
        object Visit(FunctionCallExpression call);
        object Visit(ConstantExpression value);
        object Visit(NegateExpression value);
        object Visit(VariableDefinitionExpression definedVariable);
        object Visit(LabelExpression label);
        object Visit(ParameterExpression parameter);
        object Visit(FunctionExpression function);
        object Visit(StructExpression node);
        object Visit(FieldExpression node);
    }
}