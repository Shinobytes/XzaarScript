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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    [Serializable]
    public class UnaryExpression : XzaarExpression
    {
        private readonly XzaarExpression item;
        private ExpressionType nodeType;
        private XzaarType type;

        internal UnaryExpression(XzaarExpression item, ExpressionType nodeType)
        {
            this.item = item;
            this.nodeType = nodeType;
        }

        public XzaarExpression Item => item;

        public override ExpressionType NodeType => nodeType;

        public override XzaarType Type => XzaarBaseTypes.Number;
    }

    public partial class XzaarExpression
    {
        public static UnaryExpression UnaryOperation(XzaarExpression item, ExpressionType type)
        {
            return new UnaryExpression(item, type);
        }

        public static UnaryExpression PostIncrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.PostIncrementAssign);
        }

        public static UnaryExpression Incrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.Increment);
        }

        public static UnaryExpression PostDecrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.PostDecrementAssign);
        }

        public static UnaryExpression Decrementor(XzaarExpression item)
        {
            return UnaryOperation(item, ExpressionType.Decrement);
        }
    }
}