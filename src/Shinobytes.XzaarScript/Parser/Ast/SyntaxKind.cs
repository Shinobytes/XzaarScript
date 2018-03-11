/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

namespace Shinobytes.XzaarScript.Parser.Ast
{
    /// <summary>
    /// all enum values are ordered by their precedence order
    /// </summary>
    public enum SyntaxKind
    {
        Identifier,
        Expression,
        ArrayIndexExpression,

        // Highest precedence operators            
        MemberAccess,                // x.y
        NullConditionalMemberAccess, // x?.y
        FunctionInvocation,          // f(x)
        AggregateObjectIndex,        // a[x]
        PostfixIncrement,            // x++, returns the value of x and then increments the value by 1
        PostfixDecrement,            // x--, returns the value of x and then decrements the value by 1
        TypeInstantiation,           // Commonly 'new' keyword, for               

        // sub types
        None,
        Number,
        String,

        //
        UnaryNot,
        UnaryBitwiseComplement, // tilde
        UnaryIncrement,
        UnaryDecrement,

        ConditionalLessThan,
        ConditionalGreaterThan,
        ConditionalLessOrEquals,
        ConditionalGreaterOrEquals,

        BitAnd,
        BitXor,
        BitOr,
        BitNot, // ~

        Not,

        LogicalAnd,
        LogicalOr,

        //Assign,
        //AssignPlus,
        //AssignMinus,
        //AssignMultiply,
        //AssignDivide,
        //AssignModulo,
        //AssignOr,
        //AssignAnd,
        //AssignXor,
        //AssignLeftShift,
        //AssignRightShift,

        KeywordIf,
        KeywordSwitch,
        KeywordMatch,
        KeywordCase,
        KeywordWhile,
        KeywordStruct,
        KeywordClass,
        KeywordFn,
        KeywordLoop,
        KeywordDo,
        KeywordFor,
        KeywordForEach,
        KeywordExtern,
        KeywordEnum,
        KeywordNumber,
        KeywordString,
        KeywordBoolean,

        KeywordDate,
        KeywordAny,
        KeywordVar,
        KeywordReturn,
        KeywordGoto,
        KeywordContinue,
        KeywordBreak,
        KeywordVoid,
        KeywordElse,
        KeywordNew,
        KeywordTrue,
        KeywordFalse,
        KeywordNull,
        KeywordIn,
        KeywordDefault,

        KeywordIs,
        KeywordAs,

        // token types

        Tilde,
        Minus,
        MinusEquals,
        MinusMinus,
        MinusGreater,
        Plus,
        PlusPlus,
        PlusEquals,
        Multiply,
        MultiplyEquals,
        Divide,
        DivideEquals,
        DivideDivide,
        Modulo,
        ModuloEquals,
        Equals,
        EqualsEquals,
        EqualsGreater,
        Greater,
        GreaterEquals,
        GreaterGreater,
        GreaterGreaterEquals,
        Less,
        LessEquals,
        LessLess,
        LessLessEquals,
        And,
        AndEquals,
        AndAnd,
        Or,
        OrEquals,
        OrOr,
        Caret,
        CaretEquals,


        NotEquals,

        Question,
        QuestionQuestion,

        Semicolon,
        Colon,
        ColonColon,
        Comma,
        Dot,
        OpenParen,
        CloseParen,
        OpenCurly,
        CloseCurly,
        OpenBracket,
        CloseBracket,
        ArgList,
        ArgumentList,

        Newline,
        Whitespace,
        CommentMultiLine,
        CommentSingleLine,

        Block,

        Undefined,
        Empty,
        StaticMemberAccess,

        // Expressions

        ArgumentExpression,
        MemberDefinitionExpression,

        //// +x, -x, !x, ~x, ++x, --x
        UnaryExpression,

        //// x * y, x / y, x % y, x + y, x - y, x << y, x >> y
        ArithmeticExpression,

        //// x < y, x > y, x <= y, x >= y
        LogicalConditionalExpression,

        //// x == y, x != y
        EqualityExpression,

        //// x & y, x ^ y, x | y
        LogicalBitExpression,

        //// x && y, x || y
        ConditionalExpression,

        //// x ?? y - returns ix if non-null; oteherwise returns y
        NullCoalescingExpression,

        //// x = y, x+= y, x -= y, x *= y, x /= y, x %= y, x |= y, x ^= y, x <<= y, x >>= y, =>
        AssignmentExpression,

        FieldDefinitionExpression,
        FunctionDefinitionExpression,
        LambdaFunctionDefinitionExpression,

        ParameterDefinitionExpression,
        StructDefinitionExpression,
        VariableDefinitionExpression,

        // When making a number into a negative number by starting with an unary minus (-). E.g. -9999
        NegativeExpression,
        // when inverting a boolean expression's return value using the bang/not (!) E.g. !true
        NegateExpression,
        // same as negate really
        LogicalNotExpression,

        // Can be Arithmetic Expression
        BinaryExpression,

        // Rust-like Match expression
        MatchExpression,

        ParameterListExpression,
        LabelDefinitionExpression,
        LoopExpression,
        LiteralExpression
    }
}