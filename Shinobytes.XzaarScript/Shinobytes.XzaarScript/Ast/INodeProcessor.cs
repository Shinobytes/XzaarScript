using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public interface INodeProcessor
    {
        XzaarAstNode Process(XzaarAstNode syntaxTree);
    }
}