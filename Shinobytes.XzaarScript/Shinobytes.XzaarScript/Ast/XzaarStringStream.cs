using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarStringStream : XzaarCollectionStream<char>
    {
        public XzaarStringStream(IList<char> items) : base(items)
        {
        }
    }
}