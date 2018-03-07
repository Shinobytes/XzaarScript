namespace Shinobytes.XzaarScript
{
    public class InterpreterError
    {
        public readonly InterpreterErrorLocation ErrorLocation;
        public readonly string Error;

        public InterpreterError(InterpreterErrorLocation errorLocation, string error)
        {
            ErrorLocation = errorLocation;
            Error = error;
        }
    }
}