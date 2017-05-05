using System;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Compiler.Compilers
{
    public interface IScriptCompiler
    {
        CompiledScriptBase Compile(EntryNode ast);
        void RegisterVariable<T>(string name);
        void RegisterVariable<T>(string name, Scope scope);
        void RegisterVariable(string name, Type type);
        void RegisterVariable(string name, Type type, Scope scope);
        void UnregisterVariable(string name, Scope scope);
        void UnregisterAllVariables(Scope scope);
    }
}