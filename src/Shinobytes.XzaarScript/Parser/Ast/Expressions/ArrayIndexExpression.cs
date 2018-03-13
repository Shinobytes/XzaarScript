﻿/* 
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
 
//namespace Shinobytes.XzaarScript.Scripting.Expressions
//{
//    public class ArrayIndexExpression : XzaarExpression
//    {
//        private readonly XzaarExpression array;
//        private readonly XzaarExpression index;
//        private readonly XzaarType elementType;
//        private ExpressionType nodeType;

//        internal ArrayIndexExpression(XzaarExpression array, XzaarExpression index)
//        {
//            this.array = array;
//            this.index = index;
//            this.array.Type.GetElementType();
//        }

//        public XzaarExpression Array
//        {
//            get { return array; }
//        }

//        public XzaarExpression Index
//        {
//            get { return index; }
//        }

//        public override XzaarType Type
//        {
//            get
//            {
//                return elementType;
//            }
//        }

//        public override ExpressionType Kind
//        {
//            get { return ExpressionType.ArrayIndex; }
//        }
//    }

//    public partial class XzaarExpression
//    {
//        public static ArrayIndexExpression ArrayIndex(XzaarExpression array, XzaarExpression index)
//        {
//            return new ArrayIndexExpression(array, index);
//        }
//    }
//}