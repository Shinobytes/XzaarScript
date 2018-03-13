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
 
using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly;

namespace Shinobytes.XzaarScript.VM
{
    internal class RuntimeScope
    {
        private readonly RuntimeScope parent;
        private readonly int depth;
        private readonly Dictionary<string, RuntimeVariable> variables;
        private readonly List<RuntimeScope> scopes;
        private readonly List<Operation> operations;

        private readonly Dictionary<string, RuntimeVariable> rtVars = new Dictionary<string, RuntimeVariable>();

        internal RuntimeScope(RuntimeScope parent, int depth)
        {
            this.parent = parent;
            this.depth = depth;
            variables = new Dictionary<string, RuntimeVariable>();
            scopes = new List<RuntimeScope>();
            operations = new List<Operation>();
        }

        internal int Position { get; set; }

        internal object Result { get; set; }

        internal void SetOperations(IEnumerable<Operation> ops)
        {
            this.operations.AddRange(ops);
        }

        internal IList<Operation> GetOperations()
        {
            return this.operations;
        }

        internal void AddVariables(RuntimeVariable[] vars)
        {
            // this.variableCollection.AddRange(vars);
            for (int index = 0; index < vars.Length; index++)
            {
                var v = vars[index];
                this.variables.Add(v.Name, v);
            }
        }

        internal void Next()
        {
            Position++;
        }

        //internal IReadOnlyList<RuntimeVariable> GetVariables(bool refresh)
        //{
        //    // if (this.variableCollection.Count == 0 || refresh)
        //    {
        //        this.variableCollection.Clear();
        //        var vars = new List<RuntimeVariable>(this.variables);
        //        var p = this.parent;
        //        while (p != null)
        //        {
        //            vars.AddRange(p.variables);
        //            p = p.parent;
        //        }
        //        this.variableCollection.AddRange(vars);
        //    }
        //    return this.variableCollection;
        //}

        internal RuntimeVariable GetVariable(string name)
        {
            if (this.variables.ContainsKey(name)) return this.variables[name];
            var p = this.parent;
            while (p != null)
            {
                if (p.variables.ContainsKey(name))
                    return p.variables[name];
                p = p.parent;
            }

            //var target = this.variables.FirstOrDefault(v => v.Name == name);
            //if (target != null) return target;
            //var p = this.parent;
            //while (p != null)
            //{
            //    target = p.variables.FirstOrDefault(v => v.Name == name);
            //    if (target != null) return target;
            //    p = p.parent;
            //}
            return null;
        }


        internal RuntimeScope EndScope(object result = null)
        {
            this.Result = result;
            return this.parent;
        }

        internal RuntimeScope BeginScope()
        {
            // we only have global and function scope right now, so to improve performance we will return the same scope
            // TODO: remove this control in the future when we introduce the use of deeper scopes
            //if (this.scopes.Count > 1)
            //{
            //    return this.scopes[this.scopes.Count - 1];
            //}

            var s = new RuntimeScope(this, depth + 1);
            scopes.Add(s);
            return s;
        }

        public RuntimeVariable FindVariable(string name)
        {
            return GetVariable(name);
            ////if (!rtVars.ContainsKey(name))
            ////{
            ////    var vars = variableCollection;
            ////    var variable = vars.FirstOrDefault(v => v.Name == name);
            ////    rtVars.Add(name, variable);
            ////}
            ////var outputVar = rtVars[name];
            ////if (outputVar == null)
            ////{
            //var vars = this.GetVariables(true);
            //var variable = vars.FirstOrDefault(v => v.Name == name);
            ////rtVars[name] = variable;
            ////  }
            //return variable;
        }
    }
}