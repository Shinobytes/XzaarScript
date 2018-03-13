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

using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Ast;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Compiler
{
    public abstract class NodeToExpressionVisitor : INodeVisitor
    {
        private readonly IScopeProvider scopeProvider;

        private readonly List<string> errors = new List<string>();

        //private Dictionary<string, List<ParameterExpression>> functionParameters = new Dictionary<string, List<ParameterExpression>>();
        //private Dictionary<string, List<ParameterExpression>> scopeVariables = new Dictionary<string, List<ParameterExpression>>();
        //private Dictionary<string, List<FunctionExpression>> scopeFunctions = new Dictionary<string, List<FunctionExpression>>();
        //private Dictionary<string, Dictionary<string, LambdaExpression>> scopeLambdas = new Dictionary<string, Dictionary<string, LambdaExpression>>();

        //private Dictionary<FunctionExpression, XzaarMethodBase> compiledFunctions = new Dictionary<FunctionExpression, XzaarMethodBase>();
        private List<LabelTarget> definedLabels = new List<LabelTarget>();
        //private List<StructExpression> structs = new List<StructExpression>();

        private readonly Dictionary<string, StructExpression> structs = new Dictionary<string, StructExpression>();

        //private ExpressionScopeType currentScopeType = ExpressionScopeType.Global;
        //private string currentScopeIdentifier = GlobalScopeIdentifier;
        //private int currentScopeLevel = 0;
        private string currentFunctionName;
        private bool visitFunctionBody = true;


        public bool HasErrors => errors.Count > 0;
        public IList<string> Errors => errors;

        internal NodeToExpressionVisitor(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        private XzaarExpression Error(string message)
        {
            this.errors.Add("[Error] " + message);
            return XzaarExpression.Error(message);
            // return null;
        }

        // public void RegisterExternFunction()

        public XzaarExpression Visit(LambdaNode lambda)
        {
            var lambdaParameters = lambda.Parameters.Parameters.Select(Visit).Cast<ParameterExpression>().ToArray();
            var lambdaBody = Visit(lambda.Body);
            var lambdaExpr = XzaarExpression.Lambda(lambdaParameters, lambdaBody);



            return lambdaExpr;
        }

        public virtual XzaarExpression Visit(LogicalConditionalNode logicalConditional)
        {
            var oper = logicalConditional;
            var left = Visit(oper.Left);
            var right = Visit(oper.Right);
            switch (oper.Op)
            {
                case "&": return XzaarExpression.And(left, right);
                case "|": return XzaarExpression.Or(left, right);
                case "&&": return XzaarExpression.AndAlso(left, right);
                case "||": return XzaarExpression.OrElse(left, right);
            }
            return Error("'" + oper.Op + "' is not a known conditional operator");
        }

        public virtual XzaarExpression Visit(EqualityOperatorNode equalityOperator)
        {
            switch (equalityOperator.Op)
            {
                case "==": return XzaarExpression.Equal(Visit(equalityOperator.Left), Visit(equalityOperator.Right));
                case "!=": return XzaarExpression.NotEqual(Visit(equalityOperator.Left), Visit(equalityOperator.Right));
                case ">=": return XzaarExpression.GreaterThanOrEqual(Visit(equalityOperator.Left), Visit(equalityOperator.Right));
                case "<=": return XzaarExpression.LessThanOrEqual(Visit(equalityOperator.Left), Visit(equalityOperator.Right));
                case ">": return XzaarExpression.GreaterThan(Visit(equalityOperator.Left), Visit(equalityOperator.Right));
                case "<": return XzaarExpression.LessThan(Visit(equalityOperator.Left), Visit(equalityOperator.Right));
            }

            return Error("'" + equalityOperator.Op + "' is not a known equality operator");
        }

        public virtual XzaarExpression Visit(BinaryOperatorNode bin)
        {
            var left = Visit(bin.Left);
            var right = Visit(bin.Right);
            switch (bin.Op)
            {
                case "+": return XzaarExpression.Add(left, right);
                case "+=": return XzaarExpression.AddAssign(left, right);
                case "-": return XzaarExpression.Subtract(left, right);
                case "-=": return XzaarExpression.SubtractAssign(left, right);
                case "*": return XzaarExpression.Multiply(left, right);
                case "*=": return XzaarExpression.MultiplyAssign(left, right);
                case "/": return XzaarExpression.Divide(left, right);
                case "/=": return XzaarExpression.DivideAssign(left, right);
                case "%": return XzaarExpression.Modulo(left, right);
                case "%=": return XzaarExpression.ModuloAssign(left, right);
                case "&": return XzaarExpression.And(left, right);
                case "&=": return XzaarExpression.AndAssign(left, right);
                case "|=": return XzaarExpression.OrAssign(left, right);
                case "|": return XzaarExpression.Or(left, right);



                // fallback for new transformer implementation
                case "==": return XzaarExpression.Equal(left, right);
                case "!=": return XzaarExpression.NotEqual(left, right);
                case ">=": return XzaarExpression.GreaterThanOrEqual(left, right);
                case "<=": return XzaarExpression.LessThanOrEqual(left, right);
                case ">": return XzaarExpression.GreaterThan(left, right);
                case "<": return XzaarExpression.LessThan(left, right);

                case "&&": return XzaarExpression.AndAlso(left, right);
                case "||": return XzaarExpression.OrElse(left, right);
            }
            return Error("'" + bin.Op + "' is not a known binary operator");
        }

        public virtual XzaarExpression Visit(LogicalNotNode node)
        {
            var expression = Visit(node.Expression);
            return XzaarExpression.LogicalNot(expression);
        }

        public virtual XzaarExpression Visit(IfElseNode ifElse)
        {
            XzaarExpression ifTrue, ifFalse;

            var c = ifElse.GetCondition();
            var test = Visit(c);
            if (test == null)
            {
                throw new ExpressionException("if statements cannot have empty conditions, did you mean to do 'if (true) { ...' ?");
            }
            var t = ifElse.GetTrue();
            var f = ifElse.GetFalse();
            ifTrue = t != null && !t.IsEmpty() ? Visit(t) : XzaarExpression.Empty();
            ifFalse = f != null && !f.IsEmpty() ? Visit(f) : XzaarExpression.Empty();
            return XzaarExpression.IfElse(test, ifTrue, ifFalse);
        }

        public virtual XzaarExpression Visit(FunctionParametersNode parameters)
        {
            return Error("Failed to parse function parameters");
        }

        public virtual XzaarExpression Visit(ReturnNode returnNode)
        {
            if (returnNode.ReturnExpression != null)
            {
                return XzaarExpression.Return(null, Visit(returnNode.ReturnExpression));
            }
            return XzaarExpression.Return(null);
        }

        public virtual XzaarExpression Visit(ContinueNode continueNode)
        {
            return XzaarExpression.Continue();
        }

        public virtual XzaarExpression Visit(BreakNode breakNode)
        {
            return XzaarExpression.Break();
        }

        public XzaarExpression Visit(DefinitionNode definition)
        {
            return Error("Unknown member declaration");
        }

        public virtual XzaarExpression Visit(LabelNode label)
        {
            var xzaarLabelTarget = XzaarExpression.Label(label.Name);
            definedLabels.Add(xzaarLabelTarget);
            return XzaarExpression.Label(xzaarLabelTarget);
        }

        public virtual XzaarExpression Visit(GotoNode @goto)
        {
            var label = definedLabels.FirstOrDefault(l => l.Name == @goto.LabelName);
            return XzaarExpression.Goto(label);
        }

        public virtual XzaarExpression Visit(MemberAccessChainNode access)
        {
            var last = Visit(access.LastAccessor);
            var now = Visit(access.Accessor);
            return XzaarExpression.AccessChain(last, now);
        }

        public virtual XzaarExpression Visit(CreateStructNode createStruct)
        {
            if (createStruct.FieldInitializers != null)
            {
                var initializers = createStruct.FieldInitializers.Select(Visit).ToArray();
                return XzaarExpression.CreateStruct(createStruct.StructNode.Name, initializers);
            }
            return XzaarExpression.CreateStruct(createStruct.StructNode.Name);
        }

        public virtual XzaarExpression Visit(MemberAccessNode member)
        {
            XzaarExpression access = null;
            var literal = member.Member as LiteralNode;
            var outType = GetOrCreateType(member.MemberType);
            var arrayInitializer = false;
            if (literal != null)
            {
                arrayInitializer = member.NodeName == "ARRAY";
                var declaringType = GetOrCreateType(member.DeclaringType);
                // first try and see if we can find a matching field
                var field = FindFieldInType(literal.Value + "", declaringType);
                if (field != null)
                {
                    access = field;
                }
                else
                {
                    if ((member.DeclaringType == "any" || member.DeclaringType == "") && member.NodeName != "ARRAY")
                    {
                        if (member.NodeName != "NAME")
                        {
                            var constantAccess = XzaarExpression.Constant(member.Value);
                            if (member.ArrayIndex != null) constantAccess.ArrayIndex = Visit(member.ArrayIndex);
                            return constantAccess;
                        }
                        access = XzaarExpression.Variable(XzaarBaseTypes.Any, member.Value + "");
                    }
                    var memberTypeCheck = !string.IsNullOrEmpty(member.MemberType)
                        ? member.MemberType
                        : member.DeclaringType;
                    if (memberTypeCheck != null)
                    {

                        if ((memberTypeCheck == "array" || memberTypeCheck.EndsWith("[]")) && ArrayHelper.IsArrayProperty(member))
                            access = XzaarExpression.Variable(XzaarBaseTypes.Any, member.Value + "");

                        if (memberTypeCheck == "string" && StringHelper.IsStringProperty(member))
                            access = XzaarExpression.Variable(StringHelper.IsLengthProperty(member)
                                ? XzaarBaseTypes.Number
                                : XzaarBaseTypes.Any, member.Value + "");
                    }
                }
            }

            if (access == null)
            {
                // still null? do a normal visit
                access = Visit(member.Member);
            }

            if (arrayInitializer && access is ConstantExpression)
            {
                var c = access as ConstantExpression;
                if (member.ArrayIndex != null) c.ArrayIndex = Visit(member.ArrayIndex);
                return c;
            }

            return member.ArrayIndex != null
                ? XzaarExpression.MemberAccess(access, Visit(member.ArrayIndex), outType)
                : XzaarExpression.MemberAccess(access, outType);
        }

        private FieldExpression FindFieldInType(string s, XzaarType type)
        {
            if (type == null) type = XzaarBaseTypes.Void;
            var field = type.GetField(s);
            if (field != null)
            {
                return new FieldExpression(field.FieldType, field.Name, type);
            }

            if (structs.TryGetValue(type.Name, out var str))
            {
                return str.Fields.Cast<FieldExpression>().FirstOrDefault(b => b.Name == s);
            }

            return null;
        }

        public virtual XzaarExpression Visit(AssignNode assign)
        {
            var left = Visit(assign.Left);
            var right = Visit(assign.Right);

            if (right is LambdaExpression lambda)
            {
                this.scopeProvider.Current.BindLambda(left as ParameterExpression, lambda);
            }

            return XzaarExpression.Assign(left, right);
        }

        public virtual XzaarExpression Visit(UnaryNode unary)
        {
            var item = Visit(unary.Item);
            if (unary.Operator == "++" && unary.IsPostUnary)
                return XzaarExpression.PostIncrementor(item);
            if (unary.Operator == "++")
                return XzaarExpression.Incrementor(item);
            if (unary.Operator == "--" && unary.IsPostUnary)
                return XzaarExpression.PostDecrementor(item);
            if (unary.Operator == "--")
                return XzaarExpression.Decrementor(item);
            if (unary.Operator == "!")
                return XzaarExpression.LogicalNot(item);
            if (unary.Operator == "+")
                return item;
            if (unary.Operator == "-")
                return NegateValue(item);

            return Error("'" + unary.Operator + "' is not a known unary operator");
        }

        private XzaarExpression NegateValue(XzaarExpression item)
        {
            var constExpr = item as ConstantExpression;
            if (constExpr != null && constExpr.Type.Name == "number")
            {
                var val = double.Parse(constExpr.Value?.ToString() ?? "0");
                return XzaarExpression.Constant(-val, XzaarBaseTypes.Number);
            }

            var reference = item as ParameterExpression;
            if (reference != null && (reference.Type.Name == "number" || reference.Type.Name == "any"))
            {
                return XzaarExpression.Negate(item);
            }

            var memExpr = item as MemberExpression;
            if (memExpr != null && (Equals(memExpr.MemberType.Name, XzaarBaseTypes.Any.Name) || Equals(memExpr.MemberType.Name, XzaarBaseTypes.Number.Name)))
            {
                return XzaarExpression.Negate(memExpr);
            }

            var chainExpr = item as MemberAccessChainExpression;
            if (chainExpr != null && chainExpr.ResultType != null && (Equals(chainExpr.ResultType.Name, XzaarBaseTypes.Any.Name) || Equals(chainExpr.ResultType.Name, XzaarBaseTypes.Number.Name)))
            {
                return XzaarExpression.Negate(chainExpr);
            }

            return Error("'" + item + "' cannot be negated");
        }

        public virtual XzaarExpression Visit(CaseNode @case)
        {
            var body = Visit(@case.Body);
            if (@case.IsDefaultCase)
            {
                return XzaarExpression.DefaultCase(body);
            }

            var test = Visit(@case.Test);
            return XzaarExpression.Case(test, body);
        }

        public virtual XzaarExpression Visit(MatchNode match)
        {
            var valueExpr = Visit(match.ValueExpression);
            var cases = match.Cases.Select(Visit).ToArray();
            return XzaarExpression.Switch(valueExpr, cases);
        }

        public virtual XzaarExpression Visit(DoWhileLoopNode loop)
        {
            return XzaarExpression.DoWhile(Visit(loop.Test), Visit(loop.Body));
        }

        public virtual XzaarExpression Visit(WhileLoopNode loop)
        {
            return XzaarExpression.While(Visit(loop.Test), Visit(loop.Body));

            //var labelTarget = XzaarExpression.Label();
            //var label = XzaarExpression.Label(labelTarget);
            //var body = Visit(loop.Body);
            //var test = Visit(loop.Test);
            //if (!(body is BlockExpression))
            //{
            //    body = XzaarExpression.Block(
            //        body,
            //        XzaarExpression.IfThen(
            //            XzaarExpression.NotEqual(test, XzaarExpression.Constant(true, XzaarBaseTypes.Boolean)),
            //            XzaarExpression.Goto(labelTarget)
            //        )
            //    );
            //}
            //else
            //{
            //    var block = body as BlockExpression;
            //    var expressions = block.Expressions.ToList();
            //    expressions.Add(XzaarExpression.IfThen(
            //           XzaarExpression.NotEqual(test, XzaarExpression.Constant(true, XzaarBaseTypes.Boolean)),
            //           XzaarExpression.Goto(labelTarget)
            //       ));

            //    body = XzaarExpression.Block(expressions.ToArray());
            //}

            //return XzaarExpression.Block(XzaarExpression.Loop(body), label);
        }

        public virtual XzaarExpression Visit(ForeachLoopNode loop)
        {
            return XzaarExpression.ForEach(
                    Visit(loop.Variable),
                    Visit(loop.Source),
                    Visit(loop.Body));
        }

        public virtual XzaarExpression Visit(ForLoopNode loop)
        {
            return XzaarExpression.For(
                    Visit(loop.Initiator),
                    Visit(loop.Test),
                    Visit(loop.Incrementor),
                    Visit(loop.Body)
                );
        }

        public virtual XzaarExpression Visit(ConditionalExpressionNode node)
        {
            //if (node.Type?.ToLower() == "void")
            //    return Error($"Invalid conditional operator return type '{node.Type}'");

            return XzaarExpression.Conditional(
                Visit(node.GetCondition()),
                Visit(node.GetTrue()),
                Visit(node.GetFalse())
            );
        }

        public virtual XzaarExpression Visit(LoopNode loop)
        {
            return XzaarExpression.Loop(Visit(loop.Body));
        }

        public virtual XzaarExpression Visit(DefineVariableNode definedVariable)
        {
            VariableDefinitionExpression variable = null;
            if (definedVariable.AssignmentExpression != null)
            {
                var assignmentExpression = Visit(definedVariable.AssignmentExpression);
                variable = XzaarExpression.DefineVariable(
                    GetOrCreateType(definedVariable.Type),
                    definedVariable.Name,
                    assignmentExpression);
            }
            else
            {
                variable = XzaarExpression.DefineVariable(GetOrCreateType(definedVariable.Type), definedVariable.Name);
            }

            this.scopeProvider.Current.AddVariable(variable);

            if (variable.AssignmentExpression is LambdaExpression lambda)
            {
                this.scopeProvider.Current.BindLambda(variable, lambda);
            }

            return variable;
        }

        public XzaarExpression Visit(VariableNode variable)
        {
            var @type = GetOrCreateType(variable.Type);
            if (@type == null) throw new InvalidOperationException(variable.Type + " is an unknown type.");
            var v = XzaarExpression.Variable(@type, variable.Name);

            this.scopeProvider.Current.AddVariable(v);

            //if (!scopeVariables.ContainsKey(this.currentScopeIdentifier))
            //{
            //    scopeVariables.Add(this.currentScopeIdentifier, new List<ParameterExpression> { v });
            //}
            //else
            //{
            //    scopeVariables[this.currentScopeIdentifier].Add(v);
            //}
            return v;
        }

        public virtual XzaarExpression Visit(ParameterNode parameter)
        {
            var @type = GetOrCreateType(parameter.Type);
            if (@type == null) throw new InvalidOperationException(parameter.Type + " is an unknown type.");
            var param = XzaarExpression.Parameter(@type, parameter.Name);

            if (parameter.Parent != null && parameter.Parent.Parent != null)
            {
                if (parameter.Parent.Parent is FunctionNode function)
                {

                    scopeProvider.Current.AddParameter(param);

                    //if (!functionParameters.ContainsKey(function.Name))
                    //{
                    //    functionParameters.Add(function.Name, new List<ParameterExpression> { param });
                    //}
                    //else
                    //{
                    //    functionParameters[function.Name].Add(param);
                    //}
                }
            }
            return param;
        }

        public virtual XzaarExpression Visit(FunctionCallNode call)
        {
            string functionAlias = null;
            var arguments = call.Arguments.Select(Visit).Where(a => a != null).ToArray();
            var f = call.Function;

            if (f.Kind == SyntaxKind.FunctionDefinitionExpression)
            {
                // using an already declared function
                if (!(f is FunctionNode function))
                {
                    throw new ArgumentNullException("function", "Target function cannot be null");
                }

                if (!(Visit(function) is FunctionExpression method))
                {
                    return Error("Call to unknown function. Something must have gone really bad!");
                }

                if (call.Instance == null)
                {
                    return XzaarExpression.Call(method, arguments);
                }

                return Error("The function '" + call.Instance.StringValue + "." + function.Name + "' cannot be called as locally defined instanced functions are not supported");
            }

            if (f.Kind == SyntaxKind.FunctionInvocation)
            {
                // if we are invoking the result of the previous invocation.
                // ex: a()()
                // return a function call chain

                if (f is FunctionCallNode fcall)
                {
                    var target = Visit(fcall) as FunctionCallExpression;

                    return XzaarExpression.Call(target, arguments);
                }

                return Error(
                    "A direct invocation of the result from a previous invocation. { Ex: a()(); } is not yet supported.");
            }

            if (SyntaxFacts.IsLiteral(f.Kind) || SyntaxFacts.IsMemberAccess(f.Kind))
            {
                var functionName = "";
                if (f is MemberAccessChainNode chain)
                {
                    functionName = chain.Accessor.StringValue;
                    if (call.Instance == null) call.Instance = chain.LastAccessor;
                }
                else
                {
                    // using an undeclared function
                    // either its a missing function or a internal function               
                    functionName = f.Value?.ToString();
                }

                if (functionName == null) throw new InvalidOperationException();

                if (call.Instance == null)
                {
                    var function =
                        scopeProvider.Current.Find<AnonymousFunctionExpression>(functionName, arguments.Length);// FindMatchingFunctionInCurrentOrGlobalScope(functionName, arguments.Length);
                    if (function == null)
                    {
                        // if its not a function, lets see if we are trying to invoke a function reference
                        // or if we have a parameter or variable of type "any" with the same name. As we cannot guarantee whether or not the variable
                        // is a function reference or lambda.

                        // same thing goes with member access with an array index. Lets say we have a list of objects where one could b
                        var varOrParam = scopeProvider.Current.Find<ParameterExpression>(functionName);
                        if (varOrParam == null
                            || !varOrParam.IsFunctionReference && !varOrParam.Type.IsAny)
                        {
                            // to avoid traversing the tree here to find the source. We will just return 
                            // oh well. still can't seem to determine what function it is. still undefined 'extern' function as well?

                            if (varOrParam == null
                                || !varOrParam.Type.IsArray
                                || !Equals(f.Type, XzaarBaseTypes.Any.Name)
                                || !(f is MemberAccessNode memAccess)
                                || memAccess.ArrayIndex == null
                                || !Equals(memAccess.Type, XzaarBaseTypes.Any.Name))
                            {
                                return Error("Target function '" + functionName +
                                             "' was not found. Forgot to declare it?");
                            }
                        }

                        function = varOrParam.FunctionReference;
                        functionAlias = varOrParam.Name;

                        if (f is MemberAccessNode access)
                        {
                            var func = Visit(access);
                            return XzaarExpression.Call(functionAlias, func as MemberExpression, arguments);
                        }
                    }

                    if (function is FunctionExpression invocation)
                    {
                        return XzaarExpression.Call(functionAlias, invocation, arguments);
                    }

                    return XzaarExpression.Call(functionAlias, function as LambdaExpression, arguments);
                }

                var instanceVariableName = call.Instance.Value + "";
                var variableOrParameter = this.scopeProvider.Current.Find<ParameterExpression>(instanceVariableName);
                if (variableOrParameter != null)
                {
                    if (Equals(call.Function.Type, XzaarBaseTypes.Any.Name) || Equals(variableOrParameter.Type, XzaarBaseTypes.Any) || (ArrayHelper.IsArrayFunction(functionName) && variableOrParameter.Type.IsArray))
                    {
                        return XzaarExpression.Call(variableOrParameter,
                            XzaarExpression.Function(functionName, new ParameterExpression[arguments.Length], XzaarBaseTypes.Any, true),
                            arguments);
                    }
                }

                //var p = scopeProvider.Current.FindParameter(instanceVariableName);
                //if (currentFunctionName != null && this.functionParameters.ContainsKey(currentFunctionName))
                //{
                //    //var parameters = functionParameters[currentFunctionName];
                //    //var p = parameters.FirstOrDefault(pa => pa.Name == instanceVariableName);
                //    if (p != null)
                //    {
                //        if (Equals(p.Type, XzaarBaseTypes.Any) || (ArrayHelper.IsArrayFunction(call.Value + "") && p.Type.IsArray))
                //        {
                //            return XzaarExpression.Call(p,
                //                XzaarExpression.Function(functionName, new ParameterExpression[arguments.Length], XzaarBaseTypes.Any, true),
                //                arguments);
                //        }
                //    }
                //}

                ////var v = this.FindVariable(instanceVariableName, true);
                //var v = this.scopeProvider.Current.FindVariable(instanceVariableName);
                //if (v != null)
                //{
                //    if (Equals(v.Type, XzaarBaseTypes.Any) || (ArrayHelper.IsArrayFunction(functionName) && v.Type.IsArray))
                //    {
                //        return XzaarExpression.Call(v,
                //            XzaarExpression.Function(functionName, new ParameterExpression[arguments.Length], XzaarBaseTypes.Any, true),
                //            arguments);
                //    }
                //}

                // find function based on the instance
                return Error("Target function '" + functionName + "' was not found. Forgot to declare it?");
            }

            return Error("Call to unknown function. Something must have gone really bad!");
        }

        public virtual XzaarExpression Visit(ExpressionNode expression)
        {
            if (expression.Children.Count == 0)
                return null;

            throw new System.NotImplementedException();
        }

        public virtual XzaarExpression Visit(ArgumentNode argument)
        {
            var arg = Visit(argument.Item);

            //if (arg == null || arg.Type == XzaarBaseTypes.Void)
            //{
            //    // no arguments supplied
            //    return null;
            //}


            return arg;
        }

        public virtual XzaarExpression Visit(FunctionNode function)
        {
            if (FindMatchingFunctionInGlobalScope(function) is FunctionExpression existing)
            {
                // if the function has already been added but the body or return type hasnt been defined then we want to do that.
                // this is because the function was already added during the 'function discovery' step
                if (existing.ReturnType == null || existing.GetBody() == null)
                {
                    using (EnterScope(function.Name, ExpressionScopeType.Function))
                    {

                        this.currentFunctionName = function.Name;

                        // Increase scope depth.

                        //SetCurrentScope(function.Name, ExpressionScopeType.Function);

                        existing.SetReturnType(function.GetReturnType(
                            new XzaarTypeFinderContext(
                                // FindMatchingFunctionInGlobalScope,
                                (a, b) => scopeProvider.Current.Find<AnonymousFunctionExpression>(a, b),
                                (a, b) => scopeProvider.Current.Find<ParameterExpression>(a)
                            ))).SetBody(Visit(function.Body));

                        if (function.IsReturnTypeBound)
                        {
                            if (scopeProvider.Current.Find<AnonymousFunctionExpression>(function.ReturnTypeBindingName) is FunctionExpression fn)
                                //if (FindMatchingFunctionInGlobalScope(function.ReturnTypeBindingName) is FunctionExpression fn)
                                existing.BindReturnType(() => fn.ReturnType);
                        }

                    }
                }


                return existing;
            }

            //SetCurrentScope(function.Name, ExpressionScopeType.Function);

            using (EnterScope(function.Name, ExpressionScopeType.Function))
            {

                var f = XzaarExpression.Function(
                    function.Name,
                    function.Parameters.Parameters.Select(Visit).Cast<ParameterExpression>().ToArray(),
                    function.ReturnType,
                    null,
                    function.IsExtern
                );

                // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
                var targetFunction = AddFunctionToGlobalScope(f)
                    .SetReturnType(function.GetReturnType(
                         new XzaarTypeFinderContext(
                             //FindMatchingFunctionInGlobalScope,
                             (a, b) => scopeProvider.Current.Find<AnonymousFunctionExpression>(a, b),
                             (a, b) => scopeProvider.Current.Find<ParameterExpression>(a))
                         { IgnoreMissingMembers = !visitFunctionBody }));
                if (visitFunctionBody) targetFunction.SetBody(Visit(function.Body));

                // leave scope

                return f;
            }
        }

        public virtual XzaarExpression Visit(StructNode node)
        {
            var structFields = node.Fields.Select(Visit).ToArray();
            return XzaarExpression.Struct(
                node.Name,
                structFields
            );
        }

        public virtual XzaarExpression Visit(FieldNode node)
        {
            var declaringType = GetOrCreateType(node.DeclaringType);

            if (structs.TryGetValue(node.Type, out var a))
            {
                if (XzaarType.TryGetType(a.Name, a, out var newType))
                {
                    return XzaarExpression.Field(newType, node.Name, declaringType);
                }
            }
            return XzaarExpression.Field(XzaarType.GetType(node.Type), node.Name, declaringType);
        }

        public virtual XzaarExpression Visit(NumberNode number)
        {
            return XzaarExpression.Constant(number.Value, XzaarBaseTypes.Number);
        }

        public virtual XzaarExpression Visit(LiteralNode literal)
        {
            if (literal.NodeName == "NAME")
            {
                var value = literal.Value;
                if (IsKnownConstant(value))
                {
                    return GetKnownConstant(value);
                }

                var variableOrParameter = scopeProvider.Current.Find<ParameterExpression>(literal.StringValue);

                if (variableOrParameter != null)
                {
                    if (variableOrParameter.IsParameter)
                    {
                        return XzaarExpression.Parameter(variableOrParameter.Type, variableOrParameter.Name);
                    }

                    return XzaarExpression.Variable(variableOrParameter.Type, variableOrParameter.Name);
                }
            }
            else
            {
                if (literal.NodeName == "ARRAY" && literal.Children.Count > 0)
                {
                    // return XzaarExpression.ArrayInitializer(expr.ToArray());
                    return XzaarExpression.Constant(literal.Children.Select(Visit).ToArray(), GetOrCreateType(literal.NodeName.ToLower()));
                }

                return XzaarExpression.Constant(literal.Value, GetOrCreateType(literal.NodeName.ToLower()));
            }

            var function = FindFunction(literal.StringValue);
            if (function != null)
            {
                return XzaarExpression.FunctionReference(function, literal.StringValue);
            }

            return XzaarExpression.Variable(XzaarBaseTypes.Any, literal.Value + "");
        }

        private bool IsKnownConstant(object value)
        {
            return GetKnownConstant(value) != null;
        }

        private XzaarExpression GetKnownConstant(object value)
        {
            var strValue = value?.ToString();
            if (value == null || strValue == "Null" || strValue == "null") return XzaarExpression.Constant(null, XzaarBaseTypes.Any);
            if (strValue == "True" || strValue == "true") return XzaarExpression.Constant(true, XzaarBaseTypes.Boolean);
            if (strValue == "False" || strValue == "false") return XzaarExpression.Constant(false, XzaarBaseTypes.Boolean);
            return null;
        }

        private AnonymousFunctionExpression FindFunction(string functionName)
        {
            return scopeProvider.Current.Find<AnonymousFunctionExpression>(functionName);

            //var function = FindMatchingFunctionInCurrentScope(functionName);
            //if (function != null)
            //{
            //    return function;
            //}

            //return FindMatchingFunctionInGlobalScope(functionName);
        }

        //private ParameterExpression FindVariable(string variableOrValue, bool includeGlobalVariables)
        //{
        //    // need a way to traverse scope upwards, right now we only have global and function scope at undefined level

        //    if (variableOrValue.StartsWith("$"))
        //    {
        //        return new ParameterExpression(variableOrValue);
        //    }

        //    if (includeGlobalVariables)
        //    {
        //        if (this.currentScopeIdentifier != GlobalScopeIdentifier && this.scopeVariables.ContainsKey(currentScopeIdentifier))
        //        {
        //            var vlist = this.scopeVariables[this.currentScopeIdentifier];
        //            var v = vlist.FirstOrDefault(p => p.Name == variableOrValue);
        //            if (v != null) return v;
        //        }

        //        if (this.scopeVariables.ContainsKey(GlobalScopeIdentifier))
        //        {
        //            var vlist = this.scopeVariables[GlobalScopeIdentifier];
        //            var v = vlist.FirstOrDefault(p => p.Name == variableOrValue);
        //            if (v != null) return v;
        //        }

        //    }
        //    else
        //    {
        //        if (this.scopeVariables.ContainsKey((this.currentScopeIdentifier)))
        //        {
        //            var vlist = this.scopeVariables[this.currentScopeIdentifier];
        //            return vlist.FirstOrDefault(p => p.Name == variableOrValue);
        //        }
        //    }

        //    return null;
        //}


        public virtual BlockExpression Visit(BlockNode block)
        {
            var xzaarExpressions = block.Children.Select(Visit).ToArray();
            return XzaarExpression.Block(
               xzaarExpressions
           );
        }

        public virtual XzaarExpression Visit(EmptyNode empty)
        {
            return XzaarExpression.Empty();
        }

        public virtual XzaarExpression Visit(EntryNode node)
        {
            DiscoverStructsAndFunctions(node);

            var items = node.Children.Select(Visit).ToArray();
            if (items.Length > 1) return XzaarExpression.Block(items);
            return items.FirstOrDefault();
        }


        private void DiscoverStructsAndFunctions(EntryNode node)
        {
            var items = node.Children;
            if (node.Body.Kind == SyntaxKind.Block)
                items = node.Body.Children;

            foreach (var c in items)
            {
                if (c is FunctionNode function)
                {
                    if (function.ReturnType == null)
                    {
                        //var identifier = currentScopeIdentifier;
                        //var type = currentScopeType;
                        //var level = currentScopeLevel;

                        visitFunctionBody = false;

                        Visit(function);

                        visitFunctionBody = true;

                        //SetCurrentScope(identifier, type, level);

                        if (function.ReturnType != null && function.ReturnType.IsAny)
                        {
                            function.SetReturnType(null);
                        }
                    }

                    var f = XzaarExpression.Function(
                        function.Name,
                        function.Parameters.Parameters.Select(Visit).Cast<ParameterExpression>().ToArray(),
                        function.ReturnType,
                        null,
                        function.IsExtern
                    );

                    // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
                    AddFunctionToGlobalScope(f);
                }
                else
                {
                    if (c is StructNode str)
                    {
                        var s = XzaarExpression.Struct(
                            str.Name,
                            str.Fields.Select(Visit).ToArray()
                        );

                        // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
                        AddStructToGlobalScope(s);
                    }

                    //if (c is ClassNode clss)
                    //{
                    //}

                    //if (c is EnumNode enm)
                    //{
                    //}
                }
            }
        }

        public virtual BlockExpression Visit(BodyNode body)
        {
            if (body.Children.Count == 0)
                return XzaarExpression.Block();
            return XzaarExpression.Block(body.Children.Select(Visit).ToArray());
        }

        public virtual XzaarExpression Visit(AstNode node)
        {

            if (node == null || node is ErrorNode) // only caused when we parse an error node
                return null;

            var name = node.GetType().Name;
            if (name.EndsWith("XzaarNode"))
                return Error("Unknown node '" + name + "' found. Something really really bad happened here");
            if (node is LambdaNode lambdaNode) return Visit(lambdaNode);
            if (node is ConditionalExpressionNode expressionNode) return Visit(expressionNode);
            if (node is LogicalConditionalNode conditionalNode) return Visit(conditionalNode);
            if (node is EqualityOperatorNode operatorNode) return Visit(operatorNode);
            if (node is BinaryOperatorNode binaryOperatorNode) return Visit(binaryOperatorNode);
            if (node is LogicalNotNode notNode) return Visit(notNode);
            if (node is IfElseNode elseNode) return Visit(elseNode);
            if (node is FunctionParametersNode parametersNode) return Visit(parametersNode);
            if (node is ReturnNode returnNode) return Visit(returnNode);
            if (node is ContinueNode continueNode) return Visit(continueNode);
            if (node is BreakNode breakNode) return Visit(breakNode);
            if (node is LabelNode labelNode) return Visit(labelNode);
            if (node is GotoNode gotoNode) return Visit(gotoNode);
            if (node is MemberAccessChainNode chainNode) return Visit(chainNode);
            if (node is CreateStructNode structNode) return Visit(structNode);
            if (node is MemberAccessNode accessNode) return Visit(accessNode);
            if (node is AssignNode assignNode) return Visit(assignNode);
            if (node is UnaryNode unaryNode) return Visit(unaryNode);
            if (node is CaseNode caseNode) return Visit(caseNode);
            if (node is MatchNode matchNode) return Visit(matchNode);
            if (node is DoWhileLoopNode loopNode) return Visit(loopNode);
            if (node is WhileLoopNode whileLoopNode) return Visit(whileLoopNode);
            if (node is ForeachLoopNode foreachLoopNode) return Visit(foreachLoopNode);
            if (node is ForLoopNode forLoopNode) return Visit(forLoopNode);
            if (node is LoopNode node1) return Visit(node1);
            if (node is DefineVariableNode variableNode) return Visit(variableNode);
            if (node is VariableNode variableNode1) return Visit(variableNode1);
            if (node is ParameterNode parameterNode) return Visit(parameterNode);
            if (node is FunctionCallNode callNode) return Visit(callNode);
            if (node is ExpressionNode expressionNode1) return Visit(expressionNode1);
            if (node is ArgumentNode argumentNode) return Visit(argumentNode);
            if (node is FunctionNode functionNode) return Visit(functionNode);
            if (node is StructNode structNode1) return Visit(structNode1);
            if (node is FieldNode fieldNode) return Visit(fieldNode);
            if (node is NumberNode numberNode) return Visit(numberNode);
            if (node is LiteralNode literalNode) return Visit(literalNode);
            if (node is BodyNode bodyNode) return Visit(bodyNode);
            if (node is BlockNode blockNode) return Visit(blockNode);
            if (node is EmptyNode emptyNode) return Visit(emptyNode);
            if (node is EntryNode entryNode) return Visit(entryNode);

            return Visit(node);

            // or just simply:
            // return Visit((dynamic)node);
        }


        private XzaarType GetOrCreateType(string typeName)
        {
            if (this.structs.TryGetValue(typeName, out var a))
            {
                if (XzaarType.TryGetType(a.Name, a, out var newType))
                {
                    return newType;
                }
            }

            return XzaarType.GetType(typeName) ?? XzaarBaseTypes.Void;
        }

        //private void SetCurrentScope(string identifier)
        //{
        //    SetCurrentScope(identifier, currentScopeType, currentScopeLevel);
        //}

        //private void SetCurrentScope(string identifier, ExpressionScopeType type)
        //{
        //    SetCurrentScope(identifier, type, currentScopeLevel);
        //}

        //private void SetCurrentScope(string identifier, ExpressionScopeType type, int level)
        //{
        //    this.currentScopeIdentifier = identifier;
        //    this.currentScopeType = type;
        //    this.currentScopeLevel = level;
        //}

        private ExpressionScope EnterScope(string name, ExpressionScopeType scopeType)
        {
            return scopeProvider.Get(name, scopeType);
        }


        //private XzaarMethodBase BuildMethod(FunctionExpression func)
        //{
        //    if (this.compiledFunctions.ContainsKey(func))
        //    {
        //        return this.compiledFunctions[func];
        //    }

        //    var f = XzaarExpression.BuildMethod(func);

        //    this.compiledFunctions.Add(func, f);

        //    return f;
        //}

        //private AnonymousFunctionExpression FindMatchingFunctionInGlobalScope(string name, int argCount = -1)
        //{
        //    return FindMatchingFunctionInScope(name, GlobalScopeIdentifier, argCount);
        //}

        //private AnonymousFunctionExpression FindMatchingFunctionInCurrentScope(string name, int argCount = -1)
        //{
        //    return FindMatchingFunctionInScope(name, this.currentScopeIdentifier, argCount);
        //}

        //private AnonymousFunctionExpression FindMatchingFunctionInCurrentOrGlobalScope(string name, int argCount = -1)
        //{
        //    var function = FindMatchingFunctionInScope(name, this.currentScopeIdentifier, argCount);
        //    if (function != null)
        //    {
        //        return function;
        //    }

        //    return FindMatchingFunctionInGlobalScope(name, argCount);
        //}

        //private AnonymousFunctionExpression FindMatchingFunctionInScope(string name, string scope, int argCount = -1)
        //{
        //    if (this.scopeFunctions.ContainsKey(scope))
        //    {
        //        var function = this.scopeFunctions[scope].FirstOrDefault(f => f.Name == name && (argCount == -1 || f.GetParameters().Length == argCount));
        //        if (function != null)
        //        {
        //            return function;
        //        }
        //    }

        //    if (this.scopeLambdas.ContainsKey(scope))
        //    {
        //        // in case its a lambda invocation, test to see if we got any variables names that may match.
        //        if (this.scopeLambdas[scope].TryGetValue(name, out var result) && (argCount == -1 || result.Parameters.Length == argCount))
        //        {
        //            return result;
        //        }
        //    }

        //    return null;
        //}

        //private AnonymousFunctionExpression FindMatchingFunctionInGlobalScope(string name, int argCount)
        //{
        //    if (this.scopeFunctions.ContainsKey(GlobalScopeIdentifier))
        //    {
        //        var function = this.scopeFunctions[GlobalScopeIdentifier].FirstOrDefault(f => f.Name == name && (argCount == -1 || f.GetParameters().Length == argCount));
        //        if (function != null)
        //        {
        //            return function;
        //        }
        //    }
        //    // in case its a lambda invocation, test to see if we got any variables names that may match.
        //    if (this.scopeLambdas.ContainsKey(GlobalScopeIdentifier))
        //    {
        //        if (this.scopeLambdas[GlobalScopeIdentifier].TryGetValue(name, out var result) && (argCount == -1 || result.Parameters.Length == argCount))
        //        {
        //            return result;
        //        }
        //    }
        //    return null;
        //}


        //private AnonymousFunctionExpression FindMatchingFunctionInGlobalScope(string name, XzaarType returnType, int argCount)
        //{
        //    if (!this.scopeFunctions.ContainsKey(GlobalScopeIdentifier))
        //    {
        //        return null;
        //    }

        //    // ... don't support lambdas here. besides i do

        //    return this.scopeFunctions[GlobalScopeIdentifier].FirstOrDefault(f => f.Name == name && f.ReturnType.Name == returnType.Name && f.GetParameters().Length == argCount);
        //}

        private AnonymousFunctionExpression FindMatchingFunctionInGlobalScope(FunctionNode function)
        {
            var functions = scopeProvider.Current.FindFunctions(function.Name, function.Parameters.Parameters.Count);
            //var possibleMatches = this.scopeFunctions[GlobalScopeIdentifier].Where(
            //    f => f.Name == function.Name);//&& f.ReturnType.Name == function.GetReturnType(finderContext).Name);
            foreach (var f in functions)
            {
                var param1 = function.Parameters.Parameters;
                var param2 = f.Parameters;
                if (ParameterSequenceMatch(param1, param2))
                {
                    return f;
                }
            }
            return null;
        }

        private bool ParameterSequenceMatch(IList<ParameterNode> param1, ParameterExpression[] param2)
        {
            if (param1.Count != param2.Length) return false;
            for (var i = 0; i < param1.Count; i++)
            {
                var t1 = GetOrCreateType(param1[i].Type);
                var t2 = param2[i].Type;
                if (param1[i].Name != param2[i].Name || t1.Name != t2.Name)
                {
                    return false;
                }
            }
            return true;
        }

        private StructExpression AddStructToGlobalScope(StructExpression structExpr)
        {
            return this.structs[structExpr.Name] = structExpr;
        }

        private FunctionExpression AddFunctionToGlobalScope(FunctionExpression functionExpression)
        {
            this.scopeProvider.Current.AddFunction(functionExpression);
            return functionExpression;
            //if (this.scopeFunctions.ContainsKey(GlobalScopeIdentifier))
            //{
            //    var wasModified = false;
            //    for (var i = 0; i < this.scopeFunctions[GlobalScopeIdentifier].Count; i++)
            //    {
            //        if (this.scopeFunctions[GlobalScopeIdentifier][i].Name == functionExpression.Name)
            //        {
            //            this.scopeFunctions[GlobalScopeIdentifier][i] = functionExpression;
            //            wasModified = true;
            //            break;
            //        }
            //    }

            //    if (!wasModified)
            //        this.scopeFunctions[GlobalScopeIdentifier].Add(functionExpression);
            //}
            //else
            //    this.scopeFunctions.Add(GlobalScopeIdentifier, new List<FunctionExpression> { functionExpression });
            //return functionExpression;
        }
    }
}