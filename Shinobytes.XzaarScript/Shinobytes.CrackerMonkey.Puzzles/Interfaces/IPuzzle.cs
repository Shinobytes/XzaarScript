namespace Shinobytes.Bytecode.Puzzles
{
    public interface IPuzzle : ISolvable
    {
        string Name { get; }

        string Hint { get; }

        string StartingCode { get; }        
    }
}