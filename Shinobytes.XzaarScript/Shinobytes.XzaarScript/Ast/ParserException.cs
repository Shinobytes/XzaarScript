using System;

namespace Shinobytes.XzaarScript.Ast
{
    public class ParserException : Exception
    {
        public ParserException(string msg) : base(msg) { }
    }
}