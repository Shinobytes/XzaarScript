namespace Shinobytes.XzaarScript.Transpilers
{
    public struct TranspilerResult
    {
        public readonly string TranspiledCode;

        public TranspilerResult(string transpiledCode)
        {
            TranspiledCode = transpiledCode;
        }
    }
}