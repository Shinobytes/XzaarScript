namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class VariableReference : MemberReference
    {
        private VariableReference reference;
        public TypeReference Type { get; set; }

        public object InitialValue { get; set; }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Variable; }
        }

        public object ArrayIndex { get; set; }

        public VariableReference Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        public bool IsRef { get { return this.reference != null; } }

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Name + "[" + ArrayIndex + "]";
            }
            return Name + ": " + Type.Name;
        }

        public virtual VariableReference Clone()
        {
            return new VariableReference
            {
                ArrayIndex = this.ArrayIndex,
                InitialValue = this.InitialValue,
                Reference = this.reference,
                Type = this.Type,
                Name = this.Name
            };
        }
    }

    public class FieldReference : VariableReference
    {
        public VariableReference Instance { get; set; }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Field; }
        }

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Name + "[" + ArrayIndex + "]";
            }
            return Name + ": " + Type.Name;
        }

        public override VariableReference Clone()
        {
            return new FieldReference
            {
                Instance = this.Instance,
                Name =  this.Name,
                Type = this.Type,
                InitialValue = this.InitialValue,
                ArrayIndex = this.ArrayIndex,
                Reference = this.Reference
            };
        }
    }
}