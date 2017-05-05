namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class Label : XzaarBinaryCode
    {
        public Label()
        {            
        }

        public Label(string name)
        {
            Name = name;
        }

        public Label(int offset, string name)
        {
            Offset = offset;
            Name = name;
        }

        public Label(int offset) : this(offset, null) { }

        public string Name { get; }
    }
}