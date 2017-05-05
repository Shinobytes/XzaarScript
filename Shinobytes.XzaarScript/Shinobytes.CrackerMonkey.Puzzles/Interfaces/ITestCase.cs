namespace Shinobytes.Bytecode.Puzzles
{
    public interface ITestCase
    {
        string Hint { get; }
        object[] Input { get; }
    }
}