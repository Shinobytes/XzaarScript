/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public class SyntaxTokenProvider
    {
        public static SyntaxToken[] AvailableTokens;

        static SyntaxTokenProvider()
        {
            AvailableTokens = new[]
            {
                new SyntaxToken(SyntaxKind.MemberAccess, "INSTANCE", "."),
                new SyntaxToken(SyntaxKind.StaticMemberAccess, "STATIC", "::"),
                new SyntaxToken(SyntaxKind.NullConditionalMemberAccess, "QUESTION_DOT", "?."),
                new SyntaxToken(SyntaxKind.MinusGreater, "MINUS_GREATER", "->"),
                new SyntaxToken(SyntaxKind.Comma, "COMMA", ","),
                new SyntaxToken(SyntaxKind.Semicolon, "SEMI_COLON", ";"),
                new SyntaxToken(SyntaxKind.Colon, "COLON", ":"),
                new SyntaxToken(SyntaxKind.OpenParen, "LPARAN", "("),
                new SyntaxToken(SyntaxKind.CloseParen, "RPARAN", ")"),
                new SyntaxToken(SyntaxKind.OpenCurly, "LBRACE", "{"),
                new SyntaxToken(SyntaxKind.CloseCurly, "RBRACE", "}"),
                new SyntaxToken(SyntaxKind.OpenBracket, "LBRACKET", "["),
                new SyntaxToken(SyntaxKind.CloseBracket, "RBRACKET", "]"),
                new SyntaxToken(SyntaxKind.Tilde, "TILDE", "~"),
                new SyntaxToken(SyntaxKind.QuestionQuestion, "QUESTION_QUESTION", "??"),
                new SyntaxToken(SyntaxKind.Caret, "CARET", "^"),
                new SyntaxToken(SyntaxKind.CaretEquals, "CARET_EQUALS", "^="),
                new SyntaxToken(SyntaxKind.EqualsGreater, "EQUALS_GREATER", "=>"),
                new SyntaxToken(SyntaxKind.PlusPlus,"PLUS_PLUS", "++"),
                new SyntaxToken(SyntaxKind.MinusMinus,"MINUS_MINUS", "--"),
                new SyntaxToken(SyntaxKind.PlusEquals,"PLUS_EQUALS", "+="),
                new SyntaxToken(SyntaxKind.MultiplyEquals,"MULTIPLY_EQUALS", "*="),
                new SyntaxToken(SyntaxKind.DivideEquals,"DIVIDE_EQUALS", "/="),
                new SyntaxToken(SyntaxKind.MinusEquals,"MINUS_EQUALS", "-="),
                new SyntaxToken(SyntaxKind.Equals,"ASSIGN", "="),
                new SyntaxToken(SyntaxKind.AndEquals,"AND_EQUALS","&="),
                new SyntaxToken(SyntaxKind.OrEquals,"OR_EQUALS","|="),
                new SyntaxToken(SyntaxKind.Question, "QUESTION", "?"),
                new SyntaxToken(SyntaxKind.AndAnd,"AND", "&&"),
                new SyntaxToken(SyntaxKind.OrOr,"OR", "||"),
                new SyntaxToken(SyntaxKind.EqualsEquals,"EQUALS","=="),
                new SyntaxToken(SyntaxKind.NotEquals,"NOT_EQUALS","!="),
                new SyntaxToken(SyntaxKind.Greater,"GREATER",">"),
                new SyntaxToken(SyntaxKind.GreaterEquals,"GREATER_EQUALS",">="),
                new SyntaxToken(SyntaxKind.Less,"LESS","<"),
                new SyntaxToken(SyntaxKind.LessEquals,"LESS_EQUALS","<="),
                new SyntaxToken(SyntaxKind.LessLess, "LSHIFT","<<"),
                new SyntaxToken(SyntaxKind.GreaterGreater, "RSHIFT",">>"),
                new SyntaxToken(SyntaxKind.BitAnd, "BITWISE_AND","&"),
                new SyntaxToken(SyntaxKind.BitOr, "BITWISE_OR","|"),
                new SyntaxToken(SyntaxKind.BitXor, "BITWISE_XOR","^"),
                new SyntaxToken(SyntaxKind.Multiply, "MULTIPLY","*"),
                new SyntaxToken(SyntaxKind.Divide,"DIVIDE","/"),
                new SyntaxToken(SyntaxKind.Minus,"MINUS","-"),
                new SyntaxToken(SyntaxKind.Plus,"PLUS","+"),
                new SyntaxToken(SyntaxKind.Modulo,"MOD","%"),
                new SyntaxToken(SyntaxKind.Not, "NOT", "!"),
                new SyntaxToken(SyntaxKind.KeywordExtern, "EXTERN", "extern"),
                new SyntaxToken(SyntaxKind.KeywordFn, "FUNCTION", "fn"),
                new SyntaxToken(SyntaxKind.KeywordStruct, "STRUCT", "struct"),
                new SyntaxToken(SyntaxKind.KeywordIf, "IF", "if"),
                new SyntaxToken(SyntaxKind.KeywordElse, "ELSE", "else"),
                new SyntaxToken(SyntaxKind.KeywordWhile, "WHILE", "while"),
                new SyntaxToken(SyntaxKind.KeywordDo, "DO", "do"),
                new SyntaxToken(SyntaxKind.KeywordFor, "FOR", "for"),
                new SyntaxToken(SyntaxKind.KeywordForEach, "FOREACH", "foreach"),
                new SyntaxToken(SyntaxKind.KeywordIn, "IN", "in"),
                new SyntaxToken(SyntaxKind.KeywordSwitch, "SWITCH", "switch"),
                new SyntaxToken(SyntaxKind.KeywordCase, "CASE", "case"),
                new SyntaxToken(SyntaxKind.KeywordMatch, "MATCH", "match"),
                new SyntaxToken(SyntaxKind.KeywordLoop, "LOOP", "loop"),


                new SyntaxToken(SyntaxKind.KeywordDefault, "DEFAULT", "default"),

                new SyntaxToken(SyntaxKind.KeywordIs, "TYPECHECK_IS", "is"),
                new SyntaxToken(SyntaxKind.KeywordAs, "TYPECAST_AS", "as"),

                new SyntaxToken(SyntaxKind.KeywordVar, "VARIABLE", "let"),
                new SyntaxToken(SyntaxKind.KeywordVar, "VARIABLE", "var"),
            

                // Constants
                new SyntaxToken(SyntaxKind.KeywordTrue, "CONSTANT", "true"),
                new SyntaxToken(SyntaxKind.KeywordFalse, "CONSTANT", "false"),
                new SyntaxToken(SyntaxKind.KeywordNull, "CONSTANT", "null"),

                // flow control
                new SyntaxToken(SyntaxKind.KeywordReturn, "RETURN", "return"),
                new SyntaxToken(SyntaxKind.KeywordGoto, "GOTO", "goto"),
                new SyntaxToken(SyntaxKind.KeywordContinue, "CONTINUE", "continue"),
                new SyntaxToken(SyntaxKind.KeywordBreak, "BREAK", "break"),
            };
        }

        public static SyntaxToken Get(char value) => Get(value.ToString());

        public static SyntaxToken Get(SyntaxToken tokenSource)
        {
            var token = AvailableTokens.FirstOrDefault(t => t.Value == tokenSource.Value);
            return token != null
                ? new SyntaxToken(
                    token.Kind,
                    token.TypeName,
                    token.Value,
                    tokenSource.IsSingleQuouteString,
                    tokenSource.TokenIndex,
                    tokenSource.SourceIndex,
                    tokenSource.SourceLine,
                    tokenSource.SourceColumn)
                : new SyntaxToken(SyntaxKind.Identifier, "NAME", tokenSource.Value);
        }

        public static SyntaxToken Get(string value)
        {
            return AvailableTokens.FirstOrDefault(t => t.Value == value) ?? new SyntaxToken(SyntaxKind.Identifier, "NAME", value);
        }
    }
}