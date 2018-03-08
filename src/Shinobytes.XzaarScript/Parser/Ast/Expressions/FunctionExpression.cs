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

using System;

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public class FunctionExpression : XzaarExpression
    {
        private readonly string name;
        private readonly ParameterExpression[] parameters;
        private XzaarType returnType;
        private XzaarExpression body;
        private Func<XzaarType> returnTypeBinding;

        internal FunctionExpression(string name, ParameterExpression[] parameters, XzaarType returnType, XzaarExpression body, bool isExtern)
        {
            this.name = name;
            this.parameters = parameters;
            this.returnType = returnType;
            this.body = body;
            this.IsExtern = isExtern;
        }

        public string Name => name;

        public ParameterExpression[] GetParameters()
        {
            return parameters;
        }

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