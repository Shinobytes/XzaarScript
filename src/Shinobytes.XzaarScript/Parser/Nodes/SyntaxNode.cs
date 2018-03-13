/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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