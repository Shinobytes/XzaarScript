using System.Linq;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarExpressionTokens
    {
        public static XzaarSyntaxToken[] AvailableTokens = new[]
        {
            new XzaarSyntaxToken(XzaarNodeTypes.ACCESSOR, "INSTANCE", "."),
            new XzaarSyntaxToken(XzaarNodeTypes.ACCESSOR, "REFERENCE", "->"),
            new XzaarSyntaxToken(XzaarNodeTypes.ACCESSOR, "STATIC", "::"),
            new XzaarSyntaxToken(XzaarNodeTypes.SEPARATOR, "COMMA", ","),
            new XzaarSyntaxToken(XzaarNodeTypes.COLON, "SEMI_COLON", ";"),
            new XzaarSyntaxToken(XzaarNodeTypes.COLON, "COLON", ":"),
            new XzaarSyntaxToken(XzaarNodeTypes.PARAN, "LPARAN", "("),
            new XzaarSyntaxToken(XzaarNodeTypes.PARAN, "RPARAN", ")"),
            new XzaarSyntaxToken(XzaarNodeTypes.BRACE, "LBRACE", "{"),
            new XzaarSyntaxToken(XzaarNodeTypes.BRACE, "RBRACE", "}"),
            new XzaarSyntaxToken(XzaarNodeTypes.BRACKET, "LBRACKET", "["),
            new XzaarSyntaxToken(XzaarNodeTypes.BRACKET, "RBRACKET", "]"),
            new XzaarSyntaxToken(XzaarNodeTypes.NULL_COALESCING_OPERATOR, "QUESTION_QUESTION", "??"),
            new XzaarSyntaxToken(XzaarNodeTypes.LAMBDA_OPERATOR, "EQUALS_GREATER", "=>"),
            new XzaarSyntaxToken(XzaarNodeTypes.UNARY_OPERATOR,"PLUS_PLUS", "++"),
            new XzaarSyntaxToken(XzaarNodeTypes.UNARY_OPERATOR,"MINUS_MINUS", "--"),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"PLUS_EQUALS", "+="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"MULTIPLY_EQUALS", "*="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"DIVIDE_EQUALS", "/="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"MINUS_EQUALS", "-="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"ASSIGN", "="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"AND_EQUALS","&="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"OR_EQUALS","|="),
            new XzaarSyntaxToken(XzaarNodeTypes.ASSIGNMENT_OPERATOR,"XOR_EQUALS","^="),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR, "QUESTION", "?"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"OR", "or"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"AND", "&&"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"AND", "and"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"OR", "||"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"OR", "or"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"AND", "&&"),
            new XzaarSyntaxToken(XzaarNodeTypes.CONDITIONAL_OPERATOR,"AND", "and"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"EQUALS","eq"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"EQUALS","eq"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"EQUALS","=="),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"NOT_EQUALS","!="),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"GREATER","gt"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"GREATER",">"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"GREATER_EQUALS","gte"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"GREATER_EQUALS",">="),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"LESS","lt"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"LESS","<"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"LESS_EQUALS","lte"),
            new XzaarSyntaxToken(XzaarNodeTypes.EQUALITY_OPERATOR,"LESS_EQUALS","<="),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR, "LSHIFT","<<"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR, "RSHIFT",">>"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR, "BITWISE_AND","&"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR, "BITWISE_OR","|"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR, "BITWISE_XOR","^"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR, "MULTIPLY","*"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR,"DIVIDE","/"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR,"MINUS","-"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR,"PLUS","+"),
            new XzaarSyntaxToken(XzaarNodeTypes.ARITHMETIC_OPERATOR,"MOD","%"),
            new XzaarSyntaxToken(XzaarNodeTypes.NEGATE_OPERATOR, "NOT", "!"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "EXTERN", "extern"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "FUNCTION", "fn"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "STRUCT", "struct"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "IF", "if"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "ELSE", "else"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "WHILE", "while"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "DO", "do"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "FOR", "for"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "FOREACH", "foreach"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "IN", "in"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "SWITCH", "switch"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "CASE", "case"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "MATCH", "match"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "LOOP", "loop"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "VARIABLE", "let"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "VARIABLE", "var"),

            // flow control
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "RETURN", "return"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "GOTO", "goto"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "CONTINUE", "continue"),
            new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "BREAK", "break"),
        };

        public static XzaarSyntaxToken Get(char value) => Get(value.ToString());
        public static XzaarSyntaxToken Get(string value)
        {
            var token = AvailableTokens.FirstOrDefault(t => t.Value == value);
            if (token != null) return token;
            return new XzaarSyntaxToken(XzaarNodeTypes.LITERAL, "NAME", value);
        }
    }
}