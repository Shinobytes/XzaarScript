namespace Shinobytes.XzaarScript.Ast.Nodes
{
    // I've set the values already in case we add more later and/or want to reorder the enum values,    
    public enum XzaarNodeTypes
    {
        NULL_EMPTY = 0,
        PROGRAM = 1,
        EQUALITY = 2,
        ARGUMENT = 3,
        ASSIGN = 4,
        ACCESS = 5,
        MATH = 6,
        BLOCK = 7,
        BODY = 8,
        CONDITION = 9,
        CONDITIONAL = 26,
        DECLARATION = 10,
        EXPRESSION = 11,
        CALL = 12,
        FUNCTION = 13,
        PARAMETERS = 14,
        PARAMETER = 15,
        LITERAL = 16,
        LOOP = 18,
        MATCH = 19,
        VARIABLE = 20,
        RETURN = 21,
        GOTO = 22,
        CONTINUE = 23,
        BREAK = 24,
        LABEL = 25,
        ARRAYINDEX = 41,
        CASE = 42,
        FIELD = 43,
        STRUCT = 44,
        CREATE_STRUCT = 45,
        ACCESSOR_CHAIN = 46,
        NEGATE = 47,

        // Syntax Token
        ARITHMETIC_OPERATOR = 27,
        COLON = 28,
        ASSIGNMENT_OPERATOR = 29,
        UNARY_OPERATOR = 30,
        CONDITIONAL_OPERATOR = 31,
        EQUALITY_OPERATOR = 32,
        ACCESSOR = 33,
        PARAN = 34,
        BRACE = 35,
        BRACKET = 36,
        NULL_COALESCING_OPERATOR = 37,
        LAMBDA_OPERATOR = 38,
        NEGATE_OPERATOR = 39,
        SEPARATOR = 40,

        COMMENT = 9999

    }
}