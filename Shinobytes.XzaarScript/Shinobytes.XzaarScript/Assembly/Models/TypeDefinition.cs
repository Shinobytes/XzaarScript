namespace Shinobytes.XzaarScript.Assembly.Models
{
    public class TypeDefinition : TypeReference
    {    
        public TypeDefinition(XzaarType type)
            : base(type)
        {          
        }

        public TypeDefinition(string name, bool isStruct)
            : base(name, null)
        {
            this.IsClass = !isStruct;
        }
    }
}