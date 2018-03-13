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

namespace Shinobytes.XzaarScript.Parser.Ast
{
    public class TokenStream : CollectionStream<SyntaxToken>
    {
        public TokenStream(IList<SyntaxToken> items)
            : base(items)
        {
        }

        public bool NextIs(SyntaxKind kind)
        {
            var peekNext = PeekNext();
            return peekNext != null && peekNext.Kind == kind;
        }

        public bool CurrentIs(SyntaxKind kind)
        {
            return Current != null && Current.Kind == kind;
        }

        public SyntaxToken Consume(SyntaxKind kind)
        {
            return this.Consume(x => x.Kind == kind);
        }

        public SyntaxToken ConsumeExpected(SyntaxKind kind)
        {
            return this.ConsumeExpected(x => x.Kind == kind);
        }
    }
}