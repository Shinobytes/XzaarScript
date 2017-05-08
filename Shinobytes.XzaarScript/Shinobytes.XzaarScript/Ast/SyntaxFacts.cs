using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms.ComponentModel.Com2Interop;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class SyntaxFacts
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

        public static bool IsKeyword(object strObject) => IsKeyword(strObject.ToString());

        public static bool IsKeyword(string s) => knownKeywords.Contains(s.ToLower()) || IsKnownConstant(s);

        public static bool IsKnownConstant(string s) => knownConstants.Contains(s.ToLower());

        public static SyntaxKind GetKeywordSubType(string keyword)
        {
            var kw = keyword.ToLower();
            switch (kw)
            {
                case "in": return SyntaxKind.KeywordIn;
                case "loop": return SyntaxKind.KeywordLoop;
                case "else": return SyntaxKind.KeywordElse;
                case "if": return SyntaxKind.KeywordIf;
                case "while": return SyntaxKind.KeywordWhile;
                case "do": return SyntaxKind.KeywordDo;
                case "for": return SyntaxKind.KeywordFor;
                case "foreach": return SyntaxKind.KeywordForEach;
                case "switch": return SyntaxKind.KeywordSwitch;
                case "case": return SyntaxKind.KeywordCase;
                case "default": return SyntaxKind.KeywordDefault;

                case "true": return SyntaxKind.KeywordTrue;
                case "false": return SyntaxKind.KeywordFalse;
                case "null": return SyntaxKind.KeywordNull;

                case "return": return SyntaxKind.KeywordReturn;
                case "goto": return SyntaxKind.KeywordGoto;
                case "continue": return SyntaxKind.KeywordContinue;
                case "break": return SyntaxKind.KeywordBreak;

                case "number": return SyntaxKind.KeywordNumber;
                case "string": return SyntaxKind.KeywordString;
                case "bool":
                case "boolean": return SyntaxKind.KeywordBoolean;
                case "date": return SyntaxKind.KeywordDate;
                case "any": return SyntaxKind.KeywordAny;

                case "var": case "let": return SyntaxKind.KeywordVar;
                case "class": return SyntaxKind.KeywordClass;
                case "enum": return SyntaxKind.KeywordEnum;
                case "struct": return SyntaxKind.KeywordStruct;
                case "fn": return SyntaxKind.KeywordFn;
            }
            return SyntaxKind.None;
        }

        //public static SyntaxKind GetSubKind(SyntaxKind syntaxKind, SyntaxKind tokenType)
        //{
        //    switch (syntaxKind)
        //    {
        //        case SyntaxKind.String:
        //        case SyntaxKind.Number:
        //        case SyntaxKind.Number:
        //        case SyntaxKind.String:
        //            return syntaxKind;

        //        case SyntaxKind.UnaryOperator:
        //            {
        //                if (tokenType == SyntaxKind.Not) return SyntaxKind.UnaryNot;
        //                // if (tokenType == TokenType.Tilde)
        //                if (tokenType == SyntaxKind.Plus) return SyntaxKind.UnaryPlus;
        //                if (tokenType == SyntaxKind.Minus) return SyntaxKind.UnaryMinus;
        //                if (tokenType == SyntaxKind.PlusPlus) return SyntaxKind.UnaryIncrement;
        //                if (tokenType == SyntaxKind.MinusMinus) return SyntaxKind.UnaryDecrement;
        //                break;
        //            }
        //        case SyntaxKind.ArithmeticOperator:
        //            {
        //                if (tokenType == SyntaxKind.Multiply) return SyntaxKind.ArithmeticMultiply;
        //                if (tokenType == SyntaxKind.Divide) return SyntaxKind.ArithmeticDivide;
        //                if (tokenType == SyntaxKind.Plus) return SyntaxKind.ArithmeticAdd;
        //                if (tokenType == SyntaxKind.Minus) return SyntaxKind.ArithmeticSubtract;
        //                if (tokenType == SyntaxKind.Modulo) return SyntaxKind.ArithmeticModulo;
        //                break;
        //            }
        //        case SyntaxKind.LogicalConditionalOperator:
        //            {
        //                if (tokenType == SyntaxKind.Less) return SyntaxKind.ConditionalLessThan;
        //                if (tokenType == SyntaxKind.Greater) return SyntaxKind.ConditionalGreaterThan;
        //                if (tokenType == SyntaxKind.LessEquals) return SyntaxKind.ConditionalLessOrEquals;
        //                if (tokenType == SyntaxKind.GreaterEquals) return SyntaxKind.ConditionalGreaterOrEquals;
        //                if (tokenType == SyntaxKind.OrOr) return SyntaxKind.LogicalOr;
        //                if (tokenType == SyntaxKind.AndAnd) return SyntaxKind.LogicalAnd;
        //                break;
        //            }
        //        case SyntaxKind.EqualityOperator:
        //            {
        //                if (tokenType == SyntaxKind.EqualsEquals) return SyntaxKind.EqualsEquals;
        //                if (tokenType == SyntaxKind.NotEquals) return SyntaxKind.NotEquals;
        //                break;
        //            }
        //        case SyntaxKind.LogicalBitOperator:
        //            {
        //                if (tokenType == SyntaxKind.And) return SyntaxKind.BitAnd;
        //                // if (tokenType == TokenType.Xor) return SyntaxKind.BitXor;
        //                if (tokenType == SyntaxKind.Or) return SyntaxKind.BitOr;
        //                break;
        //            }
        //        case SyntaxKind.ConditionalOperator:
        //            {
        //                if (tokenType == SyntaxKind.AndAnd) return SyntaxKind.LogicalAnd;
        //                if (tokenType == SyntaxKind.OrOr) return SyntaxKind.LogicalOr;
        //                break;
        //            }
        //        case SyntaxKind.NullCoalescingOperator:
        //            {
        //                if (tokenType == SyntaxKind.QuestionQuestion) return SyntaxKind.NullCoalescingOperator;
        //                break;
        //            }
        //        case SyntaxKind.AssignmentOperator:
        //            {
        //                if (tokenType == SyntaxKind.Equals) return SyntaxKind.Assign;
        //                if (tokenType == SyntaxKind.PlusEquals) return SyntaxKind.AssignPlus;
        //                if (tokenType == SyntaxKind.MinusEquals) return SyntaxKind.AssignMinus;
        //                if (tokenType == SyntaxKind.DivideEquals) return SyntaxKind.AssignDivide;
        //                if (tokenType == SyntaxKind.MultiplyEquals) return SyntaxKind.AssignMultiply;
        //                if (tokenType == SyntaxKind.ModuloEquals) return SyntaxKind.AssignModulo;
        //                if (tokenType == SyntaxKind.LessLessEquals) return SyntaxKind.AssignLeftShift;
        //                if (tokenType == SyntaxKind.GreaterGreaterEquals) return SyntaxKind.AssignRightShift;
        //                if (tokenType == SyntaxKind.EqualsGreater) return SyntaxKind.Lambda;
        //                if (tokenType == SyntaxKind.OrEquals) return SyntaxKind.AssignOr;
        //                break;
        //            }
        //    }
        //    return SyntaxKind.None;
        //}

        public static SyntaxKind GetNodeType(SyntaxKind tokenType)
        {
            switch (tokenType)
            {
                case SyntaxKind.Dot: return SyntaxKind.MemberAccess;

                // case TokenType.QuestionMarkDot: return Kind.NullConditionalMemberAccess;
                case SyntaxKind.Identifier: return SyntaxKind.Identifier;

                case SyntaxKind.Number:
                case SyntaxKind.String:
                case SyntaxKind.MinusGreater:
                    return tokenType;

                case SyntaxKind.MinusMinus:
                case SyntaxKind.PlusPlus:
                case SyntaxKind.Not:
                    // return SyntaxKind.UnaryOperator;
                    return tokenType;

                case SyntaxKind.MinusEquals:
                case SyntaxKind.PlusEquals:
                case SyntaxKind.MultiplyEquals:
                case SyntaxKind.DivideEquals:
                case SyntaxKind.ModuloEquals:
                case SyntaxKind.Equals:
                case SyntaxKind.EqualsGreater:
                case SyntaxKind.AndEquals:
                case SyntaxKind.OrEquals:
                case SyntaxKind.GreaterGreaterEquals:
                case SyntaxKind.LessLessEquals:
                    // return SyntaxKind.AssignmentOperator;
                    return tokenType;

                case SyntaxKind.GreaterGreater:
                case SyntaxKind.LessLess:
                case SyntaxKind.Minus:
                case SyntaxKind.Plus:
                case SyntaxKind.Multiply:
                case SyntaxKind.Divide:
                case SyntaxKind.Modulo:
                    // return SyntaxKind.ArithmeticOperator;
                    return tokenType;

                case SyntaxKind.GreaterEquals:
                case SyntaxKind.Greater:
                case SyntaxKind.LessEquals:
                case SyntaxKind.Less:
                case SyntaxKind.AndAnd:
                case SyntaxKind.OrOr:
                    // return SyntaxKind.LogicalConditionalOperator;
                    return tokenType;

                case SyntaxKind.Or:
                case SyntaxKind.And:
                    // return SyntaxKind.LogicalBitOperator;
                    return tokenType;

                case SyntaxKind.EqualsEquals:
                case SyntaxKind.NotEquals:
                    // return SyntaxKind.EqualityOperator;
                    return tokenType;

                case SyntaxKind.ColonColon:
                case SyntaxKind.Colon:
                    return tokenType;

                case SyntaxKind.Comma:
                    return SyntaxKind.Comma;

                case SyntaxKind.Semicolon:
                    return SyntaxKind.Semicolon;
                    // case TokenType.QuestionMark: return Kind.EqualityOperator;
            }

            return SyntaxKind.Identifier;
        }

        public static bool IsRightAssociative(SyntaxKind op)
        {
            switch (op)
            {
                case SyntaxKind.Equals:
                case SyntaxKind.PlusEquals:
                case SyntaxKind.MinusEquals:
                case SyntaxKind.MultiplyEquals:
                case SyntaxKind.DivideEquals:
                case SyntaxKind.ModuloEquals:
                case SyntaxKind.AndEquals:
                case SyntaxKind.CaretEquals:
                case SyntaxKind.OrEquals:
                case SyntaxKind.LessLessEquals:
                case SyntaxKind.GreaterGreaterEquals:
                case SyntaxKind.QuestionQuestion:
                    // case SyntaxKind.Coalesce:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsInvalidSubExpression(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.KeywordBreak:
                case SyntaxKind.KeywordCase:
                case SyntaxKind.KeywordContinue:
                case SyntaxKind.KeywordDo:
                case SyntaxKind.KeywordFor:
                case SyntaxKind.KeywordForEach:
                case SyntaxKind.KeywordGoto:
                case SyntaxKind.KeywordIf:
                case SyntaxKind.KeywordReturn:
                case SyntaxKind.KeywordSwitch:
                case SyntaxKind.KeywordWhile:
                    return true;
                default:
                    return false;
            }
        }

        public static Precedence GetPrecedence(SyntaxKind op)
        {
            if (IsAssignment(op)) return Precedence.Assignment;

            switch (op)
            {
                // case SyntaxKind.CoalesceExpression: return Precedence.Coalescing;
                case SyntaxKind.LogicalOr: return Precedence.ConditionalOr;
                case SyntaxKind.LogicalAnd: return Precedence.ConditionalAnd;
                case SyntaxKind.BitOr: return Precedence.LogicalOr;
                case SyntaxKind.BitXor: return Precedence.LogicalXor;
                case SyntaxKind.BitAnd: return Precedence.LogicalAnd;
                case SyntaxKind.EqualsEquals:
                case SyntaxKind.NotEquals: return Precedence.Equality;
                case SyntaxKind.ConditionalLessThan:
                case SyntaxKind.ConditionalLessOrEquals:
                case SyntaxKind.ConditionalGreaterThan:
                case SyntaxKind.ConditionalGreaterOrEquals:
                    //case SyntaxKind.IsExpression:
                    //case SyntaxKind.AsExpression:
                    //case SyntaxKind.IsPatternExpression:
                    return Precedence.Relational;
                case SyntaxKind.LessLess:
                case SyntaxKind.GreaterGreater: return Precedence.Shift;
                case SyntaxKind.Plus:
                case SyntaxKind.Minus: return Precedence.Additive;
                case SyntaxKind.Multiply:
                case SyntaxKind.Divide:
                case SyntaxKind.Modulo: return Precedence.Mutiplicative;
                case SyntaxKind.PlusPlus:
                case SyntaxKind.MinusMinus:
                case SyntaxKind.BitNot:
                case SyntaxKind.Not:
                case SyntaxKind.UnaryIncrement:
                case SyntaxKind.UnaryDecrement:
                    // case SyntaxKind.AwaitExpression:
                    return Precedence.Unary;
                // case SyntaxKind.CastExpression: return Precedence.Cast;                
                default:
                    return Precedence.Expression;
            }
        }

        internal static bool IsStatementExpression(SyntaxNode node)
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

            switch (node.Kind)
            {
                case SyntaxKind.PostfixDecrement:
                case SyntaxKind.PostfixIncrement:
                case SyntaxKind.FunctionInvocation:
                    return true;
                    //case Kind.NullConditionalMemberAccess:
                    //    var access = ... get the non-null node and check with that one
                    //    return IsStatementExpression(node....);
            }
            if (IsAssignment(node.Kind))
                return true;

            switch (node.Kind)
            {
                case SyntaxKind.UnaryIncrement:
                case SyntaxKind.UnaryDecrement:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsExpectedPrefixUnaryOperator(SyntaxKind kind) => IsPrefixUnaryExpression(kind);

        public static bool IsExpectedBinaryOperator(SyntaxKind kind) => IsBinaryExpression(kind);

        public static bool IsExpectedAssignmentOperator(SyntaxKind kind) => IsAssignment(kind);

        public static bool IsAnyUnaryExpression(SyntaxKind token) => IsPrefixUnaryExpression(token) || IsPostfixUnaryExpression(token);

        public static bool IsPrefixUnaryExpression(SyntaxKind token) => GetPrefixUnaryExpression(token) != SyntaxKind.None;

        public static bool IsPrefixUnaryExpressionOperatorToken(SyntaxKind token) => GetPrefixUnaryExpression(token) != SyntaxKind.None;

        public static bool IsPostfixUnaryExpression(SyntaxKind token) => GetPostfixUnaryExpression(token) != SyntaxKind.None;

        public static bool IsPostfixUnaryExpressionToken(SyntaxKind token) => GetPostfixUnaryExpression(token) != SyntaxKind.None;

        public static SyntaxKind GetPostfixUnaryExpression(SyntaxKind token)
        {
            switch (token)
            {
                case SyntaxKind.UnaryIncrement: case SyntaxKind.PlusPlus: return SyntaxKind.PostfixIncrement;
                case SyntaxKind.UnaryDecrement: case SyntaxKind.PostfixDecrement: case SyntaxKind.MinusMinus: return SyntaxKind.PostfixDecrement;
                default:
                    return SyntaxKind.None;
            }
        }
        public static SyntaxKind GetPrefixUnaryExpression(SyntaxKind token)
        {
            switch (token)
            {
                case SyntaxKind.Plus: return SyntaxKind.PlusPlus;
                case SyntaxKind.Minus: return SyntaxKind.MinusMinus;
                // case SyntaxKind.UnaryNot: case SyntaxKind.Tilde: return SyntaxKind.UnaryNot;
                case SyntaxKind.UnaryNot: case SyntaxKind.Not: return SyntaxKind.UnaryNot;
                case SyntaxKind.UnaryIncrement: case SyntaxKind.PlusPlus: return SyntaxKind.UnaryIncrement;
                case SyntaxKind.UnaryDecrement: case SyntaxKind.MinusMinus: return SyntaxKind.UnaryDecrement;
                case SyntaxKind.UnaryExpression: return SyntaxKind.UnaryExpression;
                default: return SyntaxKind.None;
            }
        }

        public static bool IsBinaryExpression(SyntaxKind token)
        {
            return GetBinaryExpression(token) != SyntaxKind.None;
        }

        public static bool IsBinaryExpressionOperatorToken(SyntaxKind token)
        {
            return GetBinaryExpression(token) != SyntaxKind.None;
        }

        public static SyntaxKind GetBinaryExpression(SyntaxKind token)
        {
            switch (token)
            {
                // case SyntaxKind.QuestionQuestion: return SyntaxKind.CoalesceExpression;
                case SyntaxKind.BitOr: case SyntaxKind.Or: return SyntaxKind.BitOr;
                case SyntaxKind.BitXor: case SyntaxKind.Caret: return SyntaxKind.BitXor;
                case SyntaxKind.BitAnd: case SyntaxKind.And: return SyntaxKind.BitAnd;
                case SyntaxKind.EqualsEquals: return SyntaxKind.EqualsEquals;
                case SyntaxKind.NotEquals: return SyntaxKind.NotEquals;
                case SyntaxKind.ConditionalLessThan: case SyntaxKind.Less: return SyntaxKind.ConditionalLessThan;
                case SyntaxKind.ConditionalLessOrEquals: case SyntaxKind.LessEquals: return SyntaxKind.ConditionalLessOrEquals;
                case SyntaxKind.ConditionalGreaterThan: case SyntaxKind.Greater: return SyntaxKind.ConditionalGreaterThan;
                case SyntaxKind.ConditionalGreaterOrEquals: case SyntaxKind.GreaterEquals: return SyntaxKind.ConditionalGreaterOrEquals;

                case SyntaxKind.LessLess:
                case SyntaxKind.GreaterGreater:
                case SyntaxKind.Plus:
                case SyntaxKind.Minus:
                case SyntaxKind.Divide:
                case SyntaxKind.Modulo:
                case SyntaxKind.Multiply:
                    return token;

                case SyntaxKind.LogicalAnd: case SyntaxKind.AndAnd: return SyntaxKind.LogicalAnd;
                case SyntaxKind.LogicalOr: case SyntaxKind.OrOr: return SyntaxKind.LogicalOr;
                default: return SyntaxKind.None;
            }
        }

        public static bool IsMath(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.Plus:
                case SyntaxKind.Minus:
                case SyntaxKind.Divide:
                case SyntaxKind.Modulo:
                case SyntaxKind.And:
                case SyntaxKind.BitAnd:
                case SyntaxKind.Or:
                case SyntaxKind.BitOr:
                case SyntaxKind.BitXor:
                case SyntaxKind.Tilde:
                case SyntaxKind.GreaterGreater:
                case SyntaxKind.LessLess:
                case SyntaxKind.BinaryExpression:
                    return true;
                default: return false;
            }
        }

        public static bool IsAssignment(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.OrEquals:
                case SyntaxKind.AndEquals:
                case SyntaxKind.CaretEquals:
                case SyntaxKind.LessLessEquals:
                case SyntaxKind.GreaterGreaterEquals:
                case SyntaxKind.PlusEquals:
                case SyntaxKind.MinusEquals:
                case SyntaxKind.MultiplyEquals:
                case SyntaxKind.DivideEquals:
                case SyntaxKind.ModuloEquals:
                case SyntaxKind.Equals:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsIncrementOrDecrementOperator(SyntaxKind token)
        {
            switch (token)
            {
                case SyntaxKind.PlusPlus:
                case SyntaxKind.MinusMinus:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsPredefinedType(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.KeywordNumber:
                case SyntaxKind.KeywordBoolean:
                case SyntaxKind.KeywordString:
                case SyntaxKind.KeywordDate:
                case SyntaxKind.KeywordAny:
                case SyntaxKind.KeywordVoid:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLiteral(SyntaxKind kind)
        {
            return kind == SyntaxKind.Number || kind == SyntaxKind.String || kind == SyntaxKind.LiteralExpression;
        }

        public static bool IsMemberAccess(SyntaxKind kind)
        {
            return kind == SyntaxKind.Dot || kind == SyntaxKind.MemberAccess ||
                   kind == SyntaxKind.NullConditionalMemberAccess || kind == SyntaxKind.MinusGreater ||
                   kind == SyntaxKind.StaticMemberAccess;
        }


        public static bool IsEquality(SyntaxKind kind)
        {
            return kind == SyntaxKind.EqualsEquals || kind == SyntaxKind.NotEquals || kind == SyntaxKind.LessEquals || kind == SyntaxKind.Less || kind == SyntaxKind.Greater || kind == SyntaxKind.Greater;
        }

        public static bool IsOpenStatement(SyntaxKind kind) => kind == SyntaxKind.OpenParan;
        public static bool IsCloseStatement(SyntaxKind kind) => kind == SyntaxKind.CloseParan;
        public static bool IsOpenIndexer(SyntaxKind kind) => kind == SyntaxKind.OpenBracket;
        public static bool IsCloseIndexer(SyntaxKind kind) => kind == SyntaxKind.CloseBracket;
        public static bool IsOpenBody(SyntaxKind kind) => kind == SyntaxKind.OpenCurly;
        public static bool IsCloseBody(SyntaxKind kind) => kind == SyntaxKind.CloseCurly;
    }
}
