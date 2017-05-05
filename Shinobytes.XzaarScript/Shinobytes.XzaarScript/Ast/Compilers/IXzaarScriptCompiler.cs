using System;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast.Compilers
{
    public interface IXzaarScriptCompiler
    {
        XzaarCompiledScriptBase Compile(EntryNode ast);
        void RegisterVariable<T>(string name);
        void RegisterVariable<T>(string name, XzaarScope scope);
        void RegisterVariable(string name, Type type);
        void RegisterVariable(string name, Type type, XzaarScope scope);
        void UnregisterVariable(string name, XzaarScope scope);
        void UnregisterAllVariables(XzaarScope scope);
    }
}