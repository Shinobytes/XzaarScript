using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Scripting.Expressions;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting.Compilers
{
    public enum XzaarScopeType
    {
        Global,
        Class,
        Function
    }

    public abstract class XzaarNodeVisitor : IXzaarNodeVisitor
    {
        private const string GlobalScopeName = "$GLOBAL";
        private Dictionary<string, List<ParameterExpression>> functionParameters = new Dictionary<string, List<ParameterExpression>>();
        private Dictionary<string, List<ParameterExpression>> scopeVariables = new Dictionary<string, List<ParameterExpression>>();
        private Dictionary<string, List<FunctionExpression>> scopeFunctions = new Dictionary<string, List<FunctionExpression>>();
        private Dictionary<FunctionExpression, XzaarMethodInfo> compiledFunctions = new Dictionary<FunctionExpression, XzaarMethodInfo>();
        private List<XzaarLabelTarget> definedLabels = new List<XzaarLabelTarget>();
        private List<StructExpression> structs = new List<StructExpression>();
        private XzaarScopeType currentScopeType = XzaarScopeType.Global;
        private string currentScopeIdentifier = GlobalScopeName;
        private int currentScopeLevel = 0;


        // public void RegisterExternFunction()

        public virtual ConditionalExpression Visit(ConditionalOperatorNode conditionalOperator)
        {
            throw new System.NotImplementedException();
        }

        public virtual BinaryExpression Visit(EqualityOperatorNode equalityOperator)
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
            throw new System.NotImplementedException();
        }

        public virtual BinaryExpression Visit(BinaryOperatorNode bin)
        {
            switch (bin.Op)
            {
                case "+": return XzaarExpression.Add(Visit(bin.Left), Visit(bin.Right));
                case "+=": return XzaarExpression.AddAssign(Visit(bin.Left), Visit(bin.Right));
                case "-": return XzaarExpression.Subtract(Visit(bin.Left), Visit(bin.Right));
                case "-=": return XzaarExpression.SubtractAssign(Visit(bin.Left), Visit(bin.Right));
                case "*": return XzaarExpression.Multiply(Visit(bin.Left), Visit(bin.Right));
                case "*=": return XzaarExpression.MultiplyAssign(Visit(bin.Left), Visit(bin.Right));
                case "/": return XzaarExpression.Divide(Visit(bin.Left), Visit(bin.Right));
                case "/=": return XzaarExpression.DivideAssign(Visit(bin.Left), Visit(bin.Right));
                case "%": return XzaarExpression.Modulo(Visit(bin.Left), Visit(bin.Right));
                case "%=": return XzaarExpression.ModuloAssign(Visit(bin.Left), Visit(bin.Right));
                case "&": return XzaarExpression.And(Visit(bin.Left), Visit(bin.Right));
                case "&=": return XzaarExpression.AndAssign(Visit(bin.Left), Visit(bin.Right));
                case "|=": return XzaarExpression.OrAssign(Visit(bin.Left), Visit(bin.Right));
                case "|": return XzaarExpression.Or(Visit(bin.Left), Visit(bin.Right));
            }
            throw new System.NotImplementedException();
        }

        public virtual ConditionalExpression Visit(ConditionalNode conditional)
        {
            XzaarExpression ifTrue, ifFalse;

            var c = conditional.GetCondition();
            var test = Visit(c);
            if (test == null)
            {
                throw new XzaarExpressionTransformerException("if statements cannot have empty conditions, did you mean to do 'if (true) { ...' ?");
            }
            var t = conditional.GetTrue();
            var f = conditional.GetFalse();
            ifTrue = t != null && !t.IsEmpty() ? Visit(t) : XzaarExpression.Empty();
            ifFalse = f != null && !f.IsEmpty() ? Visit(f) : XzaarExpression.Empty();
            return XzaarExpression.Condition(test, ifTrue, ifFalse);
        }

        public virtual XzaarExpression Visit(FunctionParametersNode parameters)
        {
            throw new System.NotImplementedException();
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
            throw new NotImplementedException();
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
            return XzaarExpression.CreateStruct(createStruct.StructNode.Name);
        }

        public virtual MemberExpression Visit(MemberAccessNode member)
        {
            XzaarExpression access = null;
            var literal = member.Member as LiteralNode;
            var outType = GetOrCreateType(member.MemberType);
            if (literal != null)
            {
                var declaringType = GetOrCreateType(member.DeclaringType);
                // first try and see if we can find a matching field
                var field = FindFieldInType(literal.Value + "", declaringType);
                if (field != null)
                {
                    access = field;
                }
            }

            if (access == null)
            {
                // still null? do a normal visit
                access = Visit(member.Member);
            }


            if (member.ArrayIndex != null)
                return XzaarExpression.MemberAccess(access, Visit(member.ArrayIndex), outType);

            return XzaarExpression.MemberAccess(access, outType);
        }

        private FieldExpression FindFieldInType(string s, XzaarType type)
        {
            if (type == null) type = XzaarBaseTypes.Void;
            var field = type.GetField(s);
            if (field != null)
            {
                return new FieldExpression(field.FieldType, field.Name, type);
            }

            var str = this.structs.FirstOrDefault(s1 => s1.Name == type.Name);
            if (str != null)
            {
                return str.Fields.Cast<FieldExpression>().FirstOrDefault(b => b.Name == s);
            }

            return null;
        }

        public virtual BinaryExpression Visit(AssignNode assign)
        {
            return XzaarExpression.Assign(Visit(assign.Left), Visit(assign.Right));
        }

        public UnaryExpression Visit(UnaryNode unary)
        {
            var item = Visit(unary.Item);
            if (unary.IsIncrementor && unary.IsPostUnary)
                return XzaarExpression.PostIncrementor(item);
            if (unary.IsIncrementor)
                return XzaarExpression.Incrementor(item);
            if (!unary.IsIncrementor && unary.IsPostUnary)
                return XzaarExpression.PostDecrementor(item);
            return XzaarExpression.Decrementor(item);
        }

        public SwitchCaseExpression Visit(CaseNode matchCase)
        {
            return matchCase.IsDefaultCase
                ? XzaarExpression.DefaultCase(Visit(matchCase.Body))
                : XzaarExpression.Case(Visit(matchCase.Test), Visit(matchCase.Body));
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
                variable = XzaarExpression.DefineVariable(
                    GetOrCreateType(definedVariable.Type),
                    definedVariable.Name,
                    Visit(definedVariable.AssignmentExpression));
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

        public virtual FunctionCallExpression Visit(FunctionCallNode call)
        {
            var arguments = call.Arguments.Select(Visit).Where(a => a != null).ToArray();

            var f = call.Function;

            if (f.NodeType == XzaarNodeTypes.FUNCTION)
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
                        throw new NotImplementedException();
                    }
                }
            }
            else if (f.NodeType == XzaarNodeTypes.LITERAL)
            {
                // using an undeclared function
                // either its a missing function or a internal function               
                var functionName = f.Value?.ToString();
                if (functionName == null) throw new InvalidOperationException();
                if (call.Instance == null)
                {
                    var function = FindMatchingFunctionInGlobalScope(functionName, arguments.Length);
                    if (function == null)
                    {
                        // oh well. still can't seem to determine what function it is. still undefined 'extern' function as well?

                        throw new XzaarExpressionTransformerException("Target function '" + functionName + "' was not found. Forgot to declare it?");
                    }
                    else
                    {
                        return XzaarExpression.Call(function, arguments);
                    }
                }
                else
                {
                    // find function based on the instance
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            throw new NotImplementedException();
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
            if (arg == null || arg.Type == XzaarBaseTypes.Void)
            {
                // no arguments supplied
                return null;
            }

            if (arg is MemberExpression)
            {
                var m = arg as MemberExpression;
                arg = m.Expression;
            }

            if (arg is ParameterExpression || arg is ConstantExpression)
            {
                return arg;
            }

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
                    SetCurrentScope(function.Name, XzaarScopeType.Function);

                    existing.SetReturnType(function.GetReturnType(
                     new XzaarTypeFinderContext(
                         FindMatchingFunctionInGlobalScope,
                         FindVariable
                     )))
                .SetBody(Visit(function.Body));
                }
                return existing;
            }

            SetCurrentScope(function.Name, XzaarScopeType.Function);

            var f = XzaarExpression.Function(
                function.Name,
                function.Parameters.Parameters.Select(Visit).ToArray(),
                null,
                null,
                function.IsExtern
            );

            // late bind the body so we can add this function to the global scope before we parse the body (instance functions are not supported yet)
            AddFunctionToGlobalScope(f)
                .SetReturnType(function.GetReturnType(
                     new XzaarTypeFinderContext(
                         FindMatchingFunctionInGlobalScope,
                         FindVariable
                     )))
                .SetBody(Visit(function.Body));

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
                return XzaarExpression.Constant(literal.Value, GetOrCreateType(literal.NodeName.ToLower()));
            }

            throw new XzaarExpressionTransformerException("Use of unknown or undefined variable '" + literal.Value + "'");
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
            if (node.Body.NodeType == XzaarNodeTypes.BLOCK)
                items = node.Body.Children;

            foreach (var c in items)
            {
                var function = c as FunctionNode;
                if (function != null)
                {
                    var f = XzaarExpression.Function(
                        function.Name,
                        function.Parameters.Parameters.Select(Visit).ToArray(),
                        null,
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

        public virtual XzaarExpression Visit(XzaarNode node)
        {
            var name = node.GetType().Name;
            if (name.EndsWith("XzaarNode"))
            {
                throw new System.NotImplementedException();
            }

            return Visit((dynamic)node);
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

        private bool ParameterSequenceMatch(IReadOnlyList<ParameterNode> param1, ParameterExpression[] param2)
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