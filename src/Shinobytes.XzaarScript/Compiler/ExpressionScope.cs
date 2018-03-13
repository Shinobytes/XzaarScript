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
using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler
{
    public class ExpressionScope : IDisposable
    {
        private readonly IScopeProvider scopeProvider;

        private readonly string name;

        private readonly Dictionary<string, ExpressionScope> innerScopes
            = new Dictionary<string, ExpressionScope>();

        private readonly Dictionary<string, List<FunctionExpression>> scopeFunctions
            = new Dictionary<string, List<FunctionExpression>>();

        private readonly Dictionary<string, ParameterExpression> scopeFunctionParameters
            = new Dictionary<string, ParameterExpression>();

        private readonly Dictionary<string, LambdaExpression> scopeLambdas
            = new Dictionary<string, LambdaExpression>();

        private readonly Dictionary<string, ParameterExpression> scopeVariables
            = new Dictionary<string, ParameterExpression>();

        internal ExpressionScope(IScopeProvider scopeProvider, ExpressionScope parent, string name, ExpressionScopeType scopeType, int depth)
        {
            this.scopeProvider = scopeProvider;
            this.Parent = parent;
            this.name = name;
            this.Type = scopeType;
            this.Depth = depth;
        }

        public ExpressionScope Parent { get; }
        public ExpressionScopeType Type { get; }
        public int Depth { get; }

        public ParameterExpression FindParameter(string name)
        {
            if (this.scopeFunctionParameters.TryGetValue(name, out var param))
            {
                return param;
            }
            return null;
        }

        public FunctionExpression[] FindFunctions(string name, int argCount)
        {
            var functions = new List<FunctionExpression>();
            if (this.Parent != null)
            {
                functions.AddRange(this.Parent.FindFunctions(name, argCount));
            }

            if (this.scopeFunctions.TryGetValue(name, out var list))
            {
                functions.AddRange(list.Where(x => x.Parameters.Length == argCount).ToArray());
            }

            return functions.ToArray();
        }

        public T Find<T>(string memberName, int argCount = -1) where T : XzaarExpression
        {
            if (typeof(T) == typeof(ParameterExpression))
            {
                if (memberName.StartsWith("$"))
                {
                    return new ParameterExpression(memberName) as T;
                }

                if (this.scopeFunctionParameters.TryGetValue(memberName, out var param))
                {
                    return param as T;
                }

                if (this.scopeVariables.TryGetValue(memberName, out var variable))
                {
                    return variable as T;
                }
            }

            if (typeof(T) == typeof(AnonymousFunctionExpression))
            {
                if (argCount == -1)
                {
                    if (this.scopeFunctions.TryGetValue(memberName, out var functions))
                    {
                        var function = functions.OrderBy(x => x.Parameters.Length).FirstOrDefault();
                        if (function != null)
                        {
                            return function as T;
                        }
                    }
                }
                else
                {
                    if (this.scopeFunctions.TryGetValue(memberName, out var functions))
                    {
                        var function = functions.FirstOrDefault(x => x.Parameters.Length == argCount);
                        if (function != null)
                        {
                            return function as T;
                        }
                    }
                }

                if (this.scopeLambdas.TryGetValue(memberName, out var lambda))
                {
                    return lambda as T;
                }
            }

            if (typeof(T) == typeof(FunctionExpression))
            {
                if (this.scopeFunctions.TryGetValue(memberName, out var functions))
                {
                    var function = functions.OrderBy(x => x.Parameters.Length).FirstOrDefault(x => argCount == -1 || x.Parameters.Length == argCount);
                    if (function != null)
                    {
                        return function as T;
                    }
                }
            }

            if (typeof(T) == typeof(LambdaExpression))
            {
                if (this.scopeLambdas.TryGetValue(memberName, out var lambda))
                {
                    return lambda as T;
                }
            }

            return Parent?.Find<T>(memberName, argCount);
        }

        public void Dispose()
        {
            this.scopeProvider.EndScope(this);
        }

        public void BindLambda(ParameterExpression variable, LambdaExpression lambda)
        {
            lambda.AssignmentName = variable.Name;
            scopeLambdas[variable.Name] = lambda;
        }

        public ParameterExpression AddVariable(ParameterExpression variable)
        {
            return scopeVariables[variable.Name] = variable;
        }

        public ParameterExpression AddParameter(ParameterExpression parameterExpression)
        {
            return scopeFunctionParameters[parameterExpression.Name] = parameterExpression;
        }

        public FunctionExpression AddFunction(FunctionExpression functionExpression)
        {

            if (this.scopeFunctions.TryGetValue(functionExpression.Name, out var list))
            {
                var possibleExistingFunction =
                    list.Where(x => x.Parameters.Length == functionExpression.Parameters.Length).ToArray();
                if (possibleExistingFunction.Length == 0)
                {
                    list.Add(functionExpression);
                }
                else
                {
                    if (list.All(x => !ParameterSequenceMatch(x.Parameters, functionExpression.Parameters)))
                    {
                        list.Add(functionExpression);
                    }
                }
            }
            else
            {
                this.scopeFunctions[functionExpression.Name] = new List<FunctionExpression> { functionExpression };
            }

            return functionExpression;
        }


        private bool ParameterSequenceMatch(ParameterExpression[] param1, ParameterExpression[] param2)
        {
            if (param1.Length != param2.Length) return false;
            for (var i = 0; i < param1.Length; i++)
            {
                var t1 = param1[i].Type;
                var t2 = param2[i].Type;
                if (param1[i].Name != param2[i].Name || t1.Name != t2.Name)
                {
                    return false;
                }
            }
            return true;
        }
    }
}