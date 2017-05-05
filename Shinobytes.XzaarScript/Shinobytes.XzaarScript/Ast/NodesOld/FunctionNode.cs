using System.Linq;

namespace Shinobytes.XzaarScript.Ast.NodesOld
{
    public class FunctionNode : XzaarAstNode
    {
        public string Name { get; }
        public FunctionParametersNode Parameters { get; }
        public XzaarAstNode Body { get; private set; }
        public bool IsExtern { get; set; }

        private XzaarType returnType;

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
            get { return returnType; }
        }


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
    }
}