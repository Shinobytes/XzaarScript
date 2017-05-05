using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarScriptParser
    {
        private readonly XzaarSyntaxTokenStream tokenStream;
        private readonly List<string> errors;
        private int currentNodeIndex = 0;

        private string[] knownKeywords =
        {
            "if", "do", "while", "foreach", "loop", "switch", "case",
            "new", "break", "continue", "return", "fn", "var", "let",
            "struct", "class", "number", "string", "boolean", "date",
            "any", "else"
        };

        private string[] knownConstants =
        {
            "null", "true", "false"
        };

        public XzaarScriptParser(IReadOnlyList<XzaarToken> tokens)
        {
            this.errors = new List<string>();
            this.tokenStream = new XzaarSyntaxTokenStream(tokens);
        }

        public bool HasErrors { get { return errors.Count > 0; } }
        public IReadOnlyList<string> Errors { get { return errors; } }

        public XzaarNode Parse()
        {
            Reset();

            var entry = new XzaarNode(NextIndex(), NodeType.Scope);
            var currentToken = tokenStream.Current;
            while (!tokenStream.EndOfStream())
            {
                var node = WalkToken(currentToken);
                if (node != null) entry.AddChild(node);
                currentToken = tokenStream.Next();
            }
            return entry;
        }

        private XzaarNode WalkToken(XzaarToken token)
        {
            if (token.Type == TokenType.RightBracket) return Error("Unexpected ending bracket ']'");
            if (token.Type == TokenType.RightParan) return Error("Unexpected ending paranthesis ')'");
            if (token.Type == TokenType.RightCurly) return Error("Unexpected ending curly bracket '}'");

            if (token.Type == TokenType.LeftParan) return WalkExpression();
            if (token.Type == TokenType.LeftBracket) return WalkBracketExpression();
            if (token.Type == TokenType.LeftCurly) return WalkScope();

            object outputValue = token.Value;

            switch (token.Type)
            {
                case TokenType.Number:
                    if (outputValue.ToString().Contains("."))
                        outputValue = double.Parse(outputValue.ToString(), CultureInfo.InvariantCulture);
                    else if (outputValue.ToString().ToLower().StartsWith("0x"))
                        outputValue = Convert.ToInt32(outputValue.ToString().Substring(2), 16);
                    else
                        outputValue = int.Parse(outputValue.ToString());
                    return new XzaarNode(NextIndex(), outputValue, NodeType.Literal, SubNodeType.LiteralNumber);

                case TokenType.String: return new XzaarNode(NextIndex(), outputValue, NodeType.Literal, SubNodeType.LiteralString);
                case TokenType.Identifier:
                    {
                        if (IsKeyword(outputValue + "")) return new XzaarNode(NextIndex(), outputValue, NodeType.Keyword);
                        if (IsKnownConstant(outputValue + "")) return new XzaarNode(NextIndex(), outputValue, NodeType.Constant);
                        return new XzaarNode(NextIndex(), outputValue, NodeType.Identifier);
                    }
                default:
                    {
                        return OperatorNode(NextIndex(), outputValue, token.Type);
                        // return new XzaarNode(NextIndex(), outputValue, NodeType.Identifier);
                    }
            }
        }

        private XzaarNode OperatorNode(int nextIndex, object outputValue, TokenType tokenType)
        {
            var nodeType = GetNodeType(tokenType);
            var nodeSubType = GetSubNodeType(nodeType, tokenType);
            return new XzaarNode(nextIndex, outputValue, nodeType, nodeSubType);
        }

        private SubNodeType GetSubNodeType(NodeType nodeType, TokenType tokenType)
        {
            switch (nodeType)
            {
                case NodeType.Literal:
                    {
                        if (tokenType == TokenType.Number) return SubNodeType.LiteralNumber;
                        if (tokenType == TokenType.String) return SubNodeType.LiteralString;
                        break;
                    }
                case NodeType.UnaryOperator:
                    {
                        if (tokenType == TokenType.Not) return SubNodeType.UnaryNot;
                        // if (tokenType == TokenType.Tilde)
                        if (tokenType == TokenType.Plus) return SubNodeType.UnaryPlus;
                        if (tokenType == TokenType.Minus) return SubNodeType.UnaryMinus;
                        if (tokenType == TokenType.PlusPlus) return SubNodeType.UnaryIncrement;
                        if (tokenType == TokenType.MinusMinus) return SubNodeType.UnaryDecrement;
                        break;
                    }
                case NodeType.ArithmeticOperator:
                    {
                        if (tokenType == TokenType.Multiply) return SubNodeType.ArithmeticMultiply;
                        if (tokenType == TokenType.Divide) return SubNodeType.ArithmeticDivide;
                        if (tokenType == TokenType.Plus) return SubNodeType.ArithmeticAdd;
                        if (tokenType == TokenType.Minus) return SubNodeType.ArithmeticSubtract;
                        if (tokenType == TokenType.Modulus) return SubNodeType.ArithmeticModulus;
                        break;
                    }
                case NodeType.LogicalConditionalOperator:
                    {
                        if (tokenType == TokenType.Less) return SubNodeType.ConditionalLessThan;
                        if (tokenType == TokenType.Greater) return SubNodeType.ConditionalGreaterThan;
                        if (tokenType == TokenType.LessEquals) return SubNodeType.ConditionalLessOrEquals;
                        if (tokenType == TokenType.GreaterEquals) return SubNodeType.ConditionalGreaterOrEquals;
                        break;
                    }
                case NodeType.EqualityOperator:
                    {
                        if (tokenType == TokenType.EqualsEquals) return SubNodeType.EqualityEquals;
                        if (tokenType == TokenType.NotEquals) return SubNodeType.EqualityNotEquals;
                        break;
                    }
                case NodeType.LogicalBitOperator:
                    {
                        if (tokenType == TokenType.And) return SubNodeType.BitAnd;
                        // if (tokenType == TokenType.Xor) return SubNodeType.BitXor;
                        if (tokenType == TokenType.Or) return SubNodeType.BitOr;
                        break;
                    }
                case NodeType.ConditionalOperator:
                    {
                        if (tokenType == TokenType.AndAnd) return SubNodeType.ConditionalAnd;
                        if (tokenType == TokenType.OrOr) return SubNodeType.ConditionalOr;
                        break;
                    }
                //case NodeType.NullCoalescingOperator:
                //    {
                //        if (tokenType == TokenType.QuestionMarkQuestionMark) return SubNodeType...;                        
                //        break;
                //    }
                case NodeType.AssignmentOperator:
                    {
                        if (tokenType == TokenType.Equals) return SubNodeType.Assign;
                        if (tokenType == TokenType.PlusEquals) return SubNodeType.AssignPlus;
                        if (tokenType == TokenType.MinusEquals) return SubNodeType.AssignMinus;
                        if (tokenType == TokenType.DivideEquals) return SubNodeType.AssignDivide;
                        if (tokenType == TokenType.MultiplyEquals) return SubNodeType.AssignMultiply;
                        if (tokenType == TokenType.ModulusEquals) return SubNodeType.AssignModulus;
                        if (tokenType == TokenType.LessLessEquals) return SubNodeType.AssignLeftShift;
                        if (tokenType == TokenType.GreaterGreaterEquals) return SubNodeType.AssignRightShift;
                        if (tokenType == TokenType.EqualsGreater) return SubNodeType.Lambda;
                        if (tokenType == TokenType.OrEquals) return SubNodeType.AssignOr;
                        break;
                    }
            }
            return SubNodeType.None;
        }

        private NodeType GetNodeType(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Dot:
                    return NodeType.MemberAccess;

                // case TokenType.QuestionMarkDot: return NodeType.NullConditionalMemberAccess;
                case TokenType.Identifier:
                    return NodeType.Identifier;

                case TokenType.Number:
                case TokenType.String:
                    return NodeType.Literal;

                case TokenType.MinusGreater:
                    return NodeType.PointerMemberAccess;

                case TokenType.MinusMinus:
                case TokenType.PlusPlus:
                case TokenType.Not:
                    return NodeType.UnaryOperator;

                case TokenType.MinusEquals:
                case TokenType.PlusEquals:
                case TokenType.MultiplyEquals:
                case TokenType.DivideEquals:
                case TokenType.ModulusEquals:
                case TokenType.Equals:
                case TokenType.EqualsGreater:
                case TokenType.AndEquals:
                case TokenType.OrEquals:
                case TokenType.GreaterGreaterEquals:
                case TokenType.LessLessEquals:
                    return NodeType.AssignmentOperator;

                case TokenType.GreaterGreater:
                case TokenType.LessLess:
                case TokenType.Minus:
                case TokenType.Plus:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.Modulus:
                    return NodeType.ArithmeticOperator;

                case TokenType.GreaterEquals:
                case TokenType.Greater:
                case TokenType.LessEquals:
                case TokenType.Less:
                case TokenType.AndAnd:
                case TokenType.OrOr:
                    return NodeType.LogicalConditionalOperator;

                case TokenType.Or:
                case TokenType.And:
                    return NodeType.LogicalBitOperator;

                case TokenType.EqualsEquals:
                case TokenType.NotEquals:
                    return NodeType.EqualityOperator;

                case TokenType.Comma:
                    return NodeType.Separator;

                case TokenType.Semicolon:
                    return NodeType.StatementTerminator;
                    // case TokenType.QuestionMark: return NodeType.EqualityOperator;
            }

            return NodeType.Identifier;
        }


        private bool IsKeyword(string s)
        {
            return knownKeywords.Contains(s.ToLower());
        }

        private bool IsKnownConstant(string s)
        {
            return knownConstants.Contains(s.ToLower());
        }

        private XzaarNode WalkScope()
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending '}' was found.");
            return WalkRecursive(NodeType.Scope, TokenType.RightCurly);
        }


        private XzaarNode WalkBracketExpression()
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ']' was found.");
            return WalkRecursive(NodeType.ArrayIndexExpression, TokenType.RightBracket);
        }

        private XzaarNode WalkExpression()
        {
            var token = tokenStream.PeekNext();
            if (token == null) return Error("Unexpected end of script. No matching ending ')' was found.");
            return WalkRecursive(NodeType.Expression, TokenType.RightParan);
        }

        private XzaarNode WalkRecursive(NodeType type, TokenType endingTokenType)
        {
            var node = new XzaarNode(NextIndex(), type);
            var token = tokenStream.Next();
            while (!tokenStream.EndOfStream() && token != null && token.Type != endingTokenType)
            {
                var n = WalkToken(token);
                if (n != null)
                    node.AddChild(n);
                token = tokenStream.Next();
            }
            return node;
        }

        private XzaarNode Error(string message)
        {
            this.errors.Add(message);
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