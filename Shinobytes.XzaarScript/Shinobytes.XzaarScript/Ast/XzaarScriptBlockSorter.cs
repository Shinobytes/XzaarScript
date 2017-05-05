using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarScriptBlockSorter : NodeProcessorVisitor
    {
        public static EntryNode SortBlocks(EntryNode entryNode)
        {
            return new XzaarScriptBlockSorter().Visit(entryNode);
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