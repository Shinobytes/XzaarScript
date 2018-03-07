using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class MemberExpression : XzaarExpression
    {
       
        private XzaarExpression _expression;
        private readonly XzaarExpression arrayIndex;

        public XzaarMemberInfo Member => GetMember();

        public XzaarExpression Expression => _expression;

        internal MemberExpression(XzaarExpression expression, XzaarExpression arrayIndex, XzaarType memberType)
        {
            MemberType = memberType;
            _expression = expression;
            this.arrayIndex = arrayIndex;
        }

        //internal static MemberExpression Make(XzaarExpression expression, XzaarMemberInfo member)
        //{
        //    if (member.MemberType == XzaarMemberTypes.Field)
        //    {
        //        XzaarFieldInfo fi = (XzaarFieldInfo)member;
        //        return new FieldExpression(expression, fi);
        //    }
        //    else
        //    {
        //        XzaarPropertyInfo pi = (XzaarPropertyInfo)member;
        //        return new PropertyExpression(expression, pi);
        //    }
        //}

        public XzaarType MemberType { get; set; }

        public XzaarExpression ArrayIndex => arrayIndex;

        public sealed override ExpressionType NodeType => ExpressionType.MemberAccess;

        internal virtual XzaarMemberInfo GetMember()
        {
            return null;
            // throw new InvalidOperationException();
        }
        //public MemberExpression Update(XzaarExpression expression)
        //{
        //    if (expression == Expression)
        //    {
        //        return this;
        //    }
        //    return XzaarExpression.MakeMemberAccess(expression, Member);
        //}
        public override XzaarType Type => TryGetType(Expression);

        private XzaarType TryGetType(XzaarExpression expression)
        {
            if (this.Member != null)
            {
                return this.Member.GetXzaarType();
            }

            var paramExpr = expression as ParameterExpression;
            if (paramExpr != null)
            {
                return paramExpr.Type;
            }

            return XzaarBaseTypes.Any;
        }
    }

    //internal class FieldExpression : MemberExpression
    //{
    //    private readonly XzaarFieldInfo _field;

    //    public FieldExpression(XzaarExpression expression, XzaarFieldInfo member)
    //        : base(expression, null)
    //    {
    //        _field = member;
    //    }

    //    internal override XzaarMemberInfo GetMember()
    //    {
    //        return _field;
    //    }

    //    public sealed override XzaarType Type
    //    {
    //        get { return _field.FieldType; }
    //    }
    //}

    //internal class PropertyExpression : MemberExpression
    //{
    //    private readonly XzaarPropertyInfo _property;
    //    public PropertyExpression(XzaarExpression expression, XzaarPropertyInfo member)
    //        : base(expression, null)
    //    {
    //        _property = member;
    //    }

    //    internal override XzaarMemberInfo GetMember()
    //    {
    //        return _property;
    //    }

    //    public sealed override XzaarType Type
    //    {
    //        get { return _property.PropertyType; }
    //    }
    //}
    public partial class XzaarExpression
    {

        #region Field

        public static MemberExpression Field(XzaarExpression expression, XzaarFieldInfo field)
        {
            //ContractUtils.RequiresNotNull(field, "field");

            //if (field.IsStatic)
            //{
            //    if (expression != null) throw new ArgumentException(Strings.OnlyStaticFieldsHaveNullInstance, "expression");
            //}
            //else
            //{
            //    if (expression == null) throw new ArgumentException(Strings.OnlyStaticFieldsHaveNullInstance, "field");
            //    RequiresCanRead(expression, "expression");
            //    if (!TypeUtils.AreReferenceAssignable(field.DeclaringType, expression.Type))
            //    {
            //        throw Error.FieldInfoNotDefinedForType(field.DeclaringType, field.Name, expression.Type);
            //    }
            //}
            throw new NotImplementedException();
            // return MemberExpression.Make(expression, field);
        }

        public static MemberExpression Field(XzaarExpression expression, string fieldName)
        {
            throw new NotImplementedException();
            //RequiresCanRead(expression, "expression");

            //// bind to public names first
            //FieldInfo fi = expression.Type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (fi == null)
            //{
            //    fi = expression.Type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //}
            //if (fi == null)
            //{
            //    throw Error.InstanceFieldNotDefinedForType(fieldName, expression.Type);
            //}
            //return XzaarExpression.Field(expression, fi);
        }

        public static MemberExpression Field(XzaarExpression expression, XzaarType type, string fieldName)
        {
            throw new NotImplementedException();
            //ContractUtils.RequiresNotNull(type, "type");

            //// bind to public names first
            //FieldInfo fi = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (fi == null)
            //{
            //    fi = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //}

            //if (fi == null)
            //{
            //    throw Error.FieldNotDefinedForType(fieldName, type);
            //}
            //return Expression.Field(expression, fi);
        }
        #endregion

        #region Property

        public static MemberExpression Property(XzaarExpression expression, string propertyName)
        {
            throw new NotImplementedException();
            //RequiresCanRead(expression, "expression");
            //ContractUtils.RequiresNotNull(propertyName, "propertyName");
            //// bind to public names first
            //PropertyInfo pi = expression.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (pi == null)
            //{
            //    pi = expression.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //}
            //if (pi == null)
            //{
            //    throw Error.InstancePropertyNotDefinedForType(propertyName, expression.Type);
            //}
            //return Property(expression, pi);
        }


        public static MemberExpression Property(XzaarExpression expression, XzaarType type, string propertyName)
        {
            throw new NotImplementedException();
            //ContractUtils.RequiresNotNull(type, "type");
            //ContractUtils.RequiresNotNull(propertyName, "propertyName");
            //// bind to public names first
            //PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (pi == null)
            //{
            //    pi = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //}
            //if (pi == null)
            //{
            //    throw Error.PropertyNotDefinedForType(propertyName, type);
            //}
            //return Property(expression, pi);
        }

        public static MemberExpression Property(XzaarExpression expression, XzaarPropertyInfo property)
        {
            throw new NotImplementedException();
            //ContractUtils.RequiresNotNull(property, "property");

            //MethodInfo mi = property.GetGetMethod(true) ?? property.GetSetMethod(true);

            //if (mi == null)
            //{
            //    throw Error.PropertyDoesNotHaveAccessor(property);
            //}

            //if (mi.IsStatic)
            //{
            //    if (expression != null) throw new ArgumentException(Strings.OnlyStaticPropertiesHaveNullInstance, "expression");
            //}
            //else
            //{
            //    if (expression == null) throw new ArgumentException(Strings.OnlyStaticPropertiesHaveNullInstance, "property");
            //    RequiresCanRead(expression, "expression");
            //    if (!TypeUtils.IsValidInstanceType(property, expression.Type))
            //    {
            //        throw Error.PropertyNotDefinedForType(property, expression.Type);
            //    }
            //}
            //return MemberExpression.Make(expression, property);
        }


        public static MemberExpression Property(XzaarExpression expression, XzaarMethodInfo propertyAccessor)
        {
            //ContractUtils.RequiresNotNull(propertyAccessor, "propertyAccessor");
            //ValidateMethodInfo(propertyAccessor);
            return Property(expression, GetProperty(propertyAccessor));
        }

        private static XzaarPropertyInfo GetProperty(XzaarMethodInfo mi)
        {
            throw new NotImplementedException();
            //Type type = mi.DeclaringType;
            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic;
            //flags |= (mi.IsStatic) ? BindingFlags.Static : BindingFlags.Instance;
            //PropertyInfo[] props = type.GetProperties(flags);
            //foreach (PropertyInfo pi in props)
            //{
            //    if (pi.CanRead && CheckMethod(mi, pi.GetGetMethod(true)))
            //    {
            //        return pi;
            //    }
            //    if (pi.CanWrite && CheckMethod(mi, pi.GetSetMethod(true)))
            //    {
            //        return pi;
            //    }
            //}
            //throw Error.MethodNotPropertyAccessor(mi.DeclaringType, mi.Name);
        }

        private static bool CheckMethod(XzaarMethodInfo method, XzaarMethodInfo propertyMethod)
        {
            if (method == propertyMethod)
            {
                return true;
            }
            // If the type is an interface then the handle for the method got by the compiler will not be the 
            // same as that returned by reflection.
            // Check for this condition and try and get the method from reflection.
            XzaarType type = method.DeclaringType;
            if (method.Name == propertyMethod.Name && type.GetMethod(method.Name) == propertyMethod)
            {
                return true;
            }
            return false;
        }

        #endregion

        /// <returns>The created <see cref="MemberExpression"/>.</returns>
        public static MemberExpression PropertyOrField(XzaarExpression expression, string propertyOrFieldName)
        {
            throw new NotImplementedException();
            //RequiresCanRead(expression, "expression");
            //// bind to public names first
            //PropertyInfo pi = expression.Type.GetProperty(propertyOrFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (pi != null)
            //    return Property(expression, pi);
            //FieldInfo fi = expression.Type.GetField(propertyOrFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (fi != null)
            //    return Field(expression, fi);
            //pi = expression.Type.GetProperty(propertyOrFieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (pi != null)
            //    return Property(expression, pi);
            //fi = expression.Type.GetField(propertyOrFieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy);
            //if (fi != null)
            //    return Field(expression, fi);

            //throw Error.NotAMemberOfType(propertyOrFieldName, expression.Type);
        }

        public static MemberExpression MemberAccess(XzaarExpression expression, XzaarType memberType)
        {
            return new MemberExpression(expression, null, memberType);
        }

        public static MemberExpression MemberAccess(XzaarExpression expression, XzaarExpression arrayIndex, XzaarType memberType)
        {
            return new MemberExpression(expression, arrayIndex, memberType);
        }


        //public static MemberExpression MakeMemberAccess(XzaarExpression expression, XzaarMemberInfo member)
        //{
        //    throw new NotImplementedException();
        //    //ContractUtils.RequiresNotNull(member, "member");

        //    //FieldInfo fi = member as FieldInfo;
        //    //if (fi != null)
        //    //{
        //    //    return Expression.Field(expression, fi);
        //    //}
        //    //PropertyInfo pi = member as PropertyInfo;
        //    //if (pi != null)
        //    //{
        //    //    return Expression.Property(expression, pi);
        //    //}
        //    //throw Error.MemberNotFieldOrProperty(member);
        //}
    }
}