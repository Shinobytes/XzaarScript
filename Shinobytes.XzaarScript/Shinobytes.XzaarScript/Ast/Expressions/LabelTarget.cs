namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public sealed class LabelTarget
    {
        private readonly XzaarType type;
        private readonly string name;

        internal LabelTarget(XzaarType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Name => name;

        public XzaarType Type => type;

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "UnamedLabel" : Name;
        }
    }

    public partial class XzaarExpression
    {
        public static LabelTarget Label()
        {
            return Label(XzaarBaseTypes.Void, null);
        }

        public static LabelTarget Label(string name)
        {
            return Label(XzaarBaseTypes.Void, name);
        }

        public static LabelTarget Label(XzaarType type)
        {
            return Label(type, null);
        }

        public static LabelTarget Label(XzaarType type, string name)
        {
            return new LabelTarget(type, name);
        }
    }
}