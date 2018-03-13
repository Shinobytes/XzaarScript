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
    public class SyntaxToken
    {
        public readonly string TypeName;
        public readonly SyntaxKind Kind;
        public readonly string Value;
        public readonly int TokenIndex;
        public readonly int SourceIndex;
        public readonly int SourceLine;
        public readonly int SourceColumn;
        public readonly int Size;
        public readonly bool IsSingleQuouteString;

        public SyntaxToken(int tokenIndex, int sourceIndex)
            : this(SyntaxKind.Undefined, null, null, false, tokenIndex, sourceIndex, 0, 0)
        {
            TokenIndex = tokenIndex;
            SourceIndex = sourceIndex;
        }

        public SyntaxToken(SyntaxKind kind, string typeName, string value)
            : this(kind, typeName, value, false, 0, 0, 0, 0)
        {
        }

        internal SyntaxToken(SyntaxKind kind, string typeName, string value, int tokenIndex, int sourceIndex, int sourceLine, int sourceColumn)
            : this(kind, typeName, value, false, tokenIndex, sourceIndex, sourceLine, sourceColumn)
        {
        }

        internal SyntaxToken(SyntaxKind kind, string typeName, string value, bool isSingleQuouteString, int tokenIndex, int sourceIndex, int sourceLine, int sourceColumn)
        {
            TokenIndex = tokenIndex;
            SourceIndex = sourceIndex;
            SourceLine = sourceLine + 1;
            SourceColumn = sourceColumn + 1;
            IsSingleQuouteString = isSingleQuouteString;
            Kind = kind;
            Value = value;
            TypeName = typeName;
            Size = value.Length;
        }


        public TokenPosition Position => new TokenPosition(SourceIndex, SourceLine, SourceColumn, -1);

        public override string ToString()
        {
            return $"{Kind} {TypeName} {Value}";
        }
    }
}