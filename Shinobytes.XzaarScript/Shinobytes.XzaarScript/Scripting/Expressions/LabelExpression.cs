namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public sealed class LabelExpression : XzaarExpression
    {
        private readonly XzaarLabelTarget target;
        private readonly XzaarExpression defaultValue;        

        internal LabelExpression(XzaarLabelTarget label, XzaarExpression defaultValue)
        {
            this.target = label;
            this.defaultValue = defaultValue;
        }

        public sealed override XzaarType Type
        {
            get { return this.target.Type; }
        }

        public XzaarLabelTarget Target
        {
            get { return target; }
        }

        public XzaarExpression DefaultValue
        {
            get { return defaultValue; }
        }

        public LabelExpression Update(XzaarLabelTarget target, XzaarExpression defaultValue)
        {
            if (target == Target && defaultValue == DefaultValue)
            {
                return this;
            }
            return XzaarExpression.Label(target, defaultValue);
        }


        public override XzaarExpressionType NodeType
        {
            get { return XzaarExpressionType.Label; }
        }
    }

    public partial class XzaarExpression
    {
        public static LabelExpression Label(XzaarLabelTarget target)
        {
            return Label(target, null);
        }

        public static LabelExpression Label(XzaarLabelTarget target, XzaarExpression defaultValue)
        {
            return new LabelExpression(target, defaultValue);
        }
    }
}