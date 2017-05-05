namespace Shinobytes.Bytecode.Puzzles
{
    public class CabinDoorPuzzle : CallbackPuzzleBase
    {
        public CabinDoorPuzzle(string password, ICodeInvoker codeInvoker)
            : base(new PasswordPuzzle(password, codeInvoker), codeInvoker)
        {
        }

        public override string Name { get { return "The Cabin Door"; } }

        public override string Hint { get { return "A 4 number pin... I could bruteforce it"; } }

        public override string StartingCode
        {
            get
            {
                return @"let door = $door

fn puzzle() {
   // perhaps a for loop?
   door.Unlock(0000)
}

puzzle()";
            }
        }
    }
}
