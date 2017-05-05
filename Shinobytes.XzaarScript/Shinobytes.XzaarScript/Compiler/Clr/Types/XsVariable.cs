using System;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsVariable : XsTypedObject
    {
        private readonly XsILGenerator ilGenerator;
        private LocalBuilder builder;

        public XsVariable(XsILGenerator ilGenerator, string name, Type type) : base(name, type)
        {
            this.ilGenerator = ilGenerator;
            this.builder = this.ilGenerator.DeclareLocal(type);
            if (!string.IsNullOrEmpty(name))
                this.builder.SetLocalSymInfo(name);
        }

        public int Index => this.builder.LocalIndex;

        public LocalBuilder VariableInfo => builder;
    }
}