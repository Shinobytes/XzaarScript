using System.IO;

namespace Shinobytes.XzaarScript.Assembly.IO
{
    internal class AssemblyReaderContext
    {
        public readonly byte[] AssemblyBytes;
        public readonly BinaryReader Reader;
        public readonly XzaarAssembly Assembly;
        public AssemblyReaderContext(byte[] assemblyBytes, XzaarAssembly assembly)
        {
            this.AssemblyBytes = assemblyBytes;
            this.Assembly = assembly;
            this.Reader = new BinaryReader(new MemoryStream(this.AssemblyBytes));
        }
    }
}