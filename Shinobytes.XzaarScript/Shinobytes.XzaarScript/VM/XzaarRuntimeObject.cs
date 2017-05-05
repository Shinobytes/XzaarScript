using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.VM
{
    internal class XzaarRuntimeObject
    {
        private readonly TypeReference objectType;
        private readonly List<XzaarRuntimeVariable> fields;
        public readonly XzaarRuntime Runtime;

        public XzaarRuntimeObject(XzaarRuntime rt, TypeReference objectType)
        {
            this.Runtime = rt;
            this.objectType = objectType;
            this.fields = new List<XzaarRuntimeVariable>();
            this.InitFields();
        }

        private void InitFields()
        {
            foreach (var f in this.objectType.Fields)
            {
                this.fields.Add(new XzaarRuntimeVariable(Runtime, f));
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

        public XzaarRuntimeVariable GetField(string fieldName)
        {
            var field = this.fields.FirstOrDefault(f => f.Name == fieldName);
            if (field == null) throw new XzaarRuntimeException("The object '" + this.objectType.Name + "' does not contain any fields with the name '" + fieldName + "'");
            return field;
        }
    }
}