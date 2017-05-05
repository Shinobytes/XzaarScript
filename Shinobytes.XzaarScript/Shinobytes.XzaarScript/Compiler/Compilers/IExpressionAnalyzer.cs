using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public interface IExpressionAnalyzer
    {
        AnalyzedTree AnalyzeExpression(EntryNode entry, out IList<string> errors);
    }
}