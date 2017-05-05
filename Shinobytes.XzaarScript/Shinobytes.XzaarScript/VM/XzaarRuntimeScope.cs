using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.VM
{
    internal class XzaarRuntimeScope
    {
        private readonly XzaarRuntimeScope parent;
        private readonly int depth;
        private readonly Dictionary<string, XzaarRuntimeVariable> variables;
        private readonly List<XzaarRuntimeScope> scopes;
        private readonly List<XzaarBinaryCode> operations;

        private readonly Dictionary<string, XzaarRuntimeVariable> rtVars = new Dictionary<string, XzaarRuntimeVariable>();

        internal XzaarRuntimeScope(XzaarRuntimeScope parent, int depth)
        {
            this.parent = parent;
            this.depth = depth;
            variables = new Dictionary<string, XzaarRuntimeVariable>();
            scopes = new List<XzaarRuntimeScope>();
            operations = new List<XzaarBinaryCode>();
        }

        internal int Offset { get; set; }

        internal object Result { get; set; }

        internal void SetOperations(IEnumerable<XzaarBinaryCode> ops)
        {
            this.operations.AddRange(ops);
        }

        internal IList<XzaarBinaryCode> GetOperations()
        {
            return this.operations;
        }

        internal void AddVariables(XzaarRuntimeVariable[] vars)
        {
            // this.variableCollection.AddRange(vars);
            for (int index = 0; index < vars.Length; index++)
            {
                var v = vars[index];
                this.variables.Add(v.Name, v);
            }
        }

        //internal IReadOnlyList<XzaarRuntimeVariable> GetVariables(bool refresh)
        //{
        //    // if (this.variableCollection.Count == 0 || refresh)
        //    {
        //        this.variableCollection.Clear();
        //        var vars = new List<XzaarRuntimeVariable>(this.variables);
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

        internal XzaarRuntimeVariable GetVariable(string name)
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


        internal XzaarRuntimeScope EndScope(object result = null)
        {
            this.Result = result;
            return this.parent;
        }

        internal XzaarRuntimeScope BeginScope()
        {
            // we only have global and function scope right now, so to improve performance we will return the same scope
            // TODO: remove this control in the future when we introduce the use of deeper scopes
            //if (this.scopes.Count > 1)
            //{
            //    return this.scopes[this.scopes.Count - 1];
            //}

            var s = new XzaarRuntimeScope(this, depth + 1);
            scopes.Add(s);
            return s;
        }

        public XzaarRuntimeVariable FindVariable(string name)
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