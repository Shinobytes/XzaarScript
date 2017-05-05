namespace Shinobytes.XzaarScript.Ast
{
    /// <summary>
    /// all enum values are ordered by their precedence order
    /// </summary>
    public enum XzaarSyntaxKind
    {
        Literal,
        Identifier,
        Keyword,
        Constant,
        Expression,
        ArrayIndexExpression,
        Scope,

        Separator,
        StatementTerminator,

        // Highest precedence operators            
        MemberAccess,                // x.y
        NullConditionalMemberAccess, // x?.y
        FunctionInvocation,          // f(x)
        AggregateObjectIndex,        // a[x]
        PostfixIncrement,            // x++, returns the value of x and then increments the value by 1
        PostfixDecrement,            // x--, returns the value of x and then decrements the value by 1
        TypeInstantiation,           // Commonly 'new' keyword, for 
        PointerMemberAccess,         // ->        

        // +x, -x, !x, ~x, ++x, --x
        UnaryOperator,

        // x * y, x / y, x % y, x + y, x - y, x << y, x >> y
        ArithmeticOperator,

        // x < y, x > y, x <= y, x >= y
        LogicalConditionalOperator,

        // x == y, x != y
        EqualityOperator,

        // x & y, x ^ y, x | y
        LogicalBitOperator,

        // x && y, x || y
        ConditionalOperator,

        // x ?? y - returns ix if non-null; oteherwise returns y
        NullCoalescingOperator,

        // x = y, x+= y, x -= y, x *= y, x /= y, x %= y, x |= y, x ^= y, x <<= y, x >>= y, =>
        AssignmentOperator,





        // sub types
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
        ArithmeticModulo,
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
        BitNot, // ~

        Not,

        LogicalAnd,
        LogicalOr,

        Assign,
        AssignPlus,
        AssignMinus,
        AssignMultiply,
        AssignDivide,
        AssignModulo,
        AssignOr,
        AssignAnd,
        AssignXor,
        AssignLeftShift,
        AssignRightShift,
        Lambda,

        KeywordIf,
        KeywordSwitch,
        KeywordCase,
        KeywordWhile,
        KeywordStruct,
        KeywordClass,
        KeywordFn,
        KeywordLoop,
        KeywordDo,
        KeywordFor,
        KeywordForEach,
        KeywordExtern,
        KeywordEnum,
        KeywordNumber,
        KeywordString,
        KeywordBoolean,
        KeywordDate,
        KeywordAny,
        KeywordVar,
        KeywordReturn,
        KeywordGoto,
        KeywordContinue,
        KeywordBreak,
        KeywordVoid,
        KeywordElse,
        KeywordNew,
        KeywordTrue,
        KeywordFalse,
        KeywordNull,
        KeywordIn,
        KeywordDefault,

        // token types
        Number,
        String,

        Tilde,
        Minus,
        MinusEquals,
        MinusMinus,
        MinusGreater,
        Plus,
        PlusPlus,
        PlusEquals,
        Multiply,
        MultiplyEquals,
        Divide,
        DivideEquals,
        DivideDivide,
        Modulo,
        ModuloEquals,
        Equals,
        EqualsEquals,
        EqualsGreater,
        Greater,
        GreaterEquals,
        GreaterGreater,
        GreaterGreaterEquals,
        Less,
        LessEquals,
        LessLess,
        LessLessEquals,
        And,
        AndEquals,
        AndAnd,
        Or,
        OrEquals,
        OrOr,
        Caret,
        CaretEquals,


        NotEquals,

        Question,
        QuestionQuestion,

        Semicolon,
        Colon,
        ColonColon,
        Comma,
        Dot,
        LeftParan,
        RightParan,
        LeftCurly,
        RightCurly,
        LeftBracket,
        RightBracket,
        ArgList,
        ArgumentList,

        Newline,
        Whitespace,
        CommentMultiLine,
        CommentSingleLine
    }
}