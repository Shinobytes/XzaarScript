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
            return "fn " + this.Name + "(" + this.Parameters + ")";
        }

        public void BindReturnType(string name, Func<XzaarType> func)
        {
            ReturnTypeBindingName = name;
            this.returnTypeBinding = func;
        }
    }
}