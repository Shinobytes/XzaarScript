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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public enum LoopTypes
    {
        Loop,
        For,
        Foreach,
        While,
        DoWhile
    }

    public class LoopExpression : XzaarExpression
    {
        private readonly XzaarExpression body;
        private readonly LabelTarget @break;
        private readonly LabelTarget @continue;
        private readonly LoopTypes loopType;

        internal LoopExpression(XzaarExpression body, LabelTarget @break, LabelTarget @continue, LoopTypes loopType)
        {
            this.body = body;
            this.@break = @break;
            this.@continue = @continue;
            this.loopType = loopType;
        }

        public LoopTypes LoopType => loopType;

        public sealed override XzaarType Type => @break == null ? XzaarBaseTypes.Void : @break.Type;

        public sealed override ExpressionType NodeType => ExpressionType.Loop;

        public XzaarExpression Body => body;

        public LabelTarget BreakLabel => @break;

        public LabelTarget ContinueLabel => @continue;

        public LoopExpression Update(LabelTarget breakLabel, LabelTarget continueLabel, XzaarExpression body)
        {
            if (breakLabel == BreakLabel && continueLabel == ContinueLabel && body == Body)
            {
                return this;
            }
            return XzaarExpression.Loop(body, breakLabel, continueLabel);
        }
    }

    public partial class XzaarExpression
    {
        public static LoopExpression Loop(XzaarExpression body)
        {
            return Loop(body, null);
        }

        public static LoopExpression Loop(XzaarExpression body, LabelTarget @break)
        {
            return Loop(body, @break, null);
        }

        public static LoopExpression Loop(XzaarExpression body, LabelTarget @break, LabelTarget @continue)
        {
            if (@continue != null && @continue.Type != XzaarBaseTypes.Void) throw new InvalidOperationException("Label type must be void");
            return new LoopExpression(body, @break, @continue, LoopTypes.Loop);
        }
    }
}