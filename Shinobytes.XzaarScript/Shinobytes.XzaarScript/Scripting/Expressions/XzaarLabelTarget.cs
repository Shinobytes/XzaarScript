namespace Shinobytes.XzaarScript.Scripting.Expressions
{
    public sealed class XzaarLabelTarget
    {
        private readonly XzaarType type;
        private readonly string name;

        internal XzaarLabelTarget(XzaarType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public XzaarType Type { get { return type; } }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "UnamedLabel" : Name;
        }
    }

    public partial class XzaarExpression
    {
        public static XzaarLabelTarget Label()
        {
            return Label(XzaarBaseTypes.Void, null);
        }

        public static XzaarLabelTarget Label(string name)
        {
            return Label(XzaarBaseTypes.Void, name);
        }

        public static XzaarLabelTarget Label(XzaarType type)
        {
            return Label(type, null);
        }

        public static XzaarLabelTarget Label(XzaarType type, string name)
        {
            return new XzaarLabelTarget(type, name);
        }
    }
}