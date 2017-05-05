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

        //public static bool operator ==(XzaarMemberInfo left, XzaarMemberInfo right)
        //{

        //    if ((object)left == null || (object)right == null)
        //        return false;


        //    if (ReferenceEquals(left, right))
        //        return true;
            

        //    var type1 = left as XzaarType;
        //    var type2 = right as XzaarType;

        //    if ((object)type1 != null && (object)type2 != null)
        //    {
        //        if (type1.Name == type2.Name) return true;
        //    }

        //    var method1 = left as XzaarMethodBase;
        //    var method2 = right as XzaarMethodBase;

        //    if ((object)method1 != null && (object)method2 != null)
        //    {
        //        if (method1.Name == method2.Name && ParameterSequenceMatch(method1, method2) && method1.ReturnType.Name == method2.ReturnType.Name) return true;
        //    }

        //    var field1 = left as XzaarFieldInfo;
        //    var field2 = right as XzaarFieldInfo;

        //    if ((object)field1 != null && (object)field2 != null)
        //    {
        //        if (field1.Name == field2.Name && field1.FieldType.Name == field2.FieldType.Name) return true;
        //    }

        //    var property1 = left as XzaarPropertyInfo;
        //    var property2 = right as XzaarPropertyInfo;

        //    if ((object)property1 != null && (object)property2 != null)
        //    {
        //        if (property1.Name == property2.Name && property1.PropertyType.Name == property2.PropertyType.Name) return true;
        //    }

        //    return false;
        //}

        //private static bool ParameterSequenceMatch(XzaarMethodBase method1, XzaarMethodBase method2)
        //{
        //    var p1 = method1.GetParameters();
        //    var p2 = method2.GetParameters();
        //    if (p1.Length != p2.Length) return false;
        //    for (var i = 0; i < p1.Length; i++)
        //    {
        //        if (p1[i].Name != p2[i].Name || p1[i].ParameterType.Name != p2[i].ParameterType.Name)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public static bool operator !=(XzaarMemberInfo left, XzaarMemberInfo right)
        //{
        //    return !(left == right);
        //}

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}