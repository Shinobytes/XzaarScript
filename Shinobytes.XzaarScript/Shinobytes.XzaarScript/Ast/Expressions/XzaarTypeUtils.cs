namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public static class XzaarTypeUtils
    {
        internal static bool IsNullableType(this XzaarType type)
        {
            return type == XzaarBaseTypes.Char || type == XzaarBaseTypes.Date || type == XzaarBaseTypes.String || type == XzaarBaseTypes.Any;
        }

        internal static bool IsBool(XzaarType type)
        {
            return GetNonNullableType(type) == XzaarBaseTypes.Boolean;
        }


        internal static XzaarType GetNullableType(XzaarType type)
        {
            // Debug.Assert(type != null, "type cannot be null");            
            return type;
        }


        internal static XzaarType GetNonNullableType(XzaarType type)
        {
            return type;
        }

        internal static bool IsNumeric(XzaarType type)
        {
            type = GetNonNullableType(type);

            switch (XzaarType.GetTypeCode(type))
            {
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }

            return false;
        }

        internal static bool IsInteger(XzaarType type)
        {
            type = GetNonNullableType(type);
            switch (XzaarType.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }


        internal static bool IsArithmetic(XzaarType type)
        {
            type = GetNonNullableType(type);
            //if (!type.IsEnum)
            //{
            switch (XzaarType.GetTypeCode(type))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            //}
            return false;
        }

        internal static bool IsUnsignedInt(XzaarType type)
        {
            type = GetNonNullableType(type);
            //if (!type.IsEnum)
            //{
            switch (XzaarType.GetTypeCode(type))
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            //}
            return false;
        }

        internal static bool IsIntegerOrBool(XzaarType type)
        {
            type = GetNonNullableType(type);
            //if (!type.IsEnum)
            //{
            var xzaarTypeCode = XzaarType.GetTypeCode(type);
            switch (xzaarTypeCode)
            {
                case TypeCode.Int64:
                case TypeCode.Int32:
                case TypeCode.Int16:
                case TypeCode.UInt64:
                case TypeCode.UInt32:
                case TypeCode.UInt16:
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return true;
            }
            //}
            return false;
        }

        public static bool AreReferenceAssignable(XzaarType dest, XzaarType src)
        {
            if (dest.IsAny || dest.IsNumeric && src.IsNumeric) return true;            
            if (AreEquivalent(dest, src))
            {
                return true;
            }
            return false;
        }

        public static bool AreEquivalent(XzaarType t1, XzaarType t2)
        {
            return t1.BaseType == t2 || t2.BaseType == t1 || t1 == t2 || t1.IsEquivalentTo(t2);
        }

        public static bool HasBuiltInEqualityOperator(XzaarType leftType, XzaarType rightType)
        {
            if (!AreEquivalent(leftType, rightType))
            {
                // object can be compared with any type as long as its not a void
                if ((leftType != XzaarBaseTypes.Void && rightType == XzaarBaseTypes.Any)
                    || (rightType != XzaarBaseTypes.Void && leftType == XzaarBaseTypes.Any))
                {
                    return true;
                }

                return false;
            }
            return leftType == XzaarBaseTypes.Boolean || IsNumeric(leftType);
        }

        internal static XzaarMethodInfo GetBooleanOperator(XzaarType type, string name)
        {
            do
            {
                XzaarMethodInfo result = type.GetMethod(name, new XzaarType[] { type });
                if (result != null)
                {
                    return result;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        public static bool HasReferenceEquality(XzaarType leftType, XzaarType rightType)
        {
            return false;
            //if (left.IsValueType || right.IsValueType)
            //{
            //    return false;
            //}

            //// If we have an interface and a reference type then we can do 
            //// reference equality.

            //// If we have two reference types and one is assignable to the
            //// other then we can do reference equality.

            //return left.IsInterface || right.IsInterface ||
            //    AreReferenceAssignable(left, right) ||
            //    AreReferenceAssignable(right, left);            
        }
    }
}