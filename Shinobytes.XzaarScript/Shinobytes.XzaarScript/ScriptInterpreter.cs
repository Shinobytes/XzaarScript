using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Extensions;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript
{
    public class ScriptInterpreter
    {
        private readonly Runtime runtime;
        private readonly InterpreterErrorCollection errors;

        public ScriptInterpreter(string scriptCode)
        {
            TryLoad(scriptCode, out this.runtime, out errors);
        }

        public Runtime Runtime => runtime;

        public void RegisterVariable<T>(string name, T value)
        {
            this.runtime.RegisterGlobalVariable(name, value);
        }

        public void Run()
        {
            this.runtime.Run();
        }

        //public void Stop()
        //{
        //    this.runtime.
        //}

        public bool HasErrors => errors != null && errors.Count > 0;

        public InterpreterErrorCollection Errors => errors;

        #region Quick access functions

        public static bool TryLoad(string inputCode, out Runtime runtime, out InterpreterErrorCollection errors)
        {
            errors = new InterpreterErrorCollection();
            IList<string> lexerErrors;
            IList<string> parserErrors;
            IList<string> transformerErrors;
            List<string> compilerErrors;

            var assembly = Compile(inputCode, out lexerErrors, out parserErrors, out transformerErrors, out compilerErrors);
            runtime = assembly.Load();

            errors.AddRange(lexerErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Lexer, x)));
            errors.AddRange(parserErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Parser, x)));
            errors.AddRange(transformerErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Transformer, x)));
            errors.AddRange(compilerErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Compiler, x)));

            return errors.Count == 0;
        }

        public static Runtime Run(string inputCode)
        {
            return Compile(inputCode).Run();
        }

        public static Runtime Load(string inputCode)
        {
            return Compile(inputCode).Load();
        }

        public static XzaarAssembly Compile(
            string inputCode,
            out IList<string> lexerErrors,
            out IList<string> parserErrors,
            out IList<string> transformerErrors,
            out List<string> compilerErrors)
        {
            IList<string> transformerErrors2;
            var result = inputCode
                .Tokenize(out lexerErrors)
                .Parse(out parserErrors)
                .Transform(out transformerErrors)
                .AnalyzeExpression(out transformerErrors2)
                .Compile(out compilerErrors);

            foreach (var item in transformerErrors2)
                transformerErrors.Add(item);

            return result;
        }

        public static XzaarAssembly Compile(string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .Transform()
                .AnalyzeExpression()
                .Compile();
        }
        #endregion
    }
}
