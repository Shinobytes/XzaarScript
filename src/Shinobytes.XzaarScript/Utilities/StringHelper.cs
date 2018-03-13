﻿/* 
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
}