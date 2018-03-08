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
    public class CreateStructNode : AstNode
    {        
        private readonly StructNode structNode;

        public CreateStructNode(StructNode structNode, int nodeIndex)
            : base(SyntaxKind.TypeInstantiation, "CREATE_STRUCT", null, nodeIndex)
        {
            this.structNode = structNode;
            this.Type = this.structNode.ValueText;
        }

        public CreateStructNode(StructNode structNode, AstNode[] structFieldInitializers, int nodeIndex)
            : base(SyntaxKind.TypeInstantiation, "CREATE_STRUCT", null, nodeIndex)
        {
            FieldInitializers = structFieldInitializers;
            this.structNode = structNode;
            this.Type = this.structNode.ValueText;
        }

        public AstNode[] FieldInitializers { get; set; }

        public StructNode StructNode => structNode;

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return "new_struct " + this.structNode.Name;
        }
    }
}