using System;

namespace Shinobytes.Bytecode.Puzzles
{
    public abstract class ValuePuzzleBase : IPuzzle
    {
        private readonly ITestCaseProvider testCaseProvider;
        private readonly ICodeInvoker codeInvoker;

        protected ValuePuzzleBase(ITestCaseProvider testCaseProvider, ICodeInvoker codeInvoker)
        {
            this.testCaseProvider = testCaseProvider;
            this.codeInvoker = codeInvoker;
        }

        /// <summary>
        /// Gets the amount of test cases that should be generated and tested. 
        /// The puzzle will generally increase in difficulty the more test cases you have
        /// as all test cases must be successefull to complete the puzzle. 
        /// Therefor the code does not necessarily have to be the correct solution as long as it generates
        /// the correct/expected value.
        /// </summary>
        protected abstract int TestCaseCount { get; }

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
        public virtual string StartingCode
        {
            get
            {
                return "fn puzzle(input : any) -> any {" + Environment.NewLine +
                       "  return input" + Environment.NewLine +
                       "}";
            }
        }

        /// <summary>
        /// This function implements the actual puzzle logic that generates the expected test results.         
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract object Puzzle(params object[] input);

        /// <summary>
        /// Try and solve this puzzle by generating testcases and invoking the code the player has written and tests the result against the expected values. 
        /// </summary>
        /// <returns>A instance of <see cref="PuzzleSolveResult"/> that contains whether the player successefully solved the puzzle or not</returns>
        public PuzzleSolveResult Solve()
        {
            var result = new PuzzleSolveResult();
            for (var i = 0; i < TestCaseCount; i++)
            {
                var testCase = this.testCaseProvider.Get(i);
                var expectedResult = Puzzle(testCase.Input);
                var puzzleResult = codeInvoker.InvokePuzzleFunction(testCase.Input);
                result.Results.Add(new PuzzleResult(testCase, puzzleResult, puzzleResult == expectedResult));
            }
            return result;
        }
    }
}