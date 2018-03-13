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
 
using System.Linq;

namespace Shinobytes.XzaarScript.Assembly
{
    public class InstructionVariableCollection : Collection<MemberReference>
    {
        public string ToString(string del)
        {
            var value = base.InternalItems.Select(GetStringRepresentation);
            return string.Join(del, value.ToArray());
        }

        private static string GetStringRepresentation(MemberReference i)
        {
            if (i == null) return null;
            var c = i as Constant;
            if (c != null)
            {
                if (c.ArrayIndex != null)
                {
                    var value = GetStringRepresentation(c.ArrayIndex as VariableReference);
                    if (value != null)
                    {
                        return (c.Value is string ? "\"" + c.Value + "\"" : c.Value) + "[" + value + "]";
                    }
                }

                return c.Value is string ? "\"" + c.Value + "\"" : (c.Value + "");
            }
            var vref = i as VariableReference;
            if (vref != null)
            {
                var value = GetStringRepresentation(vref.ArrayIndex as VariableReference);
                if (value != null)
                {
                    return i.Name + "[" + value + "]";
                }
            }
            return i.Name;
        }
    }
}