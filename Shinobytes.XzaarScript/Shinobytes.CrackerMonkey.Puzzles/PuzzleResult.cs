namespace Shinobytes.Bytecode.Puzzles
{
    public class PuzzleResult
    {
        public PuzzleResult(ITestCase testCase, object result, bool success)
        {
            TestCase = testCase;
            Result = result;
            Success = success;
        }

        public ITestCase TestCase { get; }
        public object Result { get; }
        public bool Success { get; }
    }
}