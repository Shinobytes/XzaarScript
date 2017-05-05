using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    internal sealed class VariableBinder : NodeVisitor
    {
        private readonly AnalyzedTree _tree = new AnalyzedTree();
        private readonly Stack<Scope> _scopes = new Stack<Scope>();
        private readonly Stack<BoundConstants> _constants = new Stack<BoundConstants>();

        private VariableBinder() { }

        public static AnalyzedTree Bind(EntryNode entry, out IList<string> errors)
        {
            var binder = new VariableBinder();
            var program = binder.Visit(entry);
            errors = binder.Errors;
            binder._tree.SetProgram(program);
            return binder._tree;
        }
    }
}