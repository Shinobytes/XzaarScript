using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Transpilers.ES6
{
    // TODO(Zerratar): We currently don't support "arrow-functions" / "lambdas" nor passing functions as arguments to other functions
    public class Transpiler : IScriptTranspiler
    {
        private readonly QuoutePreference quoutePreference;

        public Transpiler(QuoutePreference quoutePreference = QuoutePreference.DoubleQuoute)
        {
            this.quoutePreference = quoutePreference;
        }

        public TranspilerResult Transpile(XzaarExpression expression)
        {
            var visitor = new ES6ExpressionVisitor(quoutePreference);
            var codeResult = visitor.Visit(expression).Trim('\r', '\n');
            return new TranspilerResult(codeResult);
        }
    }
}
