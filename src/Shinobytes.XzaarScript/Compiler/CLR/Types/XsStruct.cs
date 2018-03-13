/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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