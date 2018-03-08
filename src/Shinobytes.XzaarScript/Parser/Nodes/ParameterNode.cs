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
    public class ParameterNode : AstNode
    {
        private bool isExtern;

        public ParameterNode(string name, string type, int nodeIndex)
            : base(SyntaxKind.ParameterDefinitionExpression, null, null, nodeIndex)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public bool IsExtern
        {
            get { return isExtern; }
            set { isExtern = value; }
        }

        public override string ToString()
        {
            return "Parameter: (Type " + Type + ") " + Name;
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}