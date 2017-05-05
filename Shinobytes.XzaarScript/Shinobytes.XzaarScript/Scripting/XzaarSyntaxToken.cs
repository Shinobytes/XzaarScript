using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarSyntaxToken
    {
        public string Type;
        public XzaarNodeTypes Key;
        public string Value;
        public XzaarSyntaxToken() { }
        public XzaarSyntaxToken(XzaarNodeTypes key, string type, string value)
        {
            Key = key;
            Value = value;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Key} {Type} {Value}";
        }
    }
}