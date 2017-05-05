using System;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarScriptParserException : Exception
    {
        public XzaarScriptParserException(string msg) : base(msg) { }
    }
}