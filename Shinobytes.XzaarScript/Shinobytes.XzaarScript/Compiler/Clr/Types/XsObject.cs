namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsObject
    {
        public string Name { get; }

        internal XsObject(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || ((object)this) == obj;
        }

        protected bool Equals(XsObject other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}