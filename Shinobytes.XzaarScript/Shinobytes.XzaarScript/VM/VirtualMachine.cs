using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.XzaarScript.VM
{
    public class VirtualMachine
    {
        private VirtualMachineInstructionInterpreter interpreter;
        private readonly XzaarAssembly asm;
        private bool isRunning;
        private bool isDebugging;

        public VirtualMachine(XzaarAssembly asm)
        {
            this.asm = asm;
            this.interpreter = new VirtualMachineInstructionInterpreter(this);
        }

        public bool IsRunning => isRunning;

        public bool IsDebugging => isDebugging;


        public static Runtime Load(XzaarAssembly asm)
        {
            return new VirtualMachine(asm).CreateRuntimeInstance();
        }

        public static Runtime Run(XzaarAssembly asm)
        {
            return new VirtualMachine(asm).RunNow(RuntimeStepType.Complete);
        }

        public static Runtime Debug(XzaarAssembly asm)
        {
            return new VirtualMachine(asm).RunNow(RuntimeStepType.StepByStep);
        }

        internal void SetRunningState(bool state)
        {
            this.isRunning = state;
            if (!isRunning) isDebugging = false;
        }

        internal bool Execute(Runtime rt, IList<XzaarBinaryCode> op)
        {
            if (op == null || rt.CurrentScope.Offset >= op.Count) return false;
            return interpreter.Execute(rt, op[rt.CurrentScope.Offset]);
        }
        internal object Invoke(string functionName, Runtime rt, object[] args)
        {
            // run all global instructions first.            
            rt.Run(RuntimeStepType.Complete);
            
            // then invoke our target
            return interpreter.Invoke(rt, functionName, args);
        }

        private Runtime RunNow(RuntimeStepType stepType)
        {
            var r = CreateRuntimeInstance();
            isRunning = true;
            isDebugging = stepType == RuntimeStepType.StepByStep;
            return r.Run(stepType);
        }

        private Runtime CreateRuntimeInstance()
        {
            return new Runtime(this, asm);
        }

    }
}
