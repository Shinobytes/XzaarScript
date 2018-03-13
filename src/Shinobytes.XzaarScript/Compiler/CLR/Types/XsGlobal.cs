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