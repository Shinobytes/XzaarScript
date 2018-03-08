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
    public class DoWhileLoopNode : LoopNode
    {
        private readonly AstNode test;

        public DoWhileLoopNode(AstNode test, AstNode body, int nodeIndex)
            : base("DO", body, nodeIndex)
        {
            this.test = test;
        }

        public AstNode Test => test;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }


        public override string ToString()
        {
            var testExpr = test.ToString();
            if (test.Kind != SyntaxKind.Expression)
                testExpr = "(" + testExpr + ")";
            return "do { " + Body + " } while " + testExpr;
        }
    }
}