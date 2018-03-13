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