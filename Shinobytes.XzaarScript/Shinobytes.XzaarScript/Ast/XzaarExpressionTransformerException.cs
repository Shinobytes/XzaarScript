using System;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarExpressionTransformerException : Exception
    {
        public XzaarExpressionTransformerException(string msg = null) : base(msg)
        {            
        }
    }
}