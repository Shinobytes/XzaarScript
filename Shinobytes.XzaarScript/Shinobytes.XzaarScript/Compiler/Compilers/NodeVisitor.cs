using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Expressions;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public enum XzaarScopeType
    {
        Global,
        Class,
        Function
    }

    public abstract class NodeVisitor : INodeVisitor
    {
        private const string GlobalScopeName = "$GLOBAL";

        private readonly List<string> errors = new List<string>();
        private Dictionary<string, List<ParameterExpression>> functionParameters = new Dictionary<string, List<ParameterExpression>>();
        private Dictionary<string, List<ParameterExpression>> scopeVariables = new Dictionary<string, List<ParameterExpression>>();
        private Dictionary<string, List<FunctionExpression>> scopeFunctions = new Dictionary<string, List<FunctionExpression>>();
        private Dictionary<FunctionExpression, XzaarMethodInfo> compiledFunctions = new Dictionary<FunctionExpression, XzaarMethodInfo>();
        private List<LabelTarget> definedLabels = new List<LabelTarget>();
        private List<StructExpression> structs = new List<StructExpression>();
        private XzaarScopeType currentScopeType = XzaarScopeType.Global;
        private string currentScopeIdentifier = GlobalScopeName;
        private int currentScopeLevel = 0;
        private string currentFunctionName;
        private bool visitFunctionBody = true;


        public bool HasErrors => errors.Count > 0;
        public IList<string> Errors => errors;

        private XzaarExpression Error(string message)
        {
            this.errors.Add("[Error] " + message);
            return null;
        }

        // public void RegisterExternFunction()

        public virtual XzaarExpression Visit(ConditionalOperatorNode conditionalOperator)
        {
            var oper = conditionalOperator;
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
            return XzaarExpression.Condition(test, ifTrue, ifFalse);
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

        public virtual LabelExpression Visit(LabelNode label)
        {
            var xzaarLabelTarget = XzaarExpression.Label(label.Name);
            definedLabels.Add(xzaarLabelTarget);
            return XzaarExpression.Label(xzaarLabelTarget);
        }

        public virtual GotoExpression Visit(GotoNode @goto)
        {
            var label = definedLabels.FirstOrDefault(l => l.Name == @goto.LabelName);
            return XzaarExpression.Goto(label);
        }

        public MemberAccessChainExpression Visit(MemberAccessChainNode access)
        {
            var last = Visit(access.LastAccessor);
            var now = Visit(access.Accessor);
            return XzaarExpression.AccessChain(last, now);
        }

        public CreateStructExpression Visit(CreateStructNode createStruct)
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
            if (field != null) return new FieldExpression(field.FieldType, field.Name, type);

            var str = this.structs.FirstOrDefault(s1 => s1.Name == type.Name);
            if (str != null) return str.Fields.Cast<FieldExpression>().FirstOrDefault(b => b.Name == s);

            return null;
        }

        public virtual BinaryExpression Visit(AssignNode assign)
        {
            var left = Visit(assign.Left);
            var right = Visit(assign.Right);
            return XzaarExpression.Assign(left, right);
        }

        public XzaarExpression Visit(UnaryNode unary)
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

        public SwitchCaseExpression Visit(CaseNode @case)
        {
            var body = Visit(@case.Body);
            if (@case.IsDefaultCase)
            {
                return XzaarExpression.DefaultCase(body);
            }

            var test = Visit(@case.Test);
            return XzaarExpression.Case(test, body);
        }

        public virtual SwitchExpression Visit(MatchNode match)
        {
            var valueExpr = Visit(match.ValueExpression);
            var cases = match.Cases.Select(Visit).ToArray();
            return XzaarExpression.Switch(valueExpr, cases);
        }

        public virtual DoWhileExpression Visit(DoWhileLoopNode loop)
        {
            return XzaarExpression.DoWhile(Visit(loop.Test), Visit(loop.Body));
        }

        public virtual WhileExpression Visit(WhileLoopNode loop)
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

        public virtual ForEachExpression Visit(ForeachLoopNode loop)
        {
            return XzaarExpression.ForEach(
                    Visit(loop.Variable),
                    Visit(loop.Source),
                    Visit(loop.Body));
        }

        public virtual ForExpression Visit(ForLoopNode loop)
        {
            return XzaarExpression.For(
                    Visit(loop.Initiator),
                    Visit(loop.Test),
                    Visit(loop.Incrementor),
                    Visit(loop.Body)
                );
        }

        public virtual LoopExpression Visit(LoopNode loop)
        {
            return XzaarExpression.Loop(Visit(loop.Body));
        }

        public virtual VariableDefinitionExpression Visit(DefineVariableNode definedVariable)
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

            if (!scopeVariables.ContainsKey(this.currentScopeIdentifier))
            {
                scopeVariables.Add(this.currentScopeIdentifier, new List<ParameterExpression> { variable });
            }
            else
            {
                scopeVariables[this.currentScopeIdentifier].Add(variable);
            }

            return variable;
        }

        public ParameterExpression Visit(VariableNode variable)
        {
            var @type = GetOrCreateType(variable.Type);
            if (@type == null) throw new InvalidOperationException(variable.Type + " is an unknown type.");
            var v = XzaarExpression.Variable(@type, variable.Name);

            if (!scopeVariables.ContainsKey(this.currentScopeIdentifier))
            {
                scopeVariables.Add(this.currentScopeIdentifier, new List<ParameterExpression> { v });
            }
            else
            {
                scopeVariables[this.currentScopeIdentifier].Add(v);
            }
            return v;
        }

        public virtual ParameterExpression Visit(ParameterNode parameter)
        {
            var @type = GetOrCreateType(parameter.Type);
            if (@type == null) throw new InvalidOperationException(parameter.Type + " is an unknown type.");
            var param = XzaarExpression.Parameter(@type, parameter.Name);

            if (parameter.Parent != null && parameter.Parent.Parent != null)
            {
                var function = parameter.Parent.Parent as FunctionNode;
                if (function != null)
                {
                    if (!functionParameters.ContainsKey(function.Name))
                    {
                        functionParameters.Add(function.Name, new List<ParameterExpression> { param });
                    }
                    else
                    {
                        functionParameters[function.Name].Add(param);
                    }
                }
            }
            return param;
        }

        public virtual XzaarExpression Visit(FunctionCallNode call)
        {
            var arguments = call.Arguments.Select(Visit).Where(a => a != null).ToArray();

            var f = call.Function;

            if (f.Kind == SyntaxKind.FunctionDefinitionExpression)
            {
                // using an already declared function
                var function = f as FunctionNode;
                if (function == null) throw new ArgumentNullException("function", "Target function cannot be null");

                var method = Visit(function) as FunctionExpression;
                if (method != null)
                {
                    if (call.Instance == null)
                    {
                        return XzaarExpression.Call(method, arguments);
                    }
                    else
                    {
                        return Error("The function '" + call.Instance.ValueText + "." + function.Name + "' cannot be called as locally defined instanced functions are not supported");
                    }
                }
            }

            else if (SyntaxFacts.IsLiteral(f.Kind) || SyntaxFacts.IsMemberAccess(f.Kind))
            {
                var functionName = "";
                if (f is MemberAccessChainNode chain)
                {
                    functionName = chain.Accessor.ValueText;
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
                    var function = FindMatchingFunctionInGlobalScope(functionName, arguments.Length);
                    if (function == null)
                    {
                        // oh well. still can't seem to determine what function it is. still undefined 'extern' function as well?
                        return Error("Target function '" + functionName + "' was not found. Forgot to declare it?");
                    }
                    return XzaarExpression.Call(function, arguments);
                }
                var instanceVariableName = call.Instance.Value + "";
                if (currentFunctionName != null && this.functionParameters.ContainsKey(currentFunctionName))
                {
                    var parameters = functionParameters[currentFunctionName];
                    var p = parameters.FirstOrDefault(pa => pa.Name == instanceVariableName);
                    if (p != null)
                    {
                        if (Equals(p.Type, XzaarBaseTypes.Any) || (ArrayHelper.IsArrayFunction(call.Value + "") && p.Type.IsArray))
                        {
                            return XzaarExpression.Call(p,
                                XzaarExpression.Function(functionName, new ParameterExpression[arguments.Length], XzaarBaseTypes.Any, true),
                                arguments);
                        }
                    }
                }

                var v = this.FindVariable(instanceVariableName, true);
                if (v != null)
                {
                    if (Equals(v.Type, XzaarBaseTypes.Any) || (ArrayHelper.IsArrayFunction(functionName) && v.Type.IsArray))
                    {
                        return XzaarExpression.Call(v,
                            XzaarExpression.Function(functionName, new ParameterExpression[arguments.Length], XzaarBaseTypes.Any, true),
                            arguments);
                    }
                }

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
            var existing = FindMatchingFunctionInGlobalScope(function);
            if (existing != null)
            {
                // if the function has already been added but the body or return type hasnt been defined then we want to do that.
                // this is because the function was already added during the 'function discovery' step
                if (existing.ReturnType == null || existing.GetBody() == null)
                {
                    this.currentFunctionName = function.Name;

                    SetCurrentScope(function.Name, XzaarScopeType.Function);

                    existing.SetReturnType(function.GetReturnType(
                     new XzaarTypeFinderContext(
                         FindMatchingFunctionInGlobalScope,
                         FindVariable
                     ))).SetBody(Visit(function.Body));

                    if (function.IsReturnTypeBound)
                    {
                        var fn = FindMatchingFunctionInGlobalScope(function.ReturnTypeBindingName);
                        if (fn != null)
                            existing.BindReturnType(() => fn.ReturnType);
                    }
                }
                return existing;
            }

            SetCurrentScope(function.Name, XzaarScopeType.Function);

            var f = XzaarExpression.Function(
                function.Name,
                function.Parameters.Parameters.Select(Visit).ToArray(),
                function.ReturnType,
                null,
                function.IsExtern
            );

            // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
            var targetFunction = AddFunctionToGlobalScope(f)
                .SetReturnType(function.GetReturnType(
                     new XzaarTypeFinderContext(
                         FindMatchingFunctionInGlobalScope,
                         FindVariable)
                     { IgnoreMissingMembers = !visitFunctionBody }));
            if (visitFunctionBody) targetFunction.SetBody(Visit(function.Body));

            return f;
        }

        public virtual StructExpression Visit(StructNode node)
        {
            var structFields = node.Fields.Select(Visit).ToArray();
            return XzaarExpression.Struct(
                node.Name,
                structFields
            );
        }

        public virtual FieldExpression Visit(FieldNode node)
        {
            var declaringType = GetOrCreateType(node.DeclaringType);
            var a = this.structs.FirstOrDefault(s => s.Name == node.Type);
            if (a != null)
            {
                XzaarType newType;
                if (XzaarType.TryGetType(a.Name, a, out newType))
                {
                    return XzaarExpression.Field(newType, node.Name, declaringType);
                }
            }
            return XzaarExpression.Field(XzaarType.GetType(node.Type), node.Name, declaringType);
        }

        public virtual ConstantExpression Visit(NumberNode number)
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

                if (this.currentScopeType == XzaarScopeType.Function)
                {
                    if (this.functionParameters.ContainsKey(this.currentScopeIdentifier))
                    {
                        var plist = this.functionParameters[this.currentScopeIdentifier];
                        var param = plist.FirstOrDefault(p => p.Name == literal.Value.ToString());
                        if (param != null)
                        {
                            return XzaarExpression.Parameter(param.Type, param.Name);
                        }
                    }

                    var variable = FindVariable(literal.Value.ToString(), true);

                    if (variable != null)
                    {
                        return XzaarExpression.Variable(variable.Type, variable.Name);
                    }
                }
                else
                {
                    var variable = FindVariable(literal.Value.ToString(), true);

                    if (variable != null)
                    {
                        return XzaarExpression.Variable(variable.Type, variable.Name);
                    }
                }
            }
            else
            {
                if (literal.NodeName == "ARRAY" && literal.Children.Count > 0)
                {
                    var expr = new List<XzaarExpression>();
                    foreach (var c in literal.Children)
                    {
                        expr.Add(Visit(c));
                    }

                    // return XzaarExpression.ArrayInitializer(expr.ToArray());
                    return XzaarExpression.Constant(expr.ToArray(), GetOrCreateType(literal.NodeName.ToLower()));
                }
                else
                {
                    return XzaarExpression.Constant(literal.Value, GetOrCreateType(literal.NodeName.ToLower()));
                }
            }

            // if ((literal.Value + "").StartsWith("$"))
            {
                return XzaarExpression.Variable(XzaarBaseTypes.Any, literal.Value + "");
            }

            return Error("Use of unknown or undefined variable '" + literal.Value + "'");
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

        private ParameterExpression FindVariable(string variableOrValue, bool includeGlobalVariables)
        {
            // need a way to traverse scope upwards, right now we only have global and function scope at undefined level

            if (variableOrValue.StartsWith("$"))
            {
                return new ParameterExpression(variableOrValue);
            }

            if (includeGlobalVariables)
            {
                if (this.currentScopeIdentifier != GlobalScopeName && this.scopeVariables.ContainsKey(currentScopeIdentifier))
                {
                    var vlist = this.scopeVariables[this.currentScopeIdentifier];
                    var v = vlist.FirstOrDefault(p => p.Name == variableOrValue);
                    if (v != null) return v;
                }

                if (this.scopeVariables.ContainsKey(GlobalScopeName))
                {
                    var vlist = this.scopeVariables[GlobalScopeName];
                    var v = vlist.FirstOrDefault(p => p.Name == variableOrValue);
                    if (v != null) return v;
                }

            }
            else
            {
                if (this.scopeVariables.ContainsKey((this.currentScopeIdentifier)))
                {
                    var vlist = this.scopeVariables[this.currentScopeIdentifier];
                    return vlist.FirstOrDefault(p => p.Name == variableOrValue);
                }
            }

            return null;
        }


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
            if (items.Length > 1)
                return XzaarExpression.Block(
                    items
                );
            return items.FirstOrDefault();
        }


        private void DiscoverStructsAndFunctions(EntryNode node)
        {
            var items = node.Children;
            if (node.Body.Kind == SyntaxKind.Block)
                items = node.Body.Children;

            foreach (var c in items)
            {
                var function = c as FunctionNode;
                if (function != null)
                {

                    if (function.ReturnType == null)
                    {
                        var identifier = currentScopeIdentifier;
                        var type = currentScopeType;
                        var level = currentScopeLevel;
                        visitFunctionBody = false;

                        Visit(function);

                        visitFunctionBody = true;
                        SetCurrentScope(identifier, type, level);

                        if (function.ReturnType != null && function.ReturnType.IsAny)
                        {
                            function.SetReturnType(null);
                        }
                    }

                    var f = XzaarExpression.Function(
                        function.Name,
                        function.Parameters.Parameters.Select(Visit).ToArray(),
                        function.ReturnType,
                        null,
                        function.IsExtern
                    );

                    // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
                    AddFunctionToGlobalScope(f);
                }
                else
                {
                    var str = c as StructNode;
                    if (str != null)
                    {
                        var s = XzaarExpression.Struct(
                            str.Name,
                            str.Fields.Select(Visit).ToArray()
                        );

                        // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
                        AddStructToGlobalScope(s);
                    }
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

            if (node is ConditionalOperatorNode) return Visit(node as ConditionalOperatorNode);
            if (node is EqualityOperatorNode) return Visit(node as EqualityOperatorNode);
            if (node is BinaryOperatorNode) return Visit(node as BinaryOperatorNode);
            if (node is LogicalNotNode) return Visit(node as LogicalNotNode);
            if (node is IfElseNode) return Visit(node as IfElseNode);
            if (node is FunctionParametersNode) return Visit(node as FunctionParametersNode);
            if (node is ReturnNode) return Visit(node as ReturnNode);
            if (node is ContinueNode) return Visit(node as ContinueNode);
            if (node is BreakNode) return Visit(node as BreakNode);
            if (node is LabelNode) return Visit(node as LabelNode);
            if (node is GotoNode) return Visit(node as GotoNode);
            if (node is MemberAccessChainNode) return Visit(node as MemberAccessChainNode);
            if (node is CreateStructNode) return Visit(node as CreateStructNode);
            if (node is MemberAccessNode) return Visit(node as MemberAccessNode);
            if (node is AssignNode) return Visit(node as AssignNode);
            if (node is UnaryNode) return Visit(node as UnaryNode);
            if (node is CaseNode) return Visit(node as CaseNode);
            if (node is MatchNode) return Visit(node as MatchNode);
            if (node is DoWhileLoopNode) return Visit(node as DoWhileLoopNode);
            if (node is WhileLoopNode) return Visit(node as WhileLoopNode);
            if (node is ForeachLoopNode) return Visit(node as ForeachLoopNode);
            if (node is ForLoopNode) return Visit(node as ForLoopNode);
            if (node is LoopNode) return Visit(node as LoopNode);
            if (node is DefineVariableNode) return Visit(node as DefineVariableNode);
            if (node is VariableNode) return Visit(node as VariableNode);
            if (node is ParameterNode) return Visit(node as ParameterNode);
            if (node is FunctionCallNode) return Visit(node as FunctionCallNode);
            if (node is ExpressionNode) return Visit(node as ExpressionNode);
            if (node is ArgumentNode) return Visit(node as ArgumentNode);
            if (node is FunctionNode) return Visit(node as FunctionNode);
            if (node is StructNode) return Visit(node as StructNode);
            if (node is FieldNode) return Visit(node as FieldNode);
            if (node is NumberNode) return Visit(node as NumberNode);
            if (node is LiteralNode) return Visit(node as LiteralNode);
            if (node is BodyNode) return Visit(node as BodyNode);
            if (node is BlockNode) return Visit(node as BlockNode);
            if (node is EmptyNode) return Visit(node as EmptyNode);
            if (node is EntryNode) return Visit(node as EntryNode);


            return Visit(node);
            // return Visit((dynamic)node);
        }


        private XzaarType GetOrCreateType(string typeName)
        {
            var a = this.structs.FirstOrDefault(s => s.Name == typeName);
            if (a != null)
            {
                XzaarType newType;
                if (XzaarType.TryGetType(a.Name, a, out newType))
                {
                    return newType;
                }
            }
            var final = XzaarType.GetType(typeName);
            if (final == null) final = XzaarBaseTypes.Void;
            return final;
        }

        private void SetCurrentScope(string identifier)
        {
            SetCurrentScope(identifier, currentScopeType, currentScopeLevel);
        }

        private void SetCurrentScope(string identifier, XzaarScopeType type)
        {
            SetCurrentScope(identifier, type, currentScopeLevel);
        }

        private void SetCurrentScope(string identifier, XzaarScopeType type, int level)
        {
            this.currentScopeIdentifier = identifier;
            this.currentScopeType = type;
            this.currentScopeLevel = level;
        }

        //private XzaarMethodInfo BuildMethod(FunctionExpression func)
        //{
        //    if (this.compiledFunctions.ContainsKey(func))
        //    {
        //        return this.compiledFunctions[func];
        //    }

        //    var f = XzaarExpression.BuildMethod(func);

        //    this.compiledFunctions.Add(func, f);

        //    return f;
        //}

        private FunctionExpression FindMatchingFunctionInGlobalScope(string name)
        {
            if (!this.scopeFunctions.ContainsKey(GlobalScopeName)) return null;
            return this.scopeFunctions[GlobalScopeName].FirstOrDefault(f => f.Name == name);
        }

        private FunctionExpression FindMatchingFunctionInGlobalScope(string name, int argCount)
        {
            if (!this.scopeFunctions.ContainsKey(GlobalScopeName)) return null;
            return this.scopeFunctions[GlobalScopeName].FirstOrDefault(f => f.Name == name && f.GetParameters().Length == argCount);
        }


        private FunctionExpression FindMatchingFunctionInGlobalScope(string name, XzaarType returnType, int argCount)
        {
            if (!this.scopeFunctions.ContainsKey(GlobalScopeName)) return null;
            return this.scopeFunctions[GlobalScopeName].FirstOrDefault(f => f.Name == name && f.ReturnType.Name == returnType.Name && f.GetParameters().Length == argCount);
        }

        private FunctionExpression FindMatchingFunctionInGlobalScope(FunctionNode function)
        {
            if (!this.scopeFunctions.ContainsKey(GlobalScopeName)) return null;
            var finderContext = new XzaarTypeFinderContext(FindMatchingFunctionInGlobalScope, FindVariable);
            var possibleMatches = this.scopeFunctions[GlobalScopeName].Where(
                f => f.Name == function.Name);//&& f.ReturnType.Name == function.GetReturnType(finderContext).Name);

            foreach (var f in possibleMatches)
            {
                var param1 = function.Parameters.Parameters;
                var param2 = f.GetParameters();
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
            if (this.scopeFunctions.ContainsKey(GlobalScopeName))
            {
                var wasModified = false;
                for (var i = 0; i < this.structs.Count; i++)
                {
                    if (this.structs[i].Name == structExpr.Name)
                    {
                        this.structs[i] = structExpr;
                        wasModified = true;
                        break;
                    }
                }

                if (!wasModified)
                    this.structs.Add(structExpr);
            }
            else
                this.structs.Add(structExpr);
            return structExpr;
        }

        private FunctionExpression AddFunctionToGlobalScope(FunctionExpression functionExpression)
        {
            if (this.scopeFunctions.ContainsKey(GlobalScopeName))
            {
                var wasModified = false;
                for (var i = 0; i < this.scopeFunctions[GlobalScopeName].Count; i++)
                {
                    if (this.scopeFunctions[GlobalScopeName][i].Name == functionExpression.Name)
                    {
                        this.scopeFunctions[GlobalScopeName][i] = functionExpression;
                        wasModified = true;
                        break;
                    }
                }

                if (!wasModified)
                    this.scopeFunctions[GlobalScopeName].Add(functionExpression);
            }
            else
                this.scopeFunctions.Add(GlobalScopeName, new List<FunctionExpression> { functionExpression });
            return functionExpression;
        }
    }
}