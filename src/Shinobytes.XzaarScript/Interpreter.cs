/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Extensions;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript
{
    public class Interpreter
    {
        private readonly RuntimeSettings settings;
        private readonly Runtime runtime;
        private readonly InterpreterErrorCollection errors;

        public Interpreter(string scriptCode, RuntimeSettings settings = null)
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

        public void Interrupt()
        {
            this.runtime.Interrupt();
        }

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
            var result = inputCode
                .Tokenize(out lexerErrors)
                .Parse(out parserErrors)
                .CompileExpression()
                .Compile(out compilerErrors);

            return result;
        }

        public static XzaarAssembly Compile(string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .CompileExpression()
                .Compile();
        }
        #endregion
    }
}
