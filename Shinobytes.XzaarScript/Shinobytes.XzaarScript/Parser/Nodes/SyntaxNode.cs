using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class SyntaxNode
    {
        private readonly Guid instanceId;
        private SyntaxToken trailingToken;
        private SyntaxToken leadingToken;
        private SyntaxKind kind;

        public SyntaxNode(int index, SyntaxKind kind)
            : this(null, index, null, kind)
        {
        }

        public SyntaxNode(int index, object value, SyntaxKind kind)
            : this(null, index, value, kind)
        {
        }

        public SyntaxNode(SyntaxNode parent, int index, object value, SyntaxKind kind)
        {
            this.instanceId = Guid.NewGuid();
            this.Parent = parent;
            this.Index = index;
            this.Value = value;
            Kind = kind;
            this.Children = new List<SyntaxNode>();
            if (this.Parent != null)
            {
                this.Parent.AddChild(this);
            }
        }

        public SyntaxKind Kind { get; set; }

        public int Index { get; set; }
        public object Value { get; set; }
        public string StringValue => Value + "";
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Children { get; set; }

        public bool HasChildren => Children != null && Children.Count > 0;

        public SyntaxToken TrailingToken
        {
            get { return trailingToken; }
            set { trailingToken = value; }
        }

        public SyntaxToken LeadingToken
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
            return $"{this.Index} {this.Kind} {{{this.Value}}}";
        }
    }
}