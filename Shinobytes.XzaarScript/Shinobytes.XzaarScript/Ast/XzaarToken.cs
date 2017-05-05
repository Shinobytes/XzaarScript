namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarToken
    {
        

        public XzaarToken(XzaarSyntaxKind type, string value)
        {
            Type = type;
            Value = value;
        }

        public XzaarSyntaxKind Type { get; }

        public string Value { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public bool IsCharString { get; set; }

        public override string ToString()
        {
            return Type + " {" + Value + "}";
        }
    }
}