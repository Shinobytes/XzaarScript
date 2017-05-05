using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Shinobytes.XzaarScript.Scripting;
using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript
{

    [Serializable]
    public abstract class XzaarType : XzaarMemberInfo
    {
        public static readonly char Delimiter = '.';

        public virtual XzaarMethodBase DeclaringMethod
        {
            get { return null; }
        }

        public abstract XzaarMethodInfo[] GetMethods();
        //public abstract XzaarPropertyInfo[] GetProperties();
        public abstract XzaarFieldInfo[] GetFields();
        public abstract XzaarMethodInfo GetMethod(string name);

        public abstract XzaarFieldInfo GetField(string name);
        //public abstract XzaarPropertyInfo GetProperty(string name);

        protected abstract bool IsArrayImpl();

        public abstract XzaarType UnderlyingSystemType { get; }

        protected abstract XzaarMethodInfo GetMethodImp(string name, XzaarType[] types);

        public abstract XzaarType GetElementType();

        public abstract XzaarType BaseType { get; }

        internal XzaarMethodInfo GetMethod(string name, XzaarType[] types)
        {
            if (types == null) throw new ArgumentNullException("types");
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == null)
                    throw new ArgumentNullException("types");
            }
            return GetMethodImp(name, types);
        }

        public static XzaarType GetType(string name)
        {
            var xt = XzaarBaseTypes.BaseTypes.FirstOrDefault(t => t.Name == name);
            if ((object)xt != null) return xt;
            if (!string.IsNullOrEmpty(name))
            {
                switch (name.ToLower())
                {
                    case "void":
                        return XzaarBaseTypes.Void;
                    case "number":
                    case "int":
                    case "float":
                    case "byte":
                    case "short":
                    case "double":
                    case "long":
                        return XzaarBaseTypes.Number;
                }
            }
            // return (XzaarType)typeof(void);
            return null;
        }

        public static XzaarTypeCode GetTypeCode(XzaarType type)
        {
            if (type == null) return XzaarTypeCode.Empty;
            return type.GetTypeCodeImpl();
        }

        private XzaarTypeCode GetTypeCodeImpl()
        {
            switch (this.Name)
            {
                case "number":
                    return XzaarTypeCode.Int32;
                case "string":
                    return XzaarTypeCode.String;
                case "object": case "any":
                    return XzaarTypeCode.Any;
                case "datetime":
                    return XzaarTypeCode.DateTime;
            }
            if (this != UnderlyingSystemType && UnderlyingSystemType != null)
                return XzaarType.GetTypeCode(UnderlyingSystemType);
            return XzaarTypeCode.Any;
        }

        public bool IsEquivalentTo(XzaarType other)
        {
            return (this == other);
        }

        public virtual bool Equals(Type t)
        {
            if (t == null) return false;
            return XzaarBaseTypes.Typeof(t) == this;
        }


        public override bool Equals(object o)
        {
            if ((object)o == null)
                return false;

            return Equals(o as XzaarType);
        }

        public virtual bool Equals(XzaarType o)
        {
            if ((object)o == null)
                return false;

            if (o.Name == this.Name)
            {
                return true;
                // for now we wont expect multiple types with the same name since we don't support namespaces just yet. otherwise we could do o.FullName == ..
            }

            return (Object.ReferenceEquals(this.UnderlyingSystemType, o.UnderlyingSystemType));
        }

        public bool IsArray
        {
            get { return IsArrayImpl(); }
        }

        public bool IsByRef
        {
            get { return IsByRefImpl(); }
        }

        protected abstract bool IsByRefImpl();

        public override int GetHashCode()
        {
            XzaarType SystemType = UnderlyingSystemType;
            if (!Object.ReferenceEquals(SystemType, this))
                return SystemType.GetHashCode();
            return base.GetHashCode();
        }

        public static explicit operator XzaarType(Type t)
        {
            return XzaarBaseTypes.Typeof(t);
        }

        //public static explicit operator XzaarType(Type t)
        //{
        //    return XzaarBaseTypes.Typeof(t);
        //}


        public override string ToString()
        {
            return this.Name;

        }

        public static bool TryGetType(string typeName, StructExpression typeExpression, out XzaarType newType)
        {
            var a = GetType(typeName);
            if (a == null || Equals(a, XzaarBaseTypes.Void) || Equals(a, XzaarBaseTypes.Any))
            {
                newType = XzaarBaseTypes.CreateTypeFromStructExpression(typeExpression);
                XzaarBaseTypes.AddTypeToCache(newType);
            }
            else
            {
                newType = a;
            }
            return true;
        }
    }
}
