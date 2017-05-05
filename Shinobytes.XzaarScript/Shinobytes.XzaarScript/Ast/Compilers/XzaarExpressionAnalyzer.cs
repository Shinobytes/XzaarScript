using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast.Compilers
{
    public class XzaarExpressionAnalyzer : IXzaarExpressionAnalyzer
    {
        public XzaarAnalyzedTree AnalyzeExpression(EntryNode entry)
        {
            IList<string> errors;
            return AnalyzeExpression(entry, out errors);
        }

        public XzaarAnalyzedTree AnalyzeExpression(EntryNode entry, out IList<string> errors)
        {
            // 1. do analyzing steps on the entry node..
            // 2. Bind any variable references
            return XzaarVariableBinder.Bind(entry, out errors);
        }
    }
}