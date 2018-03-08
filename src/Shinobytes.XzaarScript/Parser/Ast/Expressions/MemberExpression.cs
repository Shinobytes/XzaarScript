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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public class MemberExpression : XzaarExpression
    {
       
        private XzaarExpression _expression;

        public XzaarMemberInfo Member => GetMember();

        public XzaarExpression Expression => _expression;

        internal MemberExpression(XzaarExpression expression, XzaarExpression arrayIndex, XzaarType memberType)
        {
            MemberType = memberType;
            _expression = expression;
            this.ArrayIndex = arrayIndex;
        }


        public XzaarType MemberType { get; set; }

        public XzaarExpression ArrayIndex { get; }

        public sealed override ExpressionType NodeType => ExpressionType.MemberAccess;

        internal virtual XzaarMemberInfo GetMember()
        {
            return null;
            // throw new InvalidOperationException();
        }
     
        public override XzaarType Type => TryGetType(Expression);

        private XzaarType TryGetType(XzaarExpression expression)
        {
            if (this.Member != null)
            {
                return this.Member.GetXzaarType();
            }

            if (expression is ParameterExpression paramExpr)
            {
                return paramExpr.Type;
            }

            return XzaarBaseTypes.Any;
        }
    }

    public partial class XzaarExpression
    {
        public static MemberExpression MemberAccess(XzaarExpression expression, XzaarType memberType)
        {
            return new MemberExpression(expression, null, memberType);
        }

        public static MemberExpression MemberAccess(XzaarExpression expression, XzaarExpression arrayIndex, XzaarType memberType)
        {
            return new MemberExpression(expression, arrayIndex, memberType);
        }
    }
}