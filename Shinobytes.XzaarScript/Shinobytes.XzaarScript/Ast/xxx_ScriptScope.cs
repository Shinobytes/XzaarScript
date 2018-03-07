using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{    
    public class xxx_ScriptScope
    {
        public const string GlobalScopeName = "$GLOBAL";

        public string Name { get; set; }

        public int Depth { get; set; }

        private int index = 0;

        public readonly List<VariableNode> Variables = new List<VariableNode>();

        public readonly List<ParameterNode> Parameters = new List<ParameterNode>();

        public readonly List<FunctionNode> Functions = new List<FunctionNode>();

        public readonly List<StructNode> Structs = new List<StructNode>();

        private readonly List<xxx_ScriptScope> scopes = new List<xxx_ScriptScope>();

        private xxx_ScriptScope parent;

        private AstNode associatedNode;

        public static xxx_ScriptScope Global()
        {
            return new xxx_ScriptScope(GlobalScopeName, 0, 0, null);
        }

        public xxx_ScriptScope(string name, int depth, int scopeIndex, xxx_ScriptScope parent, AstNode associatedNode = null)
        {
            this.index = scopeIndex;
            this.parent = parent;
            this.associatedNode = associatedNode;
            this.Name = name;
            this.Depth = depth;
        }

        public AstNode Node => associatedNode;

        public xxx_ScriptScope In(AstNode associatedNode, string name = null)
        {
            var newScope = new xxx_ScriptScope(Name + "." + (name != null ? name : Depth + "#" + (index++)), Depth + 1, index, this, associatedNode);
            this.scopes.Add(newScope);
            return newScope;
        }

        public xxx_ScriptScope Out()
        {
            return parent;
        }

        public xxx_ScriptScope Find(string name)
        {
            if (this.Name == name) return this;
            foreach (var s in scopes)
            {
                var correct = s.Find(name);
                if (correct != null) return correct;
            }
            return null;
        }

        public void SetScopeNode(AstNode result)
        {
            this.associatedNode = result;
        }

        internal xxx_ScriptScope Find(AstNode node)
        {
            if (this.Node == node) return this;
            foreach (var s in scopes)
            {
                var correct = s.Find(node);
                if (correct != null) return correct;
            }
            return null;
        }

        public void SetChildScopeNode(int childIndex, AstNode node)
        {
            this.scopes[childIndex].SetScopeNode(node);
        }
    }
}