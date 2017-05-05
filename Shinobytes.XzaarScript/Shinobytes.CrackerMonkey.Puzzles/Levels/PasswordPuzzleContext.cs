namespace Shinobytes.Bytecode.Puzzles
{
    public class PasswordPuzzleContext
    {
        private readonly PasswordPuzzle puzzle;
        private readonly object password;

        public PasswordPuzzleContext(PasswordPuzzle puzzle, object password)
        {
            this.puzzle = puzzle;
            this.password = password;
        }

        public bool Unlock(object value)
        {
            if (value.Equals(password))
            {
                puzzle.IsSolved = true;
            }
            return puzzle.IsSolved;
        }
    }
}