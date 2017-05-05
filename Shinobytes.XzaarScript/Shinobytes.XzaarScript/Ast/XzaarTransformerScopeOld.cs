using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{    
    public class XzaarTransformerScopeOld
    {
        public const string GlobalScopeName = "$GLOBAL";

        public string Name { get; set; }

        public int Depth { get; set; }

        private int index = 0;

        public readonly List<VariableNode> Variables = new List<VariableNode>();

        public readonly List<ParameterNode> Parameters = new List<ParameterNode>();

        public readonly List<FunctionNode> Functions = new List<FunctionNode>();

        public readonly List<StructNode> Structs = new List<StructNode>();

        private readonly List<XzaarTransformerScopeOld> scopes = new List<XzaarTransformerScopeOld>();

        private XzaarTransformerScopeOld parent;

        private XzaarAstNode associatedNode;

        public static XzaarTransformerScopeOld Global()
        {
            return new XzaarTransformerScopeOld(GlobalScopeName, 0, 0, null);
        }

        public XzaarTransformerScopeOld(string name, int depth, int scopeIndex, XzaarTransformerScopeOld parent, XzaarAstNode associatedNode = null)
        {
            this.index = scopeIndex;
            this.parent = parent;
            this.associatedNode = associatedNode;
            this.Name = name;
            this.Depth = depth;
        }

        public XzaarAstNode Node { get { return associatedNode; } }

        public XzaarTransformerScopeOld In(XzaarAstNode associatedNode, string name = null)
        {
            var newScope = new XzaarTransformerScopeOld(Name + "." + (name != null ? name : Depth + "#" + (index++)), Depth + 1, index, this, associatedNode);
            this.scopes.Add(newScope);
            return newScope;
        }

        public XzaarTransformerScopeOld Out()
        {
            return parent;
        }

        public XzaarTransformerScopeOld Find(string name)
        {
            if (this.Name == name) return this;
            foreach (var s in scopes)
            {
                var correct = s.Find(name);
                if (correct != null) return correct;
            }
            return null;
        }

        public void SetScopeNode(XzaarAstNode result)
        {
            this.associatedNode = result;
        }

        internal XzaarTransformerScopeOld Find(XzaarAstNode node)
        {
            if (this.Node == node) return this;
            foreach (var s in scopes)
            {
                var correct = s.Find(node);
                if (correct != null) return correct;
            }
            return null;
        }

        public void SetChildScopeNode(int childIndex, XzaarAstNode node)
        {
            this.scopes[childIndex].SetScopeNode(node);
        }
    }
}