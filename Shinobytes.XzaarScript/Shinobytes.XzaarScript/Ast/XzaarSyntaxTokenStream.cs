using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarSyntaxTokenStream : XzaarCollectionStream<XzaarToken>
    {
        public XzaarSyntaxTokenStream(IList<XzaarToken> items) : base(items)
        {
        }
    }
}