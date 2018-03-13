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
using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast;
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