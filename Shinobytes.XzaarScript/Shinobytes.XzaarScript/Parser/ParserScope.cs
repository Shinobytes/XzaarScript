using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser
{
    public class ParserScope
    {
        public static int OVERFLOW_RROTECTION_MAXIMUM_DEPTH = 9999;

        private readonly List<ParserScope> scopes = new List<ParserScope>();
        private readonly List<DefineVariableNode> variables = new List<DefineVariableNode>();

        private readonly ParserScope parent;
        private readonly NodeStream scopeNodes;
        private readonly int depth;
        private readonly string name;

        private ParserScope(ParserScope parent, NodeStream scopeNodes, int depth)
        {
            this.parent = parent;
            this.scopeNodes = scopeNodes;
            this.depth = depth;
            if (parent != null)
            {
                this.parent.scopes.Add(this);
            }
        }

        public ParserScope()
        {
            name = "GLOBAL";
            scopeNodes = new NodeStream(new List<SyntaxNode>());
        }

        public bool IsGlobalScope => name == "GLOBAL";

        public NodeStream Nodes => this.scopeNodes;

        public ParserScope BeginScope(NodeStream scopeNodes)
        {
            if (depth + 1 >= OVERFLOW_RROTECTION_MAXIMUM_DEPTH)
                throw new ParserException("PANIC! Maximum scope depth reached!!!");

            return new ParserScope(this, scopeNodes, this.depth + 1);
        }

        public ParserScope EndScope()
        {
            return this.parent;
        }

        public void AddVariable(DefineVariableNode defineVariable)
        {
            this.variables.Add(defineVariable);
        }

        public DefineVariableNode FindVariable(string localName, bool findInParents = true)
        {
            var target = variables.FirstOrDefault(n => n.Name == localName);
            return target ?? parent?.FindVariable(localName, findInParents);
        }
    }
}