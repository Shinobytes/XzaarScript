using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsGlobal : XsClass
    {
        private const string MainMethodName = "$__main";

        private List<XsStruct> definedTypes = new List<XsStruct>();

        internal XsGlobal(ModuleBuilder module) : base(module, "Global_Script_Scope")
        {
            this.MainMethod = this.DefineMethod(MainMethodName, this.TypeBuilder, XsParameter.NoParameters);
        }

        protected override void OnCreateType()
        {
            var il = MainMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ret);
        }

        public XsClass DefineClass(string name)
        {
            var type = new XsClass(this.module, name);
            definedTypes.Add(type);
            return type;
        }

        public XsStruct DefineStruct(string name)
        {
            var type = new XsStruct(this.module, name, TypeAttributes.Public);
            definedTypes.Add(type);
            return type;
        }

        public XsILGenerator GetILGenerator() => this.MainMethod.GetILGenerator();

        public XsMethod MainMethod { get; set; }

        public XsStruct[] DefinedTypes => definedTypes.ToArray();
    }
}