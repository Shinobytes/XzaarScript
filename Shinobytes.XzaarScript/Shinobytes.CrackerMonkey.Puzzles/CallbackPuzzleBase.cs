namespace Shinobytes.Bytecode.Puzzles
{
    public abstract class CallbackPuzzleBase : IPuzzle
    {
        private ISolvable solvableCallback;

        protected readonly ICodeInvoker CodeInvoker;

        protected CallbackPuzzleBase(ISolvable solvableCallback, ICodeInvoker codeInvoker)
        {
            this.solvableCallback = solvableCallback;
            this.CodeInvoker = codeInvoker;
        }

        /// <summary>
        /// The name of the level
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a hint that can be shown to the player when in need of help.
        /// </summary>
        public abstract string Hint { get; }

        /// <summary>
        /// Gets the code the player should start with on this puzzle
        /// </summary>        
        public abstract string StartingCode { get; }

        /// <summary>
        /// Try and solve this puzzle by generating testcases and invoking the code the player has written and tests the result against the expected values. 
        /// </summary>
        /// <returns>A instance of <see cref="PuzzleSolveResult"/> that contains whether the player successefully solved the puzzle or not</returns>
        public PuzzleSolveResult Solve()
        {
            return solvableCallback.Solve();
        }
    }
}