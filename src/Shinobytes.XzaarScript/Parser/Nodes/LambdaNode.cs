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
 
using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class LambdaNode : AstNode
    {
        public LambdaNode(FunctionParametersNode parameters, AstNode body, bool isSimple, int nodeIndex)
            : base(SyntaxKind.LambdaFunctionDefinitionExpression, null, null, nodeIndex)
        {
            Parameters = parameters;
            Body = body;
            this.HasCurlyBrackets = body is BlockNode;
            this.IsSimple = isSimple;
        }

        public bool HasCurlyBrackets { get; }

        public FunctionParametersNode Parameters { get; }

        public AstNode Body { get; private set; }

        public bool IsSimple { get; }

        public override void Accept(INodeVisitor nodeVisitor)
        {
            nodeVisitor.Visit(this);
        }

        public override bool IsEmpty()
        {
            return this.Body == null || this.Body.IsEmpty();
        }


        public override string ToString()
        {
            var parameters = this.Parameters.Parameters.Select(x => x.ToString());
            var str0 = $"({string.Join(", ", parameters.ToArray())}) => ";
            return HasCurlyBrackets ? str0 + "{" + Body + "}" : str0 + Body;
        }
    }
}