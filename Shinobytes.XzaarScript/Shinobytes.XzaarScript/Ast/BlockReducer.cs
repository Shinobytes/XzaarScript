using System.Linq;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class BlockReducer : NodeProcessorVisitor
    {
        public static EntryNode ReduceBlocks(EntryNode entryNode)
        {
            return new BlockReducer().Visit(entryNode);
        }

        public override BodyNode Visit(BodyNode block)
        {
            if (block.Children.Count == 1 && block.Children[0].NodeType == NodeTypes.BLOCK)
            {
                
                return AstNode.Body(block.Children[0].Children.Select(Visit).ToArray());
            }
            return block;
        }

        public override BlockNode Visit(BlockNode block)
        {
            if (block.Children.Count == 1 && block.Children[0].NodeType == NodeTypes.BLOCK)
            {
                return AstNode.Block(block.Children[0].Children.Select(Visit).ToArray());
            }
            return block;
        }

        public override EntryNode Visit(EntryNode entry)
        {
            if (entry.Body.NodeType == NodeTypes.BLOCK)
            {
                if (entry.Body.Children.Count == 1)
                {
                    return new EntryNode(Visit(entry.Body.Children[0]));
                }
            }
            return entry;
        }
    }
}