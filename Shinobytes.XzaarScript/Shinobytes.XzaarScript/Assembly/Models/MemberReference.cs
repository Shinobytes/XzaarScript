namespace Shinobytes.XzaarScript.Assembly.Models
{

    public enum MemberTypes
    {
        Field,
        Property,
        Variable,
        Method,
        Parameter,
        Struct,
        Class,
        Label,

        Constant
    }

    public abstract class MemberReference
    {
        public string Name { get; set; }

        public abstract MemberTypes MemberType { get; }
    }
}