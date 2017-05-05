using System;
using System.IO;

namespace Shinobytes.XzaarScript.Assembly.IO
{
    public class AssemblyReader
    {
        private AssemblyReader()
        {
        }

        public static XzaarAssembly ReadBytes(byte[] binary)
        {
            var reader = new AssemblyReader();

            return reader.Read(binary);
        }

        public static XzaarAssembly ReadFile(string file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            return ReadBytes(File.ReadAllBytes(file));
        }

        private XzaarAssembly Read(byte[] binary)
        {
            var assembly = new XzaarAssembly();
            var ctx = new AssemblyReaderContext(binary, assembly);
            return this.Read(ctx);
        }

        private XzaarAssembly Read(AssemblyReaderContext ctx)
        {
            return ctx.Assembly;
        }
    }
}
