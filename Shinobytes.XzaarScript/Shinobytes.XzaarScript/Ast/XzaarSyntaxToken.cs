using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarSyntaxToken
    {
        public readonly string Type;
        public readonly XzaarAstNodeTypes Key;
        public readonly string Value;
        public readonly int TokenIndex;
        public readonly int SourceIndex;
        public readonly int SourceLine;
        public readonly int SourceColumn;
        public readonly int Size;

        public XzaarSyntaxToken(int tokenIndex, int sourceIndex)
        {
            TokenIndex = tokenIndex;
            SourceIndex = sourceIndex;
        }

        public XzaarSyntaxToken(XzaarAstNodeTypes key, string type, string value)
        {
            Key = key;
            Value = value;
            Type = type;
            Size = value.Length;
        }

        internal XzaarSyntaxToken(XzaarAstNodeTypes key, string type, string value, int tokenIndex, int sourceIndex, int sourceLine, int sourceColumn)
        {
            TokenIndex = tokenIndex;
            SourceIndex = sourceIndex;
            SourceLine = sourceLine + 1;
            SourceColumn = sourceColumn + 1;

            Key = key;
            Value = value;
            Type = type;
            Size = value.Length;
        }

        public XzaarSyntaxTokenPositionInfo PositionInfo
        {
            get
            {
                return new XzaarSyntaxTokenPositionInfo(SourceIndex, SourceLine, SourceColumn, -1);
            }
        }

        public override string ToString()
        {
            return $"{Key} {Type} {Value}";
        }
    }

    public struct XzaarSyntaxTokenPositionInfo
    {
        public readonly int Column;
        public readonly int Index;
        public readonly int Line;
        public readonly int NodeIndex;

        public XzaarSyntaxTokenPositionInfo(int index, int line, int column, int nodeIndex)
        {
            this.Index = index;
            NodeIndex = nodeIndex;
            this.Line = line;
            this.Column = column;
        }


        public static XzaarSyntaxTokenPositionInfo Empty => new XzaarSyntaxTokenPositionInfo();

        public override string ToString()
        {
            return "[" + Index + "] Line: " + Line + ", Column: " + Column;
        }
    }
}