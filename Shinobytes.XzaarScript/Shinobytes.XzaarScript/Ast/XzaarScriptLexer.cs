using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarScriptLexer
    {
        private readonly string code;
        private readonly XzaarStringStream codeStream;
        private readonly List<string> errors = new List<string>();
        private char[] validHexChars = "0123456789abcdef".ToCharArray();

        public int line = 1;
        public int column = 1;

        private int tokenStart = 0;
        private bool verbose;

        public XzaarScriptLexer(string code)
        {
            this.code = code;
            this.codeStream = new XzaarStringStream(this.code.ToCharArray());
        }

        public IList<XzaarToken> Tokenize(bool verbose = false)
        {
            this.verbose = verbose;
            var tokens = new List<XzaarToken>();
            var currentChar = codeStream.Current;
            while (!codeStream.EndOfStream())
            {
                var token = Tokenize(currentChar);
                if (token != null) tokens.Add(token);
                currentChar = codeStream.Next();
            }
            return tokens;
        }

        private XzaarToken Tokenize(char symbol)
        {
            tokenStart = codeStream.Index;
            switch (symbol)
            {
                case '/':
                    {
                        if (Match('/')) return WalkSingleLineComment();
                        if (Match('*')) return WalkMultilineComment();
                        return Token(Match('=') ? XzaarSyntaxKind.DivideEquals : XzaarSyntaxKind.Divide);
                    }
                case '\'': return WalkSingleQuouteString();
                case '"': return WalkDoubleQuouteString();
                case '!': return Token(Match('=') ? XzaarSyntaxKind.NotEquals : XzaarSyntaxKind.Not);
                case '=': return Token(Match('=') ? XzaarSyntaxKind.EqualsEquals : Match('>') ? XzaarSyntaxKind.EqualsGreater : XzaarSyntaxKind.Equals);
                case '*': return Token(Match('=') ? XzaarSyntaxKind.MultiplyEquals : XzaarSyntaxKind.Multiply);
                case '%': return Token(Match('=') ? XzaarSyntaxKind.ModuloEquals : XzaarSyntaxKind.Modulo);
                case '-': return Token(Match('-') ? XzaarSyntaxKind.MinusMinus : Match('>') ? XzaarSyntaxKind.MinusGreater : Match('=') ? XzaarSyntaxKind.MinusEquals : XzaarSyntaxKind.Minus);
                case '+': return Token(Match('+') ? XzaarSyntaxKind.PlusPlus : Match('=') ? XzaarSyntaxKind.PlusEquals : XzaarSyntaxKind.Plus);
                case '>': return Token(Match('>') ? Match('=') ? XzaarSyntaxKind.GreaterGreaterEquals : XzaarSyntaxKind.GreaterGreater : Match('=') ? XzaarSyntaxKind.GreaterEquals : XzaarSyntaxKind.Greater);
                case '<': return Token(Match('<') ? Match('=') ? XzaarSyntaxKind.LessLessEquals : XzaarSyntaxKind.LessLess : Match('=') ? XzaarSyntaxKind.LessEquals : XzaarSyntaxKind.Less);
                // case '^': return Token(Match('=') ? XzaarSyntaxKind.Up : XzaarSyntaxKind.Plus);
                case '|': return Token(Match('|') ? XzaarSyntaxKind.OrOr : Match('=') ? XzaarSyntaxKind.OrEquals : XzaarSyntaxKind.Or);
                case '&': return Token(Match('&') ? XzaarSyntaxKind.AndAnd : Match('=') ? XzaarSyntaxKind.AndEquals : XzaarSyntaxKind.And);
                case '?': return Token(Match('?') ? XzaarSyntaxKind.QuestionQuestion : XzaarSyntaxKind.Question);
                case ':': return Token(Match(':') ? XzaarSyntaxKind.ColonColon : XzaarSyntaxKind.Colon);
                case ',': return Token(XzaarSyntaxKind.Comma);
                case '.': return Token(XzaarSyntaxKind.Dot);
                case ';': return Token(XzaarSyntaxKind.Semicolon);
                case '(': return Token(XzaarSyntaxKind.LeftParan);
                case ')': return Token(XzaarSyntaxKind.RightParan);
                case '[': return Token(XzaarSyntaxKind.LeftBracket);
                case ']': return Token(XzaarSyntaxKind.RightBracket);
                case '{': return Token(XzaarSyntaxKind.LeftCurly);
                case '}': return Token(XzaarSyntaxKind.RightCurly);
                case '\n':
                    line++;
                    column = 1;
                    if (verbose) return Token(XzaarSyntaxKind.Newline);
                    return null;
                case ' ':
                case '\r':
                case '\t':
                case '\0': // EOF
                    if (verbose) return Token(XzaarSyntaxKind.Whitespace);
                    return null;
                default:
                    {
                        if (IsNumber(symbol)) return WalkNumber();
                        if (IsIdentifier(symbol)) return WalkIdentifier();
                        return Error("Unexpected token '" + symbol + "' found.");
                    }
            }
        }

        private XzaarToken WalkIdentifier()
        {
            while (!codeStream.EndOfStream() && (IsIdentifier(codeStream.Current) || IsNumber(codeStream.Current)))
            {
                if (!IsIdentifier(codeStream.PeekNext()) && !IsNumber(codeStream.PeekNext())) break;
                codeStream.Next();
            }

            return Token(XzaarSyntaxKind.Identifier);
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

        private XzaarToken WalkNumber()
        {
            var isFloatingPoint = false;

            while (!codeStream.EndOfStream())
            {
                var next = codeStream.PeekNext();
                if (next == '.')
                {
                    if (isFloatingPoint) return Token(XzaarSyntaxKind.Number);
                    isFloatingPoint = true;

                    if (!IsNumber(codeStream.PeekAt(2)))
                    {
                        break;
                    }

                    codeStream.Next();
                    next = codeStream.PeekNext();
                    if (next == '.') return Error("Unexpected '.' found in middle of a number");

                    continue;
                }

                if (next == 'x')
                {
                    return WalkHexNumber();
                }

                if (!IsNumber(next)) break;
                codeStream.Next();
            }

            return Token(XzaarSyntaxKind.Number);
        }

        private XzaarToken WalkHexNumber()
        {
            var v0 = codeStream.Current;
            var vX = codeStream.PeekNext();
            if (v0 != '0' || char.ToLower(vX) != 'x') return Error("Invalid hexadecimal constant value");
            codeStream.Advance(2);
            while (!codeStream.EndOfStream())
            {
                if (!validHexChars.Contains(codeStream.PeekNext())) break;
                codeStream.Next();
            }
            return Token(XzaarSyntaxKind.Number);
        }

        private XzaarToken WalkDoubleQuouteString()
        {
            while (!codeStream.EndOfStream() && codeStream.PeekNext() != '"')
            {
                var value = codeStream.Next();
                if (value == '\\' && codeStream.PeekNext() == '"')
                {
                    codeStream.Next();
                    continue;
                }
                if (value == '\n') line++;
            }
            codeStream.Advance(1);
            return Token(XzaarSyntaxKind.String, 1, -2);
        }

        private XzaarToken WalkSingleQuouteString()
        {
            while (!codeStream.EndOfStream() && codeStream.PeekNext() != '\'')
            {
                var value = codeStream.Next();
                if (value == '\\' && codeStream.PeekNext() == '\'')
                {
                    codeStream.Next();
                    continue;
                }
                if (value == '\n') line++;
            }
            codeStream.Advance(1);
            return Token(XzaarSyntaxKind.String, 1, -2);
        }

        private XzaarToken WalkMultilineComment()
        {
            while (!codeStream.EndOfStream())
            {
                var value = codeStream.Next();
                if (value == '\n')
                {
                    line++;
                }
                if (codeStream.PeekNext() == '*'
                    && codeStream.PeekAt(2) == '/')
                {
                    codeStream.Advance(2);
                    if (verbose) return Token(XzaarSyntaxKind.CommentMultiLine);
                    return null;
                }
            }
            if (verbose) return Token(XzaarSyntaxKind.CommentMultiLine);
            return null;
        }

        private XzaarToken WalkSingleLineComment()
        {
            while (!codeStream.EndOfStream() && codeStream.PeekNext() != '\n') codeStream.Next();
            if (verbose) return Token(XzaarSyntaxKind.CommentSingleLine);
            return null;
        }

        private XzaarToken Token(XzaarSyntaxKind type, int i1 = 0, int i2 = 0)
        {
            try
            {
                var tokenLength = codeStream.Index - tokenStart;
                var tokenValue = code.Substring(tokenStart + i1, tokenLength + i2 + 1);
                column += tokenLength;
                var charString = code.Substring(tokenStart, 1) == "'";
                return new XzaarToken(type, tokenValue)
                {
                    Line = this.line,
                    Column = this.column,
                    IsCharString = charString,
                };
            }
            catch
            {
                return Error("Unexpected end of code. Maybe you forgot to close the '" + type + "'?");
            }
        }

        private bool Match(char symbol)
        {
            return codeStream.MatchNext(symbol) == symbol;
        }

        public bool HasErrors { get { return errors.Count > 0; } }
        public IList<string> Errors { get { return errors; } }

        private XzaarToken Error(string message)
        {
            this.errors.Add("[SyntaxError]: " + message + ". At line " + line);
            return null;
        }
    }
}