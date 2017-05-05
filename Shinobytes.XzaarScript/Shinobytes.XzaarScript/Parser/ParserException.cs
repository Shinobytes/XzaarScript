using System;

namespace Shinobytes.XzaarScript.Parser
{
    public class ParserException : Exception
    {
        public ParserException(string message = null) : base(message) { }
    }
}