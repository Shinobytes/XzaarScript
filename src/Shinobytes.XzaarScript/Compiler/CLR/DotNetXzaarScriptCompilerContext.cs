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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public class DotNetXzaarScriptCompilerContext
    {
        private const string MainMethodName = "$__main";
        private readonly DotNetFlowControlScope rootFlowControl;
        private DotNetFlowControlScope flowControl;

        private AssemblyName assemblyName;
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder mainModule;

        private bool mainFinalized = false;


        private Type mainTypeBuilt;
        private readonly Dictionary<string, List<XsMethod>> methods = new Dictionary<string, List<XsMethod>>();
        private readonly Dictionary<string, List<XsField>> fields = new Dictionary<string, List<XsField>>();
        private readonly HashSet<string> methodFinalized = new HashSet<string>();

        public DotNetXzaarScriptCompilerContext()
        {
            assemblyName = new AssemblyName("DynamicXzaarScriptAssembly");
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            mainModule = assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll", true);

            MainType = CurrentType = Global = new XsGlobal(mainModule);
            MainMethod = CurrentMethod = Global.MainMethod;

            var il = GetILGenerator();
            this.flowControl = new DotNetFlowControlScope(null, -1, il.DefineLabel(), il.DefineLabel());
            this.rootFlowControl = this.flowControl;

        }

        public XsGlobal Global { get; set; }

        public XsStruct MainType { get; }
        public XsMethod MainMethod { get; }

        public XsField DefineField(string fieldName, Type fieldType)
        {
            var field = this.CurrentType.DefineField(fieldName, fieldType);
            this.MapFieldToCurrentType(field);
            return field;
        }

        public XsMethod DefineMethod(string name, Type returnType, XsParameter[] parameterTypes)
        {
            var method = CurrentType.DefineMethod(name, returnType, parameterTypes);

            MapMethodToCurrentType(method);

            return method;
        }

        public XsMethod DefineMethod(string name, Type returnType)
        {
            var method = CurrentType.DefineMethod(name, returnType, XsParameter.NoParameters);
            MapMethodToCurrentType(method);
            return method;
        }

        public XsMethod DefineMethod(string name)
        {
            var method = CurrentType.DefineMethod(name, typeof(void), XsParameter.NoParameters);
            MapMethodToCurrentType(method);
            return method;
        }

        public bool IsInGlobalScope => CurrentMethod == null || Equals(CurrentMethod, MainMethod);

        public DotNetFlowControlScope CurrentScope => this.flowControl;

        public DotNetFlowControlScope BeginControlBlock()
        {
            var il = GetILGenerator();
            return this.flowControl = this.flowControl.BeginControlBlock(il.DefineLabel(), il.DefineLabel());
        }

        public void EndControlBlock()
        {
            this.flowControl = this.flowControl.EndControlBlock();
        }

        public XsMethod CurrentMethod { get; set; }
        public XsStruct CurrentType { get; set; }

        public int StackRecursionCount { get; set; }
        public XzaarExpression LastVisitedExpression { get; set; }

        public Delegate CreateDelegate()
        {
            mainTypeBuilt = this.MainType.CreateType();

            var main = mainTypeBuilt.GetMethods().First(x => x.Name == MainMethodName);

            this.assemblyBuilder.Save(assemblyName.Name + ".dll");

            var instance = Activator.CreateInstance(mainTypeBuilt);

            return Delegate.CreateDelegate(System.Linq.Expressions.Expression.GetFuncType(mainTypeBuilt), instance, main);
        }

        public Type GetClrType(XzaarType type)
        {
            Type target = Global.DefinedTypes.FirstOrDefault(t => t.Name == type.Name)?.TypeBuilder;
            if (type.IsAny) return type.IsArray ? typeof(object[]) : typeof(object);
            var lower = type.Name.ToLower();
            switch (lower)
            {
                case "void":
                    target = typeof(void);
                    break;
                case "bool":
                case "boolean":
                    target = typeof(bool);
                    break;
                case "string":
                case "char":
                    target = typeof(string);
                    break;
                case "byte":
                case "u8":
                    target = typeof(byte);
                    break;
                case "sbyte":
                case "i8":
                    target = typeof(sbyte);
                    break;
                case "ushort":
                case "u16":
                    target = typeof(ushort);
                    break;
                case "uint":
                case "u32":
                    target = typeof(uint);
                    break;
                case "ylong":
                case "u64":
                    target = typeof(ulong);
                    break;
                case "short":
                case "i16":
                    target = typeof(short);
                    break;
                case "int":
                case "i32":
                    target = typeof(int);
                    break;
                case "long":
                case "i64":
                    target = typeof(long);
                    break;
                case "float":
                case "f32":
                    target = typeof(float);
                    break;
                case "double":
                case "f64":
                    target = typeof(double);
                    break;
                case "number":
                    target = typeof(double);
                    break;
                case "datetime":
                case "date":
                    target = typeof(DateTime);
                    break;
                case "timespan":
                    target = typeof(TimeSpan);
                    break;
            }
            if (target != null) return type.IsArray ? target.MakeArrayType() : target;
            return null;
        }

        public XsMethod[] GetCurrentTypeMethods()
        {
            return methods.ContainsKey(CurrentType.Name)
                ? methods[CurrentType.Name].ToArray()
                : new XsMethod[0];
        }

        public XsField[] GetCurrentTypeFields()
        {
            return fields.ContainsKey(CurrentType.Name)
                ? fields[CurrentType.Name].ToArray()
                : new XsField[0];
        }

        private XsMethod MapMethodToCurrentType(XsMethod method)
        {
            if (methods.ContainsKey(CurrentType.FullName))
            {
                methods[CurrentType.FullName].Add(method);
            }
            else
            {
                methods[CurrentType.FullName] = new List<XsMethod> { method };
            }
            return method;
        }

        private XsField MapFieldToCurrentType(XsField field)
        {
            if (fields.ContainsKey(CurrentType.FullName))
            {
                fields[CurrentType.FullName].Add(field);
            }
            else
            {
                fields[CurrentType.Name] = new List<XsField> { field };
            }
            return field;
        }

        internal bool GetMethodBodyDefined(XsMethod method)
        {
            return methodFinalized.Contains(method.FullName);
        }

        internal void SetMethodBodyDefined(XsMethod method, bool v)
        {
            var value = method.FullName;
            if (methodFinalized.Contains(value))
            {
                methodFinalized.Remove(value);
            }

            if (v)
            {
                methodFinalized.Add(value);
            }
        }

        public List<string> Errors { get; } = new List<string>();

        public ExpressionType LastBinaryOperationType { get; internal set; }
        public bool InsideBinaryOperation { get; internal set; }
        public object LastStoredReference { get; set; }
        public object LastLoadedReference { get; set; }

        public XsILGenerator GetILGenerator()
        {
            return this.IsInGlobalScope ? Global.GetILGenerator() : this.CurrentMethod.GetILGenerator();
        }
    }
}
