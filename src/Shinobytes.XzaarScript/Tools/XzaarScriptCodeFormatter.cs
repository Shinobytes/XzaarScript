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
using System.Linq;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Ast;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Tools
{
    public class XzaarScriptCodeFormatter : IStringExpressionVisitor
    {
        private int currentIndent = 0;
        private int insideExpressionCount;
        private int noIndent;

        private bool IsInsideExpression => insideExpressionCount > 0;

        private static SyntaxParser Transformer(string code)
        {
            return new SyntaxParser(new Lexer(code).Tokenize());
        }

        private static AstNode Reduce(string code, out SyntaxParser parser)
        {
            parser = Transformer(code);
            return parser.Parse();
        }

        public static string FormatCode(string code)
        {
            var ast = new NodeTypeBinder().Process(Reduce(code, out _));
            var compiler = new ExpressionCompiler();
            var expression = compiler.Compile(ast as EntryNode);
            var codeGenerator = new XzaarScriptCodeFormatter();
            return codeGenerator.Visit(expression).TrimEnd('\r', '\n');
        }

        public string Visit(XzaarExpression expression)
        {
//#if UNITY            
            if (expression is BinaryExpression binaryExpression) return Visit(binaryExpression);
            if (expression is ConditionalExpression conditionalExpression) return Visit(conditionalExpression);
            if (expression is IfElseExpression elseExpression) return Visit(elseExpression);
            if (expression is LogicalNotExpression notExpression) return Visit(notExpression);

            if (expression is MemberExpression memberExpression) return Visit(memberExpression);
            if (expression is MemberAccessChainExpression chainExpression) return Visit(chainExpression);
            if (expression is GotoExpression gotoExpression) return Visit(gotoExpression);
            if (expression is SwitchExpression switchExpression) return Visit(switchExpression);
            if (expression is SwitchCaseExpression caseExpression) return Visit(caseExpression);
            if (expression is UnaryExpression unaryExpression) return Visit(unaryExpression);
            if (expression is BlockExpression blockExpression) return Visit(blockExpression);

            if (expression is ForExpression forExpression) return Visit(forExpression);
            if (expression is ForEachExpression eachExpression) return Visit(eachExpression);
            if (expression is DoWhileExpression whileExpression) return Visit(whileExpression);
            if (expression is WhileExpression expression1) return Visit(expression1);
            if (expression is LoopExpression loopExpression) return Visit(loopExpression);
            if (expression is DefaultExpression defaultExpression) return Visit(defaultExpression);

            if (expression is LambdaExpression lambda) return Visit(lambda);
            if (expression is FunctionCallExpression callExpression) return Visit(callExpression);
            if (expression is FunctionExpression functionExpression) return Visit(functionExpression);

            if (expression is ConstantExpression constantExpression) return Visit(constantExpression);
            if (expression is NegateExpression negateExpression) return Visit(negateExpression);
            if (expression is VariableDefinitionExpression definitionExpression) return Visit(definitionExpression);
            if (expression is FieldExpression fieldExpression) return Visit(fieldExpression);
            if (expression is LabelExpression labelExpression) return Visit(labelExpression);
            if (expression is ParameterExpression parameterExpression) return Visit(parameterExpression);

            if (expression is StructExpression structExpression1) return Visit(structExpression1);
            if (expression is CreateStructExpression structExpression) return Visit(structExpression);                

            if (expression is ErrorExpression error)
            {
                
            }


            return Visit(expression);
//#else 
//            return Visit((dynamic)expression);
//#endif
        }


        public string Visit(LambdaExpression lambda)
        {
            var codeWriter = new XzaarCodeWriter();

            codeWriter.Write("(" + string.Join(", ", lambda.Parameters.Select(Visit).ToArray()) + ") => ");
            
            // This will fail on single-line lambdas!!
            if (lambda.Body is BlockExpression)
            {
                codeWriter.Write("{" + Visit(lambda.Body) + "}");
            }
            else
            {
                codeWriter.Write(Visit(lambda.Body));
            }            

            return codeWriter.ToString();
        }


        public string Visit(BinaryExpression binaryOp)
        {
            var codeWriter = new XzaarCodeWriter();
            var op = GetBinaryOperator(binaryOp.NodeType);

            if (!IsInsideExpression && noIndent == 0) codeWriter.Write("", currentIndent);

            insideExpressionCount++;
            codeWriter.Write(Visit(binaryOp.Left));
            insideExpressionCount--;

            codeWriter.Write(" " + op + " ");
            noIndent++;
            insideExpressionCount++;
            codeWriter.Write(Visit(binaryOp.Right));
            insideExpressionCount--;
            noIndent--;
            if (!IsInsideExpression && noIndent == 0) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(ConditionalExpression expr)
        {
            var codeWriter = new XzaarCodeWriter();
            insideExpressionCount++;
            codeWriter.Write(Visit(expr.Test));
            insideExpressionCount--;
            codeWriter.Write(" ? ");
            if (expr.WhenTrue != null)
            {
                currentIndent++;
                codeWriter.Write(Visit(expr.WhenTrue));
                currentIndent--;

            }
            codeWriter.Write(" : ");

            if (expr.WhenFalse != null)
            {
                currentIndent++;
                codeWriter.Write(Visit(expr.WhenFalse));
                currentIndent--;
            }
            return codeWriter.ToString();
        }

        public string Visit(IfElseExpression ifElse)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("if (", currentIndent);
            insideExpressionCount++;
            codeWriter.Write(Visit(ifElse.Test));
            insideExpressionCount--;
            codeWriter.Write(") { ");
            codeWriter.NewLine();
            if (ifElse.IfTrue != null)
            {
                currentIndent++;
                codeWriter.Write(Visit(ifElse.IfTrue));
                currentIndent--;

            }

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();

            if (ifElse.IfFalse != null && !ifElse.IfFalse.IsEmpty())
            {
                codeWriter.Write("else {", currentIndent);
                codeWriter.NewLine();

                currentIndent++;
                codeWriter.Write(Visit(ifElse.IfFalse));
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

            if (access.Expression is ParameterExpression paramExpr)
            {
                codeWriter.Write(paramExpr.Name);
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }

                return codeWriter.ToString();
            }

            if (access.Expression is MemberExpression memExpr)
            {
                codeWriter.Write(Visit(memExpr));
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            if (access.Expression is MemberAccessChainExpression memExprChain)
            {
                codeWriter.Write(Visit(memExprChain));
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            if (access.Expression is FieldExpression fieldExpr)
            {
                codeWriter.Write(fieldExpr.Name);
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            if (access.Expression is ConstantExpression constantExpr)
            {
                codeWriter.Write(Visit(constantExpr));
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
            if (parameter.ArrayIndex != null)
            {
                codeWriter.Write("[" + Visit(parameter.ArrayIndex) + "]");
            }
            return codeWriter.ToString();
        }

        public string Visit(GotoExpression @goto)
        {
            switch (@goto.Kind)
            {
                case GotoExpressionKind.Break:
                    return VisitBreak(@goto);
                case GotoExpressionKind.Continue:
                    return VisitContinue(@goto);
                case GotoExpressionKind.Return:
                    return VisitReturn(@goto);
                case GotoExpressionKind.Goto:
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
            var indent = IsInsideExpression ? 0 : currentIndent;
            switch (unary.NodeType)
            {
                case ExpressionType.PostIncrementAssign:
                    codeWriter.Write("", indent);
                    codeWriter.Write(Visit(unary.Item));
                    codeWriter.Write("++");
                    break;
                case ExpressionType.PostDecrementAssign:
                    codeWriter.Write("", indent);
                    codeWriter.Write(Visit(unary.Item));
                    codeWriter.Write("--");
                    break;
                case ExpressionType.Increment:
                    codeWriter.Write("++", indent);
                    codeWriter.Write(Visit(unary.Item));
                    break;
                case ExpressionType.Decrement:
                    codeWriter.Write("--", indent);
                    codeWriter.Write(Visit(unary.Item));
                    break;
            }
            if (!IsInsideExpression) codeWriter.NewLine();
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

        public string Visit(LogicalNotExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = IsInsideExpression ? 0 : currentIndent;
            insideExpressionCount++;
            codeWriter.Write("!", indent);
            codeWriter.Write(Visit(node.Expression));
            insideExpressionCount--;
            if (!IsInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(CreateStructExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = IsInsideExpression ? 0 : currentIndent;
            codeWriter.Write(node.StructName, indent);

            if (node.FieldInitializers != null)
            {
                codeWriter.Write(" { ");
                codeWriter.Write(string.Join(", ", node.FieldInitializers.Select(Visit).ToArray()));
                codeWriter.Write(" }");
            }

            if (!IsInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(MemberAccessChainExpression node)
        {
            var left = Visit(node.Left);
            var right = Visit(node.Right);
            return left + "." + right;

            //var chain = new List<string>();
            //var codeWriter = new XzaarCodeWriter();

            //var left = node.Left;
            //var isLeft = true;
            //while (left != null)
            //{
            //    var arrayIndexer = "";
            //    var wasChain = false;
            //    var chainExpr = left as MemberAccessChainExpression;
            //    if (chainExpr != null)
            //    {
            //        left = chainExpr.Right;
            //        wasChain = true;
            //    }

            //    var mb = left as MemberExpression;
            //    if (mb != null && mb.Expression != null)
            //    {

            //        if (mb.ArrayIndex != null)
            //        {
            //            arrayIndexer = "[" + Visit(mb.ArrayIndex) + "]";
            //        }

            //        var added = false;
            //        var f = mb.Expression as FieldExpression;
            //        var p = mb.Expression as ParameterDefinitionExpression;
            //        var c = mb.Expression as ConstantExpression;
            //        if (p != null)
            //        {
            //            if (isLeft) chain.Insert(0, p.Name + arrayIndexer);
            //            else chain.Add(p.Name + arrayIndexer);
            //            added = true;
            //        }
            //        if (f != null)
            //        {
            //            if (isLeft) chain.Insert(0, f.Name + arrayIndexer);
            //            else chain.Add(f.Name + arrayIndexer);
            //            added = true;
            //        }
            //        if (c != null)
            //        {
            //            if (isLeft) chain.Insert(0, Visit(c) + arrayIndexer);
            //            else chain.Add(Visit(c) + arrayIndexer);
            //            added = true;
            //        }
            //        if (added)
            //        {
            //            if (wasChain && chainExpr.Left != null)
            //            {
            //                left = chainExpr.Left;
            //            }
            //            else
            //            {
            //                if (!isLeft) break;
            //                left = node.Right;
            //                isLeft = false;
            //            }
            //            continue;
            //        }
            //    }

            //    var constantValue = left as ConstantExpression;
            //    if (constantValue != null)
            //    {
            //        if (constantValue.ArrayIndex != null)
            //            arrayIndexer = "[" + Visit(constantValue.ArrayIndex) + "]";

            //        if (isLeft) chain.Insert(0, Visit(constantValue) + arrayIndexer);
            //        else chain.Add(Visit(constantValue) + arrayIndexer);
            //        if (wasChain && chainExpr.Left != null) left = chainExpr.Left;
            //        else
            //        {
            //            if (!isLeft) break;
            //            left = node.Right;
            //            isLeft = false;
            //        }
            //        continue;
            //    }

            //    var fnc = left as FunctionCallExpression;
            //    if (fnc == null)
            //        break;
            //    insideExpressionCount++;
            //    var ii = ignoreInstance;
            //    ignoreInstance = true;
            //    var fValue = Visit(fnc);
            //    ignoreInstance = ii;
            //    insideExpressionCount--;
            //    if (isLeft) chain.Insert(0, fValue);
            //    else chain.Add(fValue);

            //    if (node.Right == fnc)
            //        break;

            //    // fnc.

            //    // chain.Insert(0, );                
            //}
            //var indent = isInsideExpression ? 0 : currentIndent;
            //codeWriter.Write(string.Join(".", chain.ToArray()), indent);
            //if (!isInsideExpression && !noNewLine) codeWriter.NewLine();
            //return codeWriter.ToString();
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
            if (@goto.Value != null && !@goto.Value.IsEmpty())
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
            codeWriter.Write("var " + definedVariable.Name, IsInsideExpression ? 0 : currentIndent);
            if (definedVariable.AssignmentExpression != null)
            {
                codeWriter.Write(" = ");
                insideExpressionCount++;
                codeWriter.Write(Visit(definedVariable.AssignmentExpression));
                insideExpressionCount--;
            }
            if (!IsInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(DefaultExpression emptyOrNull)
        {
            return null;
        }

        public string Visit(FunctionCallExpression call)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = IsInsideExpression ? 0 : currentIndent;
            var instance = call.GetInstance();

            var methodInvocation = call.MethodName + "(";
            var instanceText = string.Empty;
            if (instance != null)
            {
                instanceText = instance is VariableDefinitionExpression instanceExpr
                    ? instanceExpr.Name + "."
                    : Visit(instance) + ".";
            }
            codeWriter.Write(instanceText + methodInvocation, indent);

            insideExpressionCount++;

            for (int index = 0; index < call.Arguments.Count; index++)
            {
                var arg = call.Arguments[index];
                codeWriter.Write(Visit(arg));
                if (index + 1 < call.ArgumentCount) codeWriter.Write(", ");
            }
            insideExpressionCount--;
            codeWriter.Write(")");
            if (!IsInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(ConstantExpression value)
        {
            var codeWriter = new XzaarCodeWriter();
            var arrayValue = value.ArrayIndex != null ? "[" + Visit(value.ArrayIndex) + "]" : "";

            if (value.Type.IsArray)
            {
                if (value.Value == null)
                    codeWriter.Write("[]" + arrayValue);
                else
                {
                    if (value.Value is XzaarExpression[] initializer)
                    {
                        var values = initializer.Select(Visit).ToList();
                        codeWriter.Write("[" + string.Join(", ", values.ToArray()) + "]" + arrayValue);
                    }
                    else
                    {
                        codeWriter.Write(value.Value + arrayValue);
                    }
                }
            }
            else if (value.Type.IsNumeric || value.Type.Equals(XzaarBaseTypes.Boolean))
            {
                codeWriter.Write(value.Value + arrayValue);
            }
            else
            {
                codeWriter.Write("\"" + value.Value + "\"" + arrayValue);
            }
            return codeWriter.GetSourceCode();
        }

        public string Visit(BlockExpression block)
        {
            // currentIndent++;
            var output = "";
            foreach (var expr in block.Expressions) output += Visit(expr);
            // currentIndent--;
            return output;
        }

        public string Visit(FunctionExpression function)
        {
            var codeWriter = new XzaarCodeWriter();
            codeWriter.Write("fn " + function.Name + "(", currentIndent);
            codeWriter.Write(string.Join(", ", function.Parameters.Select(v => v.Name + ":" + v.Type.Name).ToArray()));
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


        private string GetBinaryOperator(ExpressionType binaryOpNodeType)
        {
            switch (binaryOpNodeType)
            {
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddAssign: return "+=";
                case ExpressionType.AddChecked:
                case ExpressionType.Add: return "+";
                case ExpressionType.SubtractChecked:
                case ExpressionType.Subtract: return "-";
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractAssign: return "-=";
                case ExpressionType.And: return "&";
                case ExpressionType.AndAlso: return "&&";
                case ExpressionType.AndAssign: return "&=";
                case ExpressionType.Coalesce: return "~";
                case ExpressionType.Or: return "|";
                case ExpressionType.OrAssign: return "|=";
                case ExpressionType.OrElse: return "||";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.ModuloAssign:
                    return "%=";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.DivideAssign:
                    return "/=";
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyAssign:
                    return "*=";
                case ExpressionType.Assign:
                    return "=";
                case ExpressionType.Not:
                    return "!";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.Decrement:
                    return "--";
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.Increment:
                    return "++";
                default: throw new NotImplementedException(binaryOpNodeType + " has not been implemented with a known binary operator");
            }
        }
    }
}