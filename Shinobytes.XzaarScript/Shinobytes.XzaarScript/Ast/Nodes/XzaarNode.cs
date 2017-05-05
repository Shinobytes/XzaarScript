using System;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript.Ast.Nodes
{
    public class XzaarNode
    {
        private readonly Guid instanceId;

        public XzaarNode(int index, XzaarSyntaxKind type, XzaarSyntaxKind kind)
            : this(null, index, null, type, kind)
        {
        }

        public XzaarNode(int index, object value, XzaarSyntaxKind type, XzaarSyntaxKind kind)
            : this(null, index, value, type, kind)
        {
        }

        public XzaarNode(XzaarNode parent, int index, object value, XzaarSyntaxKind type, XzaarSyntaxKind kind)
        {
            this.instanceId = Guid.NewGuid();
            this.Parent = parent;
            this.Index = index;
            this.Value = value;
            Type = type;
            Kind = kind;
            this.Children = new List<XzaarNode>();
            if (this.Parent != null)
            {
                this.Parent.AddChild(this);
            }
        }

        public XzaarSyntaxKind Type { get; set; }
        public XzaarSyntaxKind Kind { get; set; }

        public int Index { get; set; }
        public object Value { get; set; }
        public string StringValue { get { return Value + ""; } }
        public XzaarNode Parent { get; set; }
        public List<XzaarNode> Children { get; set; }

        public bool HasChildren { get { return Children != null && Children.Count > 0; } }

        public void AddChild(XzaarNode child)
        {
            if (!this.Children.Contains(child))
                this.Children.Add(child);
        }

        public void RemoveChild(XzaarNode child)
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