using System;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Utilities
{
    internal static class StringHelper
    {
        public static bool IsStringProperty(MemberAccessNode member)
        {
            return IsStringProperty(member.Value + "");
        }

        public static bool IsStringProperty(string v)
        {
            return IsLengthProperty(v);
        }

        public static bool IsLengthProperty(MemberAccessNode v)
        {
            return IsLengthProperty(v.Value + "");
        }

        public static bool IsLengthProperty(string v)
        {
            return v.ToLower() == "length";
        }

        public static bool IsStringFunction(string name)
        {
            return IsCharAt(name) || IsIndexOf(name);
        }

        public static bool IsCharAt(string name)
        {
            return name.ToLower() == "charat";
        }
        public static bool IsIndexOf(string name)
        {
            return name.ToLower() == "indexof";
        }
    }

    internal static class ArrayHelper
    {
        public static bool IsArrayProperty(MemberAccessNode member)
        {
            return IsArrayLengthProperty(member.Value + "");
        }

        public static bool IsArrayLengthProperty(string member)
        {
            return member.ToLower() == "length";
        }

        public static bool IsArrayProperty(string member)
        {
            member = member.ToLower();
            return member == "length";
        }

        public static bool IsArrayFunction(string v)
        {
            return IsArrayAdd(v) || IsArrayInsert(v) || IsArrayRemoveLast(v) || IsArrayRemove(v) || IsArrayAdd(v) || IsArrayIndexOf(v) || IsArrayClear(v);
        }

        public static bool IsIndexOf(string v)
        {
            return v.ToLower() == "indexof";
        }

        public static bool IsArrayAdd(string v)
        {
            v = v.ToLower();
            return v == "push" || v == "add";
        }

        public static bool IsArrayInsert(string v)
        {
            v = v.ToLower();
            return v == "put" || v == "insert";
        }

        public static bool IsArrayRemove(string v)
        {
            v = v.ToLower();
            return v == "remove";
        }

        public static bool IsArrayRemoveLast(string v)
        {
            v = v.ToLower();
            return v == "pop";
        }

        public static bool IsArrayIndexOf(string callMethodName)
        {
            return callMethodName.ToLower() == "indexof";
        }

        public static bool IsArrayClear(string callMethodName)
        {
            return callMethodName.ToLower() == "clear" || callMethodName.ToLower() == "removeall"; ;
        }
    }
}