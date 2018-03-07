using System;

namespace Shinobytes.XzaarScript.Parser
{
    public class LexerException : Exception
    {
        public LexerException(string msg) : base(msg) { }
    }
}