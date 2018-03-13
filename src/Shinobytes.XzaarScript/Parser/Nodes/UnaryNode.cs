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
 
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class UnaryNode : AstNode
    {
        private readonly bool isPostUnary;
        private readonly bool isIncrementor;
        private readonly AstNode item;

        public UnaryNode(bool isPostUnary, bool isIncrementor, AstNode item, int nodeIndex)
            : base(SyntaxKind.UnaryExpression, "UNARY", null, nodeIndex)
        {
            this.isPostUnary = isPostUnary;
            this.isIncrementor = isIncrementor;
            this.item = item;
        }

        public UnaryNode(bool isPostfix, string op, AstNode item, int nodeIndex)
            : this(isPostfix, false, item, nodeIndex)
        {
            this.Operator = op;
        }

        public AstNode Item => item;

        public bool IsPostUnary => isPostUnary;

        public bool IsIncrementor => isIncrementor;

        public string Operator { get; set; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return Item == null;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Operator))
                return "Unary: " + item;

            if (IsPostUnary)
                return this.item + "" + this.Operator;

            return this.Operator + "" + this.item;
        }
    }
}