namespace Shinobytes.XzaarScript.Ast
{
    public abstract class CompiledScriptBase
    {
        public abstract T Invoke<T>(params object[] args);
    }
}