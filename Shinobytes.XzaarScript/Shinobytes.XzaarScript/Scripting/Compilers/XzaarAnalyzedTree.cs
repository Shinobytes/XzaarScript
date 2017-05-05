using System.Collections.Generic;
using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript.Scripting.Compilers
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