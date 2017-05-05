namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class Constant : VariableReference
    {
        public object Value { get; set; }

        public bool IsArray { get; set; }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Constant; }
        }

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Value + "[" + ArrayIndex + "]";
            }
            return Value + ": " + Type.Name;
        }

        public override VariableReference Clone()
        {
            return new Constant
            {
                Value = this.Value,
                IsArray = this.IsArray,
                Name = this.Name,
                Type = this.Type,
                InitialValue = this.InitialValue,
                ArrayIndex = this.ArrayIndex,
                Reference = this.Reference
            };
        }
    }
}