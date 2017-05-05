using System.Linq;
using System.Linq.Expressions;
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


        public static bool IsKeyword(string s)
        {

            return knownKeywords.Contains(s.ToLower()) || IsKnownConstant(s);
        }

        public static bool IsKnownConstant(string s)
        {
            return knownConstants.Contains(s.ToLower());
        }

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


        public static SyntaxKind GetSubKind(SyntaxKind syntaxKind, SyntaxKind tokenType)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.Literal:
                    {
                        if (tokenType == SyntaxKind.Number) return SyntaxKind.LiteralNumber;
                        if (tokenType == SyntaxKind.String) return SyntaxKind.LiteralString;
                        break;
                    }
                case SyntaxKind.UnaryOperator:
                    {
                        if (tokenType == SyntaxKind.Not) return SyntaxKind.UnaryNot;
                        // if (tokenType == TokenType.Tilde)
                        if (tokenType == SyntaxKind.Plus) return SyntaxKind.UnaryPlus;
                        if (tokenType == SyntaxKind.Minus) return SyntaxKind.UnaryMinus;
                        if (tokenType == SyntaxKind.PlusPlus) return SyntaxKind.UnaryIncrement;
                        if (tokenType == SyntaxKind.MinusMinus) return SyntaxKind.UnaryDecrement;
                        break;
                    }
                case SyntaxKind.ArithmeticOperator:
                    {
                        if (tokenType == SyntaxKind.Multiply) return SyntaxKind.ArithmeticMultiply;
                        if (tokenType == SyntaxKind.Divide) return SyntaxKind.ArithmeticDivide;
                        if (tokenType == SyntaxKind.Plus) return SyntaxKind.ArithmeticAdd;
                        if (tokenType == SyntaxKind.Minus) return SyntaxKind.ArithmeticSubtract;
                        if (tokenType == SyntaxKind.Modulo) return SyntaxKind.ArithmeticModulo;
                        break;
                    }
                case SyntaxKind.LogicalConditionalOperator:
                    {
                        if (tokenType == SyntaxKind.Less) return SyntaxKind.ConditionalLessThan;
                        if (tokenType == SyntaxKind.Greater) return SyntaxKind.ConditionalGreaterThan;
                        if (tokenType == SyntaxKind.LessEquals) return SyntaxKind.ConditionalLessOrEquals;
                        if (tokenType == SyntaxKind.GreaterEquals) return SyntaxKind.ConditionalGreaterOrEquals;
                        if (tokenType == SyntaxKind.OrOr) return SyntaxKind.LogicalOr;
                        if (tokenType == SyntaxKind.AndAnd) return SyntaxKind.LogicalAnd;
                        break;
                    }
                case SyntaxKind.EqualityOperator:
                    {
                        if (tokenType == SyntaxKind.EqualsEquals) return SyntaxKind.EqualityEquals;
                        if (tokenType == SyntaxKind.NotEquals) return SyntaxKind.EqualityNotEquals;
                        break;
                    }
                case SyntaxKind.LogicalBitOperator:
                    {
                        if (tokenType == SyntaxKind.And) return SyntaxKind.BitAnd;
                        // if (tokenType == TokenType.Xor) return SyntaxKind.BitXor;
                        if (tokenType == SyntaxKind.Or) return SyntaxKind.BitOr;
                        break;
                    }
                case SyntaxKind.ConditionalOperator:
                    {
                        if (tokenType == SyntaxKind.AndAnd) return SyntaxKind.LogicalAnd;
                        if (tokenType == SyntaxKind.OrOr) return SyntaxKind.LogicalOr;
                        break;
                    }
                //case NodeType.NullCoalescingOperator:
                //    {
                //        if (tokenType == TokenType.QuestionQuestion) return SyntaxKind...;                        
                //        break;
                //    }
                case SyntaxKind.AssignmentOperator:
                    {
                        if (tokenType == SyntaxKind.Equals) return SyntaxKind.Assign;
                        if (tokenType == SyntaxKind.PlusEquals) return SyntaxKind.AssignPlus;
                        if (tokenType == SyntaxKind.MinusEquals) return SyntaxKind.AssignMinus;
                        if (tokenType == SyntaxKind.DivideEquals) return SyntaxKind.AssignDivide;
                        if (tokenType == SyntaxKind.MultiplyEquals) return SyntaxKind.AssignMultiply;
                        if (tokenType == SyntaxKind.ModuloEquals) return SyntaxKind.AssignModulo;
                        if (tokenType == SyntaxKind.LessLessEquals) return SyntaxKind.AssignLeftShift;
                        if (tokenType == SyntaxKind.GreaterGreaterEquals) return SyntaxKind.AssignRightShift;
                        if (tokenType == SyntaxKind.EqualsGreater) return SyntaxKind.Lambda;
                        if (tokenType == SyntaxKind.OrEquals) return SyntaxKind.AssignOr;
                        break;
                    }
            }
            return SyntaxKind.None;
        }

        public static SyntaxKind GetNodeType(SyntaxKind tokenType)
        {
            switch (tokenType)
            {
                case SyntaxKind.Dot: return SyntaxKind.MemberAccess;

                // case TokenType.QuestionMarkDot: return NodeType.NullConditionalMemberAccess;
                case SyntaxKind.Identifier: return SyntaxKind.Identifier;

                case SyntaxKind.Number:
                case SyntaxKind.String:
                    return SyntaxKind.Literal;

                case SyntaxKind.MinusGreater:
                    return SyntaxKind.PointerMemberAccess;

                case SyntaxKind.MinusMinus:
                case SyntaxKind.PlusPlus:
                case SyntaxKind.Not:
                    return SyntaxKind.UnaryOperator;

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
                    return SyntaxKind.AssignmentOperator;

                case SyntaxKind.GreaterGreater:
                case SyntaxKind.LessLess:
                case SyntaxKind.Minus:
                case SyntaxKind.Plus:
                case SyntaxKind.Multiply:
                case SyntaxKind.Divide:
                case SyntaxKind.Modulo:
                    return SyntaxKind.ArithmeticOperator;

                case SyntaxKind.GreaterEquals:
                case SyntaxKind.Greater:
                case SyntaxKind.LessEquals:
                case SyntaxKind.Less:
                case SyntaxKind.AndAnd:
                case SyntaxKind.OrOr:
                    return SyntaxKind.LogicalConditionalOperator;

                case SyntaxKind.Or:
                case SyntaxKind.And:
                    return SyntaxKind.LogicalBitOperator;

                case SyntaxKind.EqualsEquals:
                case SyntaxKind.NotEquals:
                    return SyntaxKind.EqualityOperator;

                case SyntaxKind.ColonColon:
                case SyntaxKind.Colon:
                    return tokenType;

                case SyntaxKind.Comma:
                    return SyntaxKind.Separator;

                case SyntaxKind.Semicolon:
                    return SyntaxKind.StatementTerminator;
                    // case TokenType.QuestionMark: return NodeType.EqualityOperator;
            }

            return SyntaxKind.Identifier;
        }

        public static bool IsRightAssociative(SyntaxKind op)
        {
            switch (op)
            {
                case SyntaxKind.Assign:
                case SyntaxKind.AssignPlus:
                case SyntaxKind.AssignMinus:
                case SyntaxKind.AssignMultiply:
                case SyntaxKind.AssignDivide:
                case SyntaxKind.AssignModulo:
                case SyntaxKind.AssignAnd:
                case SyntaxKind.AssignXor:
                case SyntaxKind.AssignOr:
                case SyntaxKind.AssignLeftShift:
                case SyntaxKind.AssignRightShift:
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
            switch (op)
            {
                case SyntaxKind.Assign:
                case SyntaxKind.AssignPlus:
                case SyntaxKind.AssignMinus:
                case SyntaxKind.AssignMultiply:
                case SyntaxKind.AssignDivide:
                case SyntaxKind.AssignModulo:
                case SyntaxKind.AssignAnd:
                case SyntaxKind.AssignXor:
                case SyntaxKind.AssignOr:
                case SyntaxKind.AssignLeftShift:
                case SyntaxKind.AssignRightShift: return Precedence.Assignment;
                // case SyntaxKind.CoalesceExpression: return Precedence.Coalescing;
                case SyntaxKind.LogicalOr: return Precedence.ConditionalOr;
                case SyntaxKind.LogicalAnd: return Precedence.ConditionalAnd;
                case SyntaxKind.BitOr: return Precedence.LogicalOr;
                case SyntaxKind.BitXor: return Precedence.LogicalXor;
                case SyntaxKind.BitAnd: return Precedence.LogicalAnd;
                case SyntaxKind.EqualityEquals:
                case SyntaxKind.EqualityNotEquals: return Precedence.Equality;
                case SyntaxKind.ConditionalLessThan:
                case SyntaxKind.ConditionalLessOrEquals:
                case SyntaxKind.ConditionalGreaterThan:
                case SyntaxKind.ConditionalGreaterOrEquals:
                    //case SyntaxKind.IsExpression:
                    //case SyntaxKind.AsExpression:
                    //case SyntaxKind.IsPatternExpression:
                    return Precedence.Relational;
                case SyntaxKind.ArithmeticLeftShift:
                case SyntaxKind.ArithmeticRightShift: return Precedence.Shift;
                case SyntaxKind.ArithmeticAdd:
                case SyntaxKind.ArithmeticSubtract: return Precedence.Additive;
                case SyntaxKind.ArithmeticMultiply:
                case SyntaxKind.ArithmeticDivide:
                case SyntaxKind.ArithmeticModulo: return Precedence.Mutiplicative;
                case SyntaxKind.UnaryPlus:
                case SyntaxKind.UnaryMinus:
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

            switch (node.Type)
            {
                case SyntaxKind.PostfixDecrement:
                case SyntaxKind.PostfixIncrement:
                case SyntaxKind.FunctionInvocation:
                    return true;
                    //case NodeType.NullConditionalMemberAccess:
                    //    var access = ... get the non-null node and check with that one
                    //    return IsStatementExpression(node....);
            }

            switch (node.Kind)
            {

                case SyntaxKind.Assign:
                // case ObjectCreationExpression:
                case SyntaxKind.AssignPlus:
                case SyntaxKind.AssignMinus:
                case SyntaxKind.AssignMultiply:
                case SyntaxKind.AssignDivide:
                case SyntaxKind.AssignModulo:
                case SyntaxKind.AssignAnd:
                case SyntaxKind.AssignOr:
                case SyntaxKind.AssignXor:
                case SyntaxKind.AssignLeftShift:
                case SyntaxKind.AssignRightShift:
                case SyntaxKind.UnaryIncrement:
                case SyntaxKind.UnaryDecrement:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsExpectedPrefixUnaryOperator(SyntaxKind kind)
        {
            return IsPrefixUnaryExpression(kind);
        }

        public static bool IsExpectedBinaryOperator(SyntaxKind kind)
        {
            return IsBinaryExpression(kind);
        }

        public static bool IsExpectedAssignmentOperator(SyntaxKind kind)
        {
            return IsAssignmentExpressionOperatorToken(kind);
        }


        public static bool IsAnyUnaryExpression(SyntaxKind token)
        {
            return IsPrefixUnaryExpression(token) || IsPostfixUnaryExpression(token);
        }

        public static bool IsPrefixUnaryExpression(SyntaxKind token)
        {
            return GetPrefixUnaryExpression(token) != SyntaxKind.None;
        }

        public static bool IsPrefixUnaryExpressionOperatorToken(SyntaxKind token)
        {
            return GetPrefixUnaryExpression(token) != SyntaxKind.None;
        }
        public static bool IsPostfixUnaryExpression(SyntaxKind token)
        {
            return GetPostfixUnaryExpression(token) != SyntaxKind.None;
        }

        public static bool IsPostfixUnaryExpressionToken(SyntaxKind token)
        {
            return GetPostfixUnaryExpression(token) != SyntaxKind.None;
        }

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
                case SyntaxKind.ArithmeticAdd: case SyntaxKind.UnaryPlus: case SyntaxKind.Plus: return SyntaxKind.UnaryPlus;
                case SyntaxKind.ArithmeticSubtract: case SyntaxKind.UnaryMinus: case SyntaxKind.Minus: return SyntaxKind.UnaryMinus;
                // case SyntaxKind.UnaryNot: case SyntaxKind.Tilde: return SyntaxKind.UnaryNot;
                case SyntaxKind.UnaryNot: case SyntaxKind.Not: return SyntaxKind.UnaryNot;
                case SyntaxKind.UnaryIncrement: case SyntaxKind.PlusPlus: return SyntaxKind.UnaryIncrement;
                case SyntaxKind.UnaryDecrement: case SyntaxKind.MinusMinus: return SyntaxKind.UnaryDecrement;
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
                case SyntaxKind.EqualityEquals: case SyntaxKind.EqualsEquals: return SyntaxKind.EqualityEquals;
                case SyntaxKind.EqualityNotEquals: case SyntaxKind.NotEquals: return SyntaxKind.EqualityNotEquals;
                case SyntaxKind.ConditionalLessThan: case SyntaxKind.Less: return SyntaxKind.ConditionalLessThan;
                case SyntaxKind.ConditionalLessOrEquals: case SyntaxKind.LessEquals: return SyntaxKind.ConditionalLessOrEquals;
                case SyntaxKind.ConditionalGreaterThan: case SyntaxKind.Greater: return SyntaxKind.ConditionalGreaterThan;
                case SyntaxKind.ConditionalGreaterOrEquals: case SyntaxKind.GreaterEquals: return SyntaxKind.ConditionalGreaterOrEquals;
                case SyntaxKind.ArithmeticLeftShift: case SyntaxKind.LessLess: return SyntaxKind.ArithmeticLeftShift;
                case SyntaxKind.ArithmeticRightShift: case SyntaxKind.GreaterGreater: return SyntaxKind.ArithmeticRightShift;
                case SyntaxKind.ArithmeticAdd: case SyntaxKind.Plus: return SyntaxKind.ArithmeticAdd;
                case SyntaxKind.ArithmeticSubtract: case SyntaxKind.Minus: return SyntaxKind.ArithmeticSubtract;
                case SyntaxKind.ArithmeticMultiply: case SyntaxKind.Multiply: return SyntaxKind.ArithmeticMultiply;
                case SyntaxKind.ArithmeticDivide: case SyntaxKind.Divide: return SyntaxKind.ArithmeticDivide;
                case SyntaxKind.ArithmeticModulo: case SyntaxKind.Modulo: return SyntaxKind.ArithmeticModulo;
                case SyntaxKind.LogicalAnd: case SyntaxKind.AndAnd: return SyntaxKind.LogicalAnd;
                case SyntaxKind.LogicalOr: case SyntaxKind.OrOr: return SyntaxKind.LogicalOr;
                default: return SyntaxKind.None;
            }
        }

        public static bool IsAssignmentExpression(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AssignmentOperator:
                case SyntaxKind.AssignOr:
                case SyntaxKind.AssignAnd:
                case SyntaxKind.AssignXor:
                case SyntaxKind.AssignLeftShift:
                case SyntaxKind.AssignRightShift:
                case SyntaxKind.AssignPlus:
                case SyntaxKind.AssignMinus:
                case SyntaxKind.AssignMultiply:
                case SyntaxKind.AssignDivide:
                case SyntaxKind.AssignModulo:
                case SyntaxKind.Assign:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAssignmentExpressionOperatorToken(SyntaxKind token)
        {
            switch (token)
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

        public static SyntaxKind GetAssignmentExpression(SyntaxKind token)
        {
            switch (token)
            {
                case SyntaxKind.OrEquals: return SyntaxKind.AssignOr;
                case SyntaxKind.AndEquals: return SyntaxKind.AssignAnd;
                case SyntaxKind.CaretEquals: return SyntaxKind.AssignXor;
                case SyntaxKind.LessLessEquals: return SyntaxKind.AssignLeftShift;
                case SyntaxKind.GreaterGreaterEquals: return SyntaxKind.AssignRightShift;
                case SyntaxKind.PlusEquals: return SyntaxKind.AssignPlus;
                case SyntaxKind.MinusEquals: return SyntaxKind.AssignMinus;
                case SyntaxKind.MultiplyEquals: return SyntaxKind.AssignMultiply;
                case SyntaxKind.DivideEquals: return SyntaxKind.AssignDivide;
                case SyntaxKind.ModuloEquals: return SyntaxKind.AssignModulo;
                case SyntaxKind.Equals: return SyntaxKind.Assign;
                default:
                    return SyntaxKind.None;
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

    }
}
