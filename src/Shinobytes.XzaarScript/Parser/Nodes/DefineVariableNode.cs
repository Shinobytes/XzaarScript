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
    public class DefineVariableNode : VariableNode
    {
        public DefineVariableNode(string type, string name, AstNode value, int nodeIndex)
            : base(type, name, value, true, nodeIndex)
        {
        }

        public AstNode AssignmentExpression => Value as AstNode;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            if (Value != null)
            {
                if (Value is ArrayNode)
                {
                    var aNode = Value as ArrayNode;
                    return "var " + Name + " : " + Type + " = " + aNode;
                }
                return "var " + Name + " : " + Type + " = " + Value;
            }
            return "var " + Name;
        }

        public void SetType(string any)
        {
            this.Type = any;
        }

        public void SetValue(AstNode val)
        {
            this.Value = val;
        }
    }
}