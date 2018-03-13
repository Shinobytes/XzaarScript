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
 
using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Compiler.Types;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public class DotNetXzaarScriptDiscoveryVisitor : IScriptCompilerVisitor
    {
        private readonly DotNetXzaarScriptCompilerContext ctx;

        public DotNetXzaarScriptDiscoveryVisitor(DotNetXzaarScriptCompilerContext ctx)
        {
            this.ctx = ctx;
        }
        // this will help us to define all available functions in the current type we create
        public object Visit(XzaarExpression expression)
        {
            AssertPossibleStackOverflow(expression);

            if (expression.NodeType == ExpressionType.Block)
            {
                var block = expression as BlockExpression;
                if (block != null)
                {
                    foreach (var item in block.Expressions) Visit(item);
                }
                return null;
            }

            // NOTE: Blueh! If only we could have used dynamics here, that would have saved us so many darn lines :P But since we are compiling for 3.5, thats not possible
            // 
            if (expression is ErrorExpression errorExpression)
            {
                throw new Exception(errorExpression.ErrorMessage);
            }

//#if UNITY
            if (expression is UnaryExpression) return Visit(expression as UnaryExpression);
            if (expression is IfElseExpression) return Visit(expression as IfElseExpression);
            if (expression is ConditionalExpression) return Visit(expression as ConditionalExpression);
            if (expression is GotoExpression) return Visit(expression as GotoExpression);
            if (expression is MemberExpression) return Visit(expression as MemberExpression);
            if (expression is BinaryExpression) return Visit(expression as BinaryExpression);
            if (expression is MemberAccessChainExpression) return Visit(expression as MemberAccessChainExpression);
            if (expression is ForExpression) return Visit(expression as ForExpression);
            if (expression is ForEachExpression) return Visit(expression as ForEachExpression);
            if (expression is SwitchCaseExpression) return Visit(expression as SwitchCaseExpression);
            if (expression is DoWhileExpression) return Visit(expression as DoWhileExpression);
            if (expression is LoopExpression) return Visit(expression as LoopExpression);
            if (expression is SwitchExpression) return Visit(expression as SwitchExpression);
            if (expression is CreateStructExpression) return Visit(expression as CreateStructExpression);
            if (expression is ConstantExpression) return Visit(expression as ConstantExpression);
            if (expression is FunctionCallExpression) return Visit(expression as FunctionCallExpression);
            if (expression is VariableDefinitionExpression) return Visit(expression as VariableDefinitionExpression);
            if (expression is FieldExpression) return Visit(expression as FieldExpression);
            if (expression is LabelExpression) return Visit(expression as LabelExpression);
            if (expression is ParameterExpression) return Visit(expression as ParameterExpression);
            if (expression is FunctionExpression) return Visit(expression as FunctionExpression);
            if (expression is StructExpression) return Visit(expression as StructExpression);
            return Visit(expression);
//#else
//            return Visit((dynamic)expression);
//#endif

        }


        private void AssertPossibleStackOverflow(XzaarExpression expression)
        {
            // TODO: Dependecy Inject a stack guard validation object to do take care of this for us
            if (ctx.StackRecursionCount > 10)
                throw new CompilerException(
                    $"PANIC!! StackOverflow! {expression.GetType()} [{expression.NodeType}] could not be handled.");

            if (ctx.LastVisitedExpression == expression)
                ctx.StackRecursionCount++;
            else
                ctx.StackRecursionCount = 0;

            ctx.LastVisitedExpression = expression;
        }

        public object Visit(GotoExpression @goto)
        {
            return null;
        }

        public object Visit(MemberExpression member)
        {
            return null;
        }

        public object Visit(BinaryExpression binary)
        {
            return null;
        }

        public object Visit(MemberAccessChainExpression chain)
        {
            return null;
        }

        public object Visit(ForExpression node)
        {
            return null;
        }

        public object Visit(ForEachExpression node)
        {
            return null;
        }

        public object Visit(SwitchCaseExpression node)
        {
            return null;
        }

        public object Visit(DoWhileExpression node)
        {
            return null;
        }

        public object Visit(LoopExpression node)
        {
            return null;
        }

        public object Visit(SwitchExpression node)
        {
            return null;
        }

        public object Visit(CreateStructExpression node)
        {
            return null;
        }

        public object Visit(IfElseExpression node)
        {
            return null;
        }

        public object Visit(ConstantExpression constant)
        {
            return null;
        }

        public object Visit(UnaryExpression unary)
        {
            return null;
        }

        public object Visit(FunctionCallExpression function)
        {
            return null;
        }

        public object Visit(VariableDefinitionExpression definedVariable)
        {
            return null;
        }

        public object Visit(FieldExpression field)
        {
            return null;
        }

        public object Visit(LabelExpression label)
        {
            return null;
        }

        public object Visit(ParameterExpression parameter)
        {
            return new XsParameter(parameter.Name, ctx.GetClrType(parameter.Type));
        }


        public object Visit(ConditionalExpression expr)
        {
            return null;
        }

        public object Visit(FunctionExpression function)
        {
            var returnType = ctx.GetClrType(function.ReturnType);
            var paramTypes = new List<XsParameter>();

            // NOTE: If the function is static then parameterIndex should start at 0, but we don't support static functions yet
            int parameterIndex = 1; // 0 is the 'this' keyword
            foreach (var p in function.Parameters)
            {
                var pa = Visit(p) as XsParameter;
                pa.Index = parameterIndex++;
                paramTypes.Add(pa);
            }

            return ctx.DefineMethod(function.Name, returnType, paramTypes.ToArray());
        }

        public object Visit(StructExpression node)
        {
            return null;
        }

        public object Visit(LogicalNotExpression expr)
        {
            return null;
        }

        public object Visit(BlockExpression block)
        {
            return null;
        }

        public object Visit(WhileExpression @while)
        {
            return null;
        }

        public object Visit(DefaultExpression emptyOrNull)
        {
            return null;
        }

        public object Visit(NegateExpression value)
        {
            return null;
        }
    }
}