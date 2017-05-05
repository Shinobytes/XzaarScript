using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Ast
{
    public class SyntaxTokenStream : CollectionStream<Token>
    {
        public SyntaxTokenStream(IList<Token> items) : base(items)
        {
        }
    }
}