using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Extensions;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript
{
    public class ScriptInterpreter
    {
        private readonly RuntimeSettings settings;
        private readonly Runtime runtime;
        private readonly InterpreterErrorCollection errors;

        public ScriptInterpreter(string scriptCode, RuntimeSettings settings = null)
        {
            this.settings = settings ?? RuntimeSettings.Default;
            TryLoad(scriptCode, settings, out this.runtime, out errors);
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

        public static bool TryLoad(string inputCode, RuntimeSettings runtimeSettings, out Runtime runtime, out InterpreterErrorCollection errors)
        {
            errors = new InterpreterErrorCollection();
            try
            {
                var assembly = Compile(inputCode, out var lexerErrors, out var parserErrors, out var compilerErrors);

                runtime = assembly.Load(runtimeSettings);

                errors.AddRange(lexerErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Lexer, x)));
                errors.AddRange(parserErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Parser, x)));
                errors.AddRange(compilerErrors.Select(x => new InterpreterError(InterpreterErrorLocation.Compiler, x)));

                return errors.Count == 0;
            }
            catch (Exception exc)
            {
                runtime = null;
                errors.Add(new InterpreterError(InterpreterErrorLocation.Compiler, exc.Message));
                return false;
            }
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
            out IList<string> compilerErrors)
        {
            IList<string> transformerErrors2;
            var result = inputCode
                .Tokenize(out lexerErrors)
                .Parse(out parserErrors)
                .AnalyzeExpression(out transformerErrors2)
                .Compile(out compilerErrors);

            return result;
        }

        public static XzaarAssembly Compile(string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .AnalyzeExpression()
                .Compile();
        }
        #endregion
    }
}
