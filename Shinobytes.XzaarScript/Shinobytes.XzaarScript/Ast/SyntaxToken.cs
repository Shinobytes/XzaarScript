using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
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


        public TokenPositionInfo PositionInfo => new TokenPositionInfo(SourceIndex, SourceLine, SourceColumn, -1);

        public override string ToString()
        {
            return $"{Kind} {TypeName} {Value}";
        }
    }

    public struct TokenPositionInfo
    {
        public readonly int Column;
        public readonly int Index;
        public readonly int Line;
        public readonly int NodeIndex;

        public TokenPositionInfo(int index, int line, int column, int nodeIndex)
        {
            this.Index = index;
            NodeIndex = nodeIndex;
            this.Line = line;
            this.Column = column;
        }


        public static TokenPositionInfo Empty => new TokenPositionInfo();

        public override string ToString()
        {
            return "[" + Index + "] Line: " + Line + ", Column: " + Column;
        }
    }
}