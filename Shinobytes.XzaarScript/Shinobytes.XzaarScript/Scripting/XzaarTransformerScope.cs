using System.Collections.Generic;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{    
    public class XzaarTransformerScope
    {
        public const string GlobalScopeName = "$GLOBAL";

        public string Name { get; set; }

        public int Depth { get; set; }

        private int index = 0;

        public readonly List<VariableNode> Variables = new List<VariableNode>();

        public readonly List<ParameterNode> Parameters = new List<ParameterNode>();

        public readonly List<FunctionNode> Functions = new List<FunctionNode>();

        public readonly List<StructNode> Structs = new List<StructNode>();

        private readonly List<XzaarTransformerScope> scopes = new List<XzaarTransformerScope>();

        private XzaarTransformerScope parent;

        private XzaarNode associatedNode;

        public static XzaarTransformerScope Global()
        {
            return new XzaarTransformerScope(GlobalScopeName, 0, 0, null);
        }

        public XzaarTransformerScope(string name, int depth, int scopeIndex, XzaarTransformerScope parent, XzaarNode associatedNode = null)
        {
            this.index = scopeIndex;
            this.parent = parent;
            this.associatedNode = associatedNode;
            this.Name = name;
            this.Depth = depth;
        }

        public XzaarNode Node { get { return associatedNode; } }

        public XzaarTransformerScope In(XzaarNode associatedNode, string name = null)
        {
            var newScope = new XzaarTransformerScope(Name + "." + (name != null ? name : Depth + "#" + (index++)), Depth + 1, index, this, associatedNode);
            this.scopes.Add(newScope);
            return newScope;
        }

        public XzaarTransformerScope Out()
        {
            return parent;
        }

        public XzaarTransformerScope Find(string name)
        {
            if (this.Name == name) return this;
            foreach (var s in scopes)
            {
                var correct = s.Find(name);
                if (correct != null) return correct;
            }
            return null;
        }

        public void SetScopeNode(XzaarNode result)
        {
            this.associatedNode = result;
        }

        internal XzaarTransformerScope Find(XzaarNode node)
        {
            if (this.Node == node) return this;
            foreach (var s in scopes)
            {
                var correct = s.Find(node);
                if (correct != null) return correct;
            }
            return null;
        }

        public void SetChildScopeNode(int childIndex, XzaarNode node)
        {
            this.scopes[childIndex].SetScopeNode(node);
        }
    }
}