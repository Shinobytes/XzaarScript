using System.Linq;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarNodeReducer : INodeProcessor
    {
        // go through each node recursively and remove any expression nodes with 1 child 
        public XzaarAstNode Process(XzaarAstNode syntaxTree)
        {
            if ((syntaxTree is ExpressionNode || syntaxTree is BlockNode) && syntaxTree.Children.Count == 1)
            {
                syntaxTree = Process(syntaxTree.Children[0]);
            }
            else if (syntaxTree.Children.Count > 0)
            {
                var children = syntaxTree.Children.Select(Process).ToList();
                if (children.Count == syntaxTree.Children.Count)
                    syntaxTree.SetChildren(children);
            }
            return syntaxTree;
        }
    }
}