using System;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarTransformerException : Exception
    {
        public XzaarTransformerException(string message = null) : base(message) { }
    }
}