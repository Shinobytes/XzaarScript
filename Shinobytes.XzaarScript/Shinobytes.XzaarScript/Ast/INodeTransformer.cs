using System.Security;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public interface INodeTransformer
    {
        XzaarAstNode Transform();
    }
}