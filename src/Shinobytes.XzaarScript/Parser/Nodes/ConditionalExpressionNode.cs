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
    public class ConditionalExpressionNode : AstNode
    {
        private readonly AstNode condition;
        private AstNode whenTrue;
        private AstNode whenFalse;

        public ConditionalExpressionNode(AstNode condition, AstNode whenTrue, AstNode whenFalse, int nodeIndex)
            : base(SyntaxKind.ConditionalExpression, "CONDITIONAL", null, nodeIndex)
        {
            if (condition != null) condition.Parent = this;
            if (whenTrue != null) whenTrue.Parent = this;
            if (whenFalse != null) whenFalse.Parent = this;
            this.condition = condition;
            this.whenTrue = whenTrue;
            this.whenFalse = whenFalse;
        }

        public AstNode GetCondition() => condition;

        public AstNode GetTrue() => whenTrue;

        public AstNode GetFalse() => whenFalse;


        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return GetCondition() == null && GetTrue() == null && GetFalse() == null;
        }

        public void SetWhenTrue(AstNode node)
        {
            whenTrue = node;
        }

        public void SetWhenFalse(AstNode node)
        {
            whenFalse = node;
        }


        public override string ToString()
        {
            var conditionString = condition.ToString();
            if (condition.Kind != SyntaxKind.Expression)
                conditionString = "(" + conditionString + ")";
            return $"{conditionString} ? {{{whenTrue}}} : {{{whenFalse}}}";
        }

    }
}