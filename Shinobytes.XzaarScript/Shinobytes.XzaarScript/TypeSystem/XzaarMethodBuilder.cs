using System;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public sealed class XzaarMethodBuilder : XzaarMethodInfo
    {
        private string name;
        private XzaarType declaringType;
        private XzaarMethodAttributes attributes;

        public override string Name => name;

        public override XzaarType GetXzaarType()
        {
            throw new NotImplementedException();
        }

        public override XzaarType DeclaringType => declaringType;

        public override XzaarParameterInfo[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public override object Invoke(object obj)
        {
            throw new NotImplementedException();
        }

        public override XzaarMethodAttributes Attributes => attributes;

        public override XzaarMethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }
    }
}