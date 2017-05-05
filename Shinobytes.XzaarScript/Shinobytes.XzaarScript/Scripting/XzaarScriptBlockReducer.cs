using System;
using System.Linq;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarScriptBlockReducer : NodeProcessorVisitor
    {
        public static EntryNode ReduceBlocks(EntryNode entryNode)
        {
            return new XzaarScriptBlockReducer().Visit(entryNode);
        }

        public override BodyNode Visit(BodyNode block)
        {
            if (block.Children.Count == 1 && block.Children[0].NodeType == XzaarNodeTypes.BLOCK)
            {
                return XzaarNode.Body(block.Children[0].Children.Select(Visit).ToArray());
            }
            return block;
        }

        public override BlockNode Visit(BlockNode block)
        {
            if (block.Children.Count == 1 && block.Children[0].NodeType == XzaarNodeTypes.BLOCK)
            {
                return XzaarNode.Block(block.Children[0].Children.Select(Visit).ToArray());
            }
            return block;
        }

        public override EntryNode Visit(EntryNode entry)
        {
            if (entry.Body.NodeType == XzaarNodeTypes.BLOCK)
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