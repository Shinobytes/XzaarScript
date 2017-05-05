using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarSyntaxTokenCollection : Collection<XzaarSyntaxToken>
    {        
        private int currentTokenIndex;
        private int currentTokenOffset;
        private int currentTokenLine;
        private int currentTokenColumn;
        public override void Add(XzaarSyntaxToken item)
        {
            base.Add(new XzaarSyntaxToken(item.Key, item.Type, item.Value, currentTokenIndex++, currentTokenOffset, currentTokenLine, currentTokenColumn));
            currentTokenOffset += item.Value.Length;
        }

        public void AdvanceSourcePosition(int count = 1)
        {
            currentTokenOffset += count;
            currentTokenColumn += count;
        }

        public void AdvanceSourceNewLine()
        {
            currentTokenLine++;
            currentTokenColumn = 0;
        }
    }
}