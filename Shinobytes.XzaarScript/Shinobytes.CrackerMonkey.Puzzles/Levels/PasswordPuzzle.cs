namespace Shinobytes.Bytecode.Puzzles
{
    public class PasswordPuzzle : ISolvable
    {
        private readonly string password;

        private readonly ICodeInvoker codeInvoker;

        private PasswordPuzzleContext puzzleCtx;

        internal bool IsSolved;

        public PasswordPuzzle(string password, ICodeInvoker codeInvoker)
        {
            this.password = password;
            this.codeInvoker = codeInvoker;
        }

        public PasswordPuzzleContext PuzzleContext
        {
            get { return puzzleCtx ?? (puzzleCtx = new PasswordPuzzleContext(this, password)); }
        }

        public PuzzleSolveResult Solve()
        {
            codeInvoker.InvokePuzzleFunction(); // no result are expected here

            var result = new PuzzleSolveResult();
            result.Results.Add(new PuzzleResult(new PuzzleTestCase(new object[0], null), password, true));
            return result;
        }
    }
}