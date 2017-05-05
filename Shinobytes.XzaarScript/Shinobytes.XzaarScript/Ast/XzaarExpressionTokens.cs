using System.Linq;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarExpressionTokens
    {
        public static XzaarSyntaxToken[] AvailableTokens = new[]
        {
            new XzaarSyntaxToken(XzaarAstNodeTypes.ACCESSOR, "INSTANCE", "."),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ACCESSOR, "REFERENCE", "->"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ACCESSOR, "STATIC", "::"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.SEPARATOR, "COMMA", ","),
            new XzaarSyntaxToken(XzaarAstNodeTypes.COLON, "SEMI_COLON", ";"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.COLON, "COLON", ":"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.PARAN, "LPARAN", "("),
            new XzaarSyntaxToken(XzaarAstNodeTypes.PARAN, "RPARAN", ")"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.BRACE, "LBRACE", "{"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.BRACE, "RBRACE", "}"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.BRACKET, "LBRACKET", "["),
            new XzaarSyntaxToken(XzaarAstNodeTypes.BRACKET, "RBRACKET", "]"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.NULL_COALESCING_OPERATOR, "QUESTION_QUESTION", "??"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LAMBDA_OPERATOR, "EQUALS_GREATER", "=>"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.UNARY_OPERATOR,"PLUS_PLUS", "++"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.UNARY_OPERATOR,"MINUS_MINUS", "--"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"PLUS_EQUALS", "+="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"MULTIPLY_EQUALS", "*="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"DIVIDE_EQUALS", "/="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"MINUS_EQUALS", "-="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"ASSIGN", "="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"AND_EQUALS","&="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"OR_EQUALS","|="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ASSIGNMENT_OPERATOR,"XOR_EQUALS","^="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR, "QUESTION", "?"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"OR", "or"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"AND", "&&"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"AND", "and"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"OR", "||"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"OR", "or"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"AND", "&&"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.CONDITIONAL_OPERATOR,"AND", "and"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"EQUALS","eq"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"EQUALS","eq"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"EQUALS","=="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"NOT_EQUALS","!="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"GREATER","gt"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"GREATER",">"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"GREATER_EQUALS","gte"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"GREATER_EQUALS",">="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"LESS","lt"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"LESS","<"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"LESS_EQUALS","lte"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.EQUALITY_OPERATOR,"LESS_EQUALS","<="),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR, "LSHIFT","<<"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR, "RSHIFT",">>"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR, "BITWISE_AND","&"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR, "BITWISE_OR","|"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR, "BITWISE_XOR","^"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR, "MULTIPLY","*"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR,"DIVIDE","/"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR,"MINUS","-"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR,"PLUS","+"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.ARITHMETIC_OPERATOR,"MOD","%"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.NOT_OPERATOR, "NOT", "!"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "EXTERN", "extern"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "FUNCTION", "fn"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "STRUCT", "struct"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "IF", "if"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "ELSE", "else"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "WHILE", "while"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "DO", "do"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "FOR", "for"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "FOREACH", "foreach"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "IN", "in"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "SWITCH", "switch"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "CASE", "case"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "MATCH", "match"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "LOOP", "loop"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "VARIABLE", "let"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "VARIABLE", "var"),

            // flow control
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "RETURN", "return"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "GOTO", "goto"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "CONTINUE", "continue"),
            new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "BREAK", "break"),
        };

        public static XzaarSyntaxToken Get(char value) => Get(value.ToString());
        public static XzaarSyntaxToken Get(string value)
        {
            var token = AvailableTokens.FirstOrDefault(t => t.Value == value);
            if (token != null) return token;
            return new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "NAME", value);
        }
    }
}