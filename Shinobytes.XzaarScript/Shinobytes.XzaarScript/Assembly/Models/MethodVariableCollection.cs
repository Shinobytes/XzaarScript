using System.Linq;

namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class MethodVariableCollection : VariableCollection
    {
        public bool ContainsVariable(string vrName)
        {
            return base.InternalItems.Any(x => x.Name == vrName);
        }
    }
}
