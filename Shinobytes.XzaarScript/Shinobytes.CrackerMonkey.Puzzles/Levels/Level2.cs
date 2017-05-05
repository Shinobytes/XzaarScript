using System;

namespace Shinobytes.Bytecode.Puzzles
{
    public sealed class Level2ValuePuzzle : ValuePuzzleBase
    {
        public Level2ValuePuzzle(ITestCaseProvider testCaseProvider, ICodeInvoker codeInvoker)
            : base(testCaseProvider, codeInvoker)
        {
        }

        /// <summary>
        /// The name of the level
        /// </summary>
        public override string Name { get { return "Level 2"; } }

        /// <summary>
        /// Gets a hint that can be shown to the player when in need of help.
        /// </summary>
        public override string Hint { get { return "Seems like another arithmetic operator"; } }

        /// <summary>
        /// Gets the code the player should start with on this puzzle
        /// </summary>        
        public override string StartingCode
        {
            get
            {
                return "fn puzzle(i1 : number, i2 : number) -> any {" + Environment.NewLine +
                       "  return i1" + Environment.NewLine +
                       "}";
            }
        }

        /// <summary>
        /// Gets the amount of test cases that should be generated and tested. 
        /// The puzzle will generally increase in difficulty the more test cases you have
        /// as all test cases must be successefull to complete the puzzle. 
        /// Therefor the code does not necessarily have to be the correct solution as long as it generates
        /// the correct/expected value.
        /// </summary>
        protected override int TestCaseCount { get { return 5; } }

        /// <summary>
        /// This function implements the actual puzzle logic that generates the expected test results.         
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override object Puzzle(params object[] input)
        {
            return (int)input[0] * (int)input[0];
        }
    }

    public sealed class Level2TestCaseProvider : ITestCaseProvider
    {
        public ITestCase Get(int index)
        {
            return new PuzzleTestCase(new object[] { index }, "You can do it!");
        }
    }
}