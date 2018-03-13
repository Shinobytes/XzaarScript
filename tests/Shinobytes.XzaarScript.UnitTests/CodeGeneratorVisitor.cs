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
using System.Diagnostics;
using System.Linq;
using Shinobytes.XzaarScript.Scripting;
using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    public class CodeGeneratorVisitor : IXzaarStringExpressionVisitor
    {
        private int currentIndent = 0;
        private int insideExpressionCount;
        private int noIndent;
        private bool noNewLine;

        private bool isInsideExpression => insideExpressionCount > 0;

        public CodeGeneratorVisitor()
        {
        }

        public string Visit(XzaarExpression expression)
        {
            dynamic e = expression;

            return Visit(e);
        }

        public string Visit(BinaryExpression binaryOp)
        {
            var codeWriter = new XzaarCodeWriter();
            string op = GetBinaryOperator(binaryOp.NodeType);

            if (!isInsideExpression && noIndent == 0) codeWriter.Write("", currentIndent);

            noNewLine = true;
            codeWriter.Write(Visit(binaryOp.Left));
            noNewLine = false;

            codeWriter.Write(" " + op + " ");
            noIndent++;
            codeWriter.Write(Visit(binaryOp.Right));
            noIndent--;
            if (!isInsideExpression && noIndent == 0) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(ConditionalExpression conditional)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("if (", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(conditional.Test));
            insideExpressionCount--;
            codeWriter.Write(") { ");
            codeWriter.NewLine();
            if (conditional.IfTrue != null)
            {
                currentIndent++;
                codeWriter.Write(Visit(conditional.IfTrue));
                currentIndent--;

            }

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();

            if (conditional.IfFalse != null && !conditional.IfFalse.IsEmpty())
            {
                codeWriter.Write("else {", currentIndent);
                codeWriter.NewLine();

                currentIndent++;
                codeWriter.Write(Visit(conditional.IfFalse));
                currentIndent--;

                codeWriter.Write("}", currentIndent);
                codeWriter.NewLine();
            }
            return codeWriter.ToString();
        }

        public string Visit(MemberExpression access)
        {
            var codeWriter = new XzaarCodeWriter();
            if (access.Member != null)
            {
                codeWriter.Write(access.Member.Name);
                return null;
            }

            var paramExpr = access.Expression as ParameterExpression;
            if (paramExpr != null)
            {
                codeWriter.Write(paramExpr.Name);
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            return codeWriter.ToString();
        }

        public string Visit(ParameterExpression parameter)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write(parameter.Name);
            return codeWriter.ToString();
        }

        public string Visit(GotoExpression @goto)
        {
            switch (@goto.Kind)
            {
                case XzaarGotoExpressionKind.Break:
                    return VisitBreak(@goto);
                case XzaarGotoExpressionKind.Continue:
                    return VisitContinue(@goto);
                case XzaarGotoExpressionKind.Return:
                    return VisitReturn(@goto);
                case XzaarGotoExpressionKind.Goto:
                    return VisitGoto(@goto);

            }

            return null;
        }

        public string Visit(LabelExpression label)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write(@label.Target?.Name + ":", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(SwitchExpression match)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("switch (", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(match.Value));
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            foreach (var c in match.Cases) codeWriter.Write(Visit(c));
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(SwitchCaseExpression matchCase)
        {
            var codeWriter = new XzaarCodeWriter();
            if (matchCase.IsDefaultCase)
            {
                codeWriter.Write("default:", currentIndent);
            }
            else
            {
                insideExpressionCount++;
                codeWriter.Write("case ", currentIndent);
                codeWriter.Write(Visit(matchCase.Match));
                codeWriter.Write(":");
                insideExpressionCount--;
            }
            codeWriter.NewLine();

            currentIndent++;
            codeWriter.Write(Visit(matchCase.Body));
            currentIndent--;
            return codeWriter.ToString();
        }

        public string Visit(UnaryExpression unary)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = isInsideExpression ? 0 : currentIndent;
            switch (unary.NodeType)
            {
                case XzaarExpressionType.PostIncrementAssign:
                    codeWriter.Write("", indent);
                    codeWriter.Write(Visit(unary.Item));
                    codeWriter.Write("++");
                    break;
                case XzaarExpressionType.PostDecrementAssign:
                    codeWriter.Write("", indent);
                    codeWriter.Write(Visit(unary.Item));
                    codeWriter.Write("--");
                    break;
                case XzaarExpressionType.Increment:
                    codeWriter.Write("++", indent);
                    codeWriter.Write(Visit(unary.Item));
                    break;
                case XzaarExpressionType.Decrement:
                    codeWriter.Write("--", indent);
                    codeWriter.Write(Visit(unary.Item));
                    break;
            }
            if (!isInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(StructExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("struct " + node.Name + " {", currentIndent);
            codeWriter.NewLine();
            currentIndent++;
            foreach (var field in node.Fields)
                codeWriter.Write(Visit(field));
            currentIndent--;
            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(FieldExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write(node.FieldType + " " + node.Name, currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(CreateStructExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = isInsideExpression ? 0 : currentIndent;
            codeWriter.Write(node.StructName, indent);
            if (!isInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(MemberAccessChainExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            var chain = new List<string>();
            var left = node.Left;
            var isLeft = true;
            while (left != null)
            {
                var leftValue = "";
                var wasChain = false;
                var chainExpr = left as MemberAccessChainExpression;
                if (chainExpr != null)
                {
                    left = chainExpr.Right;
                    wasChain = true;
                }

                var mb = left as MemberExpression;
                if (mb != null && mb.Expression != null)
                {
                    string arrayIndexer = "";
                    if (mb.ArrayIndex != null)
                    {
                        arrayIndexer = "[" + Visit(mb.ArrayIndex) + "]";
                    }

                    var added = false;
                    var f = mb.Expression as FieldExpression;
                    var p = mb.Expression as ParameterExpression;
                    if (p != null)
                    {
                        if (isLeft) chain.Insert(0, p.Name + arrayIndexer);
                        else chain.Add(p.Name + arrayIndexer);
                        added = true;
                    }
                    if (f != null)
                    {
                        if (isLeft) chain.Insert(0, f.Name + arrayIndexer);
                        else chain.Add(f.Name + arrayIndexer);
                        added = true;
                    }

                    if (added)
                    {
                        if (wasChain && chainExpr.Left != null)
                        {
                            left = chainExpr.Left;
                        }
                        else
                        {
                            if (!isLeft) break;
                            left = node.Right;
                            isLeft = false;
                        }
                        continue;
                    }
                }

                var fnc = left as FunctionCallExpression;
                if (fnc == null)
                    break;
                var fValue = Visit(fnc);
                if (isLeft) chain.Insert(0, fValue);
                else chain.Add(fValue);

                // fnc.

                // chain.Insert(0, );                
            }
            var indent = isInsideExpression ? 0 : currentIndent;
            codeWriter.Write(string.Join(".", chain.ToArray()), indent);
            if (!isInsideExpression && !noNewLine) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        private string VisitGoto(GotoExpression @goto)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("goto " + @goto.Target?.Name, currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        private string VisitReturn(GotoExpression @goto)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("return", currentIndent);
            if (@goto.Value != null)
            {
                codeWriter.Write(" ");
                insideExpressionCount++;
                codeWriter.Write(Visit(@goto.Value));
                insideExpressionCount--;
            }
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        private string VisitContinue(GotoExpression @goto)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("continue", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        private string VisitBreak(GotoExpression @goto)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("break", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(ForExpression @for)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("for (", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(@for.Initiator));
            insideExpressionCount--;
            codeWriter.Write("; ");

            insideExpressionCount++;
            codeWriter.Write(Visit(@for.Test));
            insideExpressionCount--;
            codeWriter.Write("; ");
            insideExpressionCount++;
            codeWriter.Write(Visit(@for.Incrementor));
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            codeWriter.Write(Visit(@for.Body));
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(ForEachExpression @foreach)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("foreach (", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(@foreach.Variable));
            codeWriter.Write(" in ");
            codeWriter.Write(Visit(@foreach.Collection));
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            codeWriter.Write(Visit(@foreach.Body));
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(DoWhileExpression doWhile)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("do {", currentIndent);
            codeWriter.NewLine();
            currentIndent++;
            codeWriter.Write(Visit(doWhile.Body));
            currentIndent--;
            codeWriter.Write("} while(", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(doWhile.Test));
            insideExpressionCount--;
            codeWriter.Write(")");
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(WhileExpression @while)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("while (", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(@while.Test));
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            codeWriter.Write(Visit(@while.Body));
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(LoopExpression loop)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("loop {", currentIndent);
            codeWriter.NewLine();
            if (loop.Body != null)
            {
                currentIndent++;
                codeWriter.Write(Visit(loop.Body));
                currentIndent--;
            }
            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(VariableDefinitionExpression definedVariable)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("var " + definedVariable.Name, isInsideExpression ? 0 : currentIndent);
            if (definedVariable.AssignmentExpression != null)
            {
                codeWriter.Write(" = ");
                insideExpressionCount++;
                codeWriter.Write(Visit(definedVariable.AssignmentExpression));
                insideExpressionCount--;
            }
            if (!isInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(DefaultExpression emptyOrNull)
        {
            return null;
        }

        public string Visit(FunctionCallExpression call)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = isInsideExpression ? 0 : currentIndent;
            var instance = call.GetInstance();
            if (instance != null)
            {
                codeWriter.Write(Visit(instance));
                codeWriter.Write("." + call.MethodName + "(");
            }
            else
            {
                codeWriter.Write(call.MethodName + "(", indent);
            }

            for (int index = 0; index < call.Arguments.Count; index++)
            {
                var arg = call.Arguments[index];
                codeWriter.Write(Visit(arg));
                if (index + 1 < call.ArgumentCount)
                {
                    codeWriter.Write(", ");
                }
            }

            codeWriter.Write(")");
            if (!isInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(ConstantExpression value)
        {
            var codeWriter = new XzaarCodeWriter();
            if (value.Type.IsArray)
            {
                if (value.Value == null)
                    codeWriter.Write("[]");
                else
                    codeWriter.Write(value.Value);
            }
            else if (value.Type == XzaarBaseTypes.Number || value.Type == XzaarBaseTypes.Boolean)
            {
                codeWriter.Write(value.Value.ToString());
            }
            else
            {
                codeWriter.Write("\"" + value.Value + "\"");
            }
            return codeWriter.GetSourceCode();
        }

        public string Visit(BlockExpression block)
        {
            // currentIndent++;
            var output = "";
            foreach (var expr in block.Expressions)
            {
                output += Visit(expr);
            }
            // currentIndent--;
            return output;
        }

        public string Visit(FunctionExpression function)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("fn " + function.Name + "(", currentIndent);
            codeWriter.Write(string.Join(", ", function.GetParameters().Select(v => v.Type.Name + " " + v.Name).ToArray()));
            codeWriter.Write(") ");
            if (function.ReturnType.Name != "void") codeWriter.Write("-> " + function.ReturnType.Name + " ");
            codeWriter.Write("{");
            codeWriter.NewLine();

            var body = function.GetBody();
            if (body != null)
            {
                currentIndent++;
                codeWriter.Write(Visit(body));
                currentIndent--;
            }

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return codeWriter.ToString();
        }


        private string GetBinaryOperator(XzaarExpressionType binaryOpNodeType)
        {
            switch (binaryOpNodeType)
            {
                case XzaarExpressionType.AddAssignChecked:
                case XzaarExpressionType.AddAssign: return "+=";
                case XzaarExpressionType.AddChecked:
                case XzaarExpressionType.Add: return "+";
                case XzaarExpressionType.SubtractChecked:
                case XzaarExpressionType.Subtract: return "-";
                case XzaarExpressionType.SubtractAssignChecked:
                case XzaarExpressionType.SubtractAssign: return "-=";
                case XzaarExpressionType.And: return "&";
                case XzaarExpressionType.AndAlso: return "&&";
                case XzaarExpressionType.AndAssign: return "&=";
                case XzaarExpressionType.Coalesce: return "~";
                case XzaarExpressionType.Or: return "|";
                case XzaarExpressionType.OrAssign: return "|=";
                case XzaarExpressionType.OrElse: return "||";
                case XzaarExpressionType.Modulo:
                    return "%";
                case XzaarExpressionType.ModuloAssign:
                    return "%=";
                case XzaarExpressionType.Divide:
                    return "/";
                case XzaarExpressionType.DivideAssign:
                    return "/=";
                case XzaarExpressionType.MultiplyChecked:
                case XzaarExpressionType.Multiply:
                    return "*";
                case XzaarExpressionType.MultiplyAssignChecked:
                case XzaarExpressionType.MultiplyAssign:
                    return "*=";
                case XzaarExpressionType.Assign:
                    return "=";
                case XzaarExpressionType.Not:
                    return "!";
                case XzaarExpressionType.NotEqual:
                    return "!=";
                case XzaarExpressionType.Equal:
                    return "==";
                case XzaarExpressionType.LessThan:
                    return "<";
                case XzaarExpressionType.GreaterThan:
                    return ">";
                case XzaarExpressionType.LessThanOrEqual:
                    return "<=";
                case XzaarExpressionType.GreaterThanOrEqual:
                    return ">=";
                case XzaarExpressionType.PostDecrementAssign:
                case XzaarExpressionType.Decrement:
                    return "--";
                case XzaarExpressionType.PostIncrementAssign:
                case XzaarExpressionType.Increment:
                    return "++";
                default: throw new NotImplementedException(binaryOpNodeType + " has not been implemented with a known binary operator");
            }
        }
    }
}