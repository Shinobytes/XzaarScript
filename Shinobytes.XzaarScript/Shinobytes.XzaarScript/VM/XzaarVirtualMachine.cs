using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.VM
{
    public class XzaarVirtualMachine
    {
        private XzaarVirtualMachineInstructionInterpreter interpreter;
        private readonly XzaarAssembly asm;
        private bool isRunning;
        private bool isDebugging;

        public XzaarVirtualMachine(XzaarAssembly asm)
        {
            this.asm = asm;
            this.interpreter = new XzaarVirtualMachineInstructionInterpreter(this);
        }

        public bool IsRunning { get { return isRunning; } }

        public bool IsDebugging { get { return isDebugging; } }


        public static XzaarRuntime Load(XzaarAssembly asm)
        {
            return new XzaarVirtualMachine(asm).CreateRuntimeInstance();
        }

        public static XzaarRuntime Run(XzaarAssembly asm)
        {
            return new XzaarVirtualMachine(asm).RunNow(XzaarRuntimeStepType.Complete);
        }

        public static XzaarRuntime Debug(XzaarAssembly asm)
        {
            return new XzaarVirtualMachine(asm).RunNow(XzaarRuntimeStepType.StepByStep);
        }

        internal void SetRunningState(bool state)
        {
            this.isRunning = state;
            if (!isRunning) isDebugging = false;
        }

        internal bool Execute(XzaarRuntime rt, IList<XzaarBinaryCode> op)
        {
            if (op == null || rt.CurrentScope.Offset >= op.Count) return false;
            return interpreter.Execute(rt, op[rt.CurrentScope.Offset]);
        }
        internal object Invoke(string functionName, XzaarRuntime rt, object[] args)
        {
            // run all global instructions first.            
            rt.Run(XzaarRuntimeStepType.Complete);
            
            // then invoke our target
            return interpreter.Invoke(rt, functionName, args);
        }

        private XzaarRuntime RunNow(XzaarRuntimeStepType stepType)
        {
            var r = CreateRuntimeInstance();
            isRunning = true;
            isDebugging = stepType == XzaarRuntimeStepType.StepByStep;
            return r.Run(stepType);
        }

        private XzaarRuntime CreateRuntimeInstance()
        {
            return new XzaarRuntime(this, asm);
        }

    }
}
