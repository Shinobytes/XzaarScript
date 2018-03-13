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
                    if (ma.MemberType.Equals(XzaarBaseTypes.Void))
                    {
                        return XzaarBaseTypes.Any;
                    }
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