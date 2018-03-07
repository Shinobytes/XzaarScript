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
        private readonly TokenStream tokens;
        private readonly int depth;
        private readonly string name;

        private ParserScope(ParserScope parent, TokenStream tokens, int depth)
        {
            this.parent = parent;
            this.tokens = tokens;
            this.depth = depth;
            if (parent != null)
            {
                this.parent.scopes.Add(this);
            }
        }

        public ParserScope()
        {
            name = "GLOBAL";
            tokens = new TokenStream(new List<SyntaxToken>());
        }

        public bool IsGlobalScope => name == "GLOBAL";

        public TokenStream Tokens => this.tokens;

        public ParserScope BeginScope(TokenStream tokens)
        {
            if (depth + 1 >= OVERFLOW_RROTECTION_MAXIMUM_DEPTH)
                throw new ParserException("PANIC! Maximum scope depth reached!!!");

            return new ParserScope(this, tokens, this.depth + 1);
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