namespace Shinobytes.Bytecode.Puzzles
{
    public class PuzzleTestCase : ITestCase
    {
        public PuzzleTestCase(object[] input, string hint)
        {
            Hint = hint;
            Input = input;
        }

        public string Hint { get; }

        public object[] Input { get; }
    }
}