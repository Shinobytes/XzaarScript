using System.Linq;
using Shinobytes.XzaarScript.Extensions;
namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class InstructionVariableCollection : Collection<MemberReference>
    {
        public string ToString(string del)
        {
            var value = base.InternalItems.Select(GetStringRepresentation);
            return string.Join(del, value.ToArray());
        }

        private static string GetStringRepresentation(MemberReference i)
        {
            if (i == null) return null;
            var c = i as Constant;
            if (c != null)
            {
                if (c.ArrayIndex != null)
                {
                    var value = GetStringRepresentation(c.ArrayIndex as VariableReference);
                    if (value != null)
                    {
                        return (c.Value is string ? "\"" + c.Value + "\"" : c.Value) + "[" + value + "]";
                    }
                }

                return c.Value is string ? "\"" + c.Value + "\"" : (c.Value + "");
            }
            var vref = i as VariableReference;
            if (vref != null)
            {
                var value = GetStringRepresentation(vref.ArrayIndex as VariableReference);
                if (value != null)
                {
                    return i.Name + "[" + value + "]";
                }
            }
            return i.Name;
        }
    }
}