using System.Text;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    public class XzaarCodeWriter
    {
        private StringBuilder sb = new StringBuilder();

        public void Write(object text, int currentIndent = 0)
        {
            Indent(currentIndent);
            sb.Append(text);
        }

        public void Write(string text, int currentIndent = 0)
        {
            Indent(currentIndent);
            sb.Append(text);
        }

        public void NewLine()
        {
            sb.AppendLine();
        }

        private void Indent(int num)
        {
            for (var i = 0; i < num; i++)
            {
                sb.Append("  ");
            }
        }

        public override string ToString()
        {
            return GetSourceCode();
        }

        public string GetSourceCode()
        {
            return sb.ToString();
        }
    }
}