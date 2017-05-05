using System;
using System.Reflection;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarPropertyInfo : XzaarMemberInfo, IXzaarPropertyInfo
    {
        public abstract XzaarType PropertyType { get; }
        public abstract object GetValue(object obj, object[] index);
        public abstract void SetValue(object obj, object value, object[] index);
        public abstract XzaarMethodInfo GetGetMethod();
        public abstract XzaarMethodInfo GetSetMethod();
    }
}