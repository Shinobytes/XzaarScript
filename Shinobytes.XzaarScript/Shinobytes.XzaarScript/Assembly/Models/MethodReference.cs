namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class MethodReference : MemberReference
    {
        public TypeReference ReturnType { get; internal set; }

        public TypeReference DeclaringType { get; internal set; }

        public ParameterCollection Parameters { get; internal set; }        

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Method; }
        }
    }
}