/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

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