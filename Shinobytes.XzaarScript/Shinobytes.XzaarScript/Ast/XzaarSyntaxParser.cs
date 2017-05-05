using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarSyntaxParser
    {
        private readonly XzaarSyntaxTokenStream tokenStream;
        private readonly List<string> errors = new List<string>();
        private int currentNodeIndex = 0;

        public XzaarSyntaxParser(IList<XzaarToken> tokens)
        {
            this.tokenStream = new XzaarSyntaxTokenStream(tokens);
        }

        public bool HasErrors { get { return errors.Count > 0; } }
        public IList<string> Errors { get { return errors; } }

        public XzaarSyntaxNode Parse()
        {
            Reset();

            var entry = new XzaarSyntaxNode(NextIndex(), XzaarSyntaxKind.Scope, XzaarSyntaxKind.Scope)
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

        private XzaarSyntaxNode WalkToken(XzaarToken token)
        {
            if (token.Type == XzaarSyntaxKind.RightBracket) return Error("Unexpected ending bracket ']'", token);
            if (token.Type == XzaarSyntaxKind.RightParan) return Error("Unexpected ending paranthesis ')'", token);
            if (token.Type == XzaarSyntaxKind.RightCurly) return Error("Unexpected ending curly bracket '}'", token);

            if (token.Type == XzaarSyntaxKind.LeftParan) return WalkExpression(token);
            if (token.Type == XzaarSyntaxKind.LeftBracket) return WalkBracketExpression(token);
            if (token.Type == XzaarSyntaxKind.LeftCurly) return WalkScope(token);

            object outputValue = token.Value;

            switch (token.Type)
            {
                case XzaarSyntaxKind.Number:
                    if (outputValue.ToString().Contains("."))
                        outputValue = double.Parse(outputValue.ToString(), CultureInfo.InvariantCulture);
                    else if (outputValue.ToString().ToLower().StartsWith("0x"))
                        outputValue = Convert.ToInt32(outputValue.ToString().Substring(2), 16);
                    else
                        outputValue = int.Parse(outputValue.ToString());
                    return new XzaarSyntaxNode(NextIndex(), outputValue, XzaarSyntaxKind.Literal, XzaarSyntaxKind.LiteralNumber)
                    {
                        TrailingToken = tokenStream.PeekPrevious(),
                        LeadingToken = tokenStream.PeekNext()
                    };

                case XzaarSyntaxKind.String:
                    return new XzaarSyntaxNode(NextIndex(), outputValue, XzaarSyntaxKind.Literal, XzaarSyntaxKind.LiteralString)
                    {
                        TrailingToken = tokenStream.PeekPrevious(),
                        LeadingToken = tokenStream.PeekNext()
                    };
                case XzaarSyntaxKind.Identifier:
                    {
                        if (XzaarSyntaxFacts.IsKeyword(outputValue + "")) return new XzaarSyntaxNode(NextIndex(), outputValue, XzaarSyntaxKind.Keyword, XzaarSyntaxFacts.GetKeywordSubType(outputValue + ""))
                        {
                            TrailingToken = tokenStream.PeekPrevious(),
                            LeadingToken = tokenStream.PeekNext()
                        };
                        // if (XzaarSyntaxFacts.IsKnownConstant(outputValue + "")) return new XzaarSyntaxNode(NextIndex(), outputValue, XzaarSyntaxKind.Constant, XzaarSyntaxKind.Constant);
                        return new XzaarSyntaxNode(NextIndex(), outputValue, XzaarSyntaxKind.Identifier,
                            XzaarSyntaxKind.Identifier)
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

        private XzaarSyntaxNode OperatorNode(int nextIndex, object outputValue, XzaarSyntaxKind tokenType)
        {
            var nodeType = XzaarSyntaxFacts.GetNodeType(tokenType);
            var nodeSubType = XzaarSyntaxFacts.GetSubKind(nodeType, tokenType);
            return new XzaarSyntaxNode(nextIndex, outputValue, nodeType, nodeSubType)
            {
                TrailingToken = tokenStream.PeekPrevious(),
                LeadingToken = tokenStream.PeekNext()
            };
        }


        private XzaarSyntaxNode WalkScope(XzaarToken trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending '}' was found.", trailingToken);
            return WalkRecursive(trailingToken, XzaarSyntaxKind.Scope, XzaarSyntaxKind.RightCurly);
        }


        private XzaarSyntaxNode WalkBracketExpression(XzaarToken trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ']' was found.", trailingToken);
            return WalkRecursive(trailingToken, XzaarSyntaxKind.ArrayIndexExpression, XzaarSyntaxKind.RightBracket);
        }

        private XzaarSyntaxNode WalkExpression(XzaarToken trailingToken)
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ')' was found.", trailingToken);
            return WalkRecursive(trailingToken, XzaarSyntaxKind.Expression, XzaarSyntaxKind.RightParan);
        }

        private XzaarSyntaxNode WalkRecursive(XzaarToken trailingToken, XzaarSyntaxKind type, XzaarSyntaxKind endingTokenType)
        {
            var node = new XzaarSyntaxNode(NextIndex(), type, type);
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

        private XzaarSyntaxNode Error(string message, XzaarToken trailingToken)
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

