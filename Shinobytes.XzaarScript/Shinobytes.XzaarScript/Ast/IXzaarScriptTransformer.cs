using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public interface IXzaarScriptTransformer
    {
        EntryNode Transform(EntryNode ast);
    }
}