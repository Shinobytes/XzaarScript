using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast.Compilers
{
    internal sealed class XzaarVariableBinder : XzaarNodeVisitor
    {
        private readonly XzaarAnalyzedTree _tree = new XzaarAnalyzedTree();
        private readonly Stack<XzaarScope> _scopes = new Stack<XzaarScope>();
        private readonly Stack<XzaarBoundConstants> _constants = new Stack<XzaarBoundConstants>();

        private XzaarVariableBinder() { }

        public static XzaarAnalyzedTree Bind(EntryNode entry, out IList<string> errors)
        {
            var binder = new XzaarVariableBinder();
            var program = binder.Visit(entry);
            errors = binder.Errors;
            binder._tree.SetProgram(program);
            return binder._tree;
        }
    }
}