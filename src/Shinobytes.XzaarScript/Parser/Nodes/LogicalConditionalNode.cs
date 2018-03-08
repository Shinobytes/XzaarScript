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
    public class LogicalConditionalNode : AstNode
    {
        private AstNode left;
        private AstNode right;

        public LogicalConditionalNode(int operatingOrderWeight, AstNode left, string op, AstNode right, int nodeIndex)
            : base(SyntaxKind.ConditionalExpression, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            Op = op;
            OperatingOrder = operatingOrderWeight;
        }

        public AstNode Left => left;
        public string Op { get; }
        public AstNode Right => right;



        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }

        public override bool IsEmpty()
        {
            return Op == null & Left == null && Right == null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }
    }
}