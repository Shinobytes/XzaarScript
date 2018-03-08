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
    public class ForeachLoopNode : LoopNode
    {
        private readonly AstNode variableDefinition;
        private readonly AstNode sourceExpression;

        public ForeachLoopNode(AstNode variableDefinition, AstNode sourceExpression, AstNode body, int nodeIndex)
            : base("FOREACH", body, nodeIndex)
        {
            this.variableDefinition = variableDefinition;
            this.sourceExpression = sourceExpression;
        }

        public AstNode Source => sourceExpression;

        public AstNode Variable => variableDefinition;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "foreach (" + this.Variable + " in " + this.Source + ") { " + this.Body + " }";
        }
    }
}