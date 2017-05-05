using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public sealed class GotoExpression : XzaarExpression
    {
        private readonly XzaarGotoExpressionKind kind;
        private readonly XzaarExpression value;
        private readonly XzaarLabelTarget target;
        private readonly XzaarType type;

        internal GotoExpression(XzaarGotoExpressionKind kind, XzaarLabelTarget target, XzaarExpression value, XzaarType type)
        {
            this.kind = kind;
            this.value = value;
            this.target = target;
            this.type = type;
        }

        public XzaarExpression Value
        {
            get { return value; }
        }

        public XzaarLabelTarget Target
        {
            get { return target; }
        }

        public XzaarGotoExpressionKind Kind
        {
            get { return kind; }
        }

        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Goto; }
        }

        public override XzaarType Type
        {
            get { return type; }
        }
    }

    public partial class XzaarExpression
    {
        public static GotoExpression Break()
        {
            return Break(null);
        }

        public static GotoExpression Break(XzaarLabelTarget target)
        {
            return MakeGoto(XzaarGotoExpressionKind.Break, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Break(XzaarLabelTarget target, XzaarExpression value)
        {
            return MakeGoto(XzaarGotoExpressionKind.Break, target, value, XzaarBaseTypes.Void);
        }

        public static GotoExpression Break(XzaarLabelTarget target, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Break, target, null, type);
        }
        public static GotoExpression Break(XzaarLabelTarget target, XzaarExpression value, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Break, target, value, type);
        }

        public static GotoExpression Continue()
        {
            return MakeGoto(XzaarGotoExpressionKind.Continue, null, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Continue(XzaarLabelTarget target)
        {
            return MakeGoto(XzaarGotoExpressionKind.Continue, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Continue(XzaarLabelTarget target, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Continue, target, null, type);
        }

        public static GotoExpression Return(XzaarLabelTarget target)
        {
            return MakeGoto(XzaarGotoExpressionKind.Return, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Return(XzaarLabelTarget target, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Return, target, null, type);
        }

        public static GotoExpression Return(XzaarLabelTarget target, XzaarExpression value)
        {
            return MakeGoto(XzaarGotoExpressionKind.Return, target, value, XzaarBaseTypes.Void);
        }

        public static GotoExpression Return(XzaarLabelTarget target, XzaarExpression value, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Return, target, value, type);
        }

        public static GotoExpression Goto(XzaarLabelTarget target)
        {
            return MakeGoto(XzaarGotoExpressionKind.Goto, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Goto(XzaarLabelTarget target, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Goto, target, null, type);
        }

        public static GotoExpression Goto(XzaarLabelTarget target, XzaarExpression value)
        {
            return MakeGoto(XzaarGotoExpressionKind.Goto, target, value, XzaarBaseTypes.Void);
        }

        public static GotoExpression Goto(XzaarLabelTarget target, XzaarExpression value, XzaarType type)
        {
            return MakeGoto(XzaarGotoExpressionKind.Goto, target, value, type);
        }

        public static GotoExpression MakeGoto(XzaarGotoExpressionKind kind, XzaarLabelTarget target, XzaarExpression value, XzaarType type)
        {
            ValidateGoto(target, ref value, "target", "value");
            return new GotoExpression(kind, target, value, type);
        }

        private static void ValidateGoto(XzaarLabelTarget target, ref XzaarExpression value, string targetParameter, string valueParameter)
        {
            if (value == null)
            {
                if (target != null && target.Type != XzaarBaseTypes.Void) throw new InvalidOperationException("Label must be void or have expression");
            }
            else if (target != null)
            {
                ValidateGotoType(target.Type, ref value, valueParameter);
            }
        }

        private static void ValidateGotoType(XzaarType expectedType, ref XzaarExpression value, string paramName)
        {
            //RequiresCanRead(value, paramName);
            //if (expectedType != typeof(void))
            //{
            //    if (!TypeUtils.AreReferenceAssignable(expectedType, value.Type))
            //    {
            //        // C# autoquotes return values, so we'll do that here
            //        if (!TryQuote(expectedType, ref value))
            //        {
            //            throw Error.ExpressionTypeDoesNotMatchLabel(value.Type, expectedType);
            //        }
            //    }
            //}
        }

    }
}