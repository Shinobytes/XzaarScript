using Expression = Shinobytes.XzaarScript.Ast.Expressions.XzaarExpression;
namespace Shinobytes.XzaarScript.Ast.Expressions
{
    public interface IXzaarArgumentProvider
    {
        Expression GetArgument(int index);
        int ArgumentCount
        {
            get;
        }
    }
}