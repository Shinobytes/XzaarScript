using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Extensions;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript
{
    public class XzaarScriptInterpreter
    {
        private readonly XzaarRuntime runtime;
        private readonly XzaarInterpreterErrorCollection errors;

        public XzaarScriptInterpreter(string scriptCode)
        {
            TryLoad(scriptCode, out this.runtime, out errors);
        }

        public XzaarRuntime Runtime
        {
            get
            {
                return runtime;
            }
        }

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

        public bool HasErrors { get { return errors != null && errors.Count > 0; } }

        public XzaarInterpreterErrorCollection Errors { get { return errors; } }

        #region Quick access functions

        public static bool TryLoad(string inputCode, out XzaarRuntime runtime, out XzaarInterpreterErrorCollection errors)
        {
            errors = new XzaarInterpreterErrorCollection();
            IList<string> lexerErrors;
            IList<string> parserErrors;
            IList<string> transformerErrors;
            List<string> compilerErrors;

            var assembly = Compile(inputCode, out lexerErrors, out parserErrors, out transformerErrors, out compilerErrors);
            runtime = assembly.Load();

            errors.AddRange(lexerErrors.Select(x => new XzaarInterpreterError(XzaarInterpreterErrorLocation.Lexer, x)));
            errors.AddRange(parserErrors.Select(x => new XzaarInterpreterError(XzaarInterpreterErrorLocation.Parser, x)));
            errors.AddRange(transformerErrors.Select(x => new XzaarInterpreterError(XzaarInterpreterErrorLocation.Transformer, x)));
            errors.AddRange(compilerErrors.Select(x => new XzaarInterpreterError(XzaarInterpreterErrorLocation.Compiler, x)));

            return errors.Count == 0;
        }

        public static XzaarRuntime Run(string inputCode)
        {
            return Compile(inputCode).Run();
        }

        public static XzaarRuntime Load(string inputCode)
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
