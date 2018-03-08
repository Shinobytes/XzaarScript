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