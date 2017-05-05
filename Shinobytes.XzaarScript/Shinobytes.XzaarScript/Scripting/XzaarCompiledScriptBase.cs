namespace Shinobytes.XzaarScript.Scripting
{
    public abstract class XzaarCompiledScriptBase
    {
        public abstract T Invoke<T>(params object[] args);
    }
}