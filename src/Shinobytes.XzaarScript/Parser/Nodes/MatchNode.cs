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

using System;
using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class MatchNode : AstNode
    {
        private readonly AstNode valueExpression;
        private readonly CaseNode[] cases;

        public MatchNode(AstNode valueExpression, CaseNode[] cases, int nodeIndex)
            : base(SyntaxKind.MatchExpression, "MATCH", null, nodeIndex)
        {
            this.valueExpression = valueExpression;
            this.cases = cases;
        }

        public AstNode ValueExpression => valueExpression;

        public CaseNode[] Cases => cases;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return "switch (" + this.ValueExpression + ") { " + String.Join(" ", 
                Cases.Select(c => c.ToString()).ToArray()) + " }";
        }
    }
}