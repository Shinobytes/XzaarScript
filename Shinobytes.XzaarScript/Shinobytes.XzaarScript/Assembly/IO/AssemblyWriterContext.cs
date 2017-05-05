namespace Shinobytes.XzaarScript.Assembly.IO
{
    internal class AssemblyWriterContext
    {
        public readonly XzaarAssembly Assembly;
        public readonly string Filename;

        public AssemblyWriterContext(XzaarAssembly assembly, string filename)
        {
            this.Assembly = assembly;
            this.Filename = filename;
        }
    }
}