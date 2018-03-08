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

using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class CaseNode : ControlFlowNode
    {
        private readonly AstNode test;
        private readonly AstNode body;

        public CaseNode(AstNode test, AstNode body, int nodeIndex)
            : base(SyntaxKind.KeywordCase, "CASE", nodeIndex)
        {
            this.test = test;
            this.body = body;
        }

        public bool IsDefaultCase => test == null;

        public AstNode Body => body;

        public AstNode Test => test;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (test == null)
                return "default: " + body;
            return "case " + test + ": " + body;
        }
    }
}