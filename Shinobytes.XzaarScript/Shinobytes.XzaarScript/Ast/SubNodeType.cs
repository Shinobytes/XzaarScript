namespace Shinobytes.XzaarScript.Ast
{
    /// <summary>
    /// all enum values are ordered by their precedence order
    /// </summary>
    public enum SubNodeType
    {
        None,
        LiteralNumber,
        LiteralString,

        //
        UnaryPlus,
        UnaryMinus,
        UnaryNot,
        UnaryBitwiseComplement, // tilde
        UnaryIncrement,
        UnaryDecrement,

        ArithmeticMultiply,
        ArithmeticDivide,
        ArithmeticModulus,
        ArithmeticAdd,
        ArithmeticSubtract,
        ArithmeticLeftShift,
        ArithmeticRightShift,

        ConditionalLessThan,
        ConditionalGreaterThan,
        ConditionalLessOrEquals,
        ConditionalGreaterOrEquals,

        EqualityEquals,
        EqualityNotEquals,

        BitAnd,
        BitXor,
        BitOr,

        ConditionalAnd,
        ConditionalOr,

        Assign,
        AssignPlus,
        AssignMinus,
        AssignMultiply,
        AssignDivide,
        AssignModulus,
        AssignOr,
        AssignAnd,
        AssignXor,
        AssignLeftShift,
        AssignRightShift,
        Lambda
    }
}