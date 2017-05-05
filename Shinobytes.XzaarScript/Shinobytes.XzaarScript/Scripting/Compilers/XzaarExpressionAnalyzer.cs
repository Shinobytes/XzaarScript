using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting.Compilers
{
    public class XzaarExpressionAnalyzer : IXzaarExpressionAnalyzer
    {
        public XzaarAnalyzedTree AnalyzeExpression(EntryNode entry)
        {
            // 1. do analyzing steps on the entry node..
            // 2. Bind any variable references
            return XzaarVariableBinder.Bind(entry);
        }
    }
}