using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly.Models;

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