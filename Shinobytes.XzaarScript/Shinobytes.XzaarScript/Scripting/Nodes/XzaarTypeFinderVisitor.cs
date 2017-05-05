using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public class XzaarTypeFinderVisitor : NullNodeVisitor
    {
        private readonly XzaarTypeFinderContext context;

        public bool FoundType;

        public XzaarType Type;

        private Dictionary<string, XzaarType> currentFunctionVariables = new Dictionary<string, XzaarType>();

        public XzaarTypeFinderVisitor(XzaarTypeFinderContext context)
        {
            this.context = context;
        }

        public override XzaarExpression Visit(FunctionNode function)
        {
            currentFunctionVariables.Clear();
            return null;
        }

        public override VariableDefinitionExpression Visit(DefineVariableNode variable)
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

        public override ConditionalExpression Visit(ConditionalNode conditional)
        {
            var @false = conditional.GetFalse();
            var @true = conditional.GetTrue();
            if (@false != null) Visit(@false);
            if (!FoundType && @true != null) Visit(@true);
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

        private XzaarType TryGetTypeFromExpression(XzaarNode expr, FunctionNode currentFunction = null)
        {
            if (expr.NodeType == XzaarNodeTypes.NULL_EMPTY)
            {
                return XzaarBaseTypes.Void;
            }

            if (expr.NodeType == XzaarNodeTypes.MATH)
            {
                var m = expr as BinaryOperatorNode;
                if (m != null)
                {
                    var leftType = TryGetTypeFromExpression(m.Left, currentFunction);
                    var rightType = TryGetTypeFromExpression(m.Right, currentFunction);
                    if (leftType.Name == "string" || rightType.Name == "string")
                        return XzaarBaseTypes.String;
                }

                return XzaarBaseTypes.Number;
            }

            if (expr.NodeType == XzaarNodeTypes.LITERAL)
            {
                if (expr.NodeName == "STRING") return XzaarBaseTypes.String;
                if (expr.NodeName == "DATETIME") return XzaarBaseTypes.DateTime;
                if (expr.NodeName == "NUMBER") return XzaarBaseTypes.Number;
                if (expr.NodeName == "ARRAY") return XzaarBaseTypes.Array;
                if (expr.NodeName == "CHAR") return XzaarBaseTypes.Char;
                if (expr.NodeName == "Boolean") return XzaarBaseTypes.Boolean;
                if (expr.NodeName == "NAME")
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

            if (expr.NodeType == XzaarNodeTypes.EXPRESSION)
            {
                // expression can include either a boolean return, number, string or object. well any actaully. damn xD
                throw new NotImplementedException();
            }

            if (expr.NodeType == XzaarNodeTypes.ACCESS)
            {
                // find member, get type
                throw new NotImplementedException();
            }

            if (expr.NodeType == XzaarNodeTypes.CALL)
            {
                var function = context.FindFunctionByExpression(expr);

                if (function == null)
                {
                    var similar = context.FindSimilarNamedFunctionByExpression(expr);
                    throw new XzaarExpressionTransformerException("Target function '" + expr.Value + "' could not be found! Are you 100% sure that you have defined it? Or could it be a typo?" +
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
                        }
                        else
                        {
                            return function.ReturnType;
                        }
                    }
                }



                // find method, get return type
                throw new NotImplementedException();
            }
            return null;
        }


        private FunctionNode TryGetCurrentFunctionFromExpression(ReturnNode returnNode)
        {
            XzaarNode node = returnNode;
            while (node != null && node.NodeType != XzaarNodeTypes.FUNCTION)
            {
                node = node.Parent;
                if (node.NodeType == XzaarNodeTypes.FUNCTION) return node as FunctionNode;
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
        }

        public FunctionExpression FindFunctionByExpression(XzaarNode expr)
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

        public ParameterExpression FindVariableByExpression(XzaarNode expr, bool includeGlobalScope)
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
        public FunctionExpression FindSimilarNamedFunctionByExpression(XzaarNode expr)
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

                public XzaarCallStackStep Previous { get { return previousStep; } }

                public XzaarCallStackStep Next { get { return nextStep; } }

                public XzaarCallStackStep NextStep(object f, object t)
                {
                    nextStep = new XzaarCallStackStep(this, f, t);
                    return nextStep;
                }
            }
        }
    }
}