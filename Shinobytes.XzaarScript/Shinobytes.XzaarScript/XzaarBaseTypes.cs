using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Scripting.Expressions;

namespace Shinobytes.XzaarScript
{
    public static class XzaarBaseTypes
    {
        public static XzaarType Void;
        public static XzaarType String;
        public static XzaarType Number;
        public static XzaarType Boolean;
        public static XzaarType Any;
        public static XzaarType Char;
        public static XzaarType DateTime;
        public static XzaarType Array;


        public static XzaarType StringArray, NumberArray, BooleanArray, ObjectArray, CharArray, DateTimeArray;

        internal static List<XzaarType> BaseTypes;

        static XzaarBaseTypes()
        {
            Void = new XzaarTypeBuilder("void", null, null, null);
            Any = new XzaarTypeBuilder("any", null, null, null);
            Boolean = new XzaarTypeBuilder("boolean", null, Any, Any);
            Number = new XzaarTypeBuilder("number", null, Any, Any);
            String = new XzaarTypeBuilder("string", null, Any, Any);
            Char = new XzaarTypeBuilder("char", null, Any, Any);
            DateTime = new XzaarTypeBuilder("datetime", null, Any, Any);
            Array = new XzaarTypeBuilder("array", null, Any, Any, Any);

            StringArray = new XzaarTypeBuilder("string[]", null, Any, Any, String);
            NumberArray = new XzaarTypeBuilder("number[]", null, Any, Any, Number);
            BooleanArray = new XzaarTypeBuilder("boolean[]", null, Any, Any, Boolean);
            CharArray = new XzaarTypeBuilder("char[]", null, Any, Any, Char);
            ObjectArray = new XzaarTypeBuilder("object[]", null, Any, Any, Any);
            DateTimeArray = new XzaarTypeBuilder("datetime[]", null, Any, Any, DateTime);

            BaseTypes = new List<XzaarType>(new[] { Void, Any, Boolean, Number, String, Char, DateTime, Array,
                StringArray, NumberArray, BooleanArray, CharArray, DateTimeArray,ObjectArray });
        }

        internal static void AddTypeToCache(XzaarType type)
        {
            BaseTypes.Add(type);
        }

        public static XzaarType Typeof(Type type)
        {


            if (type == typeof(int)
                || type == typeof(uint)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(decimal)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(int?)
                || type == typeof(uint?)
                || type == typeof(byte?)
                || type == typeof(sbyte?)
                || type == typeof(short?)
                || type == typeof(ushort?)
                || type == typeof(long?)
                || type == typeof(ulong?)
                || type == typeof(decimal?)
                || type == typeof(float?)
                || type == typeof(double?))
            {
                if (type.IsArray) return NumberArray;
                return Number;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?)) return type.IsArray ? DateTimeArray : DateTime;
            if (type == typeof(string)) return type.IsArray ? StringArray : String;
            if (type == typeof(bool)) return type.IsArray ? BooleanArray : Boolean;
            if (type == typeof(char)) return type.IsArray ? CharArray : Char;
            if (type == typeof(void)) return Void;

            return type.IsArray ? ObjectArray : Any;
        }

        public static XzaarType CreateTypeFromStructExpression(StructExpression typeExpression)
        {
            var newType = new XzaarTypeBuilder(typeExpression.Name, null, Any, Any, null);
            foreach (var f in typeExpression.Fields)
            {
                var fn = f as FieldExpression;
                if (fn != null)
                {
                    newType.AddField(fn.Type, fn.Name);
                }
            }
            return newType;
        }
    }
}