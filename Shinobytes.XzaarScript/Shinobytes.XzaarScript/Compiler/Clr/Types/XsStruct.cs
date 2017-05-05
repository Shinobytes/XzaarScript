using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsStruct : XsObject
    {
        private readonly List<XsField> fields = new List<XsField>();
        private readonly List<XsMethod> methods = new List<XsMethod>();

        protected readonly ModuleBuilder module;
        protected readonly TypeBuilder builder;
        private Type createdType;

        public XsField[] Fields => fields.ToArray();

        public XsMethod[] Methods => methods.ToArray();

        internal XsStruct(ModuleBuilder module, string name)
            : this(module, name, TypeAttributes.Public)
        {
        }

        internal XsStruct(ModuleBuilder module, string name, TypeAttributes attributes)
            : base(name)
        {
            this.module = module;
            this.builder = module.DefineType(name, attributes);
            this.DefineConstructor();
        }

        public XsField DefineField(string name, Type fieldType)
        {
            var field = new XsField(this.builder, name, fieldType);
            this.fields.Add(field);
            return field;
        }

        public XsMethod DefineMethod(string name, Type returnType, XsParameter[] parameters)
        {
            var method = new XsMethod(this, name, MethodAttributes.Public, returnType, parameters);
            this.methods.Add(method);
            return method;
        }

        private void DefineConstructor()
        {
            var ctor = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            var il = ctor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);
        }

        public TypeBuilder TypeBuilder => builder;

        public string FullName => Name;

        public Type CreateType()
        {
            if (this.builder.IsCreated()) return createdType;

            this.OnCreateType();

            this.createdType = this.builder.CreateType();
            return createdType;
        }

        protected virtual void OnCreateType() { }
    }
}