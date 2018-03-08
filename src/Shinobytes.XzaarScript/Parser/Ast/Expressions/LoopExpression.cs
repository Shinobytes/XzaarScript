/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

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