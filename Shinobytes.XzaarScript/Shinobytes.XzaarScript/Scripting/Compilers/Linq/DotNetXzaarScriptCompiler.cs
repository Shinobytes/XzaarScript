using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting.Compilers.Linq
{
    #region Old Assembly Builder
    //public class XzaarExpressionClrAssemblyBuilder
    //{
    //    public Assembly Build(string outputDirectory, string scriptName, LambdaExpression expression)
    //    {
    //        var assemblyName = new AssemblyName(scriptName);
    //        var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave, outputDirectory);
    //        var module = asm.DefineDynamicModule(scriptName, scriptName + ".exe");

    //        var typeBuilder = module.DefineType(scriptName, TypeAttributes.Public);
    //        var mainMethod = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), new[] { typeof(string[]) });

    //        var appStartMethod = typeBuilder.DefineMethod("AppStart", MethodAttributes.Static, null, null);

    //        expression.CompileToMethod(appStartMethod);

    //        //var call = Expression.Lambda(Expression.Call(appStartMethod, new List<Expression>()));
    //        //call.CompileToMethod(mainMethod);

    //        var il = mainMethod.GetILGenerator();


    //        il.EmitCall(OpCodes.Call, appStartMethod, new Type[0]);

    //        // il.Emit(OpCodes.Pop);

    //        if (appStartMethod.ReturnType == typeof(Action))
    //        {
    //            il.EmitCall(OpCodes.Call, typeof(Action).GetMethod("Invoke", new Type[0]), new Type[0]);
    //        }

    //        il.Emit(OpCodes.Ret);

    //        var type = typeBuilder.CreateType();

    //        asm.SetEntryPoint(mainMethod);
    //        asm.Save(scriptName + ".exe");
    //        return asm;
    //    }
    //}
    #endregion

    public class DotNetXzaarScriptCompiler : IXzaarScriptCompiler
    {
        private readonly ConstantExpression True = Expression.Constant(true, typeof(bool));
        private readonly ConstantExpression False = Expression.Constant(false, typeof(bool));
        private readonly Dictionary<string, Type> variables = new Dictionary<string, Type>();
        private readonly Dictionary<string, MethodInfo> compiledFunctions = new Dictionary<string, MethodInfo>();

        private Dictionary<string, ParameterExpression> parameters;
        private Dictionary<string, Expression> knownFunctions;


        public XzaarCompiledScriptBase Compile(EntryNode ast)
        {
            parameters = new Dictionary<string, ParameterExpression>();
            knownFunctions = new Dictionary<string, Expression>();
            foreach (var v in variables)
                parameters.Add(v.Key, Expression.Parameter(v.Value, v.Key));

            if (!ast.Body.IsEmpty())
            {
                var compileExpression = CompileExpression(ast.Body);
                var lambdaExpression = Expression.Lambda(compileExpression, parameters.Values.ToList());
                return new LambdaXzaarCompiledScript(
                    lambdaExpression
                );
            }
            return new LambdaXzaarCompiledScript(Expression.Empty());
        }

        public void RegisterVariable<T>(string name, XzaarScope scope)
        {
            variables.Add(name, typeof(T));
        }

        public void RegisterVariable(string name, Type type, XzaarScope scope)
        {
            variables.Add(name, type);
        }

        public void RegisterVariable<T>(string name)
        {
            variables.Add(name, typeof(T));
        }

        public void RegisterVariable(string name, Type type)
        {
            variables.Add(name, type);
        }

        public void UnregisterVariable(string name, XzaarScope scope)
        {
            variables.Remove(name);
        }

        public void UnregisterAllVariables(XzaarScope scope)
        {
            variables.Clear();
        }

        public void RegisterExternFunction(string name, MethodInfo targetMethod, XzaarScope scope)
        {
            compiledFunctions.Add(name, targetMethod);
        }

        private Expression CompileExpression(XzaarNode node)
        {
            return CompileNode((dynamic)node);
        }

        private Expression CompileNode(BlockNode block)
        {
            var expressions = block.Children.Select(c => CompileNode((dynamic)c)).Cast<Expression>().ToList();
            return Expression.Block(expressions);
        }

        private Expression CompileNode(FunctionCallNode callNode)
        {
            var methodName = callNode.Value?.ToString();
            var arguments = callNode.Arguments.Select(CompileNode).ToArray();
            if (compiledFunctions.ContainsKey(methodName))
            {
                var method = compiledFunctions[methodName];
                return Expression.Call(method, arguments);
            }
            if (knownFunctions.ContainsKey(methodName))
            {
                var func = knownFunctions.FirstOrDefault(n => n.Key == callNode.Value?.ToString());
                var lambda = func.Value as LambdaExpression;
                if (lambda != null)
                {
                    var del = lambda.Compile().Method;
                    compiledFunctions.Add(methodName, del);
                    return Expression.Call(del, arguments);
                }
            }

            throw new NotImplementedException();
        }

        private Expression CompileNode(MemberAccessNode memberAccess)
        {

            var strValue = memberAccess.Value.ToString();
            return parameters.ContainsKey(strValue)
                 ? parameters[strValue]
                 : Expression.Variable(typeof(object), strValue + "");
        }

        private Expression CompileNode(FunctionNode function)
        {
            if (function.NodeName == "EXTERN")
                return Expression.Empty();

            var funcParams = CompileParameters(function.Parameters);
            var func = Expression.Lambda(CompileExpression(function.Body), funcParams);
            knownFunctions.Add(function.Name, func);
            return func;
        }

        private ParameterExpression[] CompileParameters(FunctionParametersNode args)
        {
            return args.Parameters.Select(CompileNode).ToArray();
        }

        private Expression CompileNode(ArgumentNode arg)
        {
            var function = arg.Parent as FunctionNode;
            if (function != null)
            {
                var targetParameter = function.Parameters.Parameters[arg.ArgumentIndex];
                if (targetParameter != null && arg.Item is LiteralNode)
                {
                    (arg.Item as LiteralNode).Type = GetClrType(targetParameter.Type);
                }
            }
            return CompileNode((dynamic)arg.Item);
        }

        private ParameterExpression CompileNode(ParameterNode p)
        {
            var typeName = GetClrType(p.Type);
            var type = Type.GetType(typeName) ?? typeof(object);
            return Expression.Parameter(type, p.Name);
        }

        private Expression CompileNode(ConditionalNode conditional)
        {
            var condition = CompileNode((dynamic)conditional.GetCondition());
            var ifTrue = CompileNode((dynamic)conditional.GetTrue());
            if (!conditional.GetFalse().IsEmpty())
            {
                var ifFalse = CompileNode((dynamic)conditional.GetFalse());
                return Expression.IfThenElse(condition, ifTrue, ifFalse);
            }
            return Expression.IfThen(condition, ifTrue);
        }

        private Expression CompileNode(AssignNode assign)
        {
            // return null;
            return Expression.Assign(
                    CompileNode((dynamic)assign.Left),
                    CompileNode((dynamic)assign.Right));
        }

        private Expression CompileNode(BinaryOperatorNode math)
        {
            var left = (Expression)CompileNode((dynamic)math.Left);
            var right = (Expression)CompileNode((dynamic)math.Right);
            switch (math.Op)
            {
                case "*": return Expression.Multiply(left, right);
                case "+":
                    {
                        if (left.Type?.Name == "String" || right.Type?.Name == "String")
                        {
                            // to append string with object we need to call ".ToString()" on the object first.
                            var toStringMethod = typeof(object).GetMethod("ToString");
                            var concatMethod = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });

                            if (left.Type?.Name != "String")
                                left = Expression.Call(left, toStringMethod);
                            if (right.Type?.Name != "String")
                                right = Expression.Call(right, toStringMethod);

                            return Expression.Add(left, right, concatMethod);
                        }
                        return Expression.Add(left, right);
                    }
                case "-": return Expression.Subtract(left, right);
                case "/": return Expression.Divide(left, right);
                case "%": return Expression.Modulo(left, right);
                default:
                    throw new InvalidOperationException($"Unknown math operator used: '{math.Op}'");
            }
        }

        private Expression CompileNode(LiteralNode literal)
        {
            var strValue = literal.Value.ToString();
            if (strValue == "true") return True;
            if (strValue == "false") return False;
            if (literal.NodeName == "NAME")
            {
                return parameters.ContainsKey(strValue)
                    ? parameters[strValue]
                    : Expression.Variable(
                                          literal.Type != null ? Type.GetType(literal.Type, false, false) : typeof(object),
                                          literal.Value + "");
            }
            return Expression.Constant(literal.Value);
        }

        private Expression CompileNode(ConditionalOperatorNode conditional)
        {
            var left = CompileNode((dynamic)conditional.Left);
            var right = CompileNode((dynamic)conditional.Right);

            switch (conditional.Op)
            {
                case "&&": case "and": return Expression.And(left, right);
                case "||": case "or": return Expression.Or(left, right);
                default:
                    throw new InvalidOperationException($"Unknown conditional operator used: '{conditional.Op}'");
            }
        }


        private Expression CompileNode(EqualityOperatorNode equality)
        {
            var left = CompileNode((dynamic)equality.Left);
            var right = CompileNode((dynamic)equality.Right);
            switch (equality.Op)
            {
                case ">=": case "gte": return Expression.GreaterThanOrEqual(left, right);
                case "<=": case "lte": return Expression.LessThanOrEqual(left, right);
                case "!=": case "neq": return Expression.NotEqual(left, right);
                case "==": case "eq": return Expression.Equal(left, right);
                case ">": case "gt": return Expression.GreaterThan(left, right);
                case "<": case "lt": return Expression.LessThan(left, right);
                default:
                    throw new InvalidOperationException($"Unknown equality operator used: '{equality.Op}'");
            }
        }



        private string GetClrType(string type)
        {
            var fixedType = Char.ToUpper(type[0]) + type.Substring(1);
            if (!fixedType.Contains("."))
                return "System." + fixedType; // <-- for now we will just name them the same.            
            return type;
        }

    }
}