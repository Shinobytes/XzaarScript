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