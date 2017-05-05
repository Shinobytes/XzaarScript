namespace Shinobytes.XzaarScript.Ast
{
    /// <summary>
    /// all enum values are ordered by their precedence order
    /// </summary>
    public enum NodeType
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
    }
}