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

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ForLoopNode : LoopNode
    {
        private readonly AstNode initiator;
        private readonly AstNode test;
        private readonly AstNode incrementor;

        public ForLoopNode(AstNode initiator, AstNode test, AstNode incrementor, AstNode body, int nodeIndex)
            : base("FOR", body, nodeIndex)
        {
            this.initiator = initiator;
            this.test = test;
            this.incrementor = incrementor;
        }

        public AstNode Initiator => initiator;

        public AstNode Incrementor => incrementor;

        public AstNode Test => test;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "for (" + this.initiator + "; " + this.test + "; " + this.incrementor + ") { " + this.Body + " }";
        }
    }
}