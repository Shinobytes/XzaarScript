using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser
{
    public class Lexer
    {
        private readonly string code;
        private readonly StringCollectionStream codeCollectionStream;
        private readonly List<string> errors = new List<string>();
        private char[] validHexChars = "0123456789abcdef".ToCharArray();

        public int line = 1;
        public int column = 1;

        private int tokenStart = 0;
        private bool verbose;

        public Lexer(string code)
        {
            this.code = code;
            this.codeCollectionStream = new StringCollectionStream(this.code.ToCharArray());
        }

        public IList<Token> Tokenize(bool verbose = false)
        {
            this.verbose = verbose;
            var tokens = new List<Token>();
            var currentChar = codeCollectionStream.Current;
            while (!codeCollectionStream.EndOfStream())
            {
                var token = Tokenize(currentChar);
                if (token != null) tokens.Add(token);
                currentChar = codeCollectionStream.Next();
            }
            return tokens;
        }

        private Token Tokenize(char symbol)
        {
            tokenStart = codeCollectionStream.Index;
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
                case '(': return Token(SyntaxKind.LeftParan);
                case ')': return Token(SyntaxKind.RightParan);
                case '[': return Token(SyntaxKind.LeftBracket);
                case ']': return Token(SyntaxKind.RightBracket);
                case '{': return Token(SyntaxKind.LeftCurly);
                case '}': return Token(SyntaxKind.RightCurly);
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

        private Token WalkIdentifier()
        {
            while (!codeCollectionStream.EndOfStream() && (IsIdentifier(codeCollectionStream.Current) || IsNumber(codeCollectionStream.Current)))
            {
                if (!IsIdentifier(codeCollectionStream.PeekNext()) && !IsNumber(codeCollectionStream.PeekNext())) break;
                codeCollectionStream.Next();
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

        private Token WalkNumber()
        {
            var isFloatingPoint = false;

            while (!codeCollectionStream.EndOfStream())
            {
                var next = codeCollectionStream.PeekNext();
                if (next == '.')
                {
                    if (isFloatingPoint) return Token(SyntaxKind.Number);
                    isFloatingPoint = true;

                    if (!IsNumber(codeCollectionStream.PeekAt(2)))
                    {
                        break;
                    }

                    codeCollectionStream.Next();
                    next = codeCollectionStream.PeekNext();
                    if (next == '.') return Error("Unexpected '.' found in middle of a number");

                    continue;
                }

                if (next == 'x')
                {
                    return WalkHexNumber();
                }

                if (!IsNumber(next)) break;
                codeCollectionStream.Next();
            }

            return Token(SyntaxKind.Number);
        }

        private Token WalkHexNumber()
        {
            var v0 = codeCollectionStream.Current;
            var vX = codeCollectionStream.PeekNext();
            if (v0 != '0' || char.ToLower(vX) != 'x') return Error("Invalid hexadecimal constant value");
            codeCollectionStream.Advance(2);
            while (!codeCollectionStream.EndOfStream())
            {
                if (!validHexChars.Contains(codeCollectionStream.PeekNext())) break;
                codeCollectionStream.Next();
            }
            return Token(SyntaxKind.Number);
        }

        private Token WalkDoubleQuouteString()
        {
            while (!codeCollectionStream.EndOfStream() && codeCollectionStream.PeekNext() != '"')
            {
                var value = codeCollectionStream.Next();
                if (value == '\\' && codeCollectionStream.PeekNext() == '"')
                {
                    codeCollectionStream.Next();
                    continue;
                }
                if (value == '\n') line++;
            }
            codeCollectionStream.Advance(1);
            return Token(SyntaxKind.String, 1, -2);
        }

        private Token WalkSingleQuouteString()
        {
            while (!codeCollectionStream.EndOfStream() && codeCollectionStream.PeekNext() != '\'')
            {
                var value = codeCollectionStream.Next();
                if (value == '\\' && codeCollectionStream.PeekNext() == '\'')
                {
                    codeCollectionStream.Next();
                    continue;
                }
                if (value == '\n') line++;
            }
            codeCollectionStream.Advance(1);
            return Token(SyntaxKind.String, 1, -2);
        }

        private Token WalkMultilineComment()
        {
            while (!codeCollectionStream.EndOfStream())
            {
                var value = codeCollectionStream.Next();
                if (value == '\n')
                {
                    line++;
                }
                if (codeCollectionStream.PeekNext() == '*'
                    && codeCollectionStream.PeekAt(2) == '/')
                {
                    codeCollectionStream.Advance(2);
                    if (verbose) return Token(SyntaxKind.CommentMultiLine);
                    return null;
                }
            }
            if (verbose) return Token(SyntaxKind.CommentMultiLine);
            return null;
        }

        private Token WalkSingleLineComment()
        {
            while (!codeCollectionStream.EndOfStream() && codeCollectionStream.PeekNext() != '\n') codeCollectionStream.Next();
            if (verbose) return Token(SyntaxKind.CommentSingleLine);
            return null;
        }

        private Token Token(SyntaxKind type, int i1 = 0, int i2 = 0)
        {
            try
            {
                var tokenLength = codeCollectionStream.Index - tokenStart;
                var tokenValue = code.Substring(tokenStart + i1, tokenLength + i2 + 1);
                column += tokenLength;
                var charString = code.Substring(tokenStart, 1) == "'";
                return new Token(type, tokenValue)
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
            return codeCollectionStream.MatchNext(symbol) == symbol;
        }

        public bool HasErrors => errors.Count > 0;
        public IList<string> Errors => errors;

        private Token Error(string message)
        {
            this.errors.Add("[SyntaxError]: " + message + ". At line " + line);
            return null;
        }
    }
}