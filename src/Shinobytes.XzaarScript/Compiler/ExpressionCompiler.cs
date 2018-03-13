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
 
using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Ast;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler
{
    public class ExpressionCompiler : NodeToExpressionVisitor, IExpressionCompiler
    {
        //private readonly XzaarExpression _tree = new XzaarExpression();
        //private readonly Stack<Scope> _scopes = new Stack<Scope>();
        //private readonly Stack<BoundConstants> _constants = new Stack<BoundConstants>();

        public ExpressionCompiler(IScopeProvider scopeProvider = null) : base(scopeProvider ?? new DefaultScopeProvider())
        {
        }

        public XzaarExpression Compile(AstNode entry)
        {
            // 1. do analyzing steps on the entry node..
            // 2. Bind any variable references
            // 3. compile expressions
            return Bind(entry);
        }

        private XzaarExpression Bind(AstNode entry)
        {
            var binder = new NodeTypeBinder();
            var boundNode = binder.Process(entry);
            var program = Visit(boundNode);
            return program;
        }
    }
}