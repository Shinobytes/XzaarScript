using System;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarMethodBase : XzaarMemberInfo
    {
        public abstract XzaarParameterInfo[] GetParameters();

        public abstract object Invoke(object obj);

        public virtual XzaarMethodBody GetMethodBody()
        {
            throw new InvalidOperationException();
        }

        public virtual XzaarType ReturnType { get { throw new NotImplementedException(); } }

        public abstract XzaarMethodAttributes Attributes { get; }

        public bool IsGlobal => (Attributes & XzaarMethodAttributes.Global) != 0;
    }
}