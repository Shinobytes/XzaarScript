namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class MethodDefinition : MethodReference
    {
        public MethodDefinition(string name, ParameterDefinition[] parameters, MethodBody body, TypeReference returnType, TypeReference declaringType, bool isExtern)
        {
            this.Parameters = new ParameterCollection(this);
            this.IsExtern = isExtern;
            this.Name = name;
            this.Parameters.AddParameters(parameters);
            this.Body = body;
            this.ReturnType = returnType;
            this.DeclaringType = declaringType;

            if (this.Body == null)
                this.Body = new MethodBody(this);
        }

        public MethodDefinition(
            string name,
            ParameterDefinition[] parameters,
            TypeReference returnType,
            TypeReference declaringType,
            bool isExtern)
            : this(name, parameters, null, returnType, declaringType, isExtern)
        {
        }

        public MethodDefinition(
            string name,
            ParameterDefinition[] parameters,
            TypeReference returnType,
            bool isExtern)
            : this(name, parameters, null, returnType, null, isExtern)
        {
        }

        public MethodDefinition(
            string name,
            ParameterDefinition[] parameters,
            bool isExtern)
            : this(name, parameters, null, null, null, isExtern)
        {
        }

        public MethodDefinition(
            string name,
            bool isExtern)
            : this(name, null, null, null, null, isExtern)
        {
        }

        public MethodDefinition SetParameters(ParameterDefinition[] parameters)
        {
            this.Parameters.Clear();
            this.Parameters.AddParameters(parameters);
            return this;
        }

        public MethodDefinition SetReturnType(TypeReference returnType)
        {
            this.ReturnType = returnType;
            return this;
        }


        public MethodDefinition SetDeclaringType(TypeReference declaringType)
        {
            this.DeclaringType = declaringType;
            return this;
        }

        public MethodDefinition SetBody(MethodBody body)
        {
            this.Body = body;
            return this;
        }

        public bool IsExtern { get; private set; }

        public MethodBody Body { get; private set; }
    }
}