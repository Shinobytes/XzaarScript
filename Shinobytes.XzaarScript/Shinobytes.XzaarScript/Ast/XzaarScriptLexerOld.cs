//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Shinobytes.XzaarScript.Ast.Nodes;

//namespace Shinobytes.XzaarScript.Ast
//{
//    public class XzaarScriptLexerOld
//    {
//        const string validHexChars = "0123456789abcdef";

//        public IReadOnlyList<XzaarSyntaxToken> Tokenize(string expression)
//        {
//            if (expression == null) throw new ArgumentNullException(nameof(expression));
//            var badNameChars = "(){}[]%*-+<>=/ \t!½§^¨~,;.§\r\n".ToCharArray();

//            var current = 0;
//            var tokens = new XzaarSyntaxTokenCollection();
//            var EOF = expression.Length;
//            var unaryOperator = "";
//            while (current < expression.Length)
//            {
//                var symbol = expression[current];
//                switch (symbol)
//                {

//                    case '\n':
//                        {
//                            current++;
//                            tokens.AdvanceSourceNewLine();
//                        }
//                        continue;
//                    case '\r':
//                        {
//                            tokens.AdvanceSourcePosition();
//                            current++;
//                        }
//                        continue;

//                    case '?':
//                        if (expression[current + 1] == symbol)
//                        {
//                            tokens.Add(XzaarExpressionTokens.Get("??"));
//                            current += 2;
//                        }
//                        else
//                        {
//                            tokens.Add(XzaarExpressionTokens.Get(symbol));
//                            current++;
//                        }
//                        continue;
//                    case '|':
//                    case '&':
//                        {
//                            if (expression[current + 1] == symbol)
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol + "" + symbol));
//                                current += 2;
//                            }
//                            else if (expression[current + 1] == '=')
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol + "="));
//                                current += 2;
//                            }
//                            else
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol));
//                                current++;
//                            }
//                        }
//                        continue;
//                    case '+':
//                    case '-':
//                        {
//                            // check for unary token
//                            var next = expression[current + 1];
//                            if (next == symbol)
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol + "" + symbol));
//                                current += 2;
//                                continue;
//                            }
//                            // is it a char?
//                            if (char.IsDigit(next)) // only allow it if there is no space afterwards. to allow: -a - b and -a - -b AND if previous 
//                            {

//                                if (tokens.Count > 0)
//                                {
//                                    var lastToken = tokens.Last();
//                                    if (lastToken.Type == "NUMBER")
//                                    {
//                                        // last token was a number,
//                                        // that means we are trying to do something as: 0 (-1)
//                                        // which is an invalid expression. This is a normal + or -
//                                        goto case '=';
//                                    }
//                                }

//                                unaryOperator = symbol.ToString();
//                                symbol = expression[++current];
//                                goto case '0';
//                            }
//                            if (symbol == '-' && next == '>')
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get("->"));
//                                current += 2;
//                                continue;
//                            }
//                            goto case '=';
//                        }
//                    case '=':
//                    case '/':
//                    case '%':
//                    case '*':
//                    case '!':
//                    case '>':
//                    case '<':
//                    case '^':
//                        {
//                            var next = expression[current + 1];
//                            if (symbol == '/' && next == symbol)
//                            {
//                                WalkSingleLineComment(ref current, expression, tokens);
//                                continue;
//                            }
//                            if (symbol == '/' && next == '*')
//                            {
//                                WalkMultiLineComment(ref current, expression, tokens);
//                                continue;
//                            }
//                            if (symbol == '=' && next == '>')
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get("=>"));
//                                current += 2;
//                            }
//                            else if ((symbol == '<' || symbol == '>') && next == symbol)
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol + "" + symbol));
//                                current += 2;
//                            }
//                            else if (next == '=')
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol + "="));
//                                current += 2;
//                            }
//                            else
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get(symbol));
//                                current++;
//                            }
//                        }
//                        continue;
//                    case ':':
//                        {
//                            var next = expression[current + 1];
//                            if (next == symbol)
//                            {
//                                tokens.Add(XzaarExpressionTokens.Get("::"));
//                                current += 2;
//                                continue;
//                            }
//                            goto case '(';
//                        }
//                    case '.':
//                    case ',':
//                    case ';':
//                    case ')':
//                    case '(':
//                    case '{':
//                    case '}':
//                    case ']':
//                    case '[':
//                        current++;
//                        tokens.Add(XzaarExpressionTokens.Get(symbol));
//                        continue;
//                    case '\'':
//                    case '"':
//                        {
//                            // ignoring backslash right now
//                            var value = "";
//                            var expected = symbol; // expect to end as the same as we start with

//                            if (current + 1 < EOF)
//                            {
//                                var isSpecial = false;
//                                do
//                                {
//                                    symbol = expression[current + 1];
//                                    if (symbol == expected && !isSpecial)
//                                        break;

//                                    if (isSpecial && (symbol == '\\' || symbol == expected))
//                                    {
//                                        value += symbol;
//                                        isSpecial = false;
//                                    }
//                                    else if (!isSpecial && symbol == '\\')
//                                    {
//                                        isSpecial = true;
//                                    }
//                                    else if (!isSpecial)
//                                    {
//                                        value += symbol;
//                                    }

//                                    current++;
//                                    if (current + 1 >= EOF) break;
//                                }
//                                while (true);

//                                current += 2;
//                            }

//                            tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "STRING", value));
//                        }
//                        continue;
//                    case '0':
//                    case '1':
//                    case '2':
//                    case '3':
//                    case '4':
//                    case '5':
//                    case '6':
//                    case '7':
//                    case '8':
//                    case '9':
//                        {
//                            var isHex = false;
//                            // ignoring floating point numbers
//                            var value = unaryOperator + symbol;
//                            if (current + 1 < EOF)
//                            {
//                                var isFloatingPoint = false;
//                                do
//                                {
//                                    symbol = expression[current + 1];
//                                    if (IsArithmeticOperator(symbol))
//                                    {
//                                        // break it short, this is the end of the number.
//                                        break;
//                                    }
//                                    if (symbol == '.' && isFloatingPoint)
//                                    {
//                                        // if we are already inside a 'floating point' parsing, then it means we
//                                        // are probably trying to use an accessor. break free now!
//                                        break;
//                                    }

//                                    if (symbol == 'x')
//                                    {
//                                        // hexadecimal
//                                        WalkHexValue(ref current, expression, tokens);
//                                        isHex = true;
//                                        break;
//                                    }

//                                    if (symbol == '.')
//                                    {
//                                        // check if we end the dot with a number. otherwise this is an accessor to that object                                        
//                                        if (current + 2 < expression.Length && !char.IsDigit(expression[current + 2]))
//                                        {
//                                            break;
//                                        }

//                                        value += symbol;
//                                        isFloatingPoint = true;
//                                    }

//                                    if (char.IsDigit(symbol))
//                                    {
//                                        value += symbol;
//                                    }

//                                    if (!char.IsDigit(symbol) && symbol != '.')
//                                    {
//                                        break;
//                                    }

//                                    current++;
//                                    if (current + 1 >= EOF) break;
//                                } while (current < expression.Length);
//                            }
//                            if (isHex) continue;
//                            current++;
//                            tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "NUMBER", value));
//                            unaryOperator = "";
//                        }
//                        continue;
//                    case '\t':
//                    case ' ':
//                        tokens.AdvanceSourcePosition();
//                        current++;
//                        continue;
//                    default:
//                        if (!badNameChars.Contains(symbol))
//                        {
//                            var value = symbol + "";

//                            if (current + 1 < EOF)
//                            {
//                                if (badNameChars.Contains(expression[current + 1]))
//                                {
//                                    tokens.Add(XzaarExpressionTokens.Get(symbol.ToString()));
//                                    current++;
//                                    continue;
//                                }
//                                while (!badNameChars.Contains(symbol = expression[current + 1]))
//                                {
//                                    value += symbol;
//                                    current++;
//                                    if (current + 1 >= EOF) break;
//                                }
//                            }
//                            current++;
//                            tokens.Add(XzaarExpressionTokens.Get(value));
//                            continue;
//                        }
//                        break;
//                }

//                throw new XzaarLexerException($"Invalid token type: {symbol}");

//            }

//            return tokens;
//        }

//        private void WalkHexValue(ref int current, string expression, XzaarSyntaxTokenCollection tokens)
//        {
//            var hexValue = "0x";
//            var v0 = expression[current++];
//            var vX = expression[current++];
//            if (v0 != '0' || char.ToLower(vX) != 'x') throw new XzaarLexerException("Invalid hexadecimal constant value");
//            var hexChars = validHexChars.ToCharArray();
//            while (current < expression.Length)
//            {
//                var v = char.ToLower(expression[current]);
//                if (!hexChars.Contains(v)) break;
//                hexValue += v;
//                current++;
//            }
//            tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.LITERAL, "NUMBER", hexValue));
//        }

//        private void WalkMultiLineComment(ref int current, string expr, XzaarSyntaxTokenCollection tokens)
//        {
//            var comment = "";
//            while (current + 1 < expr.Length)
//            {
//                var next = expr[current + 1];
//                if (next == '*' && current + 2 < expr.Length)
//                {
//                    if (expr[current + 2] == '/')
//                    {
//                        tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.COMMENT, "COMMENT", comment));
//                        current += 3;
//                        tokens.AdvanceSourcePosition(3);
//                        return;
//                    }
//                }
//                comment += next;
//                tokens.AdvanceSourcePosition();
//                current++;
//            }
//            tokens.AdvanceSourcePosition();
//            current++;
//            tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.COMMENT, "COMMENT", comment));
//        }

//        private void WalkSingleLineComment(ref int current, string expr, XzaarSyntaxTokenCollection tokens)
//        {
//            var comment = "";
//            current++;
//            tokens.AdvanceSourcePosition();
//            while (current + 1 < expr.Length)
//            {
//                var next = expr[current + 1];
//                if (next == '\n')
//                {
//                    tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.COMMENT, "COMMENT", comment));
//                    current += 2;
//                    tokens.AdvanceSourcePosition();
//                    tokens.AdvanceSourceNewLine();
//                    return;
//                }
//                comment += next;
//                tokens.AdvanceSourcePosition();
//                current++;
//            }
//            tokens.AdvanceSourcePosition();
//            current++;
//            tokens.Add(new XzaarSyntaxToken(XzaarAstNodeTypes.COMMENT, "COMMENT", comment));
//        }

//        private bool IsArithmeticOperator(char op)
//        {
//            return op == '+' || op == '-' || op == '*' || op == '/' || op == '%';
//        }
//    }
//}