using System;

namespace Shinobytes.XzaarScript.VM
{
    public class RuntimeException : Exception
    {
        public RuntimeException() { }
        public RuntimeException(string message) : base(message) { }
    }
}