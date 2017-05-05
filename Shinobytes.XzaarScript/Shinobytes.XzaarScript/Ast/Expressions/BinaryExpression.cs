using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    [Serializable]
    public abstract class BinaryExpression : XzaarExpression
    {
        private readonly XzaarExpression left;
        private readonly XzaarExpression right;
        private XzaarType type;

        internal BinaryExpression(XzaarExpression left, XzaarExpression right)
        {
            this.left = left;
            this.right = right;
        }

        private static bool IsOpAssignment(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.AddAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.DivideAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.AndAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ExclusiveOrAssign:
                    return true;
            }
            return false;
        }

        public override XzaarType Type => (this.NodeType == ExpressionType.GreaterThan || this.NodeType == ExpressionType.GreaterThanOrEqual || this.NodeType == ExpressionType.Equal || this.NodeType == ExpressionType.NotEqual || this.NodeType == ExpressionType.LessThan || this.NodeType == ExpressionType.LessThanOrEqual)
            ? XzaarBaseTypes.Boolean : XzaarBaseTypes.Any;

        private static ExpressionType GetBinaryOpFromAssignmentOp(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.AddAssign:
                    return ExpressionType.Add;
                case ExpressionType.AddAssignChecked:
                    return ExpressionType.AddChecked;
                case ExpressionType.SubtractAssign:
                    return ExpressionType.Subtract;
                case ExpressionType.SubtractAssignChecked:
                    return ExpressionType.SubtractChecked;
                case ExpressionType.MultiplyAssign:
                    return ExpressionType.Multiply;
                case ExpressionType.MultiplyAssignChecked:
                    return ExpressionType.MultiplyChecked;
                case ExpressionType.DivideAssign:
                    return ExpressionType.Divide;
                case ExpressionType.ModuloAssign:
                    return ExpressionType.Modulo;
                case ExpressionType.PowerAssign:
                    return ExpressionType.Power;
                case ExpressionType.AndAssign:
                    return ExpressionType.And;
                case ExpressionType.OrAssign:
                    return ExpressionType.Or;
                case ExpressionType.RightShiftAssign:
                    return ExpressionType.RightShift;
                case ExpressionType.LeftShiftAssign:
                    return ExpressionType.LeftShift;
                case ExpressionType.ExclusiveOrAssign:
                    return ExpressionType.ExclusiveOr;
                default:
                    throw new InvalidOperationException("op");
            }

        }

        public XzaarExpression Left => left;

        public XzaarExpression Right => right;

        public XzaarMethodInfo Method => GetMethod();

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
                if (NodeType == ExpressionType.Equal)
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
                if (NodeType == ExpressionType.Coalesce || NodeType == ExpressionType.Assign)
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
        public bool IsLiftedToNull => IsLifted && XzaarTypeUtils.IsNullableType(Type);

        internal bool IsReferenceComparison => false;
    }

    public partial class XzaarExpression
    {
        public static BinaryExpression ReferenceNotEqual(XzaarExpression left, XzaarExpression right)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (XzaarTypeUtils.HasReferenceEquality(left.Type, right.Type))
            {
                return new LogicalBinaryExpression(ExpressionType.NotEqual, left, right);
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
                return new LogicalBinaryExpression(ExpressionType.Equal, left, right);
            }
            // throw Error.ReferenceEqualityNotDefined(left.Type, right.Type);
            throw new InvalidOperationException("Reference equality not defined");
        }

        public static BinaryExpression Assign(XzaarExpression left, XzaarExpression right)
        {
            return new AssignBinaryExpression(left, right);
        }

        public static BinaryExpression MakeBinary(ExpressionType binaryType, XzaarExpression left, XzaarExpression right)
        {
            return MakeBinary(binaryType, left, right, false, null);
        }

        public static BinaryExpression MakeBinary(ExpressionType binaryType, XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            switch (binaryType)
            {
                case ExpressionType.Add: return Add(left, right, method);
                case ExpressionType.AddChecked: return AddChecked(left, right, method);
                case ExpressionType.Subtract: return Subtract(left, right, method);
                case ExpressionType.SubtractChecked: return SubtractChecked(left, right, method);
                case ExpressionType.Multiply: return Multiply(left, right, method);
                case ExpressionType.MultiplyChecked: return MultiplyChecked(left, right, method);
                case ExpressionType.Divide: return Divide(left, right, method);
                case ExpressionType.Modulo: return Modulo(left, right, method);
                case ExpressionType.Power: return Power(left, right, method);
                case ExpressionType.And: return And(left, right, method);
                case ExpressionType.AndAlso: return AndAlso(left, right, method);
                case ExpressionType.Or: return Or(left, right, method);
                case ExpressionType.OrElse: return OrElse(left, right, method);
                case ExpressionType.LessThan: return LessThan(left, right, liftToNull, method);
                case ExpressionType.LessThanOrEqual: return LessThanOrEqual(left, right, liftToNull, method);
                case ExpressionType.GreaterThan: return GreaterThan(left, right, liftToNull, method);
                case ExpressionType.GreaterThanOrEqual: return GreaterThanOrEqual(left, right, liftToNull, method);
                case ExpressionType.Equal: return Equal(left, right, liftToNull, method);
                case ExpressionType.NotEqual: return NotEqual(left, right, liftToNull, method);
                case ExpressionType.ExclusiveOr: return ExclusiveOr(left, right, method);
                case ExpressionType.Coalesce: return Coalesce(left, right);
                case ExpressionType.ArrayIndex: return ArrayIndex(left, right);
                case ExpressionType.RightShift: return RightShift(left, right, method);
                case ExpressionType.LeftShift: return LeftShift(left, right, method);
                case ExpressionType.Assign: return Assign(left, right);
                case ExpressionType.AddAssign: return AddAssign(left, right, method);
                case ExpressionType.AndAssign: return AndAssign(left, right, method);
                case ExpressionType.DivideAssign: return DivideAssign(left, right, method);
                case ExpressionType.ExclusiveOrAssign: return ExclusiveOrAssign(left, right, method);
                case ExpressionType.LeftShiftAssign: return LeftShiftAssign(left, right, method);
                case ExpressionType.ModuloAssign: return ModuloAssign(left, right, method);
                case ExpressionType.MultiplyAssign: return MultiplyAssign(left, right, method);
                case ExpressionType.OrAssign: return OrAssign(left, right, method);
                case ExpressionType.PowerAssign: return PowerAssign(left, right, method);
                case ExpressionType.RightShiftAssign: return RightShiftAssign(left, right, method);
                case ExpressionType.SubtractAssign: return SubtractAssign(left, right, method);
                case ExpressionType.AddAssignChecked: return AddAssignChecked(left, right, method);
                case ExpressionType.SubtractAssignChecked: return SubtractAssignChecked(left, right, method);
                case ExpressionType.MultiplyAssignChecked: return MultiplyAssignChecked(left, right, method);
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
                    return new SimpleBinaryExpression(ExpressionType.RightShiftAssign, left, right, resultType);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.RightShiftAssign, "op_RightShift", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.RightShiftAssign, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.AddAssignChecked, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.AddAssignChecked, "op_Addition", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.AddAssignChecked, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.ExclusiveOrAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.ExclusiveOrAssign, "op_ExclusiveOr", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.ExclusiveOrAssign, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.LeftShiftAssign, left, right, resultType);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.LeftShiftAssign, "op_LeftShift", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.LeftShiftAssign, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.RightShift, left, right, resultType);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.RightShift, "op_RightShift", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.RightShift, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.LeftShift, left, right, resultType);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.LeftShift, "op_LeftShift", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.LeftShift, left, right, method, true);
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
            return new SimpleBinaryExpression(ExpressionType.Coalesce, left, right, resultType);
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
                    return new SimpleBinaryExpression(ExpressionType.ExclusiveOr, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.ExclusiveOr, "op_ExclusiveOr", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.ExclusiveOr, left, right, method, true);
        }


        public static BinaryExpression NotEqual(XzaarExpression left, XzaarExpression right)
        {
            return NotEqual(left, right, false, null);
        }

        public static BinaryExpression NotEqual(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetEqualityComparisonOperator(ExpressionType.NotEqual, "op_Inequality", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.NotEqual, left, right, method, liftToNull);
        }

        public static BinaryExpression LessThanOrEqual(XzaarExpression left, XzaarExpression right, bool liftToNull, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                return GetComparisonOperator(ExpressionType.LessThanOrEqual, "op_LessThanOrEqual", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.LessThanOrEqual, left, right, method, liftToNull);
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
                return GetComparisonOperator(ExpressionType.GreaterThan, "op_GreaterThan", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.GreaterThan, left, right, method, liftToNull);
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
                return GetComparisonOperator(ExpressionType.GreaterThanOrEqual, "op_GreaterThanOrEqual", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.GreaterThanOrEqual, left, right, method, liftToNull);
        }

        public static BinaryExpression OrElse(XzaarExpression left, XzaarExpression right)
        {
            return OrElse(left, right, null);
        }

        public static BinaryExpression OrElse(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            XzaarType returnType;
            if (method == null)
            {
                if (left.Type == right.Type || (left.Type.Name == "any" && right.Type.Name == "bool") || (right.Type.Name == "any" && left.Type.Name == "bool"))
                {
                    if (left.Type == XzaarBaseTypes.Boolean)
                    {
                        return new LogicalBinaryExpression(ExpressionType.OrElse, left, right);
                    }
                    else // if (left.Type == typeof(bool?))
                    {
                        return new SimpleBinaryExpression(ExpressionType.OrElse, left, right, left.Type);
                    }
                }
                method = GetUserDefinedBinaryOperator(ExpressionType.OrElse, left.Type, right.Type, "op_BitwiseOr");
                if (method != null)
                {
                    ValidateUserDefinedConditionalLogicOperator(ExpressionType.OrElse, left.Type, right.Type, method);
                    returnType = (XzaarTypeUtils.IsNullableType(left.Type) && method.ReturnType == XzaarTypeUtils.GetNonNullableType(left.Type)) ? left.Type : method.ReturnType;
                    return new MethodBinaryExpression(ExpressionType.OrElse, left, right, returnType, method);
                }
                throw new InvalidOperationException("Binary operator not defined");
                // throw Error.BinaryOperatorNotDefined(ExpressionType.OrElse, left.Type, right.Type);
            }
            ValidateUserDefinedConditionalLogicOperator(ExpressionType.OrElse, left.Type, right.Type, method);
            returnType = (XzaarTypeUtils.IsNullableType(left.Type) && method.ReturnType == XzaarTypeUtils.GetNonNullableType(left.Type)) ? left.Type : method.ReturnType;
            return new MethodBinaryExpression(ExpressionType.OrElse, left, right, returnType, method);
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
                return GetComparisonOperator(ExpressionType.LessThan, "op_LessThan", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.LessThan, left, right, method, liftToNull);
        }


        public static BinaryExpression AndAlso(XzaarExpression left, XzaarExpression right)
        {
            return AndAlso(left, right, null);
        }

        public static BinaryExpression AndAlso(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            XzaarType returnType;
            if (method == null)
            {
                if (left.Type == right.Type || (left.Type.Name == "any" && right.Type.Name == "bool") || (right.Type.Name == "any" && left.Type.Name == "bool"))
                {
                    if (left.Type == XzaarBaseTypes.Boolean)
                    {
                        return new LogicalBinaryExpression(ExpressionType.AndAlso, left, right);
                    }
                    else // if (left.Type == typeof(bool?))
                    {
                        return new SimpleBinaryExpression(ExpressionType.AndAlso, left, right, left.Type);
                    }
                }
                method = GetUserDefinedBinaryOperator(ExpressionType.AndAlso, left.Type, right.Type, "op_BitwiseAnd");
                if (method != null)
                {
                    ValidateUserDefinedConditionalLogicOperator(ExpressionType.AndAlso, left.Type, right.Type, method);
                    returnType = (XzaarTypeUtils.IsNullableType(left.Type) && XzaarTypeUtils.AreEquivalent(method.ReturnType, XzaarTypeUtils.GetNonNullableType(left.Type))) ? left.Type : method.ReturnType;
                    return new MethodBinaryExpression(ExpressionType.AndAlso, left, right, returnType, method);
                }
                // throw Error.BinaryOperatorNotDefined(ExpressionType.AndAlso, left.Type, right.Type);
                throw new InvalidOperationException("Binary operation not defined");
            }
            ValidateUserDefinedConditionalLogicOperator(ExpressionType.AndAlso, left.Type, right.Type, method);
            returnType = (XzaarTypeUtils.IsNullableType(left.Type) && XzaarTypeUtils.AreEquivalent(method.ReturnType, XzaarTypeUtils.GetNonNullableType(left.Type))) ? left.Type : method.ReturnType;
            return new MethodBinaryExpression(ExpressionType.AndAlso, left, right, returnType, method);
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
                if (left.Type.Name == right.Type.Name && XzaarTypeUtils.IsIntegerOrBool(left.Type))
                {
                    return new SimpleBinaryExpression(ExpressionType.And, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.And, "op_BitwiseAnd", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.And, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.AndAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.AndAssign, "op_BitwiseAnd", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.AndAssign, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.OrAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.OrAssign, "op_BitwiseOr", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.OrAssign, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.Or, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Or, "op_BitwiseOr", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Or, left, right, method, true);
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
                //    throw Error.BinaryOperatorNotDefined(ExpressionType.Power, left.Type, right.Type);
                //}
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Power, left, right, method, true);
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
            return GetMethodBasedAssignOperator(ExpressionType.PowerAssign, left, right, method, true);
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

            return new SimpleBinaryExpression(ExpressionType.ArrayIndex, array, index, arrayType.GetElementType());
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.AddChecked, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.AddChecked, "op_Addition", left, right, false);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.AddChecked, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.Subtract, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Subtract, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Subtract, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.SubtractAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.SubtractAssign, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.SubtractAssign, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {

                    return new SimpleBinaryExpression(ExpressionType.SubtractAssignChecked, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.SubtractAssignChecked, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.SubtractAssignChecked, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type)|| (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.DivideAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.DivideAssign, "op_Division", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.DivideAssign, left, right, method, true);
        }

        public static BinaryExpression Divide(XzaarExpression left, XzaarExpression right, XzaarMethodInfo method)
        {
            RequiresCanRead(left, "left");
            RequiresCanRead(right, "right");
            if (method == null)
            {
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type)|| (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.Divide, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Divide, "op_Division", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Divide, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type)|| (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.Modulo, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Modulo, "op_Modulus", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Modulo, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.ModuloAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.ModuloAssign, "op_Modulus", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.ModuloAssign, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.Multiply, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Multiply, "op_Multiply", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Multiply, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {

                    return new SimpleBinaryExpression(ExpressionType.MultiplyAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.MultiplyAssign, "op_Multiply", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.MultiplyAssign, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.MultiplyAssignChecked, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.MultiplyAssignChecked, "op_Multiply", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.MultiplyAssignChecked, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.MultiplyChecked, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.MultiplyChecked, "op_Multiply", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.MultiplyChecked, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.SubtractChecked, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.SubtractChecked, "op_Subtraction", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.SubtractChecked, left, right, method, true);
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
                if (left.Type == right.Type && XzaarTypeUtils.IsArithmetic(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
                {
                    return new SimpleBinaryExpression(ExpressionType.Add, left, right, left.Type);
                }
                return GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Add, "op_Addition", left, right, true);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Add, left, right, method, true);
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
                    return new SimpleBinaryExpression(ExpressionType.AddAssign, left, right, left.Type);
                }
                return GetUserDefinedAssignOperatorOrThrow(ExpressionType.AddAssign, "op_Addition", left, right, true);
            }
            return GetMethodBasedAssignOperator(ExpressionType.AddAssign, left, right, method, true);
        }

        #endregion

        private static BinaryExpression GetComparisonOperator(ExpressionType binaryType, string opName, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            if (left.Type == right.Type && XzaarTypeUtils.IsNumeric(left.Type) || (left.Type.IsNumeric && right.Type.IsNumeric))
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

        private static BinaryExpression GetUserDefinedAssignOperatorOrThrow(ExpressionType binaryType, string name, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            BinaryExpression b = GetUserDefinedBinaryOperatorOrThrow(binaryType, name, left, right, liftToNull);

            //// add the conversion to the result
            //ValidateOpAssignConversionLambda(conversion, b.Left, b.Method, b.NodeType);
            //b = new OpAssignMethodConversionBinaryExpression(b.NodeType, b.Left, b.Right, b.Left.Type, b.Method);

            return b;
        }

        private static BinaryExpression GetMethodBasedAssignOperator(ExpressionType binaryType, XzaarExpression left, XzaarExpression right, XzaarMethodInfo method, bool liftToNull)
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


        private static BinaryExpression GetUserDefinedBinaryOperatorOrThrow(ExpressionType binaryType, string name, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            BinaryExpression b = GetUserDefinedBinaryOperator(binaryType, name, left, right, liftToNull);
            if (b != null)
            {
                if (b.Method == null && ((left.Type.Name == "string" || right.Type.Name == "string") || (left.Type.Name == "any" || right.Type.Name == "any")))
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
                return GetEqualityComparisonOperator(ExpressionType.Equal, "op_Equality", left, right, liftToNull);
            }
            return GetMethodBasedBinaryOperator(ExpressionType.Equal, left, right, method, liftToNull);
        }

        private static void ValidateUserDefinedConditionalLogicOperator(ExpressionType nodeType, XzaarType left, XzaarType right, XzaarMethodInfo method)
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

        private static void VerifyOpTrueFalse(ExpressionType nodeType, XzaarType left, XzaarMethodInfo opTrue)
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

        private static BinaryExpression GetEqualityComparisonOperator(ExpressionType binaryType, string opName, XzaarExpression left, XzaarExpression right, bool liftToNull)
        {
            // known comparison - numeric XzaarTypes, bools, object, enums
            if ((left.Type.IsNumeric && right.Type.IsNumeric) || left.Type == right.Type &&
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

        private static BinaryExpression GetUserDefinedBinaryOperator(ExpressionType binaryType, string name, XzaarExpression left, XzaarExpression right, bool liftToNull)
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
            if (left.Type.IsNumeric && right.Type.IsNumeric)
            {
                return new SimpleBinaryExpression(binaryType, left, right, left.Type);
            }
            if ((left.Type.Name == "string" && right.Type.Name != "void" || left.Type.Name != "void" && right.Type.Name == "string" || left.Type.Name == "any" && right.Type.Name != "void" || left.Type.Name != "void" && right.Type.Name == "any"))
            {
                if (IsBooleanExpr(binaryType))
                {
                    return new MethodBinaryExpression(binaryType, left, right, XzaarBaseTypes.Boolean, method);
                }

                return new MethodBinaryExpression(binaryType, left, right, XzaarBaseTypes.String, method);
            }
            return null;
        }

        private static bool IsBooleanExpr(ExpressionType expr)
        {
            return (expr == ExpressionType.GreaterThan ||
                    expr == ExpressionType.GreaterThanOrEqual ||
                    expr == ExpressionType.Equal || expr == ExpressionType.NotEqual ||
                    expr == ExpressionType.LessThan ||
                    expr == ExpressionType.LessThanOrEqual);
        }

        private static XzaarMethodInfo GetUserDefinedBinaryOperator(ExpressionType binaryType, XzaarType leftType, XzaarType rightType, string name)
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

        private static bool IsLiftingConditionalLogicalOperator(XzaarType left, XzaarType right, XzaarMethodInfo method, ExpressionType binaryType)
        {
            return XzaarTypeUtils.IsNullableType(right) &&
                    XzaarTypeUtils.IsNullableType(left) &&
                    method == null &&
                    (binaryType == ExpressionType.AndAlso || binaryType == ExpressionType.OrElse);
        }

        private static BinaryExpression GetMethodBasedBinaryOperator(ExpressionType binaryType, XzaarExpression left, XzaarExpression right, XzaarMethodInfo method, bool liftToNull)
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

        private static void ValidateParamswithOperandsOrThrow(XzaarType paramType, XzaarType operandType, ExpressionType exprType, string name)
        {
            if (XzaarTypeUtils.IsNullableType(paramType) && !XzaarTypeUtils.IsNullableType(operandType))
            {
                throw new InvalidOperationException("Operand XzaarTypes do not match parameters");
            }
        }



    }
}