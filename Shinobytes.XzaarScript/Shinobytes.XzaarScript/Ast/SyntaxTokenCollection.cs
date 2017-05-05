using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.Ast
{
    public class SyntaxTokenCollection : Collection<SyntaxToken>
    {        
        private int currentTokenIndex;
        private int currentTokenOffset;
        private int currentTokenLine;
        private int currentTokenColumn;
        public override void Add(SyntaxToken item)
        {
            base.Add(new SyntaxToken(item.Key, item.Type, item.Value, currentTokenIndex++, currentTokenOffset, currentTokenLine, currentTokenColumn));
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