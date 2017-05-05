using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public class ExpressionAnalyzer : IExpressionAnalyzer
    {
        public AnalyzedTree AnalyzeExpression(EntryNode entry)
        {
            IList<string> errors;
            return AnalyzeExpression(entry, out errors);
        }

        public AnalyzedTree AnalyzeExpression(EntryNode entry, out IList<string> errors)
        {
            // 1. do analyzing steps on the entry node..
            // 2. Bind any variable references
            return VariableBinder.Bind(entry, out errors);
        }
    }
}