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
using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class FunctionNode : AstNode
    {
        public string Name { get; }
        public FunctionParametersNode Parameters { get; }
        public AstNode Body { get; private set; }
        public bool IsExtern { get; set; }

        private XzaarType returnType;
        private Func<XzaarType> returnTypeBinding;

        internal FunctionNode(string name, FunctionParametersNode parameters, int nodeIndex)
            : base(SyntaxKind.FunctionDefinitionExpression, "EXTERN", name, nodeIndex)
        {
            Name = name;
            Parameters = parameters;
            IsExtern = true;
            if (Parameters != null) Parameters.Parent = this;
        }

        internal FunctionNode(string name, XzaarType returnType, FunctionParametersNode parameters, int nodeIndex)
            : base(SyntaxKind.FunctionDefinitionExpression, "EXTERN", name, nodeIndex)
        {
            this.returnType = returnType;
            if (this.returnType != null) this.Type = this.returnType.Name;
            Name = name;
            Parameters = parameters;
            IsExtern = true;
            if (Parameters != null) Parameters.Parent = this;
        }

        internal FunctionNode(string name, FunctionParametersNode parameters, AstNode body, int nodeIndex)
            : base(SyntaxKind.FunctionDefinitionExpression, null, name, nodeIndex)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
            if (body != null) Body.Parent = this;
            if (Parameters != null) Parameters.Parent = this;
        }

        internal FunctionNode(string name, XzaarType returnType, FunctionParametersNode parameters, AstNode body, int nodeIndex)
            : base(SyntaxKind.FunctionDefinitionExpression, null, name, nodeIndex)
        {
            this.returnType = returnType;
            if (this.returnType != null) this.Type = this.returnType.Name;
            Name = name;
            Parameters = parameters;
            Body = body;
            if (body != null) Body.Parent = this;
            if (Parameters != null) Parameters.Parent = this;
        }


        public ParameterNode FindParameter(string name)
        {
            if (Parameters == null || Parameters.Parameters == null || Parameters.Parameters.Count == 0) return null;
            return Parameters.Parameters.FirstOrDefault(p => p.Name == name);
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Body == null || this.Body.IsEmpty();
        }

        // try and find an appropriate return type by looking for a return instruction. if one cannot be found then we will return void
        public XzaarType GetReturnType(XzaarTypeFinderContext typeFinderContext)
        {
            if ((object)this.returnType != null) return returnType;
            var typeFinder = new TypeFinderVisitor(typeFinderContext);
            returnType = FindReturnType(Body, typeFinder) ?? (XzaarType)typeof(void);
            return returnType;
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

        public bool IsReturnTypeBound => returnTypeBinding != null;

        public string ReturnTypeBindingName { get; private set; }


        private XzaarType FindReturnType(AstNode child, TypeFinderVisitor typeFinder)
        {
            child.Accept(typeFinder);
            if (typeFinder.FoundType) return typeFinder.Type;

            foreach (var child1 in child.Children)
            {
                var returnType = FindReturnType(child1, typeFinder);
                if (returnType != null) return returnType;
            }
            return null;
        }

        public void SetBody(AstNode node)
        {
            node.Parent = this;
            this.Body = node;
        }

        public void SetReturnType(XzaarType type)
        {
            this.returnType = type;
        }

        public override string ToString()
        {
            return "fn " + this.Name + "(" + this.Parameters + ") { " + this.Body + " }";
        }

        public void BindReturnType(string name, Func<XzaarType> func)
        {
            ReturnTypeBindingName = name;
            this.returnTypeBinding = func;
        }
    }
}