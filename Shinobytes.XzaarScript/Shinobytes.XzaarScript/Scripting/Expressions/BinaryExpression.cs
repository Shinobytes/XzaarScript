using System;
using System.Linq.Expressions;

namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    [Serializable]
    public abstract class BinaryExpression : XzaarExpression
    {
        private readonly XzaarExpression left;
        private readonly XzaarExpression right;

        internal BinaryExpression(XzaarExpression left, XzaarExpression right)
        {
            this.left = left;
            this.right = right;
        }

        private static bool IsOpAssignment(XzaarExpressionType op)
        {
            switch (op)
            {
                case XzaarExpressionType.AddAssign:
                case XzaarExpressionType.SubtractAssign:
                case XzaarExpressionType.MultiplyAssign:
                case XzaarExpressionType.AddAssignChecked:
                case XzaarExpressionType.SubtractAssignChecked:
                case XzaarExpressionType.MultiplyAssignChecked:
                case XzaarExpressionType.DivideAssign:
                case XzaarExpressionType.ModuloAssign:
                case XzaarExpressionType.PowerAssign:
                case XzaarExpressionType.AndAssign:
                case XzaarExpressionType.OrAssign:
                case XzaarExpressionType.RightShiftAssign:
                case XzaarExpressionType.LeftShiftAssign:
                case XzaarExpressionType.ExclusiveOrAssign:
                    return true;
            }
            return false;
        }

        private static XzaarExpressionType GetBinaryOpFromAssignmentOp(XzaarExpressionType op)
        {
            switch (op)
            {
                case XzaarExpressionType.AddAssign:
                    return XzaarExpressionType.Add;
                case XzaarExpressionType.AddAssignChecked:
                    return XzaarExpressionType.AddChecked;
                case XzaarExpressionType.SubtractAssign:
                    return XzaarExpressionType.Subtract;
                case XzaarExpressionType.SubtractAssignChecked:
                    return XzaarExpressionType.SubtractChecked;
                case XzaarExpressionType.MultiplyAssign:
                    return XzaarExpressionType.Multiply;
                case XzaarExpressionType.MultiplyAssignChecked:
                    return XzaarExpressionType.MultiplyChecked;
                case XzaarExpressionType.DivideAssign:
                    return XzaarExpressionType.Divide;
                case XzaarExpressionType.ModuloAssign:
                    return XzaarExpressionType.Modulo;
                case XzaarExpressionType.PowerAssign:
                    return XzaarExpressionType.Power;
                case XzaarExpressionType.AndAssign:
                    return XzaarExpressionType.And;
                case XzaarExpressionType.OrAssign:
                    return XzaarExpressionType.Or;
                case XzaarExpressionType.RightShiftAssign:
                    return XzaarExpressionType.RightShift;
                case XzaarExpressionType.LeftShiftAssign:
                    return XzaarExpressionType.LeftShift;
                case XzaarExpressionType.ExclusiveOrAssign:
                    return XzaarExpressionType.ExclusiveOr;
                default:
                    throw new InvalidOperationException("op");
            }

        }

        public XzaarExpression Left
        {
            get { return left; }
        }

        public XzaarExpression Right
        {
            get { return right; }
        }

        public XzaarMethodInfo Method
        {
            get { return GetMethod(); }
        }

        internal virtual XzaarMethodInfo GetMethod()
        {
            return null;
        }

        public BinaryExpression Update(XzaarExpression left, XzaarExpression right)
        {
            if (left == Left && right == Right)
            {
                return this;
            }
            if (IsReferenceComparison)
            {
                if (NodeType == XzaarExpressionType.Equal)
                {
                    return XzaarExpression.ReferenceEqual(left, right);
                }
                else
                {
                    return XzaarExpression.ReferenceNotEqual(left, right);
                }
            }
            return XzaarExpression.MakeBinary(NodeType, left, right, IsLiftedToNull, Method);
        }

        public bool IsLifted
        {
            get
            {
                if (NodeType == XzaarExpressionType.Coalesce || NodeType == XzaarExpressionType.Assign)
                {
                    return false;
                }
                if (XzaarTypeUtils.IsNullableType(left.Type))
                {
                    XzaarMethodInfo method = GetMethod();
                    return method == null;
                    // || !XzaarTypeUtils.AreEquivalent(method.GetParameters()[0].ParameterType.GetNonRefType(), left.Type);
                }
                return false;
            }
        }
        public bool IsLiftedToNull
        {
            get
            {
                return IsLifted && XzaarTypeUtils.IsNullableType(Type);
            }
        }

        internal bool IsReferenceComparison
        {
            get
            {
                //XzaarType left = _left.Type;
                //XzaarType right = _right.Type;
                //XzaarMethodInfo method = GetMethod();
                //XzaarExpressionType kind = NodeType;

                return false;
                //return (kind == XzaarExpressionType.Equal || kind == XzaarExpressionType.NotEqual) &&
                //    method == null && !left.IsValueType && !right.IsValueType;
            }
        }
    }

    public partial class XzaarExpression
    {
        public static BinaryExpression ReferenceNotEqual(XzaarExpression left, XzaarExpression right)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (XzaarTypeUtils.HasReferenceEquality(left.Type, right.Type))
            {
                return new LogicalBinaryExpression(XzaarExpressionType.NotEqual, left, right);
            }
            // throw Error.ReferenceEqualityNotDefined(left.Type, right.Type);
            throw new InvalidOperationException("Reference equality not defined");
        }


        public static BinaryExpression ReferenceEqual(XzaarExpression left, XzaarExpression right)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (XzaarTypeUtils.HasReferenceEquality(left.Type, right.Type))
            {
                return new LogicalBinaryExpression(XzaarExpressionType.Equal, left, right);
            }
            // throw Error.ReferenceEqualityNotDefined(left.Type, right.Type);
            throw new InvalidOperationException("Reference equality not defined");
        }

        public static BinaryExpression Assign(XzaarExpression left, XzaarExpression right)
        {
            return new AssignBinaryExpression(left, right);
        }

        public static BinaryExpression MakeBinary(XzaarExpressionType binaryType, XzaarExpression left, XzaarExpression right)
        {
            return MakeBinary(binaryType, left, right, false, null);
        }

        public static BinaryExpression MakeBinary(XzaarExpressionType binaryType, XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            switch (binaryType)
            {
                case XzaarExpressionType.Add: return Add(left, right, method);
                case XzaarExpressionType.AddChecked: return AddChecked(left, right, method);
                case XzaarExpressionType.Subtract: return Subtract(left, right, method);
                case XzaarExpressionType.SubtractChecked: return SubtractChecked(left, right, method);
                case XzaarExpressionType.Multiply: return Multiply(left, right, method);
                case XzaarExpressionType.MultiplyChecked: return MultiplyChecked(left, right, method);
                case XzaarExpressionType.Divide: return Divide(left, right, method);
                case XzaarExpressionType.Modulo: return Modulo(left, right, method);
                case XzaarExpressionType.Power: return Power(left, right, method);
                case XzaarExpressionType.And: return And(left, right, method);
                case XzaarExpressionType.AndAlso: return AndAlso(left, right, method);
                case XzaarExpressionType.Or: return Or(left, right, method);
                case XzaarExpressionType.OrElse: return OrElse(left, right, method);
                case XzaarExpressionType.LessThan: return LessThan(left, right, liftToNull, method);
                case XzaarExpressionType.LessThanOrEqual: return LessThanOrEqual(left, right, liftToNull, method);
                case XzaarExpressionType.GreaterThan: return GreaterThan(left, right, liftToNull, method);
                case XzaarExpressionType.GreaterThanOrEqual: return GreaterThanOrEqual(left, right, liftToNull, method);
                case XzaarExpressionType.Equal: return Equal(left, right, liftToNull, method);
                case XzaarExpressionType.NotEqual: return NotEqual(left, right, liftToNull, method);
                case XzaarExpressionType.ExclusiveOr: return ExclusiveOr(left, right, method);
                case XzaarExpressionType.Coalesce: return Coalesce(left, right);
                case XzaarExpressionType.ArrayIndex: return ArrayIndex(left, right);
                case XzaarExpressionType.RightShift: return RightShift(left, right, method);
                case XzaarExpressionType.LeftShift: return LeftShift(left, right, method);
                case XzaarExpressionType.Assign: return Assign(left, right);
                case XzaarExpressionType.AddAssign: return AddAssign(left, right, method);
                case XzaarExpressionType.AndAssign: return AndAssign(left, right, method);
                case XzaarExpressionType.DivideAssign: return DivideAssign(left, right, method);
                case XzaarExpressionType.ExclusiveOrAssign: return ExclusiveOrAssign(left, right, method);
                case XzaarExpressionType.LeftShiftAssign: return LeftShiftAssign(left, right, method);
                case XzaarExpressionType.ModuloAssign: return ModuloAssign(left, right, method);
                case XzaarExpressionType.MultiplyAssign: return MultiplyAssign(left, right, method);
                case XzaarExpressionType.OrAssign: return OrAssign(left, right, method);
                case XzaarExpressionType.PowerAssign: return PowerAssign(left, right, method);
                case XzaarExpressionType.RightShiftAssign: return RightShiftAssign(left, right, method);
                case XzaarExpressionType.SubtractAssign: return SubtractAssign(left, right, method);
                case XzaarExpressionType.AddAssignChecked: return AddAssignChecked(left, right, method);
                case XzaarExpressionType.SubtractAssignChecked: return SubtractAssignChecked(left, right, method);
                case XzaarExpressionType.MultiplyAssignChecked: return MultiplyAssignChecked(left, right, method);
                default: throw new NotSupportedException(binaryType + " is not supported."); // throw Error.UnhandledBinary(binaryType);
            }
        }
        public static BinaryExpression RightShiftAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (IsSimpleShift(left.Type, right.Type))
                {
                    XzaarType resultType = GetResultTypeOfShift(left.Type, right.Type);
                    return new SimpleBinaryExpression(XzaarExpressionType.RightShiftAssign, left, right, resultType);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.RightShiftAssign, "op_RightShift", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.RightShiftAssign, left, right, method, true);
        }
        public static BinaryExpression AddAssignChecked(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");

            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    // conversion is not supported for binary ops on arithmetic types without operator overloading                   
                    return new SimpleBinaryExpression(XzaarExpressionType.AddAssignChecked, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.AddAssignChecked, "op_Addition", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.AddAssignChecked, left, right, method, true);
        }
        public static BinaryExpression ExclusiveOrAssign(XzaarExpression left, XzaarExpression right)
        {
            return ExclusiveOrAssign(left, right, null);
        }
        public static BinaryExpression ExclusiveOrAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.ExclusiveOrAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.ExclusiveOrAssign, "op_ExclusiveOr", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.ExclusiveOrAssign, left, right, method, true);
        }
        public static BinaryExpression LeftShiftAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (IsSimpleShift(left.Type, right.Type))
                {
                    XzaarType resultType = GetResultTypeOfShift(left.Type, right.Type);
                    return new SimpleBinaryExpression(XzaarExpressionType.LeftShiftAssign, left, right, resultType);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.LeftShiftAssign, "op_LeftShift", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.LeftShiftAssign, left, right, method, true);
        }

        public static BinaryExpression RightShift(XzaarExpression left, XzaarExpression right)
        {
            return RightShift(left, right, null);
        }
        public static BinaryExpression RightShift(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (IsSimpleShift(left.Type, right.Type))
                {
                    XzaarType resultType = GetResultTypeOfShift(left.Type, right.Type);
                    return new SimpleBinaryExpression(XzaarExpressionType.RightShift, left, right, resultType);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.RightShift, "op_RightShift", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.RightShift, left, right, method, true);
        }
        public static BinaryExpression LeftShift(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (IsSimpleShift(left.Type, right.Type))
                {
                    XzaarType resultType = GetResultTypeOfShift(left.Type, right.Type);
                    return new SimpleBinaryExpression(XzaarExpressionType.LeftShift, left, right, resultType);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.LeftShift, "op_LeftShift", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.LeftShift, left, right, method, true);
        }
        private static XzaarType ValidateCoalesceArgTypes(XzaarType left, XzaarType right)
        {
            return left;
        }
        public static BinaryExpression Coalesce(XzaarExpression left, XzaarExpression right)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");

            //if (conversion == null)
            //{
            XzaarType resultType = ValidateCoalesceArgTypes(left.Type, right.Type);
            return new SimpleBinaryExpression(XzaarExpressionType.Coalesce, left, right, resultType);
            // }

            //if (left.Type.IsValueType && !XzaarTypeUtils.IsNullableType(left.Type))
            //{
            //    throw Error.CoalesceUsedOnNonNullType();
            //}

            // XzaarType delegateType = conversion.Type;
            //Debug.Assert(typeof(System.MulticastDelegate).IsAssignableFrom(delegateType) && delegateType != typeof(System.MulticastDelegate));
            //MethodInfo method = delegateType.GetMethod("Invoke");
            //if (method.ReturnType == typeof(void))
            //{
            //    throw Error.UserDefinedOperatorMustNotBeVoid(conversion);
            //}
            //ParameterInfo[] pms = method.GetParametersCached();
            //Debug.Assert(pms.Length == conversion.Parameters.Count);
            //if (pms.Length != 1)
            //{
            //    throw Error.IncorrectNumberOfMethodCallArguments(conversion);
            //}
            //// The return type must match exactly.
            //// 


            //if (!TypeUtils.AreEquivalent(method.ReturnType, right.Type))
            //{
            //    throw Error.OperandTypesDoNotMatchParameters(ExpressionType.Coalesce, conversion.ToString());
            //}
            //// The parameter of the conversion lambda must either be assignable
            //// from the erased or unerased type of the left hand side.
            //if (!ParameterIsAssignable(pms[0], TypeUtils.GetNonNullableType(left.Type)) &&
            //    !ParameterIsAssignable(pms[0], left.Type))
            //{
            //    throw Error.OperandTypesDoNotMatchParameters(ExpressionType.Coalesce, conversion.ToString());
            //}
            //return new CoalesceConversionBinaryExpression(left, right, conversion);
        }


        public static BinaryExpression LessThanOrEqual(XzaarExpression left, XzaarExpression right)
        {
            return LessThanOrEqual(left, right, false, null);
        }
        public static BinaryExpression ExclusiveOr(XzaarExpression left, XzaarExpression right)
        {
            return ExclusiveOr(left, right, null);
        }
        public static BinaryExpression ExclusiveOr(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.ExclusiveOr, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.ExclusiveOr, "op_ExclusiveOr", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.ExclusiveOr, left, right, method, true);
        }


        public static BinaryExpression NotEqual(XzaarExpression left, XzaarExpression right)
        {
            return Equal(left, right, false, null);
        }

        public static BinaryExpression NotEqual(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetEqualityComparisonOperator(XzaarExpressionType.NotEqual, "op_Inequality", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.NotEqual, left, right, method, liftToNull);
        }

        public static BinaryExpression LessThanOrEqual(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetComparisonOperator(XzaarExpressionType.LessThanOrEqual, "op_LessThanOrEqual", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.LessThanOrEqual, left, right, method, liftToNull);
        }

        public static BinaryExpression GreaterThan(XzaarExpression left, XzaarExpression right)
        {
            return GreaterThan(left, right, false, null);
        }

        public static BinaryExpression GreaterThan(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetComparisonOperator(XzaarExpressionType.GreaterThan, "op_GreaterThan", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.GreaterThan, left, right, method, liftToNull);
        }

        public static BinaryExpression GreaterThanOrEqual(XzaarExpression left, XzaarExpression right)
        {
            return GreaterThanOrEqual(left, right, false, null);
        }
        public static BinaryExpression GreaterThanOrEqual(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetComparisonOperator(XzaarExpressionType.GreaterThanOrEqual, "op_GreaterThanOrEqual", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.GreaterThanOrEqual, left, right, method, liftToNull);
        }


        public static BinaryExpression OrElse(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            XzaarType returnType;
            if (method == null)
            {
                if (left.Type == right.Type)
                {
                    if (left.Type == XzaarBaseTypes.Boolean)
                    {
                        return new LogicalBinaryExpression(XzaarExpressionType.OrElse, left, right);
                    }
                    else // if (left.Type == typeof(bool?))
                    {
                        return new SimpleBinaryExpression(XzaarExpressionType.OrElse, left, right, left.Type);
                    }
                }
                method = GetUserDefinedBinaryOperator(XzaarExpressionType.OrElse, left.Type, right.Type, "op_BitwiseOr");
                if (method != null)
                {
                    ValidateUserDefinedConditionalLogicOperator(XzaarExpressionType.OrElse, left.Type, right.Type, method);
                    returnType = (XzaarTypeUtils.IsNullableType(left.Type) && method.ReturnType == XzaarTypeUtils.GetNonNullableType(left.Type)) ? left.Type : method.ReturnType;
                    return new MethodBinaryExpression(XzaarExpressionType.OrElse, left, right, returnType, method);
                }
                throw new InvalidOperationException("Binary operator not defined");
                // throw Error.BinaryOperatorNotDefined(XzaarExpressionType.OrElse, left.Type, right.Type);
            }
            ValidateUserDefinedConditionalLogicOperator(XzaarExpressionType.OrElse, left.Type, right.Type, method);
            returnType = (XzaarTypeUtils.IsNullableType(left.Type) && method.ReturnType == XzaarTypeUtils.GetNonNullableType(left.Type)) ? left.Type : method.ReturnType;
            return new MethodBinaryExpression(XzaarExpressionType.OrElse, left, right, returnType, method);
        }

        public static BinaryExpression LessThan(XzaarExpression left, XzaarExpression right)
        {
            return LessThan(left, right, false, null);
        }

        public static BinaryExpression LessThan(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetComparisonOperator(XzaarExpressionType.LessThan, "op_LessThan", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.LessThan, left, right, method, liftToNull);
        }



        public static BinaryExpression AndAlso(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            XzaarType returnType;
            if (method == null)
            {
                if (left.Type == right.Type)
                {
                    if (left.Type == XzaarBaseTypes.Boolean)
                    {
                        return new LogicalBinaryExpression(XzaarExpressionType.AndAlso, left, right);
                    }
                    else // if (left.Type == typeof(bool?))
                    {
                        return new SimpleBinaryExpression(XzaarExpressionType.AndAlso, left, right, left.Type);
                    }
                }
                method = GetUserDefinedBinaryOperator(XzaarExpressionType.AndAlso, left.Type, right.Type, "op_BitwiseAnd");
                if (method != null)
                {
                    ValidateUserDefinedConditionalLogicOperator(XzaarExpressionType.AndAlso, left.Type, right.Type, method);
                    returnType = (XzaarTypeUtils.IsNullableType(left.Type) && XzaarTypeUtils.AreEquivalent(method.ReturnType, XzaarTypeUtils.GetNonNullableType(left.Type))) ? left.Type : method.ReturnType;
                    return new MethodBinaryExpression(XzaarExpressionType.AndAlso, left, right, returnType, method);
                }
                // throw Error.BinaryOperatorNotDefined(ExpressionType.AndAlso, left.Type, right.Type);
                throw new InvalidOperationException("Binary operation not defined");
            }
            ValidateUserDefinedConditionalLogicOperator(XzaarExpressionType.AndAlso, left.Type, right.Type, method);
            returnType = (XzaarTypeUtils.IsNullableType(left.Type) && XzaarTypeUtils.AreEquivalent(method.ReturnType, XzaarTypeUtils.GetNonNullableType(left.Type))) ? left.Type : method.ReturnType;
            return new MethodBinaryExpression(XzaarExpressionType.AndAlso, left, right, returnType, method);
        }

        public static BinaryExpression And(XzaarExpression left, XzaarExpression right)
        {
            return And(left, right, null);
        }

        public static BinaryExpression And(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.And, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.And, "op_BitwiseAnd", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.And, left, right, method, true);
        }

        public static BinaryExpression AndAssign(XzaarExpression left, XzaarExpression right)
        {
            return AndAssign(left, right, null);
        }
        public static BinaryExpression AndAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.AndAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.AndAssign, "op_BitwiseAnd", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.AndAssign, left, right, method, true);
        }
        public static BinaryExpression OrAssign(XzaarExpression left, XzaarExpression right)
        {
            return OrAssign(left, right, null);
        }
        public static BinaryExpression OrAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.OrAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.OrAssign, "op_BitwiseOr", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.OrAssign, left, right, method, true);
        }
        public static BinaryExpression Or(XzaarExpression left, XzaarExpression right)
        {
            return Or(left, right, null);
        }
        public static BinaryExpression Or(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.Or, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.Or, "op_BitwiseOr", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Or, left, right, method, true);
        }
        public static BinaryExpression Power(XzaarExpression left, XzaarExpression right)
        {
            return Power(left, right, null);
        }
        public static BinaryExpression Power(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                //Type mathType = typeof(System.Math);
                //method = mathType.GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
                //if (method == null)
                //{
                //    throw Error.BinaryOperatorNotDefined(XzaarExpressionType.Power, left.Type, right.Type);
                //}
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Power, left, right, method, true);
        }
        public static BinaryExpression PowerAssign(XzaarExpression left, XzaarExpression right)
        {
            return PowerAssign(left, right, null);
        }
        public static BinaryExpression PowerAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                //Type mathType = typeof(System.Math);
                //method = mathType.GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
                //if (method == null)
                //{
                //    throw Error.BinaryOperatorNotDefined(ExpressionType.PowerAssign, left.Type, right.Type);
                //}
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.PowerAssign, left, right, method, true);
        }
        public static BinaryExpression ArrayIndex(XzaarExpression array, XzaarExpression index)
        {
            RequiresCanRead(array, "array");
            RequiresCanRead(index, "index");
            if (index.Type != XzaarBaseTypes.Number)
            {
                // throw Error.ArgumentMustBeArrayIndexType();
                throw new InvalidOperationException("Argument must be array index type");
            }

            XzaarType arrayType = array.Type;
            if (!arrayType.IsArray)
            {
                // throw Error.ArgumentMustBeArray();
                throw new InvalidOperationException("Argument must be array");
            }
            //if (arrayType.GetArrayRank() != 1)
            //{
            //    throw Error.IncorrectNumberOfIndexes();
            //}

            return new SimpleBinaryExpression(XzaarExpressionType.ArrayIndex, array, index, arrayType.GetElementType());
        }

        public static BinaryExpression AddChecked(XzaarExpression left, XzaarExpression right)
        {
            return AddChecked(left, right, null);
        }

        public static BinaryExpression AddChecked(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.AddChecked, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.AddChecked, "op_Addition", left, right, false);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.AddChecked, left, right, method, true);
        }
        public static BinaryExpression Subtract(XzaarExpression left, XzaarExpression right)
        {
            return Subtract(left, right, null);
        }

        public static BinaryExpression Subtract(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.Subtract, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.Subtract, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Subtract, left, right, method, true);
        }

        public static BinaryExpression SubtractAssign(XzaarExpression left, XzaarExpression right)
        {
            return SubtractAssign(left, right, null);
        }

        public static BinaryExpression SubtractAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.SubtractAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.SubtractAssign, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.SubtractAssign, left, right, method, true);
        }

        public static BinaryExpression SubtractAssignChecked(XzaarExpression left, XzaarExpression right)
        {
            return SubtractAssignChecked(left, right, null);
        }

        public static BinaryExpression SubtractAssignChecked(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {

                    return new SimpleBinaryExpression(XzaarExpressionType.SubtractAssignChecked, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.SubtractAssignChecked, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.SubtractAssignChecked, left, right, method, true);
        }
        public static BinaryExpression SubtractChecked(XzaarExpression left, XzaarExpression right)
        {
            return SubtractChecked(left, right, null);
        }
        public static BinaryExpression Divide(XzaarExpression left, XzaarExpression right)
        {
            return Divide(left, right, null);
        }
        public static BinaryExpression DivideAssign(XzaarExpression left, XzaarExpression right)
        {
            return DivideAssign(left, right, null);
        }

        public static BinaryExpression DivideAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.DivideAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.DivideAssign, "op_Division", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.DivideAssign, left, right, method, true);
        }

        public static BinaryExpression Divide(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.Divide, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.Divide, "op_Division", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Divide, left, right, method, true);
        }
        public static BinaryExpression Modulo(XzaarExpression left, XzaarExpression right)
        {
            return Modulo(left, right, null);
        }
        public static BinaryExpression Modulo(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.Modulo, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.Modulo, "op_Modulus", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Modulo, left, right, method, true);
        }
        public static BinaryExpression ModuloAssign(XzaarExpression left, XzaarExpression right)
        {
            return ModuloAssign(left, right, null);
        }
        public static BinaryExpression ModuloAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.ModuloAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.ModuloAssign, "op_Modulus", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.ModuloAssign, left, right, method, true);
        }
        public static BinaryExpression Multiply(XzaarExpression left, XzaarExpression right)
        {
            return Multiply(left, right, null);
        }
        public static BinaryExpression Multiply(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.Multiply, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.Multiply, "op_Multiply", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Multiply, left, right, method, true);
        }
        public static BinaryExpression MultiplyAssign(XzaarExpression left, XzaarExpression right)
        {
            return MultiplyAssign(left, right, null);
        }

        public static BinaryExpression MultiplyAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {

                    return new SimpleBinaryExpression(XzaarExpressionType.MultiplyAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.MultiplyAssign, "op_Multiply", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.MultiplyAssign, left, right, method, true);
        }
        public static BinaryExpression MultiplyAssignChecked(XzaarExpression left, XzaarExpression right)
        {
            return MultiplyAssignChecked(left, right, null);
        }

        public static BinaryExpression MultiplyAssignChecked(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.MultiplyAssignChecked, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.MultiplyAssignChecked, "op_Multiply", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.MultiplyAssignChecked, left, right, method, true);
        }

        public static BinaryExpression MultiplyChecked(XzaarExpression left, XzaarExpression right)
        {
            return MultiplyChecked(left, right, null);
        }

        public static BinaryExpression MultiplyChecked(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.MultiplyChecked, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.MultiplyChecked, "op_Multiply", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.MultiplyChecked, left, right, method, true);
        }
        private static bool IsSimpleShift(XzaarType left, XzaarType right)
        {
            return XzaarTypeUtils.IsInteger(left)
                && XzaarTypeUtils.GetNonNullableType(right) == XzaarBaseTypes.Typeof(typeof(int));
        }

        private static XzaarType GetResultTypeOfShift(XzaarType left, XzaarType right)
        {
            return left;
        }


        public static BinaryExpression SubtractChecked(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.SubtractChecked, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.SubtractChecked, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.SubtractChecked, left, right, method, true);
        }
        #region Arithmetic XzaarExpressions

        public static BinaryExpression Add(XzaarExpression left, XzaarExpression right)
        {
            return Add(left, right, null);
        }

        public static BinaryExpression Add(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.Add, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType.Add, "op_Addition", left, right, true);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Add, left, right, method, true);
        }

        public static BinaryExpression AddAssign(XzaarExpression left, XzaarExpression right)
        {
            return AddAssign(left, right, null);
        }

        public static BinaryExpression AddAssign(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanWrite(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type))
                {
                    return new SimpleBinaryExpression(XzaarExpressionType.AddAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType.AddAssign, "op_Addition", left, right, true);
            }
            return GetMethodBasedAssignOperator(XzaarExpressionType.AddAssign, left, right, method, true);
        }

        #endregion

        private static BinaryExpression GetComparisonOperator(XzaarExpressionType binaryType, string opName, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            if (left.Type == right.Type && XzaarTypeUtils.IsNumeric(left.Type))
            {
                if (XzaarTypeUtils.IsNullableType(left.Type) && liftToNull)
                {
                    return new SimpleBinaryExpression(binaryType, left, right, XzaarBaseTypes.Boolean);
                }
                else
                {
                    return new LogicalBinaryExpression(binaryType, left, right);
                }
            }
            return GetUserDefinedBinaryOperatorOrThrow(binaryType, opName, left, right, liftToNull);
        }

        private static bool IsNullConstant(XzaarExpression e)
        {
            var c = e as ConstantExpression;
            return c != null && c.Value == null;
        }

        private static BinaryExpression GetUserDefinedAssignOperatorOrThrow(XzaarExpressionType binaryType, string name, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            BinaryExpression b = GetUserDefinedBinaryOperatorOrThrow(binaryType, name, left, right, liftToNull);

            //// add the conversion to the result
            //ValidateOpAssignConversionLambda(conversion, b.Left, b.Method, b.NodeType);
            //b = new OpAssignMethodConversionBinaryExpression(b.NodeType, b.Left, b.Right, b.Left.Type, b.Method);

            return b;
        }

        private static BinaryExpression GetMethodBasedAssignOperator(XzaarExpressionType binaryType, XzaarExpression left, XzaarExpression right, XzaarMethodInfo method, bool liftToNull)
        {
            BinaryExpression b = GetMethodBasedBinaryOperator(binaryType, left, right, method, liftToNull);
            //if (conversion == null)
            //{
            //    // return XzaarType must be assignable back to the left XzaarType
            //    if (!TypeUtils.AreReferenceAssignable(left.Type, b.Type))
            //    {
            //        throw Error.UserDefinedOpMustHaveValidReturnType(binaryType, b.Method.Name);
            //    }
            //}
            //else
            //{
            //    // add the conversion to the result
            //    ValidateOpAssignConversionLambda(conversion, b.Left, b.Method, b.NodeType);
            //    b = new OpAssignMethodConversionBinaryExpression(b.NodeType, b.Left, b.Right, b.Left.Type, b.Method, conversion);
            //}
            return b;
        }


        private static BinaryExpression GetUserDefinedBinaryOperatorOrThrow(XzaarExpressionType binaryType, string name, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            BinaryExpression b = GetUserDefinedBinaryOperator(binaryType, name, left, right, liftToNull);
            if (b != null)
            {
                if (b.Method == null && (left.Type.Name == "string" || right.Type.Name == "string"))
                {
                    return b;
                }
                XzaarParameterInfo[] pis = b.Method.GetParameters();
                ValidateParamswithOperandsOrThrow(pis[0].ParameterType, left.Type, binaryType, name);
                ValidateParamswithOperandsOrThrow(pis[1].ParameterType, right.Type, binaryType, name);
                return b;
            }
            throw new InvalidOperationException("Binary operator not defined");
        }

        private static bool IsNullComparison(XzaarExpression left, XzaarExpression right)
        {
            // If we have x==null, x!=null, null==x or null!=x where x is
            // nullable but not null, then this is treated as a call to x.HasValue
            // and is legal even if there is no equality operator defined on the
            // XzaarType of x.
            if (IsNullConstant(left) && !IsNullConstant(right) && XzaarTypeUtils.IsNullableType(right.Type))
            {
                return true;
            }
            if (IsNullConstant(right) && !IsNullConstant(left) && XzaarTypeUtils.IsNullableType(left.Type))
            {
                return true;
            }
            return false;
        }

        public static BinaryExpression Equal(XzaarExpression left, XzaarExpression right)
        {
            return Equal(left, right, false, null);
        }

        public static BinaryExpression Equal(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetEqualityComparisonOperator(XzaarExpressionType.Equal, "op_Equality", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(XzaarExpressionType.Equal, left, right, method, liftToNull);
        }

        private static void ValidateUserDefinedConditionalLogicOperator(XzaarExpressionType nodeType, XzaarType left, XzaarType right, XzaarMethodInfo method)
        {
            ValidateOperator(method);
            XzaarParameterInfo[] pms = method.GetParameters();
            if (pms.Length != 2)
                // throw Error.IncorrectNumberOfMethodCallArguments(method);
                throw new InvalidOperationException("Incorrect number of method call arguments");

            if (!ParameterIsAssignable(pms[0], left))
            {
                if (!(XzaarTypeUtils.IsNullableType(left) && ParameterIsAssignable(pms[0], XzaarTypeUtils.GetNonNullableType(left))))
                    throw new InvalidOperationException("Operand types do not match parameters");
                // throw Error.OperandTypesDoNotMatchParameters(nodeType, method.Name);
            }
            if (!ParameterIsAssignable(pms[1], right))
            {
                if (!(XzaarTypeUtils.IsNullableType(right) && ParameterIsAssignable(pms[1], XzaarTypeUtils.GetNonNullableType(right))))
                    throw new InvalidOperationException("Operand types do not match parameters");
                // throw Error.OperandTypesDoNotMatchParameters(nodeType, method.Name);
            }
            if (pms[0].ParameterType != pms[1].ParameterType)
            {
                // throw Error.UserDefinedOpMustHaveConsistentTypes(nodeType, method.Name);
                throw new InvalidOperationException("User defined op must have consistent types");
            }
            if (method.ReturnType != pms[0].ParameterType)
            {
                // throw Error.UserDefinedOpMustHaveConsistentTypes(nodeType, method.Name);
                throw new InvalidOperationException("User defined op must have consistent types");
            }
            if (IsValidLiftedConditionalLogicalOperator(left, right, pms))
            {
                left = XzaarTypeUtils.GetNonNullableType(left);
                right = XzaarTypeUtils.GetNonNullableType(left);
            }
            XzaarMethodInfo opTrue = XzaarTypeUtils.GetBooleanOperator(method.DeclaringType, "op_True");
            XzaarMethodInfo opFalse = XzaarTypeUtils.GetBooleanOperator(method.DeclaringType, "op_False");
            if (opTrue == null || opTrue.ReturnType != XzaarBaseTypes.Boolean ||
                opFalse == null || opFalse.ReturnType != XzaarBaseTypes.Boolean)
            {
                // throw Error.LogicalOperatorMustHaveBooleanOperators(nodeType, method.Name);
                throw new InvalidOperationException("Logical operator must have boolean operators");
            }
            VerifyOpTrueFalse(nodeType, left, opFalse);
            VerifyOpTrueFalse(nodeType, left, opTrue);
        }
        private static bool IsValidLiftedConditionalLogicalOperator(XzaarType left, XzaarType right, XzaarParameterInfo[] pms)
        {
            return XzaarTypeUtils.AreEquivalent(left, right) &&
                   XzaarTypeUtils.IsNullableType(right) &&
                   XzaarTypeUtils.AreEquivalent(pms[1].ParameterType, XzaarTypeUtils.GetNonNullableType(right));
        }

        private static void VerifyOpTrueFalse(XzaarExpressionType nodeType, XzaarType left, XzaarMethodInfo opTrue)
        {
            XzaarParameterInfo[] pmsOpTrue = opTrue.GetParameters();
            if (pmsOpTrue.Length != 1)
                // throw Error.IncorrectNumberOfMethodCallArguments(opTrue);
                throw new InvalidOperationException("Incorrect number of method call arguments");

            if (!ParameterIsAssignable(pmsOpTrue[0], left))
            {
                if (!(XzaarTypeUtils.IsNullableType(left) && ParameterIsAssignable(pmsOpTrue[0], XzaarTypeUtils.GetNonNullableType(left))))
                    // throw Error.OperandTypesDoNotMatchParameters(nodeType, opTrue.Name);
                    throw new InvalidOperationException("Operand types do not match parameters");
            }
        }

        private static BinaryExpression GetEqualityComparisonOperator(XzaarExpressionType binaryType, string opName, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            // known comparison - numeric XzaarTypes, bools, object, enums
            if (left.Type == right.Type &&
                (XzaarTypeUtils.IsNumeric(left.Type) || left.Type == XzaarBaseTypes.Void || XzaarTypeUtils.IsBool(left.Type)))
            {
                if (XzaarTypeUtils.IsNullableType(left.Type) && liftToNull)
                {
                    return new SimpleBinaryExpression(binaryType, left, right, XzaarBaseTypes.Boolean);
                }
                else
                {
                    return new LogicalBinaryExpression(binaryType, left, right);
                }
            }
            // look for user defined operator
            BinaryExpression b = GetUserDefinedBinaryOperator(binaryType, opName, left, right, liftToNull);
            if (b != null)
            {
                return b;
            }
            if (XzaarTypeUtils.HasBuiltInEqualityOperator(left.Type, right.Type) || IsNullComparison(left, right))
            {
                if (XzaarTypeUtils.IsNullableType(left.Type) && liftToNull)
                {
                    return new SimpleBinaryExpression(binaryType, left, right, XzaarBaseTypes.Boolean);
                }
                else
                {
                    return new LogicalBinaryExpression(binaryType, left, right);
                }
            }


            throw new Exception("Binary operator between the types '" + left.Type.Name + "' and '" + right.Type.Name + "' are not defined");
            // throw Error.BinaryOperatorNotDefined(binaryType, left.Type, right.Type);
        }

        private static BinaryExpression GetUserDefinedBinaryOperator(XzaarExpressionType binaryType, string name, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            // try exact match first
            XzaarMethodInfo method = GetUserDefinedBinaryOperator(binaryType, left.Type, right.Type, name);
            if (method != null)
            {
                return new MethodBinaryExpression(binaryType, left, right, method.ReturnType, method);
            }
            // try lifted call
            if (XzaarTypeUtils.IsNullableType(left.Type) && XzaarTypeUtils.IsNullableType(right.Type))
            {
                XzaarType nnLeftType = XzaarTypeUtils.GetNonNullableType(left.Type);
                XzaarType nnRightType = XzaarTypeUtils.GetNonNullableType(right.Type);
                method = GetUserDefinedBinaryOperator(binaryType, nnLeftType, nnRightType, name);
                if (method != null && !XzaarTypeUtils.IsNullableType(method.ReturnType))
                {
                    if (method.ReturnType != XzaarBaseTypes.Boolean || liftToNull)
                    {
                        return new MethodBinaryExpression(binaryType, left, right, XzaarTypeUtils.GetNullableType(method.ReturnType), method);
                    }
                    else
                    {
                        return new MethodBinaryExpression(binaryType, left, right, XzaarBaseTypes.Boolean, method);
                    }
                }
            }
            if (left.Type.Name == "string" && right.Type.Name != "void" || left.Type.Name != "void" && right.Type.Name == "string")
            {
                return new MethodBinaryExpression(binaryType, left, right, XzaarBaseTypes.String, method);
            }
            return null;
        }


        private static XzaarMethodInfo GetUserDefinedBinaryOperator(XzaarExpressionType binaryType, XzaarType leftType, XzaarType rightType, string name)
        {
            // 

            XzaarType[] XzaarTypes = new XzaarType[] { leftType, rightType };
            XzaarType nnLeftType = XzaarTypeUtils.GetNonNullableType(leftType);
            XzaarType nnRightType = XzaarTypeUtils.GetNonNullableType(rightType);
            XzaarMethodInfo method = nnLeftType.GetMethod(name, XzaarTypes);
            if (method == null && !XzaarTypeUtils.AreEquivalent(leftType, rightType))
            {
                method = nnRightType.GetMethod(name, XzaarTypes);
            }

            if (IsLiftingConditionalLogicalOperator(leftType, rightType, method, binaryType))
            {
                method = GetUserDefinedBinaryOperator(binaryType, nnLeftType, nnRightType, name);
            }
            return method;
        }

        private static bool IsLiftingConditionalLogicalOperator(XzaarType left, XzaarType right, XzaarMethodInfo method, XzaarExpressionType binaryType)
        {
            return XzaarTypeUtils.IsNullableType(right) &&
                    XzaarTypeUtils.IsNullableType(left) &&
                    method == null &&
                    (binaryType == XzaarExpressionType.AndAlso || binaryType == XzaarExpressionType.OrElse);
        }

        private static BinaryExpression GetMethodBasedBinaryOperator(XzaarExpressionType binaryType, XzaarExpression left, XzaarExpression right, XzaarMethodInfo method, bool liftToNull)
        {
            System.Diagnostics.Debug.Assert(method != null);
            ValidateOperator(method);
            XzaarParameterInfo[] pms = method.GetParameters();
            if (pms.Length != 2)
                throw new InvalidOperationException("Incorrect number of method call arguments for binary operator method");
            // throw Error.IncorrectNumberOfMethodCallArguments(method);
            if (ParameterIsAssignable(pms[0], left.Type) && ParameterIsAssignable(pms[1], right.Type))
            {
                ValidateParamswithOperandsOrThrow(pms[0].ParameterType, left.Type, binaryType, method.Name);
                ValidateParamswithOperandsOrThrow(pms[1].ParameterType, right.Type, binaryType, method.Name);
                return new MethodBinaryExpression(binaryType, left, right, method.ReturnType, method);

            }
            // check for lifted call
            if (XzaarTypeUtils.IsNullableType(left.Type) && XzaarTypeUtils.IsNullableType(right.Type) &&
                ParameterIsAssignable(pms[0], XzaarTypeUtils.GetNonNullableType(left.Type)) &&
                ParameterIsAssignable(pms[1], XzaarTypeUtils.GetNonNullableType(right.Type)) &&
                /*method.ReturnType.IsValueType &&*/ !XzaarTypeUtils.IsNullableType(method.ReturnType))
            {
                if (method.ReturnType != XzaarBaseTypes.Boolean || liftToNull)
                {
                    return new MethodBinaryExpression(binaryType, left, right, XzaarTypeUtils.GetNullableType(method.ReturnType), method);
                }
                else
                {
                    return new MethodBinaryExpression(binaryType, left, right, XzaarBaseTypes.Boolean, method);
                }
            }
            throw new InvalidOperationException("Operand XzaarTypes do not match parameters");
            // throw Error.OperandTypesDoNotMatchParameters(binaryType, method.Name);
        }

        private static void ValidateOperator(XzaarMethodInfo method)
        {
            if (method.ReturnType == XzaarBaseTypes.Void)
                throw new InvalidOperationException("User defined operator must not be void");
        }

        internal static bool ParameterIsAssignable(XzaarParameterInfo pi, XzaarType argType)
        {
            XzaarType pType = pi.ParameterType;
            //if (pType.IsByRef)
            //    pType = pType.GetElementType();
            return XzaarTypeUtils.AreReferenceAssignable(pType, argType);
        }

        private static void ValidateParamswithOperandsOrThrow(XzaarType paramType, XzaarType operandType, XzaarExpressionType exprType, string name)
        {
            if (XzaarTypeUtils.IsNullableType(paramType) && !XzaarTypeUtils.IsNullableType(operandType))
            {
                throw new InvalidOperationException("Operand XzaarTypes do not match parameters");
            }
        }



    }
}