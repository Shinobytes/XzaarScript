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
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class MemberAccessChainNode : AstNode
    {
        private readonly AstNode left;
        private readonly AstNode right;
        private string resultType;

        public MemberAccessChainNode(AstNode left, AstNode right, string resultType, int nodeIndex)
            : base(SyntaxKind.MemberAccess, "ACCESSOR_CHAIN", null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            this.resultType = resultType;
        }

        public AstNode LastAccessor => left;

        public AstNode Accessor => right;

        public override string Type
        {
            get { return base.Type ?? ResultType; }
            set { base.Type = value; }
        }

        public string ResultType
        {
            get { return resultType; }
            set { resultType = value; }
        }

        public override string StringValue
        {
            get { return Accessor.StringValue; }
        }

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
            return this.LastAccessor + "." + this.Accessor;
        }
    }
}