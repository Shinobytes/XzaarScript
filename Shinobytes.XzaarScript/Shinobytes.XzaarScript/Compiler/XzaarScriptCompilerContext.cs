using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public class CompilerInstructionCollection : Collection<XzaarBinaryCode>
    {
        //private Queue<XzaarBinaryCode> lateAddInstructionQueue = new Queue<XzaarBinaryCode>();
        private XzaarScriptCompilerContext ctx;

        public CompilerInstructionCollection(XzaarScriptCompilerContext ctx)
        {
            this.ctx = ctx;
        }

        public override void Add(XzaarBinaryCode item)
        {
            base.Add(item);
            //var inst = item as Instruction;
            //var label = item as Label;

            //if (lateAddInstructionQueue.Count > 0)
            //{
            //    if (label != null || inst != null && inst.IsJump)
            //    {
            //        while (lateAddInstructionQueue.Count > 0) base.Add(lateAddInstructionQueue.Dequeue());
            //        base.Add(item);
            //    }
            //    else
            //    {
            //        while (lateAddInstructionQueue.Count > 0)
            //        {
            //            var next = lateAddInstructionQueue.Dequeue();
            //            var unary = next as Instruction; // will always be a post unary incrementor or decrementor
            //            var unaryItem = unary.Arguments[0] as VariableReference;

            //            // if the next operation uses one or more references of this unary instruction
            //            // we want to make sure that we increment it afterwards (unless its a return instruction)
            //            if (InstructionContainsReference(inst, unaryItem))
            //            {
            //                // special case on return
            //                // if we intend to return this value, we need to make sure we update the 
            //                // unary before we return, but we still need to make sure we don't return
            //                // the new value
            //                // 
            //                // For post increment:
            //                //  Add val, val, 1
            //                //  Return val                            
            //                // 
            //                // should be:
            //                //  Assign temp, val
            //                //  Add val, val, 1
            //                //  return temp
            //                // 
            //                if (inst.OpCode == OpCode.Return)
            //                {
            //                    var newTarget = ctx.TempVariable(XzaarBaseTypes.Number);
            //                    base.Add(Instruction.Create(OpCode.Assign, newTarget, unaryItem));
            //                    inst.Arguments[0] = newTarget;
            //                    base.Add(next);
            //                    base.Add(item);
            //                }
            //                else if (ComparisonInstruction(inst))
            //                {
            //                    // if the next instruction wants us to do a comparison
            //                    // then we are most likely inside an expression, and then we want to apply the
            //                    // post operation before.
            //                    // example 1:
            //                    //    j++ if ( j > 10 ) { ... }
            //                    //    then we want the j++ to be applied before doing the comparison.

            //                    // TODO: fix post increment inside expressions to only be invoked after the actual expression                                
            //                    // we also want to avoid the following to return true
            //                    //    return a++ > 0
            //                    // the variable 'a' in this case should be incremented after the expression

            //                    base.Add(next);
            //                    base.Add(item);
            //                }
            //                else
            //                {
            //                    // it is being used here, so we want to update afterwards
            //                    base.Add(item);
            //                    base.Add(next);
            //                }
            //            }
            //            else
            //            {
            //                // since we are not using the variable in the next instruction, it is safe to add it before we 
            //                // execute the next operation
            //                base.Add(next);
            //                base.Add(item);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    base.Add(item);
            //}
        }

        //private bool ComparisonInstruction(Instruction inst)
        //{
        //    return inst.OpCode == OpCode.CmpEq || inst.OpCode == OpCode.CmpGt || inst.OpCode == OpCode.CmpLt ||
        //           inst.OpCode == OpCode.CmpGte || inst.OpCode == OpCode.CmpLte;
        //}

        //private bool InstructionContainsReference(Instruction inst, MemberReference unaryItem)
        //{
        //    if (unaryItem == null) return false;
        //    if (inst.Arguments.Count > 0)
        //    {
        //        var arg = inst.Arguments.FirstOrDefault(x => x.Name == unaryItem.Name);
        //        if (arg != null) return true;
        //    }
        //    if (inst.OperandArguments.Count > 0)
        //    {
        //        var oper = inst.OperandArguments.FirstOrDefault(x => x.Name == unaryItem.Name);
        //        if (oper != null) return true;
        //    }
        //    return false;
        //}

        //public void LateAdd(XzaarBinaryCode item)
        //{
        //    lateAddInstructionQueue.Enqueue(item);
        //}

        public override IEnumerator<XzaarBinaryCode> GetEnumerator()
        {
            return this.InternalItems.GetEnumerator();

            //var items = new List<XzaarBinaryCode>();
            //items.AddRange(this.InternalItems);
            //items.AddRange(lateAddInstructionQueue.ToArray());
            //return items.GetEnumerator();
        }
    }

    public class XzaarScriptCompilerContext
    {
        internal CompilerInstructionCollection GlobalInstructions;
        internal CompilerInstructionCollection MethodInstructions;

        internal List<VariableReference> VariableReferences = new List<VariableReference>();
        internal List<VariableReference> GlobalTempVariables = new List<VariableReference>();
        private FlowControlScope rootFlowControl;
        private XzaarAnalyzedTree analyzedTree;
        private XzaarAssembly assembly;
        private object currentType;
        private FlowControlScope flowControl;
        // private int tempVariableCount;



        public XzaarScriptCompilerContext(string assemblyName, XzaarAnalyzedTree analyzedTree)
        {
            MethodInstructions = new CompilerInstructionCollection(this);
            GlobalInstructions = new CompilerInstructionCollection(this);

            this.analyzedTree = analyzedTree;
            this.KnownTypes = new List<TypeReference>();
            this.Assembly = XzaarAssembly.CreateAssembly(assemblyName);
            this.flowControl = new FlowControlScope(null, -1);
            this.rootFlowControl = this.flowControl;
        }

        public XzaarAnalyzedTree AnalyzedTree { get { return analyzedTree; } }

        public string CurrentFunctioName;
        public XzaarExpression lastVisited;

        internal bool RecordVariableReferencesOnly { get; set; }

        public XzaarAssembly Assembly
        {
            get { return assembly; }
            private set { assembly = value; }
        }

        public object CurrentType
        {
            get { return currentType; }
            set { currentType = value; }
        }

        public List<TypeReference> KnownTypes { get; }

        public int TempVariableCount
        {
            // get { return tempVariableCount; }
            get; set;
        } // VariableReferences.Count(v => v.Name.StartsWith("::temp")); } }
        public Label CurrentLoopEndLabel { get { return flowControl.CurrentEndLabel; } }
        public Label CurrentLoopStartLabel { get { return flowControl.CurrentStartLabel; } }

        public bool IsInGlobalScope
        {
            get { return rootFlowControl == flowControl || CurrentFunctioName == null; }
        }

        public int StackRecursionCount { get; set; }

        public int InstructionCount
        {
            get { return IsInGlobalScope ? this.GlobalInstructions.Count : this.MethodInstructions.Count; }
        }

        public void BeginControlBlock()
        {
            this.flowControl = this.flowControl.BeginControlBlock();
        }

        public void EndControlBlock()
        {
            this.flowControl = this.flowControl.EndControlBlock();
        }

        public void InsertInstruction(int index, XzaarBinaryCode instruction)
        {
            if (this.IsInGlobalScope) this.GlobalInstructions.Insert(index, instruction);
            else this.MethodInstructions.Insert(index, instruction);
        }
        public void AddInstruction(XzaarBinaryCode instruction)
        {
            if (this.IsInGlobalScope) this.GlobalInstructions.Add(instruction);
            else this.MethodInstructions.Add(instruction);
        }

        public VariableReference FindVariable(string name)
        {
            var variable = this.VariableReferences.FirstOrDefault(v => v.Name == name);
            if (variable != null) return variable;
            variable = this.GlobalTempVariables.FirstOrDefault(v => v.Name == name);
            if (variable != null) return variable;
            variable = this.assembly.GlobalVariables.FirstOrDefault(v => v.Name == name);
            if (variable != null) return variable;
            var func = this.assembly.GlobalMethods.FirstOrDefault(gv => gv.Name == this.CurrentFunctioName);
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