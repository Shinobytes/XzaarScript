using System;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public class XzaarParameterInfo
    {
        private int position;
        private string name;
        private XzaarType parameterType;

        public virtual object DefautlValue { get; }
        public virtual string Name { get { return name; } }
        public virtual XzaarType ParameterType { get { return parameterType; } }
        public virtual int Position { get { return position; } }
    }
}