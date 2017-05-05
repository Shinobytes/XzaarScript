using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class TransformerScope
    {
        public static int OVERFLOW_RROTECTION_MAXIMUM_DEPTH = 9999;

        private readonly List<TransformerScope> scopes = new List<TransformerScope>();
        private readonly List<DefineVariableNode> variables = new List<DefineVariableNode>();

        private readonly TransformerScope parent;
        private readonly XzaarNodeStream scopeNodes;
        private readonly int depth;
        private readonly string name;

        private TransformerScope(TransformerScope parent, XzaarNodeStream scopeNodes, int depth)
        {
            this.parent = parent;
            this.scopeNodes = scopeNodes;
            this.depth = depth;
            if (parent != null)
            {
                this.parent.scopes.Add(this);
            }
        }

        public TransformerScope()
        {
            name = "GLOBAL";
            scopeNodes = new XzaarNodeStream(new List<XzaarSyntaxNode>());
        }

        public bool IsGlobalScope { get { return name == "GLOBAL"; } }

        public XzaarNodeStream Nodes { get { return this.scopeNodes; } }

        public TransformerScope BeginScope(XzaarNodeStream scopeNodes)
        {
            if (depth + 1 >= OVERFLOW_RROTECTION_MAXIMUM_DEPTH)
                throw new XzaarTransformerException("PANIC! Maximum scope depth reached!!!");

            return new TransformerScope(this, scopeNodes, this.depth + 1);
        }

        public TransformerScope EndScope()
        {
            return this.parent;
        }

        public void AddVariable(DefineVariableNode defineVariable)
        {
            this.variables.Add(defineVariable);
        }

        public DefineVariableNode FindVariable(string name, bool findInParents = true)
        {
            var target = this.variables.FirstOrDefault(n => n.Name == name);
            if (target != null) return target;

            if (this.parent != null)
            {
                return this.parent.FindVariable(name, findInParents);
            }

            return null;
        }
    }
}