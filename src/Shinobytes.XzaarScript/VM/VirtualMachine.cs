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
