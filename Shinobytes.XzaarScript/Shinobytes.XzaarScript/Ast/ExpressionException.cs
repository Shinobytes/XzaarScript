using System;

namespace Shinobytes.XzaarScript.Ast
{
    public class ExpressionException : Exception
    {
        public ExpressionException(string msg = null) : base(msg)
        {            
        }
    }
}