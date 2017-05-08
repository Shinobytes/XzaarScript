using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class SyntaxParser
    {
        private readonly TokenStream tokenStream;
        private readonly List<string> errors = new List<string>();
        private int currentNodeIndex = 0;

        public SyntaxParser(IList<SyntaxToken> tokens)
        {
            this.tokenStream = new TokenStream(tokens);
        }

        public bool HasErrors => errors.Count > 0;
        public IList<string> Errors => errors;

        public SyntaxNode Parse()
        {
            Reset();

            var entry = new SyntaxNode(NextIndex(), SyntaxKind.Block)
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

        private SyntaxNode WalkToken(SyntaxToken token)
        {
            if (token.Kind == SyntaxKind.CloseBracket) return Error("Unexpected ending bracket ']'", token);
            if (token.Kind == SyntaxKind.CloseParan) return Error("Unexpected ending paranthesis ')'", token);
            if (token.Kind == SyntaxKind.CloseCurly) return Error("Unexpected ending curly bracket '}'", token);

            if (token.Kind == SyntaxKind.OpenParan) return WalkExpression(token);
            if (token.Kind == SyntaxKind.OpenBracket) return WalkBracketExpression(token);
            if (token.Kind == SyntaxKind.OpenCurly) return WalkScope(token);

            object outputValue = token.Value;

            switch (token.Kind)
            {
                case SyntaxKind.Number:
                    if (outputValue.ToString().Contains("."))
                        outputValue = double.Parse(outputValue.ToString(), CultureInfo.InvariantCulture);
                    else if (outputValue.ToString().ToLower().StartsWith("0x"))
                        outputValue = Convert.ToInt32(outputValue.ToString().Substring(2), 16);
                    else
                        outputValue = int.Parse(outputValue.ToString());
                    return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Number)
                    {
                        TrailingToken = tokenStream.PeekPrevious(),
                        LeadingToken = tokenStream.PeekNext()
                    };

                case SyntaxKind.String:
                    return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.String)
                    {
                        TrailingToken = tokenStream.PeekPrevious(),
                        LeadingToken = tokenStream.PeekNext()
                    };
                case SyntaxKind.Identifier:
                    {
                        if (SyntaxFacts.IsKeyword(outputValue + "")) return new SyntaxNode(NextIndex(), outputValue, SyntaxFacts.GetKeywordSubType(outputValue + ""))
                        {
                            TrailingToken = tokenStream.PeekPrevious(),
                            LeadingToken = tokenStream.PeekNext()
                        };
                        // if (SyntaxFacts.IsKnownConstant(outputValue + "")) return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Constant, SyntaxKind.Constant);
                        return new SyntaxNode(NextIndex(), outputValue, SyntaxKind.Identifier)
                        {
                            TrailingToken = tokenStream.PeekPrevious(),
                            LeadingToken = tokenStream.PeekNext()
                        };
                    }
                default:
                    {
                        return OperatorNode(NextIndex(), outputValue, token.Kind);
                        // return new XzaarNode(NextIndex(), outputValue, Kind.Identifier);
                    }
            }
        }

        private SyntaxNode OperatorNode(int nextIndex, object outputValue, SyntaxKind tokenType)
        {
            var nodeType = SyntaxFacts.GetNodeType(tokenType);
            return new SyntaxNode(nextIndex, outputValue, nodeType)
            {
                TrailingToken = tokenStream.PeekPrevious(),
                LeadingToken = tokenStream.PeekNext()
            };
        }


        private SyntaxNode WalkScope(SyntaxToken trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending '}' was found.", trailingToken);
            return WalkRecursive(trailingToken, SyntaxKind.Block, SyntaxKind.CloseCurly);
        }


        private SyntaxNode WalkBracketExpression(SyntaxToken trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ']' was found.", trailingToken);
            return WalkRecursive(trailingToken, SyntaxKind.ArrayIndexExpression, SyntaxKind.CloseBracket);
        }

        private SyntaxNode WalkExpression(SyntaxToken trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ')' was found.", trailingToken);
            return WalkRecursive(trailingToken, SyntaxKind.Expression, SyntaxKind.CloseParan);
        }

        private SyntaxNode WalkRecursive(SyntaxToken trailingToken, SyntaxKind type, SyntaxKind endingTokenType)
        {
            var node = new SyntaxNode(NextIndex(), type, type);
            node.TrailingToken = trailingToken;
            var token = tokenStream.Next();
            while (!tokenStream.EndOfStream() && token != null && token.Kind != endingTokenType)
            {
                var n = WalkToken(token);
                if (n != null)
                {
                    node.AddChild(n);
                }
                token = tokenStream.Next();
                if (token != null && token.Kind == endingTokenType)
                {
                    node.LeadingToken = token;
                }
            }

            return node;
        }

        private SyntaxNode Error(string message, SyntaxToken trailingToken)
        {
            if (trailingToken != null)
                message += ". At line " + trailingToken.SourceLine;
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

