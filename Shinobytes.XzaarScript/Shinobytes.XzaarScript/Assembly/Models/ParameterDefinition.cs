namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class ParameterDefinition : VariableReference
    {
        private MethodReference method;

        public override MemberTypes MemberType => MemberTypes.Parameter;

        public MethodReference Method
        {
            get { return method; }
            internal set { method = value; }
        }

        public override VariableReference Clone()
        {
            return new ParameterDefinition
            {
                method = this.method,
                Name = this.Name,
                Type = this.Type,
                InitialValue = this.InitialValue,
                ArrayIndex = this.ArrayIndex,
                Reference = this.Reference
            };
        }
    }
}