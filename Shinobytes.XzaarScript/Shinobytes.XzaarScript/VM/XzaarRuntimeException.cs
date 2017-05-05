using System;

namespace Shinobytes.XzaarScript.VM
{
    public class XzaarRuntimeException : Exception
    {
        public XzaarRuntimeException() { }
        public XzaarRuntimeException(string message) : base(message) { }
    }
}