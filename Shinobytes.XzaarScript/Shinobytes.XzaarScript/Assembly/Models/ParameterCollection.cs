namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class ParameterCollection : Collection<ParameterDefinition>
    {
        public ParameterCollection(MethodReference method)
        {
            Method = method;
        }

        public MethodReference Method { get; internal set; }

        public void AddParameters(ParameterDefinition[] parameters)
        {
            foreach (var p in parameters) Add(p);
        }

        public override void Add(ParameterDefinition item)
        {
            item.Method = this.Method;
            base.Add(item);
        }
    }
}