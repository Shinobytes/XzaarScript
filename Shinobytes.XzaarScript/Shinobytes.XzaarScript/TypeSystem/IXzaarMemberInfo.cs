namespace Shinobytes.XzaarScript
{
    public interface IXzaarMemberInfo
    {        
        string ToString();
        XzaarType GetXzaarType();
        XzaarMemberTypes MemberType { get; }
        XzaarType DeclaringType { get; }
    }
}