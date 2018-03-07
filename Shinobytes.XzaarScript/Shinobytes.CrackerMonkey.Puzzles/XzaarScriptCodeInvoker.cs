using Shinobytes.XzaarScript.VM;

namespace Shinobytes.Bytecode.Puzzles
{
    public class XzaarScriptCodeInvoker : ICodeInvoker
    {
        private readonly Runtime runtime;

        protected XzaarScriptCodeInvoker(Runtime runtime)
        {
            this.runtime = runtime;
        }

        /// <summary>
        /// Gets the name of the function we want to invoke, this can be overridden incase the default name 'puzzle' does not fit the criteria
        /// </summary>
        protected virtual string FunctionName { get { return "puzzle"; } }

        /// <summary>
        /// Invokes the script function using the provided input parameters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public object InvokePuzzleFunction(params object[] input)
        {
            return this.runtime.Invoke<object>(FunctionName, input);
        }
    }
}