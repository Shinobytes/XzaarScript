using System.Linq.Expressions;
using Expression = Shinobytes.XzaarScript.Scripting.Expressions.XzaarExpression;
namespace Shinobytes.XzaarScript.Scripting.Expressions
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