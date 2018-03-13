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
 
using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly;

namespace Shinobytes.XzaarScript.VM
{
    public class VirtualMachine
    {
        private readonly VirtualMachineInstructionInterpreter interpreter;
        private readonly XzaarAssembly asm;
        private Runtime currentRuntime;
        private bool isRunning;
        private bool isDebugging;

        public VirtualMachine(XzaarAssembly asm)
        {
            this.asm = asm;
            this.interpreter = new VirtualMachineInstructionInterpreter(this);
        }

        public bool IsRunning => isRunning;

        public bool IsDebugging => isDebugging;

        public Runtime Runtime => currentRuntime;

        public static Runtime Load(XzaarAssembly asm, RuntimeSettings settings)
        {
            return new VirtualMachine(asm).CreateRuntimeInstance(settings);
        }

        public static Runtime Run(XzaarAssembly asm)
        {
            return new VirtualMachine(asm).RunImpl(RuntimeStepType.Complete);
        }

        public static Runtime Run(XzaarAssembly asm, RuntimeSettings settings)
        {
            return new VirtualMachine(asm).RunImpl(RuntimeStepType.Complete, settings);
        }

        public static Runtime Debug(XzaarAssembly asm)
        {
            return new VirtualMachine(asm).RunImpl(RuntimeStepType.StepByStep);
        }
        public static Runtime Debug(XzaarAssembly asm, RuntimeSettings settings)
        {
            return new VirtualMachine(asm).RunImpl(RuntimeStepType.StepByStep, settings);
        }

        internal void SetRunningState(bool state)
        {
            this.isRunning = state;
            if (!isRunning) isDebugging = false;
        }

        internal bool Execute(Runtime rt, IList<Operation> op)
        {
            if (op == null || rt.CurrentScope.Position >= op.Count) return false;
            return interpreter.Execute(rt, op[rt.CurrentScope.Position]);
        }
        internal object Invoke(string functionName, Runtime rt, object[] args)
        {
            // run all global instructions first.            
            rt.Run(RuntimeStepType.Complete);

            // then invoke our target
            return interpreter.Invoke(rt, functionName, args);
        }

        private Runtime RunImpl(RuntimeStepType stepType, RuntimeSettings settings = null)
        {
            currentRuntime = CreateRuntimeInstance(settings);
            isRunning = true;
            isDebugging = stepType == RuntimeStepType.StepByStep;
            return currentRuntime.Run(stepType);
        }

        private Runtime CreateRuntimeInstance(RuntimeSettings settings = null)
        {
            return new Runtime(this, asm, settings);
        }

    }
}
