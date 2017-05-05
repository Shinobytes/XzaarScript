using System;
using System.Linq;

namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class FunctionNode : XzaarAstNode
    {
        public string Name { get; }
        public FunctionParametersNode Parameters { get; }
        public XzaarAstNode Body { get; private set; }
        public bool IsExtern { get; set; }

        private XzaarType returnType;
        private Func<XzaarType> returnTypeBinding;

        internal FunctionNode(string name, FunctionParametersNode parameters, int nodeIndex)
            : base(XzaarAstNodeTypes.FUNCTION, "EXTERN", name, nodeIndex)
        {
            Name = name;
            Parameters = parameters;
            IsExtern = true;
            if (Parameters != null) Parameters.Parent = this;
        }

        internal FunctionNode(string name, XzaarType returnType, FunctionParametersNode parameters, int nodeIndex)
            : base(XzaarAstNodeTypes.FUNCTION, "EXTERN", name, nodeIndex)
        {
            this.returnType = returnType;
            if (this.returnType != null) this.Type = this.returnType.Name;
            Name = name;
            Parameters = parameters;
            IsExtern = true;
            if (Parameters != null) Parameters.Parent = this;
        }

        internal FunctionNode(string name, FunctionParametersNode parameters, XzaarAstNode body, int nodeIndex)
            : base(XzaarAstNodeTypes.FUNCTION, null, name, nodeIndex)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
            if (body != null) Body.Parent = this;
            if (Parameters != null) Parameters.Parent = this;
        }

        internal FunctionNode(string name, XzaarType returnType, FunctionParametersNode parameters, XzaarAstNode body, int nodeIndex)
            : base(XzaarAstNodeTypes.FUNCTION, null, name, nodeIndex)
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

        public override void Accept(IXzaarNodeVisitor xzaarNodeVisitor)
        {
            xzaarNodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Body == null || this.Body.IsEmpty();
        }

        // try and find an appropriate return type by looking for a return instruction. if one cannot be found then we will return void
        public XzaarType GetReturnType(XzaarTypeFinderContext typeFinderContext)
        {
            if ((object)this.returnType != null) return returnType;
            var typeFinder = new XzaarTypeFinderVisitor(typeFinderContext);
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


        private XzaarType FindReturnType(XzaarAstNode child, XzaarTypeFinderVisitor typeFinder)
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

        public void SetBody(XzaarAstNode xzaarNode)
        {
            xzaarNode.Parent = this;
            this.Body = xzaarNode;
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