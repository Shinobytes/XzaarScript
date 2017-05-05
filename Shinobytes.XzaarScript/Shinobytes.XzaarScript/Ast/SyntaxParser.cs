using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class SyntaxParser
    {
        private readonly SyntaxTokenStream tokenStream;
        private readonly List<string> errors = new List<string>();
        private int currentNodeIndex = 0;

        public SyntaxParser(IList<Token> tokens)
        {
            this.tokenStream = new SyntaxTokenStream(tokens);
        }

        public bool HasErrors => errors.Count > 0;
        public IList<string> Errors => errors;

        public SyntaxNode Parse()
        {
            Reset();

            var entry = new SyntaxNode(NextIndex(), SyntaxKind.Scope, SyntaxKind.Scope)
            {
                TrailingToken = tokenStream.Current,
                LeadingToken = tokenStream.PeekNext()
            };

            var currentToken = tokenStream.Current;
            while (!tokenStream.EndOfStream())
            {
                var node = WalkToken(currentToken);
                if (node != null)
                {
                    if (node.TrailingToken == null)
                        node.TrailingToken = currentToken;
                    if (node.LeadingToken == null)
                        node.LeadingToken = tokenStream.PeekNext();
                    entry.AddChild(node);
                }
                currentToken = tokenStream.Next();
            }
            return entry;
        }

        private SyntaxNode WalkToken(Token token)
        {
            if (token.Type == SyntaxKind.RightBracket) return Error("Unexpected ending bracket ']'", token);
            if (token.Type == SyntaxKind.RightParan) return Error("Unexpected ending paranthesis ')'", token);
            if (token.Type == SyntaxKind.RightCurly) return Error("Unexpected ending curly bracket '}'", token);

            if (token.Type == SyntaxKind.LeftParan) return WalkExpression(token);
            if (token.Type == SyntaxKind.LeftBracket) return WalkBracketExpression(token);
            if (token.Type == SyntaxKind.LeftCurly) return WalkScope(token);

            object outputValue = token.Value;

            switch (token.Type)
            {
                case SyntaxKind.Number:
                    if (outputValue.ToString().Contains("."))
                        outputValue = double.Parse(outputValue.ToString(), CultureInfo.InvariantCulture);
                    else if (outputValue.ToString().ToLower().StartsWith("0x"))
                        outputValue = Convert.ToInt32(outputValue.ToString().Substring(2), 16);
                    else
                        outputValue = int.Parse(outputValue.ToString());
                    return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Literal, SyntaxKind.LiteralNumber)
                    {
                        TrailingToken = tokenStream.PeekPrevious(),
                        LeadingToken = tokenStream.PeekNext()
                    };

                case SyntaxKind.String:
                    return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Literal, SyntaxKind.LiteralString)
                    {
                        TrailingToken = tokenStream.PeekPrevious(),
                        LeadingToken = tokenStream.PeekNext()
                    };
                case SyntaxKind.Identifier:
                    {
                        if (SyntaxFacts.IsKeyword(outputValue + "")) return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Keyword, SyntaxFacts.GetKeywordSubType(outputValue + ""))
                        {
                            TrailingToken = tokenStream.PeekPrevious(),
                            LeadingToken = tokenStream.PeekNext()
                        };
                        // if (SyntaxFacts.IsKnownConstant(outputValue + "")) return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Constant, SyntaxKind.Constant);
                        return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Identifier,
                            SyntaxKind.Identifier)
                        {
                            TrailingToken = tokenStream.PeekPrevious(),
                            LeadingToken = tokenStream.PeekNext()
                        };
                    }
                default:
                    {
                        return OperatorNode(NextIndex(), outputValue, token.Type);
                        // return new XzaarNode(NextIndex(), outputValue, NodeType.Identifier);
                    }
            }
        }

        private SyntaxNode OperatorNode(int nextIndex, object outputValue, SyntaxKind tokenType)
        {
            var nodeType = SyntaxFacts.GetNodeType(tokenType);
            var nodeSubType = SyntaxFacts.GetSubKind(nodeType, tokenType);
            return new SyntaxNode(nextIndex, outputValue, nodeType, nodeSubType)
            {
                TrailingToken = tokenStream.PeekPrevious(),
                LeadingToken = tokenStream.PeekNext()
            };
        }


        private SyntaxNode WalkScope(Token trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending '}' was found.", trailingToken);
            return WalkRecursive(trailingToken, SyntaxKind.Scope, SyntaxKind.RightCurly);
        }


        private SyntaxNode WalkBracketExpression(Token trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ']' was found.", trailingToken);
            return WalkRecursive(trailingToken, SyntaxKind.ArrayIndexExpression, SyntaxKind.RightBracket);
        }

        private SyntaxNode WalkExpression(Token trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ')' was found.", trailingToken);
            return WalkRecursive(trailingToken, SyntaxKind.Expression, SyntaxKind.RightParan);
        }

        private SyntaxNode WalkRecursive(Token trailingToken, SyntaxKind type, SyntaxKind endingTokenType)
        {
            var node = new SyntaxNode(NextIndex(), type, type);
            node.TrailingToken = trailingToken;
            var token = tokenStream.Next();
            while (!tokenStream.EndOfStream() && token != null && token.Type != endingTokenType)
            {
                var n = WalkToken(token);
                if (n != null)
                {
                    node.AddChild(n);
                }
                token = tokenStream.Next();
                if (token != null && token.Type == endingTokenType)
                {
                    node.LeadingToken = token;
                }
            }

            return node;
        }

        private SyntaxNode Error(string message, Token trailingToken)
        {
            if (trailingToken != null)
                message += ". At line " + trailingToken.Line;
            this.errors.Add("[Error] " + message);
            return null;
        }

        private int NextIndex()
        {
            return currentNodeIndex++;
        }

        private void Reset()
        {
            currentNodeIndex = 0;
        }
    }
}

