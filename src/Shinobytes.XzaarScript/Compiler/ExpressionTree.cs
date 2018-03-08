using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public class ExpressionTree
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