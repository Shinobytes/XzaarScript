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