using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public interface INodeAnalyzer
    {
        AnalyzedTree Analyze(EntryNode entry, out IList<string> errors);
    }
}