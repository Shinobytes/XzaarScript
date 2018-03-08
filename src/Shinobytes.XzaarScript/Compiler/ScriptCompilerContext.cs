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
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public class ScriptCompilerContext
    {
        internal CompilerInstructionCollection GlobalInstructions;
        internal CompilerInstructionCollection MethodInstructions;

        internal List<VariableReference> VariableReferences = new List<VariableReference>();
        internal List<VariableReference> GlobalTempVariables = new List<VariableReference>();
        private readonly FlowControlScope rootFlowControl;

        private FlowControlScope flowControl;
        // private int tempVariableCount;

        public ScriptCompilerContext(string assemblyName, XzaarExpression expressionTree)
        {
            MethodInstructions = new CompilerInstructionCollection(this);
            GlobalInstructions = new CompilerInstructionCollection(this);

            this.ExpressionTree = expressionTree;
            this.KnownTypes = new List<TypeReference>();
            this.Assembly = XzaarAssembly.CreateAssembly(assemblyName);
            this.flowControl = new FlowControlScope(null, -1);
            this.rootFlowControl = this.flowControl;
        }

        public XzaarExpression ExpressionTree { get; }

        public string CurrentFunctioName;

        public XzaarExpression LastVisited;

        internal bool RecordVariableReferencesOnly { get; set; }

        public XzaarAssembly Assembly { get; }

        public object CurrentType { get; set; }

        public List<TypeReference> KnownTypes { get; }

        public int TempVariableCount { get; set; }

        public Label CurrentLoopEndLabel => flowControl.CurrentEndLabel;

        public Label CurrentLoopStartLabel => flowControl.CurrentStartLabel;

        public bool IsInGlobalScope => rootFlowControl == flowControl || CurrentFunctioName == null;

        public int StackRecursionCount { get; set; }

        public int InstructionCount => IsInGlobalScope ? this.GlobalInstructions.Count : this.MethodInstructions.Count;

        public void BeginControlBlock()
        {
            this.flowControl = this.flowControl.BeginControlBlock();
        }

        public void EndControlBlock()
        {
            this.flowControl = this.flowControl.EndControlBlock();
        }

        public void InsertInstruction(int index, Operation instruction)
        {
            if (this.IsInGlobalScope)
            {
                this.GlobalInstructions.Insert(index, instruction);
            }
            else
            {
                this.MethodInstructions.Insert(index, instruction);
            }
        }
        public void AddInstruction(Operation instruction)
        {
            if (this.IsInGlobalScope)
            {
                this.GlobalInstructions.Add(instruction);
            }
            else
            {
                this.MethodInstructions.Add(instruction);
            }
        }

        public VariableReference FindVariable(string name)
        {
            var variable = this.VariableReferences.FirstOrDefault(v => v.Name == name);
            if (variable != null) return variable;
            variable = this.GlobalTempVariables.FirstOrDefault(v => v.Name == name);
            if (variable != null) return variable;
            variable = this.Assembly.GlobalVariables.FirstOrDefault(v => v.Name == name);
            if (variable != null) return variable;
            var func = this.Assembly.GlobalMethods.FirstOrDefault(gv => gv.Name == this.CurrentFunctioName);
            if (func != null)
            {
                return func.Body.MethodVariables.FirstOrDefault(v => v.Name == name);
            }
            return null;
        }

        public VariableReference TempVariable(int value)
        {

            var tempVar = new VariableReference
            {
                Name = "::temp_number_var" + TempVariableCount++,
                Type = new TypeReference(XzaarBaseTypes.Number)
            };
            if (IsInGlobalScope) GlobalTempVariables.Add(tempVar);
            VariableReferences.Add(tempVar);
            AddInstruction(Instruction.Create(OpCode.Assign, tempVar, Constant(value)));
            return tempVar;
        }

        public VariableReference TempVariable(TypeReference type)
        {
            var tempVar = new VariableReference
            {
                Name = "::temp_" + type.Name + "_var" + TempVariableCount++,
                Type = type
            };
            if (IsInGlobalScope) GlobalTempVariables.Add(tempVar);
            VariableReferences.Add(tempVar);
            return tempVar;
        }

        public VariableReference TempVariable(XzaarType type)
        {
            return TempVariable(new TypeReference(type));
        }


        public VariableReference TempVariable(TypeReference type, VariableReference reference)
        {
            var tempVar = new VariableReference
            {
                Name = "::temp_" + type.Name + "_var" + TempVariableCount++,
                Type = type,
                Reference = reference
            };
            if (IsInGlobalScope) GlobalTempVariables.Add(tempVar);
            VariableReferences.Add(tempVar);
            return tempVar;
        }

        public VariableReference TempVariable(ConstantExpression value)
        {
            var tempVar = new VariableReference
            {
                InitialValue = value.Value,
                Name = "::temp_" + value.Type + "_var" + TempVariableCount++,
                Type = new TypeReference(value.Type)
            };
            if (IsInGlobalScope) GlobalTempVariables.Add(tempVar);
            VariableReferences.Add(tempVar);
            return tempVar;
        }

        public VariableReference Constant(int value)
        {
            var c = new Constant
            {
                Type = new TypeReference(XzaarBaseTypes.Number),
                Value = value,
            };
            return c;
        }

        public VariableReference Constant(string value)
        {
            var c = new Constant
            {
                Type = new TypeReference(XzaarBaseTypes.String),
                Value = value,
            };
            return c;
        }

        public VariableReference Constant(bool value)
        {
            var c = new Constant
            {
                Type = new TypeReference(XzaarBaseTypes.Boolean),
                Value = value,
            };
            return c;
        }
    }
}