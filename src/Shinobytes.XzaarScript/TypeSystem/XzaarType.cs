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
using System.Linq;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using TypeCode = Shinobytes.XzaarScript.Parser.Ast.TypeCode;

namespace Shinobytes.XzaarScript
{

    [Serializable]
    public abstract class XzaarType : XzaarMemberInfo
    {
        public static readonly char Delimiter = '.';

        public virtual XzaarMethodBase DeclaringMethod => null;

        public abstract XzaarMethodBase[] GetMethods();
        public abstract XzaarFieldInfo[] GetFields();
        public abstract XzaarMethodBase GetMethod(string name);

        public abstract XzaarFieldInfo GetField(string name);

        protected abstract bool IsArrayImpl();

        public abstract XzaarType UnderlyingSystemType { get; }

        protected abstract XzaarMethodBase GetMethodImp(string name, XzaarType[] types);

        public abstract XzaarType GetElementType();

        public abstract XzaarType BaseType { get; }

        internal XzaarMethodBase GetMethod(string name, XzaarType[] types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == null)
                    throw new ArgumentNullException(nameof(types));
            }
            return GetMethodImp(name, types);
        }

        public static XzaarType GetType(string name)
        {
            var xt = XzaarBaseTypes.BaseTypes.FirstOrDefault(t => t.Name == name);
            if ((object)xt != null) return xt;
            if (!string.IsNullOrEmpty(name))
            {
                switch (name.ToLower())
                {
                    case "bool":
                    case "boolean":
                        return XzaarBaseTypes.Boolean;
                    case "str":
                    case "string":
                        return XzaarBaseTypes.String;
                    case "any":
                        return XzaarBaseTypes.Any;
                    case "void":
                        return XzaarBaseTypes.Void;
                    case "i8": return XzaarBaseTypes.I8;
                    case "i16": return XzaarBaseTypes.I16;
                    case "i32": return XzaarBaseTypes.I32;
                    case "i64": return XzaarBaseTypes.I64;
                    case "u8": return XzaarBaseTypes.U8;
                    case "u16": return XzaarBaseTypes.U16;
                    case "u32": return XzaarBaseTypes.U32;
                    case "u64": return XzaarBaseTypes.U64;
                    case "f32": return XzaarBaseTypes.F32;
                    case "f64": return XzaarBaseTypes.F64;
                    case "number":
                    case "int":
                    case "float":
                    case "byte":
                    case "short":
                    case "double":
                    case "long":
                        return XzaarBaseTypes.Number;
                }
            }
            return null;
        }

        public static TypeCode GetTypeCode(XzaarType type)
        {
            if (type == null) return TypeCode.Empty;
            return type.GetTypeCodeImpl();
        }

        private TypeCode GetTypeCodeImpl()
        {
            switch (this.Name)
            {
                case "bool":
                case "boolean": return TypeCode.Boolean;
                case "number": return TypeCode.Double;

                case "i8": return TypeCode.SByte;
                case "i16": return TypeCode.Int16;
                case "i32": return TypeCode.Int32;
                case "i64": return TypeCode.Int64;
                case "u8": return TypeCode.Byte;
                case "u16": return TypeCode.UInt16;
                case "u32": return TypeCode.UInt32;
                case "u64": return TypeCode.UInt64;
                case "f32": return TypeCode.Single;
                case "f64": return TypeCode.Double;
                case "str": return TypeCode.String;
                case "string": return TypeCode.String;
                case "object": case "any": return TypeCode.Any;
                case "datetime": return TypeCode.Date;
            }
            if (this != UnderlyingSystemType && UnderlyingSystemType != null)
                return XzaarType.GetTypeCode(UnderlyingSystemType);
            return TypeCode.Any;
        }

        public bool IsEquivalentTo(XzaarType other)
        {
            return (this == other);
        }

        public virtual bool Equals(Type t)
        {
            if (t == null) return false;
            return XzaarBaseTypes.Typeof(t) == this;
        }


        public override bool Equals(object o)
        {
            if ((object)o == null)
                return false;

            return Equals(o as XzaarType);
        }

        public virtual bool Equals(XzaarType o)
        {
            if ((object)o == null)
                return false;

            if (o.Name == this.Name)
            {
                return true;
                // for now we wont expect multiple types with the same name since we don't support namespaces just yet. otherwise we could do o.FullName == ..
            }

            return (o.IsAny && IsAny) || ((object.ReferenceEquals(this.UnderlyingSystemType, o.UnderlyingSystemType)) && !o.UnderlyingSystemType.IsAny);
        }

        public bool IsArray => IsArrayImpl();

        public bool IsByRef => IsByRefImpl();

        public bool IsAny => this.Name.ToLower() == "any";

        public bool IsNumeric => this.BaseType?.Name.ToLower() == "number" || this.Name.ToLower() == "number";

        protected abstract bool IsByRefImpl();

        public override int GetHashCode()
        {
            var systemType = UnderlyingSystemType;
            return !ReferenceEquals(systemType, this) ? systemType.GetHashCode() : base.GetHashCode();
        }

        public static explicit operator XzaarType(Type t)
        {
            return XzaarBaseTypes.Typeof(t);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static bool TryGetType(string typeName, StructExpression typeExpression, out XzaarType newType)
        {
            var a = GetType(typeName);
            if (a == null || Equals(a, XzaarBaseTypes.Void) || Equals(a, XzaarBaseTypes.Any))
            {
                newType = XzaarBaseTypes.CreateTypeFromStructExpression(typeExpression);
                XzaarBaseTypes.AddTypeToCache(newType);
            }
            else
            {
                newType = a;
            }
            return true;
        }
    }
}
