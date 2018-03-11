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