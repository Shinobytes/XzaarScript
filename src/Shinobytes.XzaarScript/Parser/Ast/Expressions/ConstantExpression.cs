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
    public class ConstantExpression : XzaarExpression
    {
        private readonly object value;

        internal ConstantExpression(object value)
        {
            this.value = value;
        }
        public XzaarExpression ArrayIndex { get; set; }

        internal static ConstantExpression Make(object value, XzaarType type)
        {
            if ((value == null && type == XzaarBaseTypes.Any) ||
                (value != null && XzaarBaseTypes.Typeof(value.GetType()) == type))
            {
                return new ConstantExpression(value);
            }
            else
            {
                return new TypedConstantExpression(value, type);
            }
        }

        public override XzaarType Type
        {
            get
            {
                if (value == null)
                {
                    return XzaarBaseTypes.Any;
                }
                return XzaarBaseTypes.Typeof(value.GetType());
            }
        }

        public sealed override ExpressionType NodeType => ExpressionType.Constant;

        public object Value => value;
    }

    internal class TypedConstantExpression : ConstantExpression
    {
        private readonly XzaarType _type;

        internal TypedConstantExpression(object value, XzaarType type)
            : base(value)
        {
            _type = type;
        }

        public sealed override XzaarType Type => _type;
    }

    public partial class XzaarExpression
    {
        public static ConstantExpression Constant(object value)
        {
            return ConstantExpression.Make(value, value == null ? (XzaarType)typeof(object) : (XzaarType)value.GetType());
        }

        public static ConstantExpression Constant(object value, XzaarType type)
        {                                  
            return ConstantExpression.Make(value, type);
        }
    }
}