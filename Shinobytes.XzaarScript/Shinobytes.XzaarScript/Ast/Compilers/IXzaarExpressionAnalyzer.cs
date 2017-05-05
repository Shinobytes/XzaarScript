using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast.Compilers
{
    public interface IXzaarExpressionAnalyzer
    {
        XzaarAnalyzedTree AnalyzeExpression(EntryNode entry, out IList<string> errors);
    }
}