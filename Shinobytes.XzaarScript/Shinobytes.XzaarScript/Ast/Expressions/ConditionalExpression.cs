using System;

namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class ConditionalExpression : XzaarExpression
    {
        private XzaarExpression test;
        private XzaarExpression ifTrue;

        internal ConditionalExpression(XzaarExpression test, XzaarExpression ifTrue)
        {
            this.test = test;
            this.ifTrue = ifTrue;
        }

        internal static ConditionalExpression Make(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse, XzaarType type)
        {
            if (ifTrue.Type != type || ifFalse.Type != type)
            {
                return new FullConditionalExpressionWithType(test, ifTrue, ifFalse, type);
            }
            if (ifFalse is DefaultExpression && ifFalse.Type == XzaarBaseTypes.Void)
            {
                return new ConditionalExpression(test, ifTrue);
            }
            else
            {
                return new FullConditionalExpression(test, ifTrue, ifFalse);
            }
        }

        public sealed override ExpressionType NodeType => ExpressionType.Conditional;

        public override XzaarType Type => IfTrue.Type;

        public XzaarExpression IfTrue => this.ifTrue;

        public XzaarExpression Test => test;

        public XzaarExpression IfFalse => GetFalse();

        internal virtual XzaarExpression GetFalse()
        {
            return XzaarExpression.Empty();
        }

        public ConditionalExpression Update(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
        {
            if (test == Test && ifTrue == IfTrue && ifFalse == IfFalse)
            {
                return this;
            }
            return XzaarExpression.Condition(test, ifTrue, ifFalse, Type);
        }
    }

    internal class FullConditionalExpression : ConditionalExpression
    {
        private readonly XzaarExpression _false;

        internal FullConditionalExpression(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
            : base(test, ifTrue)
        {
            _false = ifFalse;
        }

        internal override XzaarExpression GetFalse()
        {
            return _false;
        }
    }
    internal class FullConditionalExpressionWithType : FullConditionalExpression
    {
        private readonly XzaarType _type;

        internal FullConditionalExpressionWithType(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse, XzaarType type)
            : base(test, ifTrue, ifFalse)
        {
            _type = type;
        }

        public sealed override XzaarType Type => _type;
    }
    public partial class XzaarExpression
    {
        public static ConditionalExpression Condition(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
        {
            RequiresCanRead(test, "test");
            RequiresCanRead(ifTrue, "ifTrue");
            RequiresCanRead(ifFalse, "ifFalse");

            if (test.Type != XzaarBaseTypes.Boolean && test.Type != XzaarBaseTypes.Any)
            {
                throw new InvalidOperationException("Argument must be boolean");
            }


            // only true for the '??' operator, but we don't have that in XzaarScript (yet)

            //if (!XzaarTypeUtils.AreEquivalent(ifTrue.Type, ifFalse.Type))
            //{
            //    // throw Error.ArgumentTypesMustMatch();
            //    throw new InvalidOperationException("Argument types must match");
            //}

            return ConditionalExpression.Make(test, ifTrue, ifFalse, ifTrue.Type);
        }
        public static ConditionalExpression Condition(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse, XzaarType type)
        {
            RequiresCanRead(test, "test");
            RequiresCanRead(ifTrue, "ifTrue");
            RequiresCanRead(ifFalse, "ifFalse");

            if (test.Type != XzaarBaseTypes.Boolean)
            {
                // throw Error.ArgumentMustBeBoolean();
                throw new InvalidOperationException("Argument must be boolean");
            }

            if (type != XzaarBaseTypes.Void)
            {
                if (!XzaarTypeUtils.AreReferenceAssignable(type, ifTrue.Type) ||
                    !XzaarTypeUtils.AreReferenceAssignable(type, ifFalse.Type))
                {
                    throw new InvalidOperationException("Argument types must match");
                }
            }

            return ConditionalExpression.Make(test, ifTrue, ifFalse, type);
        }

        public static ConditionalExpression IfThen(XzaarExpression test, XzaarExpression ifTrue)
        {
            return Condition(test, ifTrue, XzaarExpression.Empty(), XzaarBaseTypes.Void);
        }

        public static ConditionalExpression IfThenElse(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
        {
            return Condition(test, ifTrue, ifFalse, XzaarBaseTypes.Void);
        }
    }
}