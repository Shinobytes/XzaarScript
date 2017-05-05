using System;

namespace Shinobytes.XzaarScript.Compiler.Types
{
    public class XsParameter : XsTypedObject
    {
        public XsParameter(string name, Type type) : base(name, type)
        {
        }

        public int Index { get; set; }

        public static XsParameter[] NoParameters => new XsParameter[0];
    }
}