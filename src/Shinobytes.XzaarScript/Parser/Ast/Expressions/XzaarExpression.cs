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
    public abstract partial class XzaarExpression
    {
        public bool IsEmpty()
        {
            var block = this as BlockExpression;
            if (block != null && block.Expressions.Count == 0) return true;
            return this is DefaultExpression;
        }

        public virtual ExpressionType NodeType { get; }

        public virtual XzaarType Type { get; }

        public static void RequiresCanWrite(XzaarExpression expression, string paramName)
        {
        }

        private static void RequiresCanRead(XzaarExpression expression, string paramName)
        {
            //if (expression == null)
            //{
            //    throw new ArgumentNullException(paramName);
            //}

            //// validate that we can read the node
            //switch (expression.Kind)
            //{
            //    case ExpressionType.Index:
            //        IndexExpression index = (IndexExpression)expression;
            //        if (index.Indexer != null && !index.Indexer.CanRead)
            //        {
            //            throw new ArgumentException(Strings.ExpressionMustBeReadable, paramName);
            //        }
            //        break;
            //    case ExpressionType.MemberAccess:
            //        MemberExpression member = (MemberExpression)expression;
            //        XzaarMemberInfo memberInfo = member.Member;
            //        if (memberInfo.MemberType == XzaarMemberTypes.Property)
            //        {
            //            XzaarPropertyInfo prop = (XzaarPropertyInfo)memberInfo;
            //            if (!prop.CanRead)
            //            {
            //                throw new ArgumentException(Strings.ExpressionMustBeReadable, paramName);
            //            }
            //        }
            //        break;
            //}
        }
    }
}
