using System.Linq;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class Tokens
    {
        public static SyntaxToken[] AvailableTokens = new[]
        {
            new SyntaxToken(NodeTypes.ACCESSOR, "INSTANCE", "."),
            new SyntaxToken(NodeTypes.ACCESSOR, "REFERENCE", "->"),
            new SyntaxToken(NodeTypes.ACCESSOR, "STATIC", "::"),
            new SyntaxToken(NodeTypes.SEPARATOR, "COMMA", ","),
            new SyntaxToken(NodeTypes.COLON, "SEMI_COLON", ";"),
            new SyntaxToken(NodeTypes.COLON, "COLON", ":"),
            new SyntaxToken(NodeTypes.PARAN, "LPARAN", "("),
            new SyntaxToken(NodeTypes.PARAN, "RPARAN", ")"),
            new SyntaxToken(NodeTypes.BRACE, "LBRACE", "{"),
            new SyntaxToken(NodeTypes.BRACE, "RBRACE", "}"),
            new SyntaxToken(NodeTypes.BRACKET, "LBRACKET", "["),
            new SyntaxToken(NodeTypes.BRACKET, "RBRACKET", "]"),
            new SyntaxToken(NodeTypes.NULL_COALESCING_OPERATOR, "QUESTION_QUESTION", "??"),
            new SyntaxToken(NodeTypes.LAMBDA_OPERATOR, "EQUALS_GREATER", "=>"),
            new SyntaxToken(NodeTypes.UNARY_OPERATOR,"PLUS_PLUS", "++"),
            new SyntaxToken(NodeTypes.UNARY_OPERATOR,"MINUS_MINUS", "--"),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"PLUS_EQUALS", "+="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"MULTIPLY_EQUALS", "*="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"DIVIDE_EQUALS", "/="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"MINUS_EQUALS", "-="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"ASSIGN", "="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"AND_EQUALS","&="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"OR_EQUALS","|="),
            new SyntaxToken(NodeTypes.ASSIGNMENT_OPERATOR,"XOR_EQUALS","^="),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR, "QUESTION", "?"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"OR", "or"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"AND", "&&"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"AND", "and"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"OR", "||"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"OR", "or"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"AND", "&&"),
            new SyntaxToken(NodeTypes.CONDITIONAL_OPERATOR,"AND", "and"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"EQUALS","eq"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"EQUALS","eq"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"EQUALS","=="),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"NOT_EQUALS","!="),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"GREATER","gt"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"GREATER",">"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"GREATER_EQUALS","gte"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"GREATER_EQUALS",">="),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"LESS","lt"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"LESS","<"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"LESS_EQUALS","lte"),
            new SyntaxToken(NodeTypes.EQUALITY_OPERATOR,"LESS_EQUALS","<="),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR, "LSHIFT","<<"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR, "RSHIFT",">>"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR, "BITWISE_AND","&"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR, "BITWISE_OR","|"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR, "BITWISE_XOR","^"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR, "MULTIPLY","*"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR,"DIVIDE","/"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR,"MINUS","-"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR,"PLUS","+"),
            new SyntaxToken(NodeTypes.ARITHMETIC_OPERATOR,"MOD","%"),
            new SyntaxToken(NodeTypes.NOT_OPERATOR, "NOT", "!"),
            new SyntaxToken(NodeTypes.LITERAL, "EXTERN", "extern"),
            new SyntaxToken(NodeTypes.LITERAL, "FUNCTION", "fn"),
            new SyntaxToken(NodeTypes.LITERAL, "STRUCT", "struct"),
            new SyntaxToken(NodeTypes.LITERAL, "IF", "if"),
            new SyntaxToken(NodeTypes.LITERAL, "ELSE", "else"),
            new SyntaxToken(NodeTypes.LITERAL, "WHILE", "while"),
            new SyntaxToken(NodeTypes.LITERAL, "DO", "do"),
            new SyntaxToken(NodeTypes.LITERAL, "FOR", "for"),
            new SyntaxToken(NodeTypes.LITERAL, "FOREACH", "foreach"),
            new SyntaxToken(NodeTypes.LITERAL, "IN", "in"),
            new SyntaxToken(NodeTypes.LITERAL, "SWITCH", "switch"),
            new SyntaxToken(NodeTypes.LITERAL, "CASE", "case"),
            new SyntaxToken(NodeTypes.LITERAL, "MATCH", "match"),
            new SyntaxToken(NodeTypes.LITERAL, "LOOP", "loop"),
            new SyntaxToken(NodeTypes.LITERAL, "VARIABLE", "let"),
            new SyntaxToken(NodeTypes.LITERAL, "VARIABLE", "var"),

            // flow control
            new SyntaxToken(NodeTypes.LITERAL, "RETURN", "return"),
            new SyntaxToken(NodeTypes.LITERAL, "GOTO", "goto"),
            new SyntaxToken(NodeTypes.LITERAL, "CONTINUE", "continue"),
            new SyntaxToken(NodeTypes.LITERAL, "BREAK", "break"),
        };

        public static SyntaxToken Get(char value) => Get(value.ToString());
        public static SyntaxToken Get(string value)
        {
            var token = AvailableTokens.FirstOrDefault(t => t.Value == value);
            if (token != null) return token;
            return new SyntaxToken(NodeTypes.LITERAL, "NAME", value);
        }
    }
}