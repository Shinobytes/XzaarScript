using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Ast
{
    public class StringCollectionStream : CollectionStream<char>
    {
        public StringCollectionStream(IList<char> items) : base(items)
        {
        }
    }
}