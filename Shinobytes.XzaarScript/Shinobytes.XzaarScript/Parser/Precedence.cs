namespace Shinobytes.XzaarScript.Parser
{
    public enum Precedence : uint
    {
        Expression = 0, // Loosest possible precedence, used to accept all expressions
        Assignment,
        Lambda = Assignment, // "The => operator has the same precedence as assignment (=) and is right-associative."
        Ternary,
        Coalescing,
        ConditionalOr,
        ConditionalAnd,
        LogicalOr,
        LogicalXor,
        LogicalAnd,
        Equality,
        Relational,
        Shift,
        Additive,
        Mutiplicative,
        Unary,
        Cast,
    }
}