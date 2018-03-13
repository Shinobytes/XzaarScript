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
    public class IfElseNode : AstNode
    {
        private readonly AstNode condition;
        private AstNode ifTrue;
        private AstNode ifFalse;

        public IfElseNode(AstNode condition, AstNode ifTrue, AstNode ifFalse, int nodeIndex)
            : base(SyntaxKind.Block, "CONDITIONAL", null, nodeIndex)
        {
            if (condition != null) condition.Parent = this;
            if (ifFalse != null) ifFalse.Parent = this;
            if (ifTrue != null) ifTrue.Parent = this;


            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse ?? Empty();

        }

        public AstNode GetCondition() => condition;

        public AstNode GetTrue() => ifTrue;

        public AstNode GetFalse() => ifFalse;

        public override bool IsEmpty()
        {
            return GetCondition() == null && GetTrue() == null && GetFalse() == null;
        }

        public override string ToString()
        {
            var conditionString = condition.ToString();
            if (condition.Kind != SyntaxKind.Expression)
                conditionString = "(" + conditionString + ")";
            return $"if {conditionString} {{{ifTrue}}} else {{{ifFalse}}}";
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public void SetIfTrue(AstNode node)
        {
            ifTrue = node;
        }

        public void SetIfFalse(AstNode node)
        {
            ifFalse = node;
        }
    }
}