using System;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarScriptTransformerException : Exception
    {
        public XzaarScriptTransformerException() { }
        public XzaarScriptTransformerException(string msg) : base(msg) { }
    }
}