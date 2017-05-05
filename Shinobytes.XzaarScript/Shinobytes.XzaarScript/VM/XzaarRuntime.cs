using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.VM
{
    public class XzaarRuntime
    {
        private readonly XzaarAssembly asm;
        private readonly XzaarVirtualMachine vm;
        private readonly List<XzaarRuntimeVariable> globalVariables = new List<XzaarRuntimeVariable>();
        
        private XzaarRuntimeScope currentScope;
        private XzaarRuntimeScope globalScope;
        private XzaarRuntimeScope lastScope;

        internal XzaarRuntime(XzaarVirtualMachine vm, XzaarAssembly asm)
        {
            this.vm = vm;
            this.asm = asm;
            this.Init();
        }

        private void Init()
        {
            this.InitRuntimeVariables();
            this.InitScope();
        }

        public XzaarRuntime Run()
        {
            return Run(XzaarRuntimeStepType.Complete);
        }

        public XzaarRuntime Run(XzaarRuntimeStepType stepType)
        {
            if (stepType == XzaarRuntimeStepType.Complete)
            {
                RunToEnd();
            }
            else if (stepType == XzaarRuntimeStepType.StepByStep)
            {
                RunNextStep();
            }
            else
            {
                throw new NotImplementedException();
            }

            return this;
        }

      // public IReadOnlyList<XzaarRuntimeVariable> Variables { get { return CurrentScope.GetVariables(); } }

        public void Invoke(params object[] args)
        {
            var invokeVars = args.Select((arg, index)
                => new XzaarRuntimeVariable(this, new VariableReference { Name = "$" + index, Type = new TypeReference(XzaarBaseTypes.Any) }, arg)).ToList();

            globalScope.AddVariables(invokeVars.ToArray());

            this.Run(XzaarRuntimeStepType.Complete);
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

        internal XzaarRuntimeScope CurrentScope { get { return currentScope ?? globalScope; } }

        internal XzaarRuntimeScope LastScope { get { return lastScope; } }

        public XzaarRuntimeVariable FindVariable(string name)
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

        private void InitScope()
        {
            this.globalScope = new XzaarRuntimeScope(null, -1);
            this.globalScope.AddVariables(this.globalVariables.ToArray());
            this.globalScope.SetOperations(this.asm.GlobalInstructions);
            this.currentScope = globalScope;
        }

        private void InitRuntimeVariables()
        {
            foreach (var v in this.asm.GlobalVariables)
            {
                this.globalVariables.Add(new XzaarRuntimeVariable(this, v));
            }
        }

        private bool RunNextStep()
        {
            // return true meaning we have more stuff we can do            
            return this.vm.Execute(this, this.CurrentScope.GetOperations());
        }

        private void RunToEnd()
        {
            while (RunNextStep()) { }

            this.currentScope = null;
            this.globalScope.Offset = 0;

            vm.SetRunningState(false);
        }

        public void RegisterGlobalVariable<T>(string variableName, T variableInstance)
        {
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;
            globalScope.AddVariables(new[] { new XzaarRuntimeVariable(this, new VariableReference { Name = variableName, Type = new TypeReference(XzaarBaseTypes.Any) }, variableInstance) });
        }
    }
}