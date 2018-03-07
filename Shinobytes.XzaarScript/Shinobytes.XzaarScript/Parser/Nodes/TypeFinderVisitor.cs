using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class TypeFinderVisitor : NullNodeVisitor
    {
        private readonly XzaarTypeFinderContext context;

        public bool FoundType;

        public XzaarType Type;

        private Dictionary<string, XzaarType> currentFunctionVariables = new Dictionary<string, XzaarType>();

        public TypeFinderVisitor(XzaarTypeFinderContext context)
        {
            this.context = context;
        }

        public override XzaarExpression Visit(FunctionNode function)
        {
            currentFunctionVariables.Clear();
            return null;
        }

        public override XzaarExpression Visit(DefineVariableNode variable)
        {
            var t = XzaarType.GetType(variable.Type);
            if (currentFunctionVariables.ContainsKey(variable.Name))
            {
                if (currentFunctionVariables[variable.Name].Name == "any")
                    currentFunctionVariables[variable.Name] = t == null ? XzaarBaseTypes.Any : t;
            }
            else
            {
                currentFunctionVariables.Add(variable.Name, t == null ? XzaarBaseTypes.Any : t);
            }
            return null;
        }

        public override XzaarExpression Visit(IfElseNode ifElse)
        {
            var @false = ifElse.GetFalse();
            var @true = ifElse.GetTrue();
            if (@false != null) Visit(@false);
            if (!FoundType && @true != null) Visit(@true);
            return null;
        }

        public override XzaarExpression Visit(ConditionalExpressionNode node)
        {
            var @false = node.GetFalse();
            var @true = node.GetTrue();
            if (@false != null) Visit(@false);
            if (!FoundType && @true != null) Visit(@true);
            return null;
        }

        public override XzaarExpression Visit(ForLoopNode loop)
        {
            if (loop.Body != null) Visit(loop.Body);
            return null;
        }

        public override XzaarExpression Visit(ForeachLoopNode loop)
        {
            if (loop.Body != null) Visit(loop.Body);
            return null;
        }

        public override XzaarExpression Visit(CaseNode @case)
        {
            if (@case.Body != null) Visit(@case.Body);
            return null;
        }

        public override XzaarExpression Visit(MatchNode loop)
        {
            if (loop.Cases == null)
                return null;

            foreach (var @case in loop.Cases)
            {
                if (@case != null) Visit(@case);
            }

            return null;
        }

        public override XzaarExpression Visit(ReturnNode returnNode)
        {
            if (returnNode.ReturnExpression != null)
            {
                var currentFunction = TryGetCurrentFunctionFromExpression(returnNode);
                var t = TryGetTypeFromExpression(returnNode.ReturnExpression, currentFunction);
                if (t != null)
                {
                    Found(t);
                }
                if (t == null)
                {
                    // we know that this function isnt returning void, so lets return 'object'
                    Found(XzaarBaseTypes.Any);
                }
            }
            return null;
        }

        public override BlockExpression Visit(BlockNode block)
        {
            foreach (var c in block.Children)
            {
                Visit(c);

                if (FoundType) return null;
            }
            return null;
        }

        public override BlockExpression Visit(BodyNode body)
        {
            foreach (var c in body.Children)
            {
                Visit(c);

                if (FoundType) return null;
            }
            return null;
        }

        private void Found(XzaarType type)
        {
            FoundType = true;
            Type = type;
        }

        private XzaarType TryGetTypeFromExpression(AstNode expr, FunctionNode currentFunction = null)
        {
            if (expr.Kind == SyntaxKind.Empty)
            {
                return XzaarBaseTypes.Void;
            }

            if (SyntaxFacts.IsMath(expr.Kind))
            {
                var m = expr as BinaryOperatorNode;
                if (m != null)
                {
                    if (m.Op == "==" || m.Op == "!=") return XzaarBaseTypes.Boolean;
                    var leftType = TryGetTypeFromExpression(m.Left, currentFunction);
                    var rightType = TryGetTypeFromExpression(m.Right, currentFunction);
                    if (leftType?.Name == "string" || rightType?.Name == "string")
                        return XzaarBaseTypes.String;
                }

                return XzaarBaseTypes.Number;
            }

            if (SyntaxFacts.IsEquality(expr.Kind))
            {
                return XzaarBaseTypes.Boolean;
            }

            if (expr.Kind == SyntaxKind.KeywordTrue || expr.Kind == SyntaxKind.KeywordFalse)
                return XzaarBaseTypes.Boolean;

            if (SyntaxFacts.IsLiteral(expr.Kind))
            {
                switch (expr.NodeName.ToLower())
                {
                    case "string": return XzaarBaseTypes.String;
                    case "number": return XzaarBaseTypes.Number;
                    case "name":
                        {
                            var constValue = (expr.Value + "").ToLower();
                            if (constValue == "true" || constValue == "false")
                                return XzaarBaseTypes.Boolean;
                            if (expr.Value + "" == "null")
                                return XzaarBaseTypes.Any;
                        }
                        break;
                }
            }

            if (expr.Kind == SyntaxKind.KeywordNull)
                return XzaarBaseTypes.Any;

            if (expr.Kind == SyntaxKind.KeywordTrue || expr.Kind == SyntaxKind.KeywordTrue)
                return XzaarBaseTypes.Boolean;

            if (expr.Kind == SyntaxKind.Identifier || SyntaxFacts.IsLiteral(expr.Kind))
            {
                var nn = expr.NodeName.ToLower();
                if (nn == "string") return XzaarBaseTypes.String;
                if (nn == "date") return XzaarBaseTypes.Date;
                if (nn == "number") return XzaarBaseTypes.Number;
                if (nn == "array") return XzaarBaseTypes.Array;
                if (nn == "char") return XzaarBaseTypes.Char;
                if (nn == "boolean" || nn == "bool") return XzaarBaseTypes.Boolean;
                if (nn == "name")
                {
                    // check known constants
                    var constValue = expr.Value + "";
                    if (constValue == "true" || constValue == "True" || constValue == "false" || constValue == "False") return XzaarBaseTypes.Boolean;

                    // check parameters
                    if (currentFunction != null)
                    {
                        var param = currentFunction.FindParameter(constValue);
                        if (param != null)
                        {
                            var t = XzaarType.GetType(param.Type);
                            if (t != null) return t;
                        }

                        if (currentFunctionVariables.ContainsKey(constValue))
                        {
                            return currentFunctionVariables[constValue];
                        }
                    }
                }
            }

            if (expr.Kind == SyntaxKind.Expression)
            {
                // expression can include either a boolean return, number, string or object. well any actaully. damn xD
                throw new NotImplementedException();
            }

            if (SyntaxFacts.IsMemberAccess(expr.Kind))
            {
                var access = expr as MemberAccessNode;
                if (access != null)
                {
                    if (access.MemberType != null)
                    {
                        return XzaarType.GetType(access.MemberType);
                    }

                    var variable = context.FindVariableByExpression(access, true);
                    if (variable != null)
                    {
                        return variable.Type;
                    }
                }
                // find member, get type
                return null;
            }

            if (SyntaxFacts.IsAnyUnaryExpression(expr.Kind))
            {
                return XzaarBaseTypes.Number;
            }

            if (expr.Kind == SyntaxKind.FunctionInvocation)
            {
                var function = context.FindFunctionByExpression(expr);

                if (function == null)
                {
                    var similar = context.FindSimilarNamedFunctionByExpression(expr);
                    if (context.IgnoreMissingMembers) return null;
                    throw new ExpressionException("Target function '" + expr.Value + "' could not be found! Are you 100% sure that you have defined it? Or could it be a typo?" +
                        (similar != null ? " Did you possibly mean '" + similar.Name + "'?" : ""));
                }

                if (currentFunction != null)
                {
                    if (function.Name == currentFunction.Name) // && ParameterSequenceMatch(function, currentFunction))
                    {
                        // recursion detected, we cannot determine the return type here.
                        // unfortunately we will only check the very first 'return' node right now                        
                        // so we have to return null here so we can default to 'object' as return type
                        return null;
                    }
                    else
                    {
                        // if the function we found has a 'NULL' returntype, then add this 'call' to a temporary call-stack
                        // so we can break out from any potential 8-looped recursions (or other kind of recursions)
                        if (function.ReturnType == null)
                        {
                            this.context.AddToCallStack(currentFunction, function);

                            // we need to late bind the returntype to match this function's type,
                            // that way it will autocorrect itself the moment the other function has determined its return type.
                            currentFunction.BindReturnType(function.Name, () => function.ReturnType);


                        }
                        else
                        {
                            return function.ReturnType;
                        }
                    }
                }
            }
            return null;
        }


        private FunctionNode TryGetCurrentFunctionFromExpression(ReturnNode returnNode)
        {
            AstNode node = returnNode;
            while (node != null && node.Kind != SyntaxKind.FunctionDefinitionExpression)
            {
                node = node.Parent;
                if (node != null && node.Kind == SyntaxKind.FunctionDefinitionExpression) return node as FunctionNode;
            }
            return null;
        }

    }

    public class XzaarTypeFinderContext
    {
        private readonly Func<string, FunctionExpression> findFunction;
        private readonly Func<string, bool, ParameterExpression> findVariable;

        private XzaarCallStack callstack;



        public XzaarTypeFinderContext(
            Func<string, FunctionExpression> findFunction,
            Func<string, bool, ParameterExpression> findVariable)
        {
            this.findFunction = findFunction;
            this.findVariable = findVariable;
            this.callstack = new XzaarCallStack();
        }

        public bool IgnoreMissingMembers { get; set; }

        public FunctionExpression FindFunctionByExpression(AstNode expr)
        {
            if (findFunction == null) return null;

            var functionName = "";
            var functionCall = expr as FunctionCallNode;
            if (functionCall != null)
            {
                functionName = functionCall.Function.Value + "";
            }
            if (string.IsNullOrEmpty(functionName))
            {
                functionName = expr.Value + "";
            }
            if (string.IsNullOrEmpty(functionName)) return null;

            return this.findFunction(functionName);
        }

        public ParameterExpression FindVariableByExpression(AstNode expr, bool includeGlobalScope)
        {
            if (findVariable == null) return null;

            var variableNameOrValue = "";
            var access = expr as MemberAccessNode;
            if (access != null)
            {
                variableNameOrValue = access.Member?.Value + "";
            }
            if (string.IsNullOrEmpty(variableNameOrValue))
            {
                variableNameOrValue = expr.Value + "";
            }
            if (string.IsNullOrEmpty(variableNameOrValue)) return null;
            return findVariable(variableNameOrValue, includeGlobalScope);
        }
        public FunctionExpression FindSimilarNamedFunctionByExpression(AstNode expr)
        {
            // for now,
            return null;
        }

        public void ClearCallStack()
        {
            this.callstack.ClearSteps();
        }

        public void AddToCallStack(object fromFunction, object toFunction)
        {
            this.callstack.AddStep(fromFunction, toFunction);
        }

        internal class XzaarCallStack
        {
            private XzaarCallStackStep currentStep = null;

            public void AddStep(object fromFunction, object toFunction)
            {
                if (currentStep == null)
                {
                    currentStep = new XzaarCallStackStep(fromFunction, toFunction);
                }
                else
                {
                    currentStep = currentStep.NextStep(fromFunction, toFunction);
                }
            }

            public void ClearSteps()
            {
                currentStep = null;
            }

            internal class XzaarCallStackStep
            {
                private readonly object @from;
                private readonly object to;
                private XzaarCallStackStep previousStep;
                private XzaarCallStackStep nextStep;

                public XzaarCallStackStep(object from, object to)
                    : this(null, from, to) { }

                private XzaarCallStackStep(XzaarCallStackStep prev, object from, object to)
                {
                    this.previousStep = prev;
                    this.@from = @from;
                    this.to = to;
                }

                public XzaarCallStackStep Previous => previousStep;

                public XzaarCallStackStep Next => nextStep;

                public XzaarCallStackStep NextStep(object f, object t)
                {
                    nextStep = new XzaarCallStackStep(this, f, t);
                    return nextStep;
                }
            }
        }
    }
}