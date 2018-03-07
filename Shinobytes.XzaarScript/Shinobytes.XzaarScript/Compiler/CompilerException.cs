using System;

namespace Shinobytes.XzaarScript.Compiler
{
    public class CompilerException : Exception
    {
        public CompilerException(string message) :  base(message)
        {
        }
    }
}