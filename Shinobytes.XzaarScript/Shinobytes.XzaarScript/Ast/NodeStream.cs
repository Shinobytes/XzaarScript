using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class NodeStream : CollectionStream<SyntaxNode>
    {
        public NodeStream(IList<SyntaxNode> items)
            : base(items)
        {
        }

    }
}