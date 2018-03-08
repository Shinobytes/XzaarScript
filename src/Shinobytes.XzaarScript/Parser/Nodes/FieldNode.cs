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
    public class FieldNode : AstNode
    {
        private readonly string name;
        private readonly string declaringType;

        public FieldNode(string type, string name, string declaringType, int nodeIndex)
            : base(SyntaxKind.FieldDefinitionExpression, "FIELD", null, nodeIndex)
        {
            this.Type = type;
            this.name = name;
            this.declaringType = declaringType;
            this.Value = name;
        }

        public string DeclaringType => declaringType;

        public string Name => name;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override string ToString()
        {
            return "FIELD " + Type + " " + name;
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}