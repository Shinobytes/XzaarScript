using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class SyntaxNode
    {
        private readonly Guid instanceId;
        private Token trailingToken;
        private Token leadingToken;
        private SyntaxKind kind;

        public SyntaxNode(int index, SyntaxKind type, SyntaxKind kind)
            : this(null, index, null, type, kind)
        {
        }

        public SyntaxNode(int index, object value, SyntaxKind type, SyntaxKind kind)
            : this(null, index, value, type, kind)
        {
        }

        public SyntaxNode(SyntaxNode parent, int index, object value, SyntaxKind type, SyntaxKind kind)
        {
            this.instanceId = Guid.NewGuid();
            this.Parent = parent;
            this.Index = index;
            this.Value = value;
            Type = type;
            Kind = kind;
            this.Children = new List<SyntaxNode>();
            if (this.Parent != null)
            {
                this.Parent.AddChild(this);
            }
        }

        public SyntaxKind Type { get; set; }

        public SyntaxKind Kind
        {
            get
            {
                if (kind == SyntaxKind.None && Type != SyntaxKind.None)
                    return Type;
                return kind;
            }
            set { kind = value; }
        }

        public int Index { get; set; }
        public object Value { get; set; }
        public string StringValue => Value + "";
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Children { get; set; }

        public bool HasChildren => Children != null && Children.Count > 0;

        public Token TrailingToken
        {
            get { return trailingToken; }
            set { trailingToken = value; }
        }

        public Token LeadingToken
        {
            get { return leadingToken; }
            set { leadingToken = value; }
        }

        public void AddChild(SyntaxNode child)
        {
            if (!this.Children.Contains(child))
                this.Children.Add(child);
        }

        public void RemoveChild(SyntaxNode child)
        {
            if (child == null) return;
            child.Parent = null;
            if (this.Children.Contains(child))
                this.Children.Remove(child);
        }

        public override string ToString()
        {
            return this.Index + " " + this.Type + " (" + this.Kind + ") {" + this.Value + "}";
        }
    }
}