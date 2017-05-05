using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class FunctionExpression : XzaarExpression
    {
        private readonly string name;
        private readonly ParameterExpression[] parameters;
        private XzaarType returnType;
        private XzaarExpression body;
        private XzaarExpressionType nodeType;
        private XzaarType type;
        private Func<XzaarType> returnTypeBinding;

        internal FunctionExpression(string name, ParameterExpression[] parameters, XzaarType returnType, XzaarExpression body, bool isExtern)
        {
            this.name = name;
            this.parameters = parameters;
            this.returnType = returnType;
            this.body = body;
            this.IsExtern = isExtern;
        }

        public string Name
        {
            get { return name; }
        }

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

        public override XzaarType Type
        {
            get { return ReturnType; }
        }

        public XzaarExpression GetBody()
        {
            return body;
        }

        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Call; }
        }

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
            //if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            return new FunctionExpression(name, parameters, returnType, null, isExtern);
        }
        public static FunctionExpression Function(string name, ParameterExpression[] parameters, XzaarType returnType, XzaarExpression body, bool isExtern)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            //if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            return new FunctionExpression(name, parameters, returnType, body, isExtern);
        }
    }
}