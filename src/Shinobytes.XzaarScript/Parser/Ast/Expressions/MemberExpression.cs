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