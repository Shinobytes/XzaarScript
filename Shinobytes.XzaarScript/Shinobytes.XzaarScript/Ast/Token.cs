namespace Shinobytes.XzaarScript.Ast
{
    public class Token
    {        
        public Token(SyntaxKind type, string value)
        {
            Type = type;
            Value = value;
        }

        public SyntaxKind Type { get; }

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