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
