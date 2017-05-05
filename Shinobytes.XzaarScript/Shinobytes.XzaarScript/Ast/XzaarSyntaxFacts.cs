using System.Linq;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarSyntaxFacts
    {
        private static string[] knownKeywords =
        {
            "if", "do", "while", "for", "foreach", "loop", "switch",
            "case","break", "new", "continue", "return", "fn", "var",
            "let", "struct", "class", "number", "string", "bool", "boolean",
            "date", "any", "else", "goto", "in", "default"
        };

        private static string[] knownConstants =
        {
            "null", "true", "false"
        };


        public static bool IsKeyword(string s)
        {

            return knownKeywords.Contains(s.ToLower()) || IsKnownConstant(s);
        }

        public static bool IsKnownConstant(string s)
        {
            return knownConstants.Contains(s.ToLower());
        }

        public static XzaarSyntaxKind GetKeywordSubType(string keyword)
        {
            var kw = keyword.ToLower();
            switch (kw)
            {
                case "in": return XzaarSyntaxKind.KeywordIn;
                case "loop": return XzaarSyntaxKind.KeywordLoop;
                case "else": return XzaarSyntaxKind.KeywordElse;
                case "if": return XzaarSyntaxKind.KeywordIf;
                case "while": return XzaarSyntaxKind.KeywordWhile;
                case "do": return XzaarSyntaxKind.KeywordDo;
                case "for": return XzaarSyntaxKind.KeywordFor;
                case "foreach": return XzaarSyntaxKind.KeywordForEach;
                case "switch": return XzaarSyntaxKind.KeywordSwitch;
                case "case": return XzaarSyntaxKind.KeywordCase;
                case "default": return XzaarSyntaxKind.KeywordDefault;

                case "true": return XzaarSyntaxKind.KeywordTrue;
                case "false": return XzaarSyntaxKind.KeywordFalse;
                case "null": return XzaarSyntaxKind.KeywordNull;

                case "return": return XzaarSyntaxKind.KeywordReturn;
                case "goto": return XzaarSyntaxKind.KeywordGoto;
                case "continue": return XzaarSyntaxKind.KeywordContinue;
                case "break": return XzaarSyntaxKind.KeywordBreak;

                case "number": return XzaarSyntaxKind.KeywordNumber;
                case "string": return XzaarSyntaxKind.KeywordString;
                case "bool":
                case "boolean": return XzaarSyntaxKind.KeywordBoolean;
                case "date": return XzaarSyntaxKind.KeywordDate;
                case "any": return XzaarSyntaxKind.KeywordAny;

                case "var": case "let": return XzaarSyntaxKind.KeywordVar;
                case "class": return XzaarSyntaxKind.KeywordClass;
                case "enum": return XzaarSyntaxKind.KeywordEnum;
                case "struct": return XzaarSyntaxKind.KeywordStruct;
                case "fn": return XzaarSyntaxKind.KeywordFn;
            }
            return XzaarSyntaxKind.None;
        }


        public static XzaarSyntaxKind GetSubKind(XzaarSyntaxKind xzaarSyntaxKind, XzaarSyntaxKind tokenType)
        {
            switch (xzaarSyntaxKind)
            {
                case XzaarSyntaxKind.Literal:
                    {
                        if (tokenType == XzaarSyntaxKind.Number) return XzaarSyntaxKind.LiteralNumber;
                        if (tokenType == XzaarSyntaxKind.String) return XzaarSyntaxKind.LiteralString;
                        break;
                    }
                case XzaarSyntaxKind.UnaryOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.Not) return XzaarSyntaxKind.UnaryNot;
                        // if (tokenType == TokenType.Tilde)
                        if (tokenType == XzaarSyntaxKind.Plus) return XzaarSyntaxKind.UnaryPlus;
                        if (tokenType == XzaarSyntaxKind.Minus) return XzaarSyntaxKind.UnaryMinus;
                        if (tokenType == XzaarSyntaxKind.PlusPlus) return XzaarSyntaxKind.UnaryIncrement;
                        if (tokenType == XzaarSyntaxKind.MinusMinus) return XzaarSyntaxKind.UnaryDecrement;
                        break;
                    }
                case XzaarSyntaxKind.ArithmeticOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.Multiply) return XzaarSyntaxKind.ArithmeticMultiply;
                        if (tokenType == XzaarSyntaxKind.Divide) return XzaarSyntaxKind.ArithmeticDivide;
                        if (tokenType == XzaarSyntaxKind.Plus) return XzaarSyntaxKind.ArithmeticAdd;
                        if (tokenType == XzaarSyntaxKind.Minus) return XzaarSyntaxKind.ArithmeticSubtract;
                        if (tokenType == XzaarSyntaxKind.Modulo) return XzaarSyntaxKind.ArithmeticModulo;
                        break;
                    }
                case XzaarSyntaxKind.LogicalConditionalOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.Less) return XzaarSyntaxKind.ConditionalLessThan;
                        if (tokenType == XzaarSyntaxKind.Greater) return XzaarSyntaxKind.ConditionalGreaterThan;
                        if (tokenType == XzaarSyntaxKind.LessEquals) return XzaarSyntaxKind.ConditionalLessOrEquals;
                        if (tokenType == XzaarSyntaxKind.GreaterEquals) return XzaarSyntaxKind.ConditionalGreaterOrEquals;
                        if (tokenType == XzaarSyntaxKind.OrOr) return XzaarSyntaxKind.LogicalOr;
                        if (tokenType == XzaarSyntaxKind.AndAnd) return XzaarSyntaxKind.LogicalAnd;
                        break;
                    }
                case XzaarSyntaxKind.EqualityOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.EqualsEquals) return XzaarSyntaxKind.EqualityEquals;
                        if (tokenType == XzaarSyntaxKind.NotEquals) return XzaarSyntaxKind.EqualityNotEquals;
                        break;
                    }
                case XzaarSyntaxKind.LogicalBitOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.And) return XzaarSyntaxKind.BitAnd;
                        // if (tokenType == TokenType.Xor) return XzaarSyntaxKind.BitXor;
                        if (tokenType == XzaarSyntaxKind.Or) return XzaarSyntaxKind.BitOr;
                        break;
                    }
                case XzaarSyntaxKind.ConditionalOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.AndAnd) return XzaarSyntaxKind.LogicalAnd;
                        if (tokenType == XzaarSyntaxKind.OrOr) return XzaarSyntaxKind.LogicalOr;
                        break;
                    }
                //case NodeType.NullCoalescingOperator:
                //    {
                //        if (tokenType == TokenType.QuestionQuestion) return XzaarSyntaxKind...;                        
                //        break;
                //    }
                case XzaarSyntaxKind.AssignmentOperator:
                    {
                        if (tokenType == XzaarSyntaxKind.Equals) return XzaarSyntaxKind.Assign;
                        if (tokenType == XzaarSyntaxKind.PlusEquals) return XzaarSyntaxKind.AssignPlus;
                        if (tokenType == XzaarSyntaxKind.MinusEquals) return XzaarSyntaxKind.AssignMinus;
                        if (tokenType == XzaarSyntaxKind.DivideEquals) return XzaarSyntaxKind.AssignDivide;
                        if (tokenType == XzaarSyntaxKind.MultiplyEquals) return XzaarSyntaxKind.AssignMultiply;
                        if (tokenType == XzaarSyntaxKind.ModuloEquals) return XzaarSyntaxKind.AssignModulo;
                        if (tokenType == XzaarSyntaxKind.LessLessEquals) return XzaarSyntaxKind.AssignLeftShift;
                        if (tokenType == XzaarSyntaxKind.GreaterGreaterEquals) return XzaarSyntaxKind.AssignRightShift;
                        if (tokenType == XzaarSyntaxKind.EqualsGreater) return XzaarSyntaxKind.Lambda;
                        if (tokenType == XzaarSyntaxKind.OrEquals) return XzaarSyntaxKind.AssignOr;
                        break;
                    }
            }
            return XzaarSyntaxKind.None;
        }

        public static XzaarSyntaxKind GetNodeType(XzaarSyntaxKind tokenType)
        {
            switch (tokenType)
            {
                case XzaarSyntaxKind.Dot: return XzaarSyntaxKind.MemberAccess;

                // case TokenType.QuestionMarkDot: return NodeType.NullConditionalMemberAccess;
                case XzaarSyntaxKind.Identifier: return XzaarSyntaxKind.Identifier;

                case XzaarSyntaxKind.Number:
                case XzaarSyntaxKind.String:
                    return XzaarSyntaxKind.Literal;

                case XzaarSyntaxKind.MinusGreater:
                    return XzaarSyntaxKind.PointerMemberAccess;

                case XzaarSyntaxKind.MinusMinus:
                case XzaarSyntaxKind.PlusPlus:
                case XzaarSyntaxKind.Not:
                    return XzaarSyntaxKind.UnaryOperator;

                case XzaarSyntaxKind.MinusEquals:
                case XzaarSyntaxKind.PlusEquals:
                case XzaarSyntaxKind.MultiplyEquals:
                case XzaarSyntaxKind.DivideEquals:
                case XzaarSyntaxKind.ModuloEquals:
                case XzaarSyntaxKind.Equals:
                case XzaarSyntaxKind.EqualsGreater:
                case XzaarSyntaxKind.AndEquals:
                case XzaarSyntaxKind.OrEquals:
                case XzaarSyntaxKind.GreaterGreaterEquals:
                case XzaarSyntaxKind.LessLessEquals:
                    return XzaarSyntaxKind.AssignmentOperator;

                case XzaarSyntaxKind.GreaterGreater:
                case XzaarSyntaxKind.LessLess:
                case XzaarSyntaxKind.Minus:
                case XzaarSyntaxKind.Plus:
                case XzaarSyntaxKind.Multiply:
                case XzaarSyntaxKind.Divide:
                case XzaarSyntaxKind.Modulo:
                    return XzaarSyntaxKind.ArithmeticOperator;

                case XzaarSyntaxKind.GreaterEquals:
                case XzaarSyntaxKind.Greater:
                case XzaarSyntaxKind.LessEquals:
                case XzaarSyntaxKind.Less:
                case XzaarSyntaxKind.AndAnd:
                case XzaarSyntaxKind.OrOr:
                    return XzaarSyntaxKind.LogicalConditionalOperator;

                case XzaarSyntaxKind.Or:
                case XzaarSyntaxKind.And:
                    return XzaarSyntaxKind.LogicalBitOperator;

                case XzaarSyntaxKind.EqualsEquals:
                case XzaarSyntaxKind.NotEquals:
                    return XzaarSyntaxKind.EqualityOperator;

                case XzaarSyntaxKind.ColonColon:
                case XzaarSyntaxKind.Colon:
                    return tokenType;

                case XzaarSyntaxKind.Comma:
                    return XzaarSyntaxKind.Separator;

                case XzaarSyntaxKind.Semicolon:
                    return XzaarSyntaxKind.StatementTerminator;
                    // case TokenType.QuestionMark: return NodeType.EqualityOperator;
            }

            return XzaarSyntaxKind.Identifier;
        }

        public static bool IsRightAssociative(XzaarSyntaxKind op)
        {
            switch (op)
            {
                case XzaarSyntaxKind.Assign:
                case XzaarSyntaxKind.AssignPlus:
                case XzaarSyntaxKind.AssignMinus:
                case XzaarSyntaxKind.AssignMultiply:
                case XzaarSyntaxKind.AssignDivide:
                case XzaarSyntaxKind.AssignModulo:
                case XzaarSyntaxKind.AssignAnd:
                case XzaarSyntaxKind.AssignXor:
                case XzaarSyntaxKind.AssignOr:
                case XzaarSyntaxKind.AssignLeftShift:
                case XzaarSyntaxKind.AssignRightShift:
                    // case XzaarSyntaxKind.Coalesce:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsInvalidSubExpression(XzaarSyntaxKind kind)
        {
            switch (kind)
            {
                case XzaarSyntaxKind.KeywordBreak:
                case XzaarSyntaxKind.KeywordCase:
                case XzaarSyntaxKind.KeywordContinue:
                case XzaarSyntaxKind.KeywordDo:
                case XzaarSyntaxKind.KeywordFor:
                case XzaarSyntaxKind.KeywordForEach:
                case XzaarSyntaxKind.KeywordGoto:
                case XzaarSyntaxKind.KeywordIf:
                case XzaarSyntaxKind.KeywordReturn:
                case XzaarSyntaxKind.KeywordSwitch:
                case XzaarSyntaxKind.KeywordWhile:
                    return true;
                default:
                    return false;
            }
        }

        public static Precedence GetPrecedence(XzaarSyntaxKind op)
        {
            switch (op)
            {
                case XzaarSyntaxKind.Assign:
                case XzaarSyntaxKind.AssignPlus:
                case XzaarSyntaxKind.AssignMinus:
                case XzaarSyntaxKind.AssignMultiply:
                case XzaarSyntaxKind.AssignDivide:
                case XzaarSyntaxKind.AssignModulo:
                case XzaarSyntaxKind.AssignAnd:
                case XzaarSyntaxKind.AssignXor:
                case XzaarSyntaxKind.AssignOr:
                case XzaarSyntaxKind.AssignLeftShift:
                case XzaarSyntaxKind.AssignRightShift: return Precedence.Assignment;
                // case XzaarSyntaxKind.CoalesceExpression: return Precedence.Coalescing;
                case XzaarSyntaxKind.LogicalOr: return Precedence.ConditionalOr;
                case XzaarSyntaxKind.LogicalAnd: return Precedence.ConditionalAnd;
                case XzaarSyntaxKind.BitOr: return Precedence.LogicalOr;
                case XzaarSyntaxKind.BitXor: return Precedence.LogicalXor;
                case XzaarSyntaxKind.BitAnd: return Precedence.LogicalAnd;
                case XzaarSyntaxKind.EqualityEquals:
                case XzaarSyntaxKind.EqualityNotEquals: return Precedence.Equality;
                case XzaarSyntaxKind.ConditionalLessThan:
                case XzaarSyntaxKind.ConditionalLessOrEquals:
                case XzaarSyntaxKind.ConditionalGreaterThan:
                case XzaarSyntaxKind.ConditionalGreaterOrEquals:
                    //case XzaarSyntaxKind.IsExpression:
                    //case XzaarSyntaxKind.AsExpression:
                    //case XzaarSyntaxKind.IsPatternExpression:
                    return Precedence.Relational;
                case XzaarSyntaxKind.ArithmeticLeftShift:
                case XzaarSyntaxKind.ArithmeticRightShift: return Precedence.Shift;
                case XzaarSyntaxKind.ArithmeticAdd:
                case XzaarSyntaxKind.ArithmeticSubtract: return Precedence.Additive;
                case XzaarSyntaxKind.ArithmeticMultiply:
                case XzaarSyntaxKind.ArithmeticDivide:
                case XzaarSyntaxKind.ArithmeticModulo: return Precedence.Mutiplicative;
                case XzaarSyntaxKind.UnaryPlus:
                case XzaarSyntaxKind.UnaryMinus:
                case XzaarSyntaxKind.BitNot:
                case XzaarSyntaxKind.Not:
                case XzaarSyntaxKind.UnaryIncrement:
                case XzaarSyntaxKind.UnaryDecrement:
                    // case XzaarSyntaxKind.AwaitExpression:
                    return Precedence.Unary;
                // case XzaarSyntaxKind.CastExpression: return Precedence.Cast;                
                default:
                    return Precedence.Expression;
            }
        }

        internal static bool IsStatementExpression(XzaarSyntaxNode node)
        {
            // The grammar gives:
            //
            // expression-statement:
            //     statement-expression ;
            //
            // statement-expression:
            //     invocation-expression
            //     object-creation-expression
            //     assignment
            //     post-increment-expression
            //     post-decrement-expression
            //     pre-increment-expression
            //     pre-decrement-expression
            //     await-expression

            switch (node.Type)
            {
                case XzaarSyntaxKind.PostfixDecrement:
                case XzaarSyntaxKind.PostfixIncrement:
                case XzaarSyntaxKind.FunctionInvocation:
                    return true;
                    //case NodeType.NullConditionalMemberAccess:
                    //    var access = ... get the non-null node and check with that one
                    //    return IsStatementExpression(node....);
            }

            switch (node.Kind)
            {

                case XzaarSyntaxKind.Assign:
                // case ObjectCreationExpression:
                case XzaarSyntaxKind.AssignPlus:
                case XzaarSyntaxKind.AssignMinus:
                case XzaarSyntaxKind.AssignMultiply:
                case XzaarSyntaxKind.AssignDivide:
                case XzaarSyntaxKind.AssignModulo:
                case XzaarSyntaxKind.AssignAnd:
                case XzaarSyntaxKind.AssignOr:
                case XzaarSyntaxKind.AssignXor:
                case XzaarSyntaxKind.AssignLeftShift:
                case XzaarSyntaxKind.AssignRightShift:
                case XzaarSyntaxKind.UnaryIncrement:
                case XzaarSyntaxKind.UnaryDecrement:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsExpectedPrefixUnaryOperator(XzaarSyntaxKind kind)
        {
            return IsPrefixUnaryExpression(kind);
        }

        public static bool IsExpectedBinaryOperator(XzaarSyntaxKind kind)
        {
            return IsBinaryExpression(kind);
        }

        public static bool IsExpectedAssignmentOperator(XzaarSyntaxKind kind)
        {
            return IsAssignmentExpressionOperatorToken(kind);
        }


        public static bool IsAnyUnaryExpression(XzaarSyntaxKind token)
        {
            return IsPrefixUnaryExpression(token) || IsPostfixUnaryExpression(token);
        }

        public static bool IsPrefixUnaryExpression(XzaarSyntaxKind token)
        {
            return GetPrefixUnaryExpression(token) != XzaarSyntaxKind.None;
        }

        public static bool IsPrefixUnaryExpressionOperatorToken(XzaarSyntaxKind token)
        {
            return GetPrefixUnaryExpression(token) != XzaarSyntaxKind.None;
        }
        public static bool IsPostfixUnaryExpression(XzaarSyntaxKind token)
        {
            return GetPostfixUnaryExpression(token) != XzaarSyntaxKind.None;
        }

        public static bool IsPostfixUnaryExpressionToken(XzaarSyntaxKind token)
        {
            return GetPostfixUnaryExpression(token) != XzaarSyntaxKind.None;
        }

        public static XzaarSyntaxKind GetPostfixUnaryExpression(XzaarSyntaxKind token)
        {
            switch (token)
            {
                case XzaarSyntaxKind.UnaryIncrement: case XzaarSyntaxKind.PlusPlus: return XzaarSyntaxKind.PostfixIncrement;
                case XzaarSyntaxKind.UnaryDecrement: case XzaarSyntaxKind.PostfixDecrement: case XzaarSyntaxKind.MinusMinus: return XzaarSyntaxKind.PostfixDecrement;
                default:
                    return XzaarSyntaxKind.None;
            }
        }
        public static XzaarSyntaxKind GetPrefixUnaryExpression(XzaarSyntaxKind token)
        {
            switch (token)
            {
                case XzaarSyntaxKind.ArithmeticAdd: case XzaarSyntaxKind.UnaryPlus: case XzaarSyntaxKind.Plus: return XzaarSyntaxKind.UnaryPlus;
                case XzaarSyntaxKind.ArithmeticSubtract: case XzaarSyntaxKind.UnaryMinus: case XzaarSyntaxKind.Minus: return XzaarSyntaxKind.UnaryMinus;
                // case XzaarSyntaxKind.UnaryNot: case XzaarSyntaxKind.Tilde: return XzaarSyntaxKind.UnaryNot;
                case XzaarSyntaxKind.UnaryNot: case XzaarSyntaxKind.Not: return XzaarSyntaxKind.UnaryNot;
                case XzaarSyntaxKind.UnaryIncrement: case XzaarSyntaxKind.PlusPlus: return XzaarSyntaxKind.UnaryIncrement;
                case XzaarSyntaxKind.UnaryDecrement: case XzaarSyntaxKind.MinusMinus: return XzaarSyntaxKind.UnaryDecrement;
                default: return XzaarSyntaxKind.None;
            }
        }

        public static bool IsBinaryExpression(XzaarSyntaxKind token)
        {
            return GetBinaryExpression(token) != XzaarSyntaxKind.None;
        }

        public static bool IsBinaryExpressionOperatorToken(XzaarSyntaxKind token)
        {
            return GetBinaryExpression(token) != XzaarSyntaxKind.None;
        }

        public static XzaarSyntaxKind GetBinaryExpression(XzaarSyntaxKind token)
        {
            switch (token)
            {
                // case XzaarSyntaxKind.QuestionQuestion: return XzaarSyntaxKind.CoalesceExpression;
                case XzaarSyntaxKind.BitOr: case XzaarSyntaxKind.Or: return XzaarSyntaxKind.BitOr;
                case XzaarSyntaxKind.BitXor: case XzaarSyntaxKind.Caret: return XzaarSyntaxKind.BitXor;
                case XzaarSyntaxKind.BitAnd: case XzaarSyntaxKind.And: return XzaarSyntaxKind.BitAnd;
                case XzaarSyntaxKind.EqualityEquals: case XzaarSyntaxKind.EqualsEquals: return XzaarSyntaxKind.EqualityEquals;
                case XzaarSyntaxKind.EqualityNotEquals: case XzaarSyntaxKind.NotEquals: return XzaarSyntaxKind.EqualityNotEquals;
                case XzaarSyntaxKind.ConditionalLessThan: case XzaarSyntaxKind.Less: return XzaarSyntaxKind.ConditionalLessThan;
                case XzaarSyntaxKind.ConditionalLessOrEquals: case XzaarSyntaxKind.LessEquals: return XzaarSyntaxKind.ConditionalLessOrEquals;
                case XzaarSyntaxKind.ConditionalGreaterThan: case XzaarSyntaxKind.Greater: return XzaarSyntaxKind.ConditionalGreaterThan;
                case XzaarSyntaxKind.ConditionalGreaterOrEquals: case XzaarSyntaxKind.GreaterEquals: return XzaarSyntaxKind.ConditionalGreaterOrEquals;
                case XzaarSyntaxKind.ArithmeticLeftShift: case XzaarSyntaxKind.LessLess: return XzaarSyntaxKind.ArithmeticLeftShift;
                case XzaarSyntaxKind.ArithmeticRightShift: case XzaarSyntaxKind.GreaterGreater: return XzaarSyntaxKind.ArithmeticRightShift;
                case XzaarSyntaxKind.ArithmeticAdd: case XzaarSyntaxKind.Plus: return XzaarSyntaxKind.ArithmeticAdd;
                case XzaarSyntaxKind.ArithmeticSubtract: case XzaarSyntaxKind.Minus: return XzaarSyntaxKind.ArithmeticSubtract;
                case XzaarSyntaxKind.ArithmeticMultiply: case XzaarSyntaxKind.Multiply: return XzaarSyntaxKind.ArithmeticMultiply;
                case XzaarSyntaxKind.ArithmeticDivide: case XzaarSyntaxKind.Divide: return XzaarSyntaxKind.ArithmeticDivide;
                case XzaarSyntaxKind.ArithmeticModulo: case XzaarSyntaxKind.Modulo: return XzaarSyntaxKind.ArithmeticModulo;
                case XzaarSyntaxKind.LogicalAnd: case XzaarSyntaxKind.AndAnd: return XzaarSyntaxKind.LogicalAnd;
                case XzaarSyntaxKind.LogicalOr: case XzaarSyntaxKind.OrOr: return XzaarSyntaxKind.LogicalOr;
                default: return XzaarSyntaxKind.None;
            }
        }

        public static bool IsAssignmentExpression(XzaarSyntaxKind kind)
        {
            switch (kind)
            {
                case XzaarSyntaxKind.AssignmentOperator:
                case XzaarSyntaxKind.AssignOr:
                case XzaarSyntaxKind.AssignAnd:
                case XzaarSyntaxKind.AssignXor:
                case XzaarSyntaxKind.AssignLeftShift:
                case XzaarSyntaxKind.AssignRightShift:
                case XzaarSyntaxKind.AssignPlus:
                case XzaarSyntaxKind.AssignMinus:
                case XzaarSyntaxKind.AssignMultiply:
                case XzaarSyntaxKind.AssignDivide:
                case XzaarSyntaxKind.AssignModulo:
                case XzaarSyntaxKind.Assign:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAssignmentExpressionOperatorToken(XzaarSyntaxKind token)
        {
            switch (token)
            {
                case XzaarSyntaxKind.OrEquals:
                case XzaarSyntaxKind.AndEquals:
                case XzaarSyntaxKind.CaretEquals:
                case XzaarSyntaxKind.LessLessEquals:
                case XzaarSyntaxKind.GreaterGreaterEquals:
                case XzaarSyntaxKind.PlusEquals:
                case XzaarSyntaxKind.MinusEquals:
                case XzaarSyntaxKind.MultiplyEquals:
                case XzaarSyntaxKind.DivideEquals:
                case XzaarSyntaxKind.ModuloEquals:
                case XzaarSyntaxKind.Equals:
                    return true;
                default:
                    return false;
            }
        }

        public static XzaarSyntaxKind GetAssignmentExpression(XzaarSyntaxKind token)
        {
            switch (token)
            {
                case XzaarSyntaxKind.OrEquals: return XzaarSyntaxKind.AssignOr;
                case XzaarSyntaxKind.AndEquals: return XzaarSyntaxKind.AssignAnd;
                case XzaarSyntaxKind.CaretEquals: return XzaarSyntaxKind.AssignXor;
                case XzaarSyntaxKind.LessLessEquals: return XzaarSyntaxKind.AssignLeftShift;
                case XzaarSyntaxKind.GreaterGreaterEquals: return XzaarSyntaxKind.AssignRightShift;
                case XzaarSyntaxKind.PlusEquals: return XzaarSyntaxKind.AssignPlus;
                case XzaarSyntaxKind.MinusEquals: return XzaarSyntaxKind.AssignMinus;
                case XzaarSyntaxKind.MultiplyEquals: return XzaarSyntaxKind.AssignMultiply;
                case XzaarSyntaxKind.DivideEquals: return XzaarSyntaxKind.AssignDivide;
                case XzaarSyntaxKind.ModuloEquals: return XzaarSyntaxKind.AssignModulo;
                case XzaarSyntaxKind.Equals: return XzaarSyntaxKind.Assign;
                default:
                    return XzaarSyntaxKind.None;
            }
        }


        public static bool IsIncrementOrDecrementOperator(XzaarSyntaxKind token)
        {
            switch (token)
            {
                case XzaarSyntaxKind.PlusPlus:
                case XzaarSyntaxKind.MinusMinus:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsPredefinedType(XzaarSyntaxKind kind)
        {
            switch (kind)
            {
                case XzaarSyntaxKind.KeywordNumber:
                case XzaarSyntaxKind.KeywordBoolean:
                case XzaarSyntaxKind.KeywordString:
                case XzaarSyntaxKind.KeywordDate:
                case XzaarSyntaxKind.KeywordAny:
                case XzaarSyntaxKind.KeywordVoid:
                    return true;
                default:
                    return false;
            }
        }

    }
}
