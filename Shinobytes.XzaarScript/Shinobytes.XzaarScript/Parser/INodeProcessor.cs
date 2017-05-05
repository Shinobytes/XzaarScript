using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser
{
    public interface INodeProcessor
    {
        AstNode Process(AstNode syntaxTree);
    }
}