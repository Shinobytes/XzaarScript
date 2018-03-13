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