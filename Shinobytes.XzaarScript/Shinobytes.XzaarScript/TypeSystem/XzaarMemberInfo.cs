using System;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarMemberInfo : IXzaarMemberInfo
    {
        public abstract string Name { get; }

        public abstract XzaarType GetXzaarType();

        public abstract XzaarMemberTypes MemberType { get; }

        public abstract XzaarType DeclaringType { get; }        
    }
}