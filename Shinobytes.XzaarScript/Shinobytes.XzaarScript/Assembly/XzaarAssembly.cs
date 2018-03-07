using Shinobytes.XzaarScript.Assembly.IO;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.Assembly
{
    public class XzaarAssembly
    {
        private string name;
        private string filename;

        public static XzaarAssembly ReadFromFile(string filename)
        {
            return AssemblyReader.ReadFile(filename);
        }

        public void WriteToFile(string filename)
        {
            AssemblyWriter.WriteToFile(this, filename);
        }

        public static XzaarAssembly CreateAssembly(string name)
        {
            return new XzaarAssembly(name, null);
        }

        public static XzaarAssembly CreateAssembly(string name, string outputFilename)
        {
            return new XzaarAssembly(name, null);
        }

        internal XzaarAssembly(string name, string filename)
        {
            this.name = name;
            this.filename = filename;
            Types = new TypeCollection();
            GlobalVariables = new GlobalVariableCollection();
            GlobalInstructions = new GlobalInstructionCollection();
            GlobalMethods = new MethodCollection();
        }

        internal XzaarAssembly() : this(null, null)
        {
            Types = new TypeCollection();
            GlobalVariables = new GlobalVariableCollection();
            GlobalInstructions = new GlobalInstructionCollection();
            GlobalMethods = new MethodCollection();
        }

        internal void SetName(string name)
        {
            this.name = name;
        }

        internal void SetFilename(string filename)
        {
            this.filename = filename;
        }

        public string Name => name;

        public string Filename => filename;

        public TypeCollection Types { get; }
        public GlobalVariableCollection GlobalVariables { get; }
        public GlobalInstructionCollection GlobalInstructions { get; }
        public MethodCollection GlobalMethods { get; }
    }
}
