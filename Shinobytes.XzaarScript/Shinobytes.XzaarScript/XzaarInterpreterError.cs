namespace Shinobytes.XzaarScript
{
    public class XzaarInterpreterError
    {
        public readonly XzaarInterpreterErrorLocation ErrorLocation;
        public readonly string Error;

        public XzaarInterpreterError(XzaarInterpreterErrorLocation errorLocation, string error)
        {
            ErrorLocation = errorLocation;
            Error = error;
        }
    }
}