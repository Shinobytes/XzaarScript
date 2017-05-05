using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarNodeStream : XzaarCollectionStream<XzaarSyntaxNode>
    {
        public XzaarNodeStream(IList<XzaarSyntaxNode> items)
            : base(items)
        {
        }

    }
}