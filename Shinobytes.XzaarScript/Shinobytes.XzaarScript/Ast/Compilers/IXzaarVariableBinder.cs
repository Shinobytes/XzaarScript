using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast.Compilers
{
    public interface IXzaarVariableBinder
    {
        XzaarAnalyzedTree Bind(EntryNode entry);
    }
}