using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public interface IVariableBinder
    {
        AnalyzedTree Bind(EntryNode entry);
    }
}