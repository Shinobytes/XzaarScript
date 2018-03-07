namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public class ConstantExpression : XzaarExpression
    {
        private readonly object value;

        internal ConstantExpression(object value)
        {
            this.value = value;
        }
        public XzaarExpression ArrayIndex { get; set; }

        internal static ConstantExpression Make(object value, XzaarType type)
        {
            if ((value == null && type == XzaarBaseTypes.Any) ||
                (value != null && XzaarBaseTypes.Typeof(value.GetType()) == type))
            {
                return new ConstantExpression(value);
            }
            else
            {
                return new TypedConstantExpression(value, type);
            }
        }

        public override XzaarType Type
        {
            get
            {
                if (value == null)
                {
                    return XzaarBaseTypes.Any;
                }
                return XzaarBaseTypes.Typeof(value.GetType());
            }
        }

        public sealed override ExpressionType NodeType => ExpressionType.Constant;

        public object Value => value;
    }

    internal class TypedConstantExpression : ConstantExpression
    {
        private readonly XzaarType _type;

        internal TypedConstantExpression(object value, XzaarType type)
            : base(value)
        {
            _type = type;
        }

        public sealed override XzaarType Type => _type;
    }

    public partial class XzaarExpression
    {
        public static ConstantExpression Constant(object value)
        {
            return ConstantExpression.Make(value, value == null ? (XzaarType)typeof(object) : (XzaarType)value.GetType());
        }

        public static ConstantExpression Constant(object value, XzaarType type)
        {                                  
            return ConstantExpression.Make(value, type);
        }
    }
}