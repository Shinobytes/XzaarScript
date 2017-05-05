using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Expressions;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.UtilityVisitors
{
    public class CodeGeneratorVisitor : IStringExpressionVisitor
    {
        private int currentIndent = 0;
        private int insideExpressionCount;
        private int noIndent;
        private bool noNewLine;
        private bool ignoreInstance;

        private bool isInsideExpression => insideExpressionCount > 0;

        public CodeGeneratorVisitor()
        {
        }

        public string Visit(XzaarExpression expression)
        {
#if UNITY
            if (expression is BinaryExpression) return Visit(expression as BinaryExpression);
            if (expression is ConditionalExpression) return Visit(expression as ConditionalExpression);
            if (expression is MemberExpression) return Visit(expression as MemberExpression);
            if (expression is MemberAccessChainExpression) return Visit(expression as MemberAccessChainExpression);
            if (expression is GotoExpression) return Visit(expression as GotoExpression);
            if (expression is SwitchExpression) return Visit(expression as SwitchExpression);
            if (expression is SwitchCaseExpression) return Visit(expression as SwitchCaseExpression);
            if (expression is UnaryExpression) return Visit(expression as UnaryExpression);
            if (expression is BlockExpression) return Visit(expression as BlockExpression);
            if (expression is ForExpression) return Visit(expression as ForExpression);
            if (expression is ForEachExpression) return Visit(expression as ForEachExpression);
            if (expression is DoWhileExpression) return Visit(expression as DoWhileExpression);
            if (expression is WhileExpression) return Visit(expression as WhileExpression);
            if (expression is LoopExpression) return Visit(expression as LoopExpression);
            if (expression is DefaultExpression) return Visit(expression as DefaultExpression);
            if (expression is FunctionCallExpression) return Visit(expression as FunctionCallExpression);
            if (expression is ConstantExpression) return Visit(expression as ConstantExpression);
            if (expression is NegateExpression) return Visit(expression as NegateExpression);
            if (expression is VariableDefinitionExpression) return Visit(expression as VariableDefinitionExpression);
            if (expression is LabelExpression) return Visit(expression as LabelExpression);
            if (expression is ParameterExpression) return Visit(expression as ParameterExpression);
            if (expression is FunctionExpression) return Visit(expression as FunctionExpression);
            if (expression is StructExpression) return Visit(expression as StructExpression);
            if (expression is FieldExpression) return Visit(expression as FieldExpression);
            if (expression is LogicalNotExpression) return Visit(expression as LogicalNotExpression);
            if (expression is CreateStructExpression) return Visit(expression as CreateStructExpression);
            return Visit(expression);
#else 
            return Visit((dynamic)expression);
#endif
        }

        public string Visit(BinaryExpression binaryOp)
        {
            var codeWriter = new XzaarCodeWriter();
            var op = GetBinaryOperator(binaryOp.NodeType);

            if (!isInsideExpression && noIndent == 0) codeWriter.Write("", currentIndent);

            noNewLine = true;
            insideExpressionCount++;
            codeWriter.Write(Visit(binaryOp.Left));
            insideExpressionCount--;
            noNewLine = false;

            codeWriter.Write(" " + op + " ");
            noIndent++;
            insideExpressionCount++;
            codeWriter.Write(Visit(binaryOp.Right));
            insideExpressionCount--;
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

                return codeWriter.ToString();
            }

            var memExpr = access.Expression as MemberExpression;
            if (memExpr != null)
            {
                codeWriter.Write(Visit(memExpr));
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            var memExprChain = access.Expression as MemberAccessChainExpression;
            if (memExprChain != null)
            {
                codeWriter.Write(Visit(memExprChain));
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            var fieldExpr = access.Expression as FieldExpression;
            if (fieldExpr != null)
            {
                codeWriter.Write(fieldExpr.Name);
                if (access.ArrayIndex != null)
                {
                    codeWriter.Write("[");
                    codeWriter.Write(Visit(access.ArrayIndex));
                    codeWriter.Write("]");
                }
            }

            var constantExpr = access.Expression as ConstantExpression;
            if (constantExpr != null)
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
            var indent = isInsideExpression ? 0 : currentIndent;
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

        public string Visit(LogicalNotExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = isInsideExpression ? 0 : currentIndent;
            insideExpressionCount++;
            codeWriter.Write("!", indent);
            codeWriter.Write(Visit(node.Expression));
            insideExpressionCount--;
            if (!isInsideExpression) codeWriter.NewLine();
            return codeWriter.ToString();
        }

        public string Visit(CreateStructExpression node)
        {
            var codeWriter = new XzaarCodeWriter();
            var indent = isInsideExpression ? 0 : currentIndent;
            codeWriter.Write(node.StructName, indent);

            if (node.FieldInitializers != null)
            {
                codeWriter.Write(" { ");
                codeWriter.Write(string.Join(", ", node.FieldInitializers.Select(Visit).ToArray()));
                codeWriter.Write(" }");
            }

            if (!isInsideExpression) codeWriter.NewLine();
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
            //        var p = mb.Expression as ParameterExpression;
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
            var methodInvocation = call.MethodName + "(";
            var instanceText = string.Empty;
            if (instance != null && !ignoreInstance)
            {
                var instanceExpr = instance as VariableDefinitionExpression;
                instanceText = instanceExpr != null
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
            if (!isInsideExpression) codeWriter.NewLine();
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
                    var initializer = value.Value as XzaarExpression[];
                    if (initializer != null)
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
            codeWriter.Write(string.Join(", ", function.GetParameters().Select(v => v.Name + ":" + v.Type.Name).ToArray()));
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