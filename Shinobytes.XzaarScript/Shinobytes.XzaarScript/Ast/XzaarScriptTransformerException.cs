using System;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarScriptTransformerException : Exception
    {
        public XzaarScriptTransformerException() { }
        public XzaarScriptTransformerException(string msg) : base(msg) { }
    }
}