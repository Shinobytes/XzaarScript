using System;

namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public enum XzaarLoopTypes
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
        private readonly XzaarLabelTarget @break;
        private readonly XzaarLabelTarget @continue;
        private readonly XzaarLoopTypes loopType;

        internal LoopExpression(XzaarExpression body, XzaarLabelTarget @break, XzaarLabelTarget @continue, XzaarLoopTypes loopType)
        {
            this.body = body;
            this.@break = @break;
            this.@continue = @continue;
            this.loopType = loopType;
        }

        public XzaarLoopTypes LoopType
        {
            get { return loopType; }
        }

        public sealed override XzaarType Type
        {
            get { return @break == null ? XzaarBaseTypes.Void : @break.Type; }
        }

        public sealed override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Loop; }
        }

        public XzaarExpression Body
        {
            get { return body; }
        }

        public XzaarLabelTarget BreakLabel
        {
            get { return @break; }
        }

        public XzaarLabelTarget ContinueLabel
        {
            get { return @continue; }
        }

        public LoopExpression Update(XzaarLabelTarget breakLabel, XzaarLabelTarget continueLabel, XzaarExpression body)
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

        public static LoopExpression Loop(XzaarExpression body, XzaarLabelTarget @break)
        {
            return Loop(body, @break, null);
        }

        public static LoopExpression Loop(XzaarExpression body, XzaarLabelTarget @break, XzaarLabelTarget @continue)
        {
            if (@continue != null && @continue.Type != XzaarBaseTypes.Void) throw new InvalidOperationException("Label type must be void");
            return new LoopExpression(body, @break, @continue, XzaarLoopTypes.Loop);
        }
    }
}