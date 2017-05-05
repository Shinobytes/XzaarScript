using System;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarMethodInfo : XzaarMethodBase
    {
        protected XzaarMethodInfo() { }

        public override XzaarMemberTypes MemberType { get { return XzaarMemberTypes.Method; } }

        public virtual XzaarParameterInfo ReturnParameter { get { throw new NotImplementedException(); } }

        public abstract XzaarMethodInfo GetBaseDefinition();
    }
}