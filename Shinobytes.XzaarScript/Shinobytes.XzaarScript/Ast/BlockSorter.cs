using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class BlockSorter : NodeProcessorVisitor
    {
        public static EntryNode SortBlocks(EntryNode entryNode)
        {
            return new BlockSorter().Visit(entryNode);
        }

        public override BodyNode Visit(BodyNode block)
        {
            block.SortChildren();

            foreach (var child in block.Children) Visit(child);

            return block;
        }

        public override BlockNode Visit(BlockNode block)
        {
            block.SortChildren();
            foreach (var child in block.Children) Visit(child);
            return block;
        }

        public override EntryNode Visit(EntryNode entry)
        {
            entry.SortChildren();

            Visit(entry.Body);    

            return entry;
        }
    }
}