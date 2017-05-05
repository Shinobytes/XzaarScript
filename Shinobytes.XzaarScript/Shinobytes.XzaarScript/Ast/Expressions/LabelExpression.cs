namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public sealed class LabelExpression : XzaarExpression
    {
        private readonly LabelTarget target;
        private readonly XzaarExpression defaultValue;        

        internal LabelExpression(LabelTarget label, XzaarExpression defaultValue)
        {
            this.target = label;
            this.defaultValue = defaultValue;
        }

        public sealed override XzaarType Type => this.target.Type;

        public LabelTarget Target => target;

        public XzaarExpression DefaultValue => defaultValue;

        public LabelExpression Update(LabelTarget target, XzaarExpression defaultValue)
        {
            if (target == Target && defaultValue == DefaultValue)
            {
                return this;
            }
            return XzaarExpression.Label(target, defaultValue);
        }


        public override ExpressionType NodeType => ExpressionType.Label;
    }

    public partial class XzaarExpression
    {
        public static LabelExpression Label(LabelTarget target)
        {
            return Label(target, null);
        }

        public static LabelExpression Label(LabelTarget target, XzaarExpression defaultValue)
        {
            return new LabelExpression(target, defaultValue);
        }
    }
}