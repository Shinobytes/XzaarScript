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
    public class FunctionExpression : AnonymousFunctionExpression
    {
        private readonly string name;
        
        private XzaarType returnType;
        private XzaarExpression body;
        private Func<XzaarType> returnTypeBinding;

        internal FunctionExpression(string name, ParameterExpression[] parameters, XzaarType returnType, XzaarExpression body, bool isExtern)
        {
            this.name = name;
            this.Parameters = parameters;
            this.returnType = returnType;
            this.body = body;
            this.IsExtern = isExtern;
        }

        public string Name => name;

        public ParameterExpression[] Parameters { get; }

        public XzaarType ReturnType
        {
            get
            {
                if ((returnType == null || returnType.IsAny) && returnTypeBinding != null)
                {
                    returnType = returnTypeBinding.Invoke();
                    if ((object)returnType == null)
                    {
                        returnType = XzaarBaseTypes.Any;
                    }
                }

                return returnType;
            }
        }

        public override XzaarType Type => ReturnType;

        public XzaarExpression GetBody()
        {
            return body;
        }

        public override ExpressionType NodeType => ExpressionType.Call;

        public bool IsExtern { get; set; }

        public FunctionExpression SetBody(XzaarExpression body)
        {
            this.body = body;
            return this;
        }

        public FunctionExpression SetReturnType(XzaarType type)
        {
            this.returnType = type;
            return this;
        }

        public void BindReturnType(Func<XzaarType> func)
        {
            this.returnTypeBinding = func;
        }

        public bool IsReturnTypeBound => returnTypeBinding != null;
    }

    public partial class XzaarExpression
    {
        public static FunctionExpression Function(string name, ParameterExpression[] parameters, XzaarType returnType, bool isExtern)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return new FunctionExpression(name, parameters, returnType, null, isExtern);
        }
        public static FunctionExpression Function(string name, ParameterExpression[] parameters, XzaarType returnType, XzaarExpression body, bool isExtern)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return new FunctionExpression(name, parameters, returnType, body, isExtern);
        }
    }
}