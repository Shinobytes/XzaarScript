using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting.Compilers
{
    public interface IXzaarExpressionAnalyzer
    {
        XzaarAnalyzedTree AnalyzeExpression(EntryNode entry);
    }
}