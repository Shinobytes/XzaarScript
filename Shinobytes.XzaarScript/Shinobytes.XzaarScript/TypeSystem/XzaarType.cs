﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript
{

    [Serializable]
    public abstract class XzaarType : XzaarMemberInfo
    {
        public static readonly char Delimiter = '.';

        public virtual XzaarMethodBase DeclaringMethod
        {
            get { return null; }
        }

        public abstract XzaarMethodInfo[] GetMethods();
        public abstract XzaarFieldInfo[] GetFields();
        public abstract XzaarMethodInfo GetMethod(string name);

        public abstract XzaarFieldInfo GetField(string name);

        protected abstract bool IsArrayImpl();

        public abstract XzaarType UnderlyingSystemType { get; }

        protected abstract XzaarMethodInfo GetMethodImp(string name, XzaarType[] types);

        public abstract XzaarType GetElementType();

        public abstract XzaarType BaseType { get; }

        internal XzaarMethodInfo GetMethod(string name, XzaarType[] types)
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

        public static XzaarTypeCode GetTypeCode(XzaarType type)
        {
            if (type == null) return XzaarTypeCode.Empty;
            return type.GetTypeCodeImpl();
        }

        private XzaarTypeCode GetTypeCodeImpl()
        {
            switch (this.Name)
            {
                case "bool":
                case "boolean": return XzaarTypeCode.Boolean;
                case "number": return XzaarTypeCode.Double;

                case "i8": return XzaarTypeCode.SByte;
                case "i16": return XzaarTypeCode.Int16;
                case "i32": return XzaarTypeCode.Int32;
                case "i64": return XzaarTypeCode.Int64;
                case "u8": return XzaarTypeCode.Byte;
                case "u16": return XzaarTypeCode.UInt16;
                case "u32": return XzaarTypeCode.UInt32;
                case "u64": return XzaarTypeCode.UInt64;
                case "f32": return XzaarTypeCode.Single;
                case "f64": return XzaarTypeCode.Double;
                case "string": return XzaarTypeCode.String;
                case "object": case "any": return XzaarTypeCode.Any;
                case "datetime": return XzaarTypeCode.DateTime;
            }
            if (this != UnderlyingSystemType && UnderlyingSystemType != null)
                return XzaarType.GetTypeCode(UnderlyingSystemType);
            return XzaarTypeCode.Any;
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

        public bool IsArray
        {
            get { return IsArrayImpl(); }
        }

        public bool IsByRef
        {
            get { return IsByRefImpl(); }
        }

        public bool IsAny
        {
            get { return this.Name.ToLower() == "any"; }
        }

        public bool IsNumeric
        {
            get { return this.BaseType?.Name.ToLower() == "number" || this.Name.ToLower() == "number"; }
        }

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
