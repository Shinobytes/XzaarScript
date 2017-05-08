using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser
{
    public class Lexer
    {
        private readonly string code;
        private readonly StringCollectionStream buffer;
        private readonly List<string> errors = new List<string>();
        private readonly char[] validHexChars = "0123456789abcdef".ToCharArray();
        private readonly char[] validBinChars = "01".ToCharArray();

        public int line = 1;
        public int column = 1;

        private int tokenStart = 0;
        private bool verbose;

        public Lexer(string code, bool verbose = false)
        {
            this.verbose = verbose;
            this.code = code;
            this.buffer = new StringCollectionStream(this.code.ToCharArray());
        }

        public IList<SyntaxToken> Tokenize()
        {

            var tokens = new List<SyntaxToken>();
            var currentChar = buffer.Current;
            while (!buffer.EndOfStream())
            {
                var token = Tokenize(currentChar);
                if (token != null)
                {
                    if (token.Kind != SyntaxKind.String && token.Kind != SyntaxKind.Number)
                    {
                        tokens.Add(SyntaxTokenProvider.Get(token.Value));
                        currentChar = buffer.Next();
                        continue;
                    }
                    tokens.Add(token);
                }
                currentChar = buffer.Next();
            }
            return tokens;
        }

        private SyntaxToken Tokenize(char symbol)
        {
            tokenStart = buffer.Index;
            switch (symbol)
            {
                case '/':
                    {
                        if (Match('/')) return WalkSingleLineComment();
                        if (Match('*')) return WalkMultilineComment();
                        return Token(Match('=') ? SyntaxKind.DivideEquals : SyntaxKind.Divide);
                    }
                case '\'': return WalkSingleQuouteString();
                case '"': return WalkDoubleQuouteString();
                case '!': return Token(Match('=') ? SyntaxKind.NotEquals : SyntaxKind.Not);
                case '=': return Token(Match('=') ? SyntaxKind.EqualsEquals : Match('>') ? SyntaxKind.EqualsGreater : SyntaxKind.Equals);
                case '*': return Token(Match('=') ? SyntaxKind.MultiplyEquals : SyntaxKind.Multiply);
                case '%': return Token(Match('=') ? SyntaxKind.ModuloEquals : SyntaxKind.Modulo);
                case '-': return Token(Match('-') ? SyntaxKind.MinusMinus : Match('>') ? SyntaxKind.MinusGreater : Match('=') ? SyntaxKind.MinusEquals : SyntaxKind.Minus);
                case '+': return Token(Match('+') ? SyntaxKind.PlusPlus : Match('=') ? SyntaxKind.PlusEquals : SyntaxKind.Plus);
                case '>': return Token(Match('>') ? Match('=') ? SyntaxKind.GreaterGreaterEquals : SyntaxKind.GreaterGreater : Match('=') ? SyntaxKind.GreaterEquals : SyntaxKind.Greater);
                case '<': return Token(Match('<') ? Match('=') ? SyntaxKind.LessLessEquals : SyntaxKind.LessLess : Match('=') ? SyntaxKind.LessEquals : SyntaxKind.Less);
                // case '^': return Token(Match('=') ? SyntaxKind.Up : SyntaxKind.Plus);
                case '|': return Token(Match('|') ? SyntaxKind.OrOr : Match('=') ? SyntaxKind.OrEquals : SyntaxKind.Or);
                case '&': return Token(Match('&') ? SyntaxKind.AndAnd : Match('=') ? SyntaxKind.AndEquals : SyntaxKind.And);
                case '?': return Token(Match('?') ? SyntaxKind.QuestionQuestion : SyntaxKind.Question);
                case ':': return Token(Match(':') ? SyntaxKind.ColonColon : SyntaxKind.Colon);
                case ',': return Token(SyntaxKind.Comma);
                case '.': return Token(SyntaxKind.Dot);
                case ';': return Token(SyntaxKind.Semicolon);
                case '(': return Token(SyntaxKind.OpenParan);
                case ')': return Token(SyntaxKind.CloseParan);
                case '[': return Token(SyntaxKind.OpenBracket);
                case ']': return Token(SyntaxKind.CloseBracket);
                case '{': return Token(SyntaxKind.OpenCurly);
                case '}': return Token(SyntaxKind.CloseCurly);
                case '\n':
                    line++;
                    column = 1;
                    if (verbose) return Token(SyntaxKind.Newline);
                    return null;
                case ' ':
                case '\r':
                case '\t':
                case '\0': // EOF
                    if (verbose) return Token(SyntaxKind.Whitespace);
                    return null;
                default:
                    {
                        if (IsNumber(symbol)) return WalkNumber();
                        if (IsIdentifier(symbol)) return WalkIdentifier();
                        return Error("Unexpected token '" + symbol + "' found.");
                    }
            }
        }

        private SyntaxToken WalkIdentifier()
        {
            while (!buffer.EndOfStream() && (IsIdentifier(buffer.Current) || IsNumber(buffer.Current)))
            {
                if (!IsIdentifier(buffer.PeekNext()) && !IsNumber(buffer.PeekNext())) break;
                buffer.Next();
            }

            return Token(SyntaxKind.Identifier);
        }

        private bool IsNumber(char symbol)
        {
            return symbol >= '0' && symbol <= '9';
        }

        private bool IsIdentifier(char symbol)
        {
            var lowered = char.ToLower(symbol);
            return lowered >= 'a' && lowered <= 'z' || lowered == '_' || lowered == '@' || lowered == '$';
        }

        private SyntaxToken WalkNumber()
        {
            var isFloatingPoint = false;

            while (!buffer.EndOfStream())
            {
                var next = buffer.PeekNext();
                if (next == '.')
                {
                    if (isFloatingPoint) return Token(SyntaxKind.Number);
                    isFloatingPoint = true;

                    if (!IsNumber(buffer.PeekAt(2)))
                    {
                        break;
                    }

                    buffer.Next();
                    next = buffer.PeekNext();
                    if (next == '.') return Error("Unexpected '.' found in middle of a number");

                    continue;
                }

                if (next == 'b')
                {
                    return WalkBinaryNumber();
                }

                if (next == 'x')
                {
                    return WalkHexNumber();
                }

                if (!IsNumber(next)) break;
                buffer.Next();
            }

            return Token(SyntaxKind.Number);
        }

        private SyntaxToken WalkBinaryNumber()
        {
            var v0 = buffer.Current;
            var vB = buffer.PeekNext();
            if (v0 != '0' || char.ToLower(vB) != 'b') return Error("Invalid binary constant value");
            buffer.Advance(2);
            while (!buffer.EndOfStream())
            {
                if (!validBinChars.Contains(buffer.PeekNext())) break;
                buffer.Next();
            }
            return Token(SyntaxKind.Number);
        }

        private SyntaxToken WalkHexNumber()
        {
            var v0 = buffer.Current;
            var vX = buffer.PeekNext();
            if (v0 != '0' || char.ToLower(vX) != 'x') return Error("Invalid hexadecimal constant value");
            buffer.Advance(2);
            while (!buffer.EndOfStream())
            {
                if (!validHexChars.Contains(buffer.PeekNext())) break;
                buffer.Next();
            }
            return Token(SyntaxKind.Number);
        }

        private SyntaxToken WalkString(char stringChar)
        {
            while (!buffer.EndOfStream() && buffer.PeekNext() != stringChar)
            {
                var value = buffer.Next();
                if (value == '\\' && buffer.PeekNext() == stringChar)
                {
                    buffer.Next();
                    continue;
                }
                if (value == '\n') line++;
            }
            buffer.Advance(1);
            return Token(SyntaxKind.String, 1, -2);
        }

        private SyntaxToken WalkDoubleQuouteString() => WalkString('"');

        private SyntaxToken WalkSingleQuouteString() => WalkString('\'');

        private SyntaxToken WalkMultilineComment()
        {
            while (!buffer.EndOfStream())
            {
                var value = buffer.Next();
                if (value == '\n') line++;
                if (buffer.PeekNext() == '*' && buffer.PeekAt(2) == '/')
                {
                    buffer.Advance(2);
                    if (verbose) return Token(SyntaxKind.CommentMultiLine);
                    return null;
                }
            }
            if (verbose) return Token(SyntaxKind.CommentMultiLine);
            return null;
        }

        private SyntaxToken WalkSingleLineComment()
        {
            while (!buffer.EndOfStream() && buffer.PeekNext() != '\n') buffer.Next();
            if (verbose) return Token(SyntaxKind.CommentSingleLine);
            return null;
        }

        private SyntaxToken Token(SyntaxKind kind, int i1 = 0, int i2 = 0)
        {
            try
            {
                var tokenLength = buffer.Index - tokenStart;
                var tokenValue = code.Substring(tokenStart + i1, tokenLength + i2 + 1);
                column += tokenLength;
                var charString = code.Substring(tokenStart, 1) == "'";
                return new SyntaxToken(kind, kind.ToString(), tokenValue, charString, 0, 0, this.line, this.column);
            }
            catch
            {
                return Error("Unexpected end of code. Maybe you forgot to close the '" + kind + "'?");
            }
        }

        private bool Match(char symbol)
        {
            return buffer.MatchNext(symbol) == symbol;
        }

        public bool HasErrors => errors.Count > 0;
        public IList<string> Errors => errors;

        private SyntaxToken Error(string message)
        {
            this.errors.Add("[SyntaxError]: " + message + ". At line " + line);
            return null;
        }
    }
}