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
 
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;

namespace Shinobytes.XzaarScript.VM
{
    internal class RuntimeObject
    {
        private readonly TypeReference objectType;
        private readonly List<RuntimeVariable> fields;
        public readonly Runtime Runtime;

        public RuntimeObject(Runtime rt, TypeReference objectType)
        {
            this.Runtime = rt;
            this.objectType = objectType;
            this.fields = new List<RuntimeVariable>();
            this.InitFields();
        }

        private void InitFields()
        {
            foreach (var f in this.objectType.Fields)
            {
                this.fields.Add(new RuntimeVariable(Runtime, f));
            }
        }

        public TypeReference Type => this.objectType;

        public object GetFieldValue(string fieldName)
        {
            return GetField(fieldName).Value;
        }

        public object GetFieldValue(string fieldName, object index)
        {
            return GetField(fieldName).GetValue(index);
        }

        public void SetFieldValue(string fieldName, object value)
        {
            GetField(fieldName).SetValue(value);
        }

        public void SetFieldValue(string fieldName, object value, object index)
        {
            GetField(fieldName).SetValue(value, index);
        }

        public RuntimeVariable GetField(string fieldName)
        {
            var field = this.fields.FirstOrDefault(f => f.Name == fieldName);
            if (field == null) throw new RuntimeException("The object '" + this.objectType.Name + "' does not contain any fields with the name '" + fieldName + "'");
            return field;
        }
    }
}