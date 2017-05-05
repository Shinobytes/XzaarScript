using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Ast.Compilers
{
    public class XzaarAnalyzedTree
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