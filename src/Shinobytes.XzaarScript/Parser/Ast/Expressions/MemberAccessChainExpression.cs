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
    public class MemberAccessChainExpression : XzaarExpression
    {
        internal MemberAccessChainExpression(XzaarExpression left, XzaarExpression right)
        {
            Left = left;
            Right = right;            
        }

        public XzaarExpression Left { get; }

        public XzaarExpression Right { get; }

        public XzaarType ResultType
        {
            get
            {
                var fc = Right as FunctionCallExpression;
                if (fc != null)
                {
                    return fc.Type;
                }
                var ma = Right as MemberExpression;
                if (ma != null)
                {
                    return ma.MemberType;
                }
                return null;
            }
        }

        public override XzaarType Type => ResultType;

        public sealed override ExpressionType NodeType => ExpressionType.MemberAccess;
    }

    public partial class XzaarExpression
    {
        public static MemberAccessChainExpression AccessChain(XzaarExpression left, XzaarExpression right)
        {
            return new MemberAccessChainExpression(left, right);
        }
    }
}