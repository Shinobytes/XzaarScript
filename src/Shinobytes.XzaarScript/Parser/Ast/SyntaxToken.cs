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