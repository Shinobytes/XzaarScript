using System.Collections.Generic;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting.Compilers
{
    internal sealed class XzaarVariableBinder : XzaarNodeVisitor
    {
        private readonly XzaarAnalyzedTree _tree = new XzaarAnalyzedTree();
        private readonly Stack<XzaarScope> _scopes = new Stack<XzaarScope>();
        private readonly Stack<XzaarBoundConstants> _constants = new Stack<XzaarBoundConstants>();

        private XzaarVariableBinder() { }

        public static XzaarAnalyzedTree Bind(EntryNode entry)
        {
            var binder = new XzaarVariableBinder();            
            var program = binder.Visit(entry);
            binder._tree.SetProgram(program);
            return binder._tree;
        }
    }
}