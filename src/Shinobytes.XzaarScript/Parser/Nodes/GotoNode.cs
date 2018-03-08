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
    public class GotoNode : ControlFlowNode
    {
        private readonly AstNode label;

        private readonly string labelName;

        public GotoNode(AstNode label, int nodeIndex)
            : base(SyntaxKind.KeywordGoto, "GOTO", nodeIndex)
        {
            this.label = label;
        }

        public GotoNode(string label, int nodeIndex)
            : base(SyntaxKind.KeywordGoto, "GOTO", nodeIndex)
        {
            this.labelName = label;
        }

        public AstNode Label => label;

        public string LabelName
        {
            get
            {
                if (label != null) return label.Value?.ToString();
                return labelName;
            }
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "goto " + labelName;
        }
    }
}