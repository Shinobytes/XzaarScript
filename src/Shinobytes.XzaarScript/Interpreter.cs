/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
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
