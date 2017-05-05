using System;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class XzaarSyntaxNode
    {
        private readonly Guid instanceId;
        private XzaarToken trailingToken;
        private XzaarToken leadingToken;
        private XzaarSyntaxKind kind;

        public XzaarSyntaxNode(int index, XzaarSyntaxKind type, XzaarSyntaxKind kind)
            : this(null, index, null, type, kind)
        {
        }

        public XzaarSyntaxNode(int index, object value, XzaarSyntaxKind type, XzaarSyntaxKind kind)
            : this(null, index, value, type, kind)
        {
        }

        public XzaarSyntaxNode(XzaarSyntaxNode parent, int index, object value, XzaarSyntaxKind type, XzaarSyntaxKind kind)
        {
            this.instanceId = Guid.NewGuid();
            this.Parent = parent;
            this.Index = index;
            this.Value = value;
            Type = type;
            Kind = kind;
            this.Children = new List<XzaarSyntaxNode>();
            if (this.Parent != null)
            {
                this.Parent.AddChild(this);
            }
        }

        public XzaarSyntaxKind Type { get; set; }

        public XzaarSyntaxKind Kind
        {
            get
            {
                if (kind == XzaarSyntaxKind.None && Type != XzaarSyntaxKind.None)
                    return Type;
                return kind;
            }
            set { kind = value; }
        }

        public int Index { get; set; }
        public object Value { get; set; }
        public string StringValue { get { return Value + ""; } }
        public XzaarSyntaxNode Parent { get; set; }
        public List<XzaarSyntaxNode> Children { get; set; }

        public bool HasChildren { get { return Children != null && Children.Count > 0; } }

        public XzaarToken TrailingToken
        {
            get { return trailingToken; }
            set { trailingToken = value; }
        }

        public XzaarToken LeadingToken
        {
            get { return leadingToken; }
            set { leadingToken = value; }
        }

        public void AddChild(XzaarSyntaxNode child)
        {
            if (!this.Children.Contains(child))
                this.Children.Add(child);
        }

        public void RemoveChild(XzaarSyntaxNode child)
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