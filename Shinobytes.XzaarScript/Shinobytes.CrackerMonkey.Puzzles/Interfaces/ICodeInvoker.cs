namespace Shinobytes.Bytecode.Puzzles
{
    public interface ICodeInvoker
    {
        object InvokePuzzleFunction(params object[] input);
    }
}
