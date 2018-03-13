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

namespace Shinobytes.XzaarScript.VM
{
    public class Runtime
    {
        private readonly XzaarAssembly asm;
        private readonly RuntimeSettings settings;
        private readonly VirtualMachine vm;
        private readonly List<RuntimeVariable> globalVariables = new List<RuntimeVariable>();

        private RuntimeScope currentScope;
        private RuntimeScope globalScope;
        private RuntimeScope lastScope;
        private bool interrupted;

        internal Runtime(VirtualMachine vm, XzaarAssembly asm, RuntimeSettings settings)
        {
            this.vm = vm;
            this.asm = asm;
            this.settings = settings ?? RuntimeSettings.Default;
            this.Init();
        }

        private void Init()
        {
            this.SetupRuntimeVariables();
            this.SetupGlobalScope();
        }

        public Runtime Run()
        {
            return Run(RuntimeStepType.Complete);
        }

        public Runtime Run(RuntimeStepType stepType)
        {
            if (stepType == RuntimeStepType.Complete)
            {
                RunToEnd();
            }
            else if (stepType == RuntimeStepType.StepByStep)
            {
                RunNextStep();
            }
            else
            {
                throw new NotImplementedException();
            }

            return this;
        }

        // public IReadOnlyList<RuntimeVariable> Variables { get { return CurrentScope.GetVariables(); } }

        public void Invoke(params object[] args)
        {
            var invokeVars = args.Select((arg, index)
                => new RuntimeVariable(this, new VariableReference { Name = "$" + index, Type = new TypeReference(XzaarBaseTypes.Any) }, arg)).ToList();

            globalScope.AddVariables(invokeVars.ToArray());

            this.Run(RuntimeStepType.Complete);
        }

        public T Invoke<T>(string functionName, params object[] args)
        {
            var result = vm.Invoke(functionName, this, args);
            try
            {
                if (typeof(T) == typeof(object))
                {
                    return (T)result;
                }
                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public T GetVariableValue<T>(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var v = this.FindVariable(name); // this.Variables.FirstOrDefault(n => n.Name == name);
            if (v == null) throw new NullReferenceException();
            if (v.Value == null) return default(T);
            try
            {
                if (typeof(T) == typeof(object))
                {
                    return (T)v.Value;
                }

                return (T)Convert.ChangeType(v.Value, typeof(T));
                // allow casting from ex. double to int directly here.

            }
            catch
            {
                return default(T);
            }
        }

        internal RuntimeScope CurrentScope => currentScope ?? globalScope;

        internal RuntimeScope LastScope => lastScope;

        public RuntimeSettings Settings => settings;

        public RuntimeVariable FindVariable(string name)
        {
            return CurrentScope.FindVariable(name);
        }

        public MethodDefinition FindMethod(string functionName)
        {
            return asm.GlobalMethods.FirstOrDefault(f => f.Name == functionName);
        }

        internal void BeginScope()
        {
            this.currentScope = this.CurrentScope.BeginScope();
        }

        internal void EndScope(object result = null)
        {
            this.lastScope = this.CurrentScope;
            this.currentScope = this.CurrentScope.EndScope(result);
        }

        private void SetupGlobalScope()
        {
            this.globalScope = new RuntimeScope(null, -1);
            this.globalScope.AddVariables(this.globalVariables.ToArray());
            this.globalScope.SetOperations(this.asm.GlobalInstructions);
            this.currentScope = globalScope;
        }

        private void SetupRuntimeVariables()
        {
            foreach (var v in this.asm.GlobalVariables)
            {
                this.globalVariables.Add(new RuntimeVariable(this, v));
            }
        }

        private bool RunNextStep()
        {
            // return true meaning we have more stuff we can do            
            return this.vm.Execute(this, this.CurrentScope.GetOperations());
        }

        private void RunToEnd()
        {
            this.interrupted = false;
            while (!this.interrupted && RunNextStep()) { }

            this.currentScope = null;
            this.globalScope.Position = 0;

            vm.SetRunningState(false);
        }

        public void RegisterGlobalVariable<T>(string variableName, T variableInstance)
        {
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;
            globalScope.AddVariables(new[] { new RuntimeVariable(this, new VariableReference { Name = variableName, Type = new TypeReference(XzaarBaseTypes.Any) }, variableInstance) });
        }

        public void Interrupt()
        {
            this.interrupted = true;
        }
    }
}