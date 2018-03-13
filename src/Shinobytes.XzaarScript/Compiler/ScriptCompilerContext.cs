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

        public string CurrentFunctionName;

        public XzaarExpression LastVisited;

        internal bool RecordVariableReferencesOnly { get; set; }

        public XzaarAssembly Assembly { get; }

        public object CurrentType { get; set; }

        public List<TypeReference> KnownTypes { get; }

        public int TempVariableCount { get; set; }

        public Label CurrentLoopEndLabel => flowControl.CurrentEndLabel;

        public Label CurrentLoopStartLabel => flowControl.CurrentStartLabel;

        public bool IsInGlobalScope => rootFlowControl == flowControl || CurrentFunctionName == null;

        public int StackRecursionCount { get; set; }

        public int InstructionCount
        {
            get
            {
                if (InsideAnonymousFunctionCount > 0)
                {
                    return this.CurrentAnonymousFunctionScope.Instructions.Count;
                }

                return IsInGlobalScope
                        ? this.GlobalInstructions.Count
                        : this.MethodInstructions.Count;
            }
        }

        public int InsideAnonymousFunctionCount { get; set; }

        public AnonymousFunctionScope CurrentAnonymousFunctionScope { get; set; }

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
            if (this.InsideAnonymousFunctionCount > 0)
            {
                this.CurrentAnonymousFunctionScope.Instructions.Insert(index, instruction);
            }
            else if (this.IsInGlobalScope)
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
            if (this.InsideAnonymousFunctionCount > 0)
            {
                this.CurrentAnonymousFunctionScope.Instructions.Add(instruction);
            }
            else if (this.IsInGlobalScope)
            {
                this.GlobalInstructions.Add(instruction);
            }
            else
            {
                this.MethodInstructions.Add(instruction);
            }
        }


        private void AddVariable(VariableReference tempVar)
        {
            if (InsideAnonymousFunctionCount > 0)
            {
                this.CurrentAnonymousFunctionScope.Variables.Add(tempVar);
            }
            else if (IsInGlobalScope)
            {
                GlobalTempVariables.Add(tempVar);
            }
            else
            {
                VariableReferences.Add(tempVar);
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
            var func = this.Assembly.GlobalMethods.FirstOrDefault(gv => gv.Name == this.CurrentFunctionName);
            if (func != null)
            {
                var par = func.Parameters.FirstOrDefault(x => x.Name == name);
                return par ?? func.Body.MethodVariables.FirstOrDefault(v => v.Name == name);
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

            AddVariable(tempVar);
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

            AddVariable(tempVar);
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

            AddVariable(tempVar);
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

            AddVariable(tempVar);
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

        public void BeginAnonymousFunctionBlock(ParameterDefinition[] parameters)
        {
            // this.CurrentFunctionName
            InsideAnonymousFunctionCount++;
            var depth = (CurrentAnonymousFunctionScope?.Depth ?? 0) + 1;
            CurrentAnonymousFunctionScope = new AnonymousFunctionScope(this, CurrentAnonymousFunctionScope, parameters, depth);
        }

        public void EndAnonymousFunctionBlock()
        {
            if (CurrentAnonymousFunctionScope == null)
            {
                return;
            }

            CurrentAnonymousFunctionScope = CurrentAnonymousFunctionScope.Parent;
            InsideAnonymousFunctionCount--;
        }
    }

    public class AnonymousFunctionScope
    {
        private readonly ScriptCompilerContext ctx;

        public AnonymousFunctionScope(ScriptCompilerContext ctx, AnonymousFunctionScope parent, ParameterDefinition[] parameters, int depth)
        {
            this.ctx = ctx;
            this.Instructions = new CompilerInstructionCollection(ctx);
            Parent = parent;
            Depth = depth;
            Parameters = new Dictionary<string, ParameterDefinition>();
            foreach (var param in parameters)
            {
                Parameters[param.Name] = param;
            }
        }

        internal CompilerInstructionCollection Instructions;

        internal List<VariableReference> Variables = new List<VariableReference>();

        public VariableReference Find(string name)
        {
            if (this.Parameters.TryGetValue(name, out var param))
            {
                return param;
            }

            return Parent?.Find(name);
        }

        public bool TryFind(string name, out VariableReference param)
        {
            return (param = Find(name)) != null;
        }

        public AnonymousFunctionScope Parent { get; }

        public Dictionary<string, ParameterDefinition> Parameters { get; }

        public int Depth { get; }
    }
}