using System;
using System.Reflection;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarFieldInfo : XzaarMemberInfo, IXzaarFieldInfo
    {
        public abstract XzaarType FieldType { get; }
        public abstract object GetValue(object obj);
        public abstract void SetValue(object obj, object value);
    }
}