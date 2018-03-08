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
    public sealed class GotoExpression : XzaarExpression
    {
        private readonly GotoExpressionKind kind;
        private readonly XzaarExpression value;
        private readonly LabelTarget target;
        private readonly XzaarType type;

        internal GotoExpression(GotoExpressionKind kind, LabelTarget target, XzaarExpression value, XzaarType type)
        {
            this.kind = kind;
            this.value = value;
            this.target = target;
            this.type = type;
        }

        public XzaarExpression Value => value;

        public LabelTarget Target => target;

        public GotoExpressionKind Kind => kind;

        public override ExpressionType NodeType => ExpressionType.Goto;

        public override XzaarType Type => type;
    }

    public partial class XzaarExpression
    {
        public static GotoExpression Break()
        {
            return Break(null);
        }

        public static GotoExpression Break(LabelTarget target)
        {
            return MakeGoto(GotoExpressionKind.Break, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Break(LabelTarget target, XzaarExpression value)
        {
            return MakeGoto(GotoExpressionKind.Break, target, value, XzaarBaseTypes.Void);
        }

        public static GotoExpression Break(LabelTarget target, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Break, target, null, type);
        }
        public static GotoExpression Break(LabelTarget target, XzaarExpression value, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Break, target, value, type);
        }

        public static GotoExpression Continue()
        {
            return MakeGoto(GotoExpressionKind.Continue, null, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Continue(LabelTarget target)
        {
            return MakeGoto(GotoExpressionKind.Continue, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Continue(LabelTarget target, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Continue, target, null, type);
        }

        public static GotoExpression Return(LabelTarget target)
        {
            return MakeGoto(GotoExpressionKind.Return, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Return(LabelTarget target, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Return, target, null, type);
        }

        public static GotoExpression Return(LabelTarget target, XzaarExpression value)
        {
            return MakeGoto(GotoExpressionKind.Return, target, value, XzaarBaseTypes.Void);
        }

        public static GotoExpression Return(LabelTarget target, XzaarExpression value, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Return, target, value, type);
        }

        public static GotoExpression Goto(LabelTarget target)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, null, XzaarBaseTypes.Void);
        }

        public static GotoExpression Goto(LabelTarget target, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, null, type);
        }

        public static GotoExpression Goto(LabelTarget target, XzaarExpression value)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, value, XzaarBaseTypes.Void);
        }

        public static GotoExpression Goto(LabelTarget target, XzaarExpression value, XzaarType type)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, value, type);
        }

        public static GotoExpression MakeGoto(GotoExpressionKind kind, LabelTarget target, XzaarExpression value, XzaarType type)
        {
            ValidateGoto(target, ref value, "target", "value");
            return new GotoExpression(kind, target, value, type);
        }

        private static void ValidateGoto(LabelTarget target, ref XzaarExpression value, string targetParameter, string valueParameter)
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