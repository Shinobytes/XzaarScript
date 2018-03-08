using System;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Transpilers
{
    public interface IScriptTranspiler
    {
        TranspilerResult Transpile(XzaarExpression expression);        
    }
}
