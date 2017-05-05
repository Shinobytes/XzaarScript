using System;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsTypedObject : XsObject
    {
        public Type Type { get; set; }

        internal XsTypedObject(string name, Type type) : base(name)
        {
            this.Type = type;
        }
    }
}