/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class SyntaxNode
    {
        private readonly Guid instanceId;

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
            Parent?.AddChild(this);
        }

        public SyntaxKind Kind { get; set; }

        public int Index { get; set; }

        public object Value { get; set; }

        public string StringValue => Value + "";

        public SyntaxNode Parent { get; set; }

        public List<SyntaxNode> Children { get; set; }

        public bool HasChildren => Children != null && Children.Count > 0;

        public SyntaxToken TrailingToken { get; set; }

        public SyntaxToken LeadingToken { get; set; }

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