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
 
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Utilities
{
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