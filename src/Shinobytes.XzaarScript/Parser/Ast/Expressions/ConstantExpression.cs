/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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