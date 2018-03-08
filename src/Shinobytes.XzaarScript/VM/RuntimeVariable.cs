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
using System.Linq;
using Shinobytes.XzaarScript.Assembly;

namespace Shinobytes.XzaarScript.VM
{
    public class RuntimeVariable
    {
        public string Name;

        public object Value
        {
            get
            {
                if (value == null && (arrayValue == null)) return null;
                return value ?? arrayValue;
            }
        }

        public TypeReference Type;
        public readonly Runtime Runtime;

        private object[] arrayValue;
        private object value;

        public void AddToArray(object value)
        {
            var index = arrayValue.Length;

            SetValueInt(value, index);
        }

        public void InsertToArrray(object arrayIndex, object value)
        {
            var index = AsInt(arrayIndex);
            var tmp = arrayValue.ToList();

            if (index < 0 || index >= tmp.Count)
                throw new RuntimeException("The value '" + arrayIndex + "' is not a valid element index.");

            tmp.Insert(index, value);
            arrayValue = tmp.ToArray();
        }

        public void RemoveFromArray(object arrayIndex)
        {
            var index = AsInt(arrayIndex);
            var tmp = arrayValue.ToList();

            if (index < 0 || index >= tmp.Count)
                throw new RuntimeException("The value '" + arrayIndex + "' is not a valid element index.");

            tmp.RemoveAt(index);
            arrayValue = tmp.ToArray();
        }

        public void SetValue(object value)
        {
            this.value = value;
            if (value != null)
            {
                if (value is object[])
                {
                    this.value = this.arrayValue = (object[])value;
                    return;
                }
            }
            this.SetValue(value, 0);
        }

        public void SetValueInt(object newValue, int arrayIndex)
        {
            EnsureArrayIndexAvailable(arrayIndex);
            arrayValue[arrayIndex] = newValue;
        }

        public void SetValue(object newValue, object arrayIndex)
        {
            var index = AsInt(arrayIndex);
            SetValueInt(newValue, index);
        }


        public object GetValue(object arrayIndex)
        {
            var index = AsInt(arrayIndex);

            if (value != null && index >= 0)
            {
                var type = value.GetType();
                if (!type.IsArray)
                {
                    var vt = type.Name;
                    switch (vt.ToLower())
                    {
                        case "string": return (Value + "")[index];
                        case "number":
                        case "int":
                        case "short":
                        case "float":
                        case "double":
                        case "byte":
                        case "sbyte":
                        case "ushort":
                        case "long":
                        case "ulong":
                        case "uint":
                            {
                                // TODO: return bit
                            }
                            return Value;
                    }
                }
            }

            EnsureArrayIndexAvailable(index);
            return arrayValue[index];
        }

        private void EnsureArrayIndexAvailable(int index)
        {
            if (arrayValue == null) arrayValue = new object[index + 1];
            if (index >= arrayValue.Length)
            {
                var oldArray = arrayValue.ToList();
                for (var i = 0; i < (index + 1) - arrayValue.Length; i++) oldArray.Add(null);
                arrayValue = oldArray.ToArray();
            }
        }

        public RuntimeVariable(Runtime rt, VariableReference v)
            : this(rt, v, null)
        {
        }

        public RuntimeVariable(Runtime rt, ParameterDefinition p)
            : this(rt, p, null)
        {
        }

        public RuntimeVariable(Runtime rt, Constant constantValue)
        {
            this.Runtime = rt;
            this.Name = Guid.NewGuid().ToString();
            this.Type = constantValue.Type;
            this.value = constantValue.Value;
            if (value is object[]) this.arrayValue = (object[])value;
        }

        public RuntimeVariable(Runtime rt, VariableReference v, object value)
        {
            this.Runtime = rt;
            this.Name = v.Name ?? Guid.NewGuid().ToString();
            this.Type = v.Type;
            this.value = value;
            if (value is object[]) this.arrayValue = (object[])value;
        }

        public RuntimeVariable(Runtime rt, ParameterDefinition p, object value)
        {
            this.Runtime = rt;
            this.Name = p.Name ?? Guid.NewGuid().ToString(); ;
            this.Type = p.Type;
            this.value = value;
            if (value is object[]) this.arrayValue = (object[])value;
        }

        private int AsInt(object arrayIndex)
        {
            try
            {
                // return int.Parse(arrayIndex.ToString());
                return (int)Convert.ChangeType(arrayIndex, typeof(int));
            }
            catch
            {
                return -1;
            }
        }

        public void InitArray()
        {
            this.arrayValue = new object[0];
        }

        public int GetArrayLength()
        {
            return arrayValue.Length;
        }

        public override string ToString()
        {
            return this.Name + ": " + this.Value;
        }

        public void ClearArray()
        {
            this.arrayValue = new object[0];
            if (this.value is object[])
            {
                this.value = arrayValue;
            }
        }

        public int ArrayIndexOf(object valueComparison)
        {
            if (this.arrayValue == null) return -1;

            return Array.IndexOf(this.arrayValue.Select(GetValueFromObject).ToArray(), valueComparison);
        }

        private object GetValueFromObject(object item)
        {
            if (item == null) return null;
            var vrr = item as RuntimeVariable;
            if (vrr != null && vrr.Value != null)
            {
                return GetValueFromObject(vrr.Value);
            }
            var varRef = item as Constant;
            if (varRef != null)
            {
                return GetValueFromObject(varRef.Value);
            }
            return item;
        }
    }
}