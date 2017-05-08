using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class TokenStream : CollectionStream<SyntaxToken>
    {
        public TokenStream(IList<SyntaxToken> items)
            : base(items)
        {
        }

        public bool NextIs(SyntaxKind kind)
        {
            var peekNext = PeekNext();
            return peekNext != null && peekNext.Kind == kind;
        }

        public SyntaxToken Consume(SyntaxKind kind)
        {
            return this.Consume(x => x.Kind == kind);
        }

        public SyntaxToken ConsumeExpected(SyntaxKind kind)
        {
            return this.Consume(x => x.Kind == kind);
        }
    }

    public class NodeStream : CollectionStream<SyntaxNode>
    {
        public NodeStream(IList<SyntaxNode> items)
            : base(items)
        {
        }

    }
}