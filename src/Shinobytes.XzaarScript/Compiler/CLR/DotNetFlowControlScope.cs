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
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler
{
    public class DotNetFlowControlScope
    {
        public Label CurrentStartLabel;
        public Label CurrentEndLabel;

        public int Depth;
        private readonly DotNetFlowControlScope parent;
        private readonly List<DotNetFlowControlScope> children;
        public DotNetFlowControlScope(DotNetFlowControlScope parent, int depth, Label start, Label end)
        {
            this.CurrentStartLabel = start;
            this.CurrentEndLabel = end;
            this.parent = parent;
            this.Depth = depth;
            this.children = new List<DotNetFlowControlScope>();
        }

        public DotNetFlowControlScope BeginControlBlock(Label start, Label end)
        {
            var subScope = new DotNetFlowControlScope(this, this.Depth + 1, start, end);
            this.children.Add(subScope);
            return subScope;
        }

        public DotNetFlowControlScope EndControlBlock()
        {
            return parent;
        }
    }
}