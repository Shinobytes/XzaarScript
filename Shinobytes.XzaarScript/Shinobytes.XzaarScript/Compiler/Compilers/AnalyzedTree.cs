using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public class AnalyzedTree
    {
        private XzaarExpression program;

        internal void SetProgram(XzaarExpression program)
        {
            this.program = program;
        }

        public XzaarExpression GetExpression()
        {
            return program;
        }
    }
}