using System;
using System.Linq;
using Shinobytes.XzaarScript.Scripting;
using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    public class XzaarCodeGeneratorVisitor : IXzaarExpressionVisitor
    {
        private int currentIndent = 0;
        private XzaarCodeWriter codeWriter;
        private int insideExpressionCount;
        private int noIndent;

        private bool isInsideExpression => insideExpressionCount > 0;

        public XzaarCodeGeneratorVisitor()
        {
            codeWriter = new XzaarCodeWriter();
        }

        public XzaarExpression Visit(XzaarExpression expression)
        {
            dynamic e = expression;

            return Visit(e);
        }

        public BinaryExpression Visit(BinaryExpression binaryOp)
        {

            string op = GetBinaryOperator(binaryOp.NodeType);

            if (!isInsideExpression && noIndent == 0) codeWriter.Write("", currentIndent);

            Visit(binaryOp.Left);

            codeWriter.Write(" " + op + " ");
            noIndent++;
            Visit(binaryOp.Right);
            noIndent--;
            if (!isInsideExpression && noIndent == 0) codeWriter.NewLine();
            return null;
        }

        public ConditionalExpression Visit(ConditionalExpression conditional)
        {
            codeWriter.Write("if (", currentIndent);
            insideExpressionCount++;
            Visit(conditional.Test);
            insideExpressionCount--;
            codeWriter.Write(") { ");
            codeWriter.NewLine();
            if (conditional.IfTrue != null)
            {
                currentIndent++;
                Visit(conditional.IfTrue);
                currentIndent--;

            }

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();

            if (conditional.IfFalse != null && !conditional.IfFalse.IsEmpty())
            {
                codeWriter.Write("else {", currentIndent);
                codeWriter.NewLine();

                currentIndent++;
                Visit(conditional.IfFalse);
                currentIndent--;

                codeWriter.Write("}", currentIndent);
                codeWriter.NewLine();
            }
            return null;
        }

        public MemberExpression Visit(MemberExpression access)
        {
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
                    Visit(access.ArrayIndex);
                    codeWriter.Write("]");
                }
            }

            return null;
        }

        public ParameterExpression Visit(ParameterExpression parameter)
        {
            codeWriter.Write(parameter.Name);

            return null;
        }

        public GotoExpression Visit(GotoExpression @goto)
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

        public LabelExpression Visit(LabelExpression label)
        {
            codeWriter.Write(@label.Target?.Name + ":", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        private GotoExpression VisitGoto(GotoExpression @goto)
        {
            codeWriter.Write("goto " + @goto.Target?.Name, currentIndent);
            codeWriter.NewLine();
            return null;
        }

        private GotoExpression VisitReturn(GotoExpression @goto)
        {
            codeWriter.Write("return", currentIndent);
            if (@goto.Value != null)
            {
                codeWriter.Write(" ");
                insideExpressionCount++;
                Visit(@goto.Value);
                insideExpressionCount--;
            }
            codeWriter.NewLine();
            return null;
        }

        private GotoExpression VisitContinue(GotoExpression @goto)
        {
            codeWriter.Write("continue", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        private GotoExpression VisitBreak(GotoExpression @goto)
        {
            codeWriter.Write("break", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        public ForExpression Visit(ForExpression @for)
        {
            codeWriter.Write("for (", currentIndent);
            insideExpressionCount++;
            Visit(@for.Initiator);
            insideExpressionCount--;
            codeWriter.Write("; ");

            insideExpressionCount++;
            Visit(@for.Test);
            insideExpressionCount--;
            codeWriter.Write("; ");
            insideExpressionCount++;
            Visit(@for.Incrementor);
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            Visit(@for.Body);
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        public ForEachExpression Visit(ForEachExpression @foreach)
        {
            codeWriter.Write("foreach (", currentIndent);
            insideExpressionCount++;
            Visit(@foreach.Variable);
            codeWriter.Write(" in ");
            Visit(@foreach.Collection);
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            Visit(@foreach.Body);
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        public DoWhileExpression Visit(DoWhileExpression doWhile)
        {
            codeWriter.Write("do {", currentIndent);
            codeWriter.NewLine();
            currentIndent++;
            Visit(doWhile.Body);
            currentIndent--;
            codeWriter.Write("} while(", currentIndent);
            insideExpressionCount++;
            Visit(doWhile.Test);
            insideExpressionCount--;
            codeWriter.Write(")");
            codeWriter.NewLine();
            return null;
        }

        public WhileExpression Visit(WhileExpression @while)
        {
            codeWriter.Write("while (", currentIndent);
            insideExpressionCount++;
            Visit(@while.Test);
            insideExpressionCount--;
            codeWriter.Write(") {");
            codeWriter.NewLine();

            currentIndent++;
            Visit(@while.Body);
            currentIndent--;

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        public LoopExpression Visit(LoopExpression loop)
        {
            codeWriter.Write("loop {", currentIndent);
            codeWriter.NewLine();
            if (loop.Body != null)
            {
                currentIndent++;
                Visit(loop.Body);
                currentIndent--;
            }
            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return null;
        }

        public VariableDefinitionExpression Visit(VariableDefinitionExpression definedVariable)
        {
            codeWriter.Write("var " + definedVariable.Name, isInsideExpression ? 0 : currentIndent);
            if (definedVariable.AssignmentExpression != null)
            {
                codeWriter.Write(" = ");
                insideExpressionCount++;
                Visit(definedVariable.AssignmentExpression);
                insideExpressionCount--;
            }
            if (!isInsideExpression) codeWriter.NewLine();
            return null;
        }

        public DefaultExpression Visit(DefaultExpression emptyOrNull)
        {
            return null;
        }

        public MethodCallExpression Visit(MethodCallExpression call)
        {
            var indent = isInsideExpression ? 0 : currentIndent;
            var instance = call.GetInstance();
            if (instance != null)
            {
                Visit(instance);
                codeWriter.Write("." + call.MethodName + "(");
            }
            else
            {
                codeWriter.Write(call.MethodName + "(", indent);
            }

            for (int index = 0; index < call.Arguments.Count; index++)
            {
                var arg = call.Arguments[index];
                Visit(arg);
                if (index + 1 < call.ArgumentCount)
                {
                    codeWriter.Write(", ");
                }
            }

            codeWriter.Write(")");
            if (!isInsideExpression) codeWriter.NewLine();
            return null;
        }

        public ConstantExpression Visit(ConstantExpression value)
        {
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
            return null;
        }

        public BlockExpression Visit(BlockExpression block)
        {
            // currentIndent++;
            foreach (var expr in block.Expressions)
            {
                Visit(expr);
            }
            // currentIndent--;
            return null;
        }

        public FunctionExpression Visit(FunctionExpression function)
        {
            codeWriter.Write("fn " + function.Name + "(", currentIndent);
            codeWriter.Write(string.Join(", ", function.GetParameters().Select(v => v.Type.Name + " " + v.Name).ToArray()));
            codeWriter.Write(") {");
            codeWriter.NewLine();

            var body = function.GetBody();
            if (body != null)
            {
                currentIndent++;
                Visit(body);
                currentIndent--;
            }

            codeWriter.Write("}", currentIndent);
            codeWriter.NewLine();
            return null;
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

        public string GetSourceCode()
        {
            return codeWriter.GetSourceCode();
        }
    }
}