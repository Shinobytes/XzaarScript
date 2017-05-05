namespace Shinobytes.XzaarScript.Assembly.IO
{
    public class AssemblyWriter
    {
        public static void WriteToFile(XzaarAssembly xzaarAssembly, string filename)
        {
            var writer = new AssemblyWriter();
            writer.Write(new AssemblyWriterContext(xzaarAssembly, filename));
        }

        private void Write(AssemblyWriterContext ctx)
        {
            throw new System.NotImplementedException();
        }
    }
}
