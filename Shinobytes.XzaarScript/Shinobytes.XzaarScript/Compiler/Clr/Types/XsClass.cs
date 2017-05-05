using System.Reflection;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsClass : XsStruct
    {
        public XsClass(ModuleBuilder module, string name) : base(module, name, TypeAttributes.Public | TypeAttributes.Class)
        {
        }        
    }
}