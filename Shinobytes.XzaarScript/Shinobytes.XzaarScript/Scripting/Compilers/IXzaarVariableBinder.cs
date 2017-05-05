using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting.Compilers
{
    public interface IXzaarVariableBinder
    {
        XzaarAnalyzedTree Bind(EntryNode entry);
    }
}