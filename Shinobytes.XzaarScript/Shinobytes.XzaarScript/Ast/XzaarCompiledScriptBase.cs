namespace Shinobytes.XzaarScript.Ast
{
    public abstract class XzaarCompiledScriptBase
    {
        public abstract T Invoke<T>(params object[] args);
    }
}