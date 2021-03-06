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
    public class BinaryOperatorNode : AstNode
    {
        private AstNode left;
        private AstNode right;

        public BinaryOperatorNode(int operatingOrderWeight, AstNode left, char op, AstNode right, int nodeIndex)
            : this(operatingOrderWeight, left, op.ToString(), right, nodeIndex)
        {
        }

        public BinaryOperatorNode(int operatingOrderWeight, AstNode left, string op, AstNode right, int nodeIndex)
            : base(SyntaxKind.BinaryExpression, null, null, nodeIndex)
        {
            this.left = left;
            this.right = right;

            Op = op;
            OperatingOrder = operatingOrderWeight;
        }

        public override string Type
        {
            get
            {
                if (left.Type == XzaarBaseTypes.Number.Name && right.Type == XzaarBaseTypes.Number.Name)
                {
                    return XzaarBaseTypes.Number.Name;
                }

                if (left.Type == XzaarBaseTypes.String.Name || right.Type == XzaarBaseTypes.String.Name && this.Op == "+")
                {
                    return XzaarBaseTypes.String.Name;
                }

                return XzaarBaseTypes.Any.Name;
            }
        }


        public AstNode Left => left;

        public string Op { get; }

        public AstNode Right => right;

        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }

        public override bool IsEmpty()
        {
            return Op == null && Left == null && Right == null;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public void SetLeft(AstNode expr)
        {
            this.left = expr;
        }

        public void SetRight(AstNode expr)
        {
            this.right = expr;
        }
    }
}