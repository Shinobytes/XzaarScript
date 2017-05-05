using System;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarTypeInfo : XzaarType
    {
        internal XzaarTypeInfo() { }

        public virtual XzaarType AsType()
        {
            return this;
        }

        public virtual XzaarFieldInfo GetDeclaredField(string name)
        {
            return GetField(name);
        }

        public virtual XzaarMethodInfo GetDeclaredMethod(string name)
        {
            return GetMethod(name);
        }
        
        public virtual IEnumerable<XzaarFieldInfo> GetDeclaredFields(string name)
        {
            foreach (XzaarFieldInfo field in GetFields())
            {
                if (field.Name == name)
                    yield return field;
            }
        }

        public virtual IEnumerable<XzaarMethodInfo> GetDeclaredMethods(string name)
        {
            foreach (XzaarMethodInfo method in GetMethods())
            {
                if (method.Name == name)
                    yield return method;
            }
        }
    }
}