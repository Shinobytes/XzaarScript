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
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class MemberAccessChainNode : AstNode
    {
        private readonly AstNode left;
        private readonly AstNode right;
        private string resultType;

        public MemberAccessChainNode(AstNode left, AstNode right, string resultType, int nodeIndex)
            : base(SyntaxKind.MemberAccess, "ACCESSOR_CHAIN", null, nodeIndex)
        {
            this.left = left;
            this.right = right;
            this.resultType = resultType;
        }

        public AstNode LastAccessor => left;

        public AstNode Accessor => right;

        public override string Type
        {
            get { return base.Type ?? ResultType; }
            set { base.Type = value; }
        }

        public string ResultType
        {
            get { return resultType; }
            set { resultType = value; }
        }

        public override string StringValue
        {
            get { return Accessor.StringValue; }
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return this.LastAccessor + "." + this.Accessor;
        }
    }
}