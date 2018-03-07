using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public class NodeAnalyzer : INodeAnalyzer
    {
        public AnalyzedTree Analyze(EntryNode entry)
        {
            IList<string> errors;
            return Analyze(entry, out errors);
        }

        public AnalyzedTree Analyze(EntryNode entry, out IList<string> errors)
        {
            // 1. do analyzing steps on the entry node..
            // 2. Bind any variable references
            return VariableBinder.Bind(entry, out errors);
        }
    }
}