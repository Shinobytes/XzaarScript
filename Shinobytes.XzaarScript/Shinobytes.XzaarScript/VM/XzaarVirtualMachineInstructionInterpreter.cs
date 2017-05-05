﻿using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly.Models;
using MemberTypes = Shinobytes.XzaarScript.Assembly.Models.MemberTypes;

namespace Shinobytes.XzaarScript.VM
{
    public class XzaarVirtualMachineInstructionInterpreter
    {
        private readonly XzaarVirtualMachine vm;
        private readonly XzaarReflectionCache reflectionCache;

        public XzaarVirtualMachineInstructionInterpreter(XzaarVirtualMachine vm)
        {
            this.vm = vm;
            this.reflectionCache = new XzaarReflectionCache(this, vm);
        }

        internal bool Execute(XzaarRuntime rt, XzaarBinaryCode op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));
            var instruction = op as Instruction;
            if (instruction != null)
            {
                switch (instruction.OpCode)
                {
                    case OpCode.Assign:
                        AssignValue(rt, instruction);
                        break;
                    case OpCode.Div:
                    case OpCode.Mul:
                    case OpCode.Add:
                    case OpCode.Mod:
                    case OpCode.Sub:
                        ArithmeticValue(rt, instruction, instruction.OpCode);
                        break;
                    case OpCode.Callglobal:
                        CallGlobal(rt, instruction);
                        break;
                    case OpCode.Callmethod:
                        CallMethod(rt, instruction);
                        break;
                    case OpCode.Callextern:
                        CallExtern(rt, instruction);
                        break;
                    case OpCode.Jmpt:
                        JumpIfTrue(rt, instruction);
                        return true;

                    case OpCode.Jmpf:
                        JumpIfFalse(rt, instruction);
                        return true;

                    case OpCode.Jmp:
                        Jump(rt, instruction);
                        return true;

                    case OpCode.Return:
                        ReturnValue(rt, instruction);
                        return false;

                    case OpCode.ArrayClearElements:
                        ClearArrayValues(rt, instruction);
                        break;

                    case OpCode.ArrayCreate:
                        CreateArrayValue(rt, instruction);
                        break;
                    case OpCode.ArraySetElement:
                        SetArrayElementValue(rt, instruction);
                        break;

                    case OpCode.ArrayGetElement:
                        GetArrayElementValue(rt, instruction);
                        break;

                    case OpCode.ArrayAddElements:
                        AddArrayElements(rt, instruction);
                        break;

                    case OpCode.ArrayLength:
                        ArrayLength(rt, instruction);
                        break;

                    case OpCode.ArrayIndexOf:
                        ArrayIndexOf(rt, instruction);
                        break;

                    case OpCode.ArrayRemoveElements:
                        ArrayRemoveElements(rt, instruction);
                        break;
                    case OpCode.ArrayInsertElement:
                        ArrayInsertElement(rt, instruction);
                        break;
                    case OpCode.Not:
                        NotValue(rt, instruction);
                        break;
                    case OpCode.Neg:
                        NegateValue(rt, instruction);
                        break;
                    case OpCode.CmpNotEq:
                    case OpCode.CmpGt:
                    case OpCode.CmpGte:
                    case OpCode.CmpLt:
                    case OpCode.CmpLte:
                    case OpCode.CmpEq:
                        Compare(rt, instruction);
                        break;
                    case OpCode.StructCreate:
                        CreateStructInstance(rt, instruction);
                        break;
                    case OpCode.StructGet:
                        GetStructFieldValue(rt, instruction);
                        break;
                    case OpCode.StructSet:
                        SetStructFieldValue(rt, instruction);
                        break;
                    default:
                        throw new XzaarRuntimeException("Unhandled instruction, OpCode: '" + instruction.OpCode + "'");
                }
            }
            else
            {
                var label = op as Label;
                if (label == null) throw new NullReferenceException(nameof(label));
            }

            rt.CurrentScope.Offset++;

            if (rt.CurrentScope.Offset < rt.CurrentScope.GetOperations().Count)
                return true;

            rt.EndScope();
            return false;
        }

        private void ArrayInsertElement(XzaarRuntime rt, Instruction instruction)
        {
            // i have a feeling that this wont work.
            var instance = GetVariable(rt, instruction, 0);            
            var index = GetValueOf(rt, instruction.OperandArguments[0]);
            var item = GetValueOf(rt, instruction.OperandArguments[1]);
            instance.InsertToArrray(index, item);
        }

        private void ArrayRemoveElements(XzaarRuntime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            var index = GetValueOf(rt, instruction.OperandArguments[0]);
            instance.RemoveFromArray(index);
        }

        private void ArrayIndexOf(XzaarRuntime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            var target = GetVariable(rt, instruction, 1);
            var varRef = GetVariableFromOperand(rt, instruction, 0);
            var valueComparison = varRef != null
                ? varRef.Value
                : GetOperandValue(rt, instruction, 0);

            target.SetValue(instance.ArrayIndexOf(valueComparison));
        }


        private void ArrayLength(XzaarRuntime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            var instance = GetVariable(rt, instruction, 1);
            target.SetValue(instance.GetArrayLength());
        }

        private void AddArrayElements(XzaarRuntime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            var value = GetValueOf(rt, instruction.OperandArguments[0]);
            instance.AddToArray(value);
        }

        private void ClearArrayValues(XzaarRuntime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            target.ClearArray();
        }

        private void CreateArrayValue(XzaarRuntime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            target.InitArray();

            // target.SetValue(new XzaarRuntimeObject(rt, new TypeReference(XzaarBaseTypes.Array)));
            // no need to do anything here really.
        }

        private void SetArrayElementValue(XzaarRuntime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            var index = GetArgumentValue(rt, instruction, 1);
            var value = GetArgumentValue(rt, instruction, 2);
            target.SetValue(value, index);
        }

        private void GetArrayElementValue(XzaarRuntime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            var source = GetVariable(rt, instruction, 1);
            var index = GetArgumentValue(rt, instruction, 2);

            target.SetValue(source.GetValue(index));
        }


        private void ReturnValue(XzaarRuntime rt, Instruction instruction)
        {
            object result = null;

            if (instruction.Arguments.Count > 0)
            {
                var arg = instruction.Arguments[0];
                if (arg is Constant)
                    result = GetValueOf(rt, arg as Constant);
                else
                {
                    if (arg != null)
                    {
                        var fieldRef = arg as FieldReference;
                        if (fieldRef != null)
                        {
                            var targetVariable = GetVariable(rt, fieldRef);
                            if (targetVariable != null)
                            {

                                // result = GetValueOf(targetVariable); // GetValueOf(rt, fieldRef);

                                if (fieldRef.ArrayIndex != null)
                                {
                                    result = GetValueOf(targetVariable, fieldRef.ArrayIndex);
                                }
                                else
                                {
                                    result = GetValueOf(targetVariable);
                                }

                                //if (fieldRef.ArrayIndex != null)
                                //{
                                //    result = targetVariable.GetValue(0);
                                //}
                                //else
                                //{
                                //    result = targetVariable.Value;
                                //}
                            }
                            else
                            {
                                var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
                                TryGetClrObject(instanceVariable, fieldRef, out result);
                            }
                        }
                        else
                        {
                            var v = rt.FindVariable(arg.Name);
                            if (v != null)
                            {
                                // result = GetValueOf(rt, arg);


                                var varRef = arg as VariableReference;
                                if (varRef != null && varRef.ArrayIndex != null)
                                {
                                    result = GetValueOf(v, varRef.ArrayIndex);
                                }
                                else
                                {
                                    result = GetValueOf(v);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new XzaarRuntimeException();
                    }
                }
                // result = instruction.Arguments[0];
            }
            rt.CurrentScope.Offset++;
            rt.EndScope(result);
        }

        private void CallExtern(XzaarRuntime rt, Instruction instruction)
        {
            throw new XzaarRuntimeException();
        }

        private void CallMethod(XzaarRuntime rt, Instruction instruction)
        {
            var method = instruction.Arguments[0] as MethodDefinition;
            if (method == null)
            {
                CallManagedMethod(rt, instruction);
            }
            else
            {
                throw new XzaarRuntimeException();
                //var returnVariableRef = instruction.Arguments[1];
                //var targetVariable = rt.FindVariable(returnVariableRef.Name);
                //var args = instruction.OperandArguments;
                //targetVariable.Value = InvokeMethod(rt, method, GetFunctionArgs(rt, args));
            }
        }

        private void CallManagedMethod(XzaarRuntime rt, Instruction instruction)
        {
            var methodRef = instruction.Arguments[0] as VariableReference;
            var methodName = methodRef.Name;
            var instanceVariable = GetVariable(rt, instruction, 1);
            var targetVar = GetVariable(rt, instruction, 2);

            if (instanceVariable == null) throw new XzaarRuntimeException();
            if (instanceVariable.Value == null)
                throw new XzaarRuntimeException(instanceVariable.Name + " has not been initialized before use.");

            var instance = GetValueRecursive(instanceVariable);

            // var v = instance.Value;
            var type = instance.GetType();

            var method = reflectionCache.GetMethod(type, methodName);
            if (method == null)
                throw new XzaarRuntimeException("Target function: '" + methodName + "' could not be found.");

            var args = instruction.OperandArguments;
            var funcArgs = GetFunctionArgs(rt, args);

            var parameters = funcArgs.ToArray();
            targetVar.SetValue(method.Invoke(instance, parameters));
        }

        private void CallGlobal(XzaarRuntime rt, Instruction instruction)
        {
            var method = instruction.Arguments[0] as MethodDefinition;
            if (method == null || method.IsExtern)
                throw new XzaarRuntimeException("Function needs to be defined in the script its being used by. External functions not implemented yet");

            var returnVariableRef = instruction.Arguments[1];
            var targetVariable = rt.FindVariable(returnVariableRef.Name);
            var args = instruction.OperandArguments;
            var funcArgs = GetFunctionArgs(rt, args);
            var methodResult = InvokeMethod(rt, method, funcArgs);
            targetVariable.SetValue(methodResult);
            // instruction.
        }

        private List<object> GetFunctionArgs(XzaarRuntime rt, InstructionVariableCollection args)
        {
            var funcArgs = new List<object>();
            foreach (var p in args)
            {
                if (p is Constant)
                {
                    funcArgs.Add(GetValueOf(rt, p as Constant));
                }
                else
                {
                    var mref = p as MemberReference;
                    if (mref != null)
                    {
                        var fieldRef = mref as FieldReference;
                        if (fieldRef != null)
                        {
                            var val = GetVariable(rt, fieldRef);
                            if (val == null)
                            {
                                object outVal;
                                if (TryGetClrObject(rt, fieldRef.Instance, fieldRef, out outVal))
                                {
                                    funcArgs.Add(outVal);
                                }
                                else
                                {
                                    var v2 = rt.FindVariable(fieldRef.Instance.Name);
                                    if (v2 != null)
                                    {
                                        // what was I thinking here?
                                    }

                                    throw new XzaarRuntimeException("Failed to grab the argument reference :(");
                                }

                            }
                            else
                            {
                                funcArgs.Add(val);
                            }
                        }
                        else
                        {
                            var v = rt.FindVariable(mref.Name);
                            var varRef = p as VariableReference;
                            object arrayIndex = null;
                            if (varRef != null) arrayIndex = varRef.ArrayIndex;
                            if (v != null)
                            {

                                var varRefRaw = mref as VariableReference;
                                if (varRefRaw != null && varRefRaw.ArrayIndex != null)
                                {
                                    // what was I thinking here?
                                }

                                if (arrayIndex != null)
                                    funcArgs.Add(GetValueOf(v, arrayIndex));
                                else
                                    funcArgs.Add(v);
                            }
                            else
                            {
                                throw new XzaarRuntimeException();
                            }
                        }
                    }
                }
            }

            var finalArgs = new List<object>();
            foreach (var v in funcArgs)
            {
                var value = v;
                var c = value as Constant;
                if (c != null)
                {
                    finalArgs.Add(c.Value);
                }
                else
                {
                    var vref = value as VariableReference;
                    if (vref != null)
                    {
                        value = rt.FindVariable(vref.Name);
                    }

                    var vRef = value as XzaarRuntimeVariable;
                    if (vRef != null)
                    {
                        finalArgs.Add(GetValueOf(vRef));
                    }
                    else
                    {
                        finalArgs.Add(value);
                    }
                }
            }
            return finalArgs;
        }

        internal object Invoke(XzaarRuntime rt, string functionName, object[] args)
        {
            var method = rt.FindMethod(functionName);
            if (method == null) throw new XzaarRuntimeException("Could not found a function with the name '" + functionName + "'.");
            return InvokeMethod(rt, method, args);
        }

        private object InvokeMethod(XzaarRuntime rt, MethodDefinition method, IList<object> args)
        {
            rt.BeginScope();

            var funcArgs = InitMethodLocals(rt, method.Name, method.Body.MethodVariables, method.Parameters, args);
            rt.CurrentScope.AddVariables(funcArgs);
            var ops = method.Body.MethodInstructions;
            rt.CurrentScope.SetOperations(ops);

            if (ops.Count > 0)
            {
                while (Execute(rt, ops[rt.CurrentScope.Offset])) { }
            }
            if (rt.LastScope == null)
            {
                rt.EndScope();
            }
            return rt.LastScope.Result;
        }

        private XzaarRuntimeVariable[] InitMethodLocals(XzaarRuntime rt, string methodName, MethodVariableCollection variables, ParameterCollection parameters, IList<object> arguments)
        {
            if (parameters.Count != arguments.Count) throw new XzaarRuntimeException("The function '" + methodName + "' was invoked with wrong amount of parameters. " + parameters.Count + " parameters are expected but " + arguments.Count + " arguments were supplied.");
            var output = new List<XzaarRuntimeVariable>();
            for (var i = 0; i < parameters.Count; i++)
            {
                output.Add(new XzaarRuntimeVariable(rt, parameters[i], GetValueOf(rt, arguments[i])));
            }

            for (var index = 0; index < variables.Count; index++)
            {
                var varRef = variables[index];
                var tar = rt.FindVariable(varRef.Name);
                if (tar == null)
                {
                    output.Add(new XzaarRuntimeVariable(rt, varRef));
                }
            }

            return output.ToArray();
        }

        //private IEnumerable<XzaarRuntimeVariable> InitMethodLocals(MethodVariableCollection variables)
        //{
        //    return variables.Select(v => new XzaarRuntimeVariable(v)).ToList();
        //}


        private void JumpIfTrue(XzaarRuntime rt, Instruction instruction)
        {
            var booleanValue = GetArgumentValue(rt, instruction, 0);
            if (booleanValue is bool)
            {
                if ((bool)booleanValue) Jump(rt, instruction);
                else
                {
                    rt.CurrentScope.Offset++;
                }
            }
            else
            {
                rt.CurrentScope.Offset++;
            }
        }

        private void JumpIfFalse(XzaarRuntime rt, Instruction instruction)
        {
            var booleanValue = GetArgumentValue(rt, instruction, 0);
            if (booleanValue is bool)
            {
                if (!(bool)booleanValue) Jump(rt, instruction);
                else
                {
                    rt.CurrentScope.Offset++;
                }
            }
            else
            {
                rt.CurrentScope.Offset++;
            }
        }

        private void Jump(XzaarRuntime rt, Instruction instruction)
        {
            rt.CurrentScope.Offset = instruction.TargetLabel.Offset;
        }


        private bool TrySetClrObject(Instruction instruction, XzaarRuntimeVariable instance, object value)
        {
            var fieldRef = instruction.Arguments[1] as VariableReference;
            if (fieldRef == null) return false;

            if (instance.Value == null)
            {
                throw new XzaarRuntimeException("The field '" + fieldRef.Name + "' has not been initalized before use. It's pretty much null.");
            }

            try
            {
                var type = instance.Value.GetType();
                var field = reflectionCache.GetField(type, fieldRef.Name);
                if (field != null)
                {
                    field.SetValue(instance.Value, value);
                    return true;
                }

                var property = reflectionCache.GetProperty(type, fieldRef.Name);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(instance.Value, value, null);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private void SetStructFieldValue(XzaarRuntime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            object value = null;

            if (instruction.Arguments.Count == 3)
            {
                value = GetArgumentValue(rt, instruction, 2);
            }

            if (instruction.Arguments.Count == 4)
            {
                value = GetArgumentValue(rt, instruction, 3);
            }

            if (instance.Value != null && !(instance.Value is XzaarRuntimeVariable))
            {
                if (TrySetClrObject(instruction, instance, value))
                {
                    return;
                }
            }
            var field = GetVariable(rt, instruction, 1);


            if (instruction.Arguments.Count == 4)
            {
                var arrayIndex = GetArgumentValue(rt, instruction, 2);
                field.SetValue(value, arrayIndex);
            }
            else
            {
                field.SetValue(value);
            }
        }

        private void GetStructFieldValue(XzaarRuntime rt, Instruction instruction)
        {
            object clr;
            var variable = GetVariable(rt, instruction, 0);
            var instanceVariable = GetVariable(rt, instruction, 1);
            var fieldVariable = instruction.Arguments[2] as VariableReference;



            if (instanceVariable == null)
            {
                var instanceValue = GetArgumentValue(rt, instruction, 1);
                if (TryGetClrObject(rt, instanceValue, fieldVariable, out clr))
                {
                    variable.SetValue(clr);
                    return;
                }
                if (instanceValue != null)
                {
                    throw new XzaarRuntimeException("'" + instanceValue + "' does not have a property named '" + fieldVariable.Name + "'");
                }
            }

            // dereference the instance variable by searching for a XzaarRuntimeObject,


            if (TryGetClrObject(instanceVariable, fieldVariable, out clr))
            {
                variable.SetValue(clr);
                return;
            }



            var obj = FindRuntimeObject(instanceVariable);
            if (obj == null)
            {
                var fvar = rt.FindVariable(fieldVariable.Name);
                if (fvar != null)
                {
                    variable.SetValue(instruction.Arguments.Count == 4
                        ? fvar.GetValue(GetArgumentValue(rt, instruction, 3))
                        : fvar.Value);
                    return;
                }
                throw new XzaarRuntimeException(instanceVariable.Name + " has not been initialized before use.");
            }

            if (instruction.Arguments.Count == 4)
            {
                variable.SetValue(obj.GetFieldValue(fieldVariable.Name, GetArgumentValue(rt, instruction, 3)));
            }
            else
            {
                variable.SetValue(obj.GetFieldValue(fieldVariable.Name));
            }
        }
        private bool TryGetClrObject(XzaarRuntime rt, object instanceVariable, VariableReference fieldVariable, out object clr)
        {
            clr = null;

            var vRef = instanceVariable as VariableReference;

            if (instanceVariable == null || fieldVariable == null)
                return false;

            var v = instanceVariable;
            if (vRef != null && rt != null)
            {
                var varVar = rt.FindVariable(vRef.Name);
                v = varVar.Value;
            }

            if (vRef != null && vRef.InitialValue != null)
            {
                // when a variable reference has a initial value then its 99.9999% guaranteed to be a constant, and when its a constant then the object will be 
                // a managed type, s owe can just take the inital value as target object and then execute whatever .net field exists on that item.
                v = vRef.InitialValue;
            }

            try
            {
                var type = v.GetType();
                if (type.IsPrimitive)
                {
                    clr = v;
                    return true;
                }

                var field = reflectionCache.GetField(type, fieldVariable.Name);
                if (field != null)
                {
                    clr = field.GetValue(v);
                    return true;
                }

                var property = reflectionCache.GetProperty(type, fieldVariable.Name);
                if (property != null && property.CanRead)
                {
                    clr = property.GetValue(v, null);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private bool TryGetClrObject(XzaarRuntimeVariable instanceVariable, VariableReference fieldVariable, out object clr)
        {
            clr = null;

            if (instanceVariable == null || fieldVariable == null || instanceVariable.Value == null)
                return false;

            var v = instanceVariable.Value;
            return TryGetClrObject(instanceVariable.Runtime, v, fieldVariable, out clr);
        }

        private XzaarRuntimeObject FindRuntimeObject(XzaarRuntimeVariable instanceVariable)
        {
            if (instanceVariable == null) return null;
            if (instanceVariable.Value is XzaarRuntimeObject) return (XzaarRuntimeObject)instanceVariable.Value;
            if (instanceVariable.Value is XzaarRuntimeVariable)
            {
                object tmp = instanceVariable.Value;
                while (tmp is XzaarRuntimeVariable)
                {
                    tmp = (tmp as XzaarRuntimeVariable).Value;
                }
                var obj = tmp as XzaarRuntimeObject;
                if (obj != null) return obj;
            }

            return null;
        }

        private void CreateStructInstance(XzaarRuntime rt, Instruction instruction)
        {
            var variable = GetVariable(rt, instruction, 0);
            var targetType = instruction.Arguments[1] as TypeReference;
            variable.SetValue(new XzaarRuntimeObject(rt, targetType));
        }

        private void NotValue(XzaarRuntime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueReference = GetVariable(rt, instruction, 1);
            var value = valueReference.GetValue(0);
            targetVariable.SetValue(!bool.Parse(value + ""));
        }

        private void ModValue(XzaarRuntime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueReference = GetVariable(rt, instruction, 1);
            var value = valueReference.GetValue(0);
            targetVariable.SetValue(-double.Parse(value + ""));
        }

        private void NegateValue(XzaarRuntime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueReference = GetVariable(rt, instruction, 1);
            var value = valueReference.GetValue(0);
            targetVariable.SetValue(-double.Parse(value + ""));
        }

        private void Compare(XzaarRuntime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueLeft = GetArgumentValue(rt, instruction, 1);
            var valueRight = GetArgumentValue(rt, instruction, 2);
            targetVariable.SetValue(EqualityCondition(instruction.OpCode, valueLeft, valueRight));
        }

        private void ArithmeticValue(XzaarRuntime rt, Instruction instruction, OpCode opcode)
        {
            // add target value1 value2
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueLeft = GetArgumentValue(rt, instruction, 1);
            var valueRight = GetArgumentValue(rt, instruction, 2);

            targetVariable.SetValue(Arithmetic((ArithmeticOperation)opcode, valueLeft, valueRight));
        }

        private object GetArgumentValue(XzaarRuntime rt, Instruction instruction, int index)
        {
            return GetValueOf(rt, instruction.Arguments[index]);
        }


        private object GetOperandValue(XzaarRuntime rt, Instruction instruction, int index)
        {
            return GetValueOf(rt, instruction.OperandArguments[index]);
        }

        private XzaarRuntimeVariable GetVariableFromOperand(XzaarRuntime rt, Instruction instruction, int index)
        {
            var targetVariableRef = instruction.OperandArguments[index];
            // check if the variable is a field from a struct or other type
            var fieldRef = targetVariableRef as FieldReference;
            if (fieldRef != null)
            {
                var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
                var instanceObject = FindRuntimeObject(instanceVariable);
                return instanceObject.GetField(fieldRef.Name);
            }

            if (targetVariableRef.MemberType == MemberTypes.Constant)
            {
                return null;
            }
            else
            {
                var targetVariable = rt.FindVariable(targetVariableRef.Name);
                AssertVariableFound(targetVariable, targetVariableRef.Name);
                return targetVariable;
            }
        }



        private XzaarRuntimeVariable GetVariable(XzaarRuntime rt, Instruction instruction, int index)
        {
            var targetVariableRef = instruction.Arguments[index];
            // check if the variable is a field from a struct or other type
            var fieldRef = targetVariableRef as FieldReference;
            if (fieldRef != null)
            {
                var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
                var instanceObject = FindRuntimeObject(instanceVariable);
                return instanceObject.GetField(fieldRef.Name);
            }

            if (targetVariableRef.MemberType == MemberTypes.Constant)
            {
                return null;
            }
            else
            {
                var targetVariable = rt.FindVariable(targetVariableRef.Name);
                AssertVariableFound(targetVariable, targetVariableRef.Name);
                return targetVariable;
            }
        }


        private XzaarRuntimeVariable GetVariable(XzaarRuntime rt, FieldReference fieldRef)
        {
            var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
            var instanceObject = FindRuntimeObject(instanceVariable);
            if (instanceObject == null)
            {
                return null;
            }
            return instanceObject.GetField(fieldRef.Name);
        }

        private object EqualityCondition(OpCode op, object left, object right)
        {
            switch (op)
            {
                case OpCode.CmpEq:
                    {
                        if (left == null || right == null) return false;
                        var isEqual = left == right || left.Equals(right) || (Equals((object)left, (object)right));

                        if (!isEqual)
                        {
                            if (IsNumber(left) && IsNumber(right))
                            {
                                try
                                {
                                    return Math.Abs(Number(left) - Number(right)) < 0.0000001;
                                }
                                catch
                                {
                                    return false;
                                }
                            }
                        }
                        return isEqual;
                    }
                case OpCode.CmpNotEq:
                    return !(bool)EqualityCondition(OpCode.CmpEq, left, right);
                case OpCode.CmpGt:
                    return Number(left) > Number(right);
                case OpCode.CmpGte:
                    return Number(left) >= Number(right);
                case OpCode.CmpLt:
                    return Number(left) < Number(right);
                case OpCode.CmpLte:
                    return Number(left) <= Number(right);
            }
            return null;
        }

        private bool IsNumber(object node)
        {
            return node is decimal || node is double || node is int || node is float || node is long || node is byte ||
                   node is short || node is uint || node is ulong || node is sbyte || node is ushort;
        }

        private object Arithmetic(ArithmeticOperation op, object valueLeft, object valueRight)
        {
            switch (op)
            {
                case ArithmeticOperation.Add:
                    {
                        if (valueLeft is string || valueRight is string)
                            return valueLeft + "" + valueRight;
                        return Number(valueLeft) + Number(valueRight);
                    }
                case ArithmeticOperation.Div:
                    return Number(valueLeft) / Number(valueRight);
                case ArithmeticOperation.Mod:
                    return Number(valueLeft) % Number(valueRight);
                case ArithmeticOperation.Mul:
                    return Number(valueLeft) * Number(valueRight);
                case ArithmeticOperation.Sub:
                    return Number(valueLeft) - Number(valueRight);
            }
            return null;
        }

        private double Number(object value)
        {
            // return double.Parse(value == null ? "0" : value + "");
            try
            {
                return (double)Convert.ChangeType(value, typeof(double));
            }
            catch
            {
                return -1d;
            }
        }

        private void AssignValue(XzaarRuntime rt, Instruction instruction)
        {
            // assign target value
            var targetVariableRef = instruction.Arguments[0];
            var targetVariable = rt.FindVariable(targetVariableRef.Name);

            var valueRef = instruction.Arguments[1];
            AssertVariableFound(targetVariable, targetVariableRef.Name);
            var value = GetValueOf(rt, valueRef);
            if (value == null)
            {
                var varRef = GetVariable(rt, instruction, 1);
                value = GetValueOf(varRef);
            }
            // AssertSameType()
            if (value != null)
                targetVariable.SetValue(value);
        }


        private object GetValueOf(XzaarRuntime rt, object value)
        {
            if (value is object[])
            {
                var array = (object[])value;
                var newArray = new List<object>();
                foreach (var item in array)
                {
                    newArray.Add(GetValueOf(rt, item));
                }
                return newArray.ToArray();
            }

            var cval = value as Constant;
            if (cval != null)
            {
                return GetValueOf(rt, cval);
            }
            var vval = value as VariableReference;
            if (vval != null)
            {
                return GetValueOf(rt, vval);
            }
            var rtv = value as XzaarRuntimeVariable;
            if (rtv != null)
            {
                return rtv.Value;
            }
            return value;
        }

        private object GetValueOf(XzaarRuntime rt, MemberReference mRef)
        {
            var c = mRef as Constant;
            if (c != null)
            {
                return GetValueOf(rt, c);
            }

            var v = mRef as VariableReference;
            if (v != null)
            {
                var variable = rt.FindVariable(v.Name);
                if (variable != null)
                {
                    if (v.ArrayIndex != null) return GetValueOf(variable, v.ArrayIndex);
                    return GetValueOf(variable);
                }
            }

            return null;
        }
        private object GetValueOf(XzaarRuntimeVariable v, object arrayIndex)
        {
            if (arrayIndex == null) return v.Value;
            var ai = GetValueOf(v.Runtime, arrayIndex as MemberReference);
            return v.GetValue(ai);
        }

        private object GetValueRecursive(XzaarRuntimeVariable v)
        {
            object vref = v;
            while (vref is XzaarRuntimeVariable)
            {
                var b = vref as XzaarRuntimeVariable;
                vref = b.Value;
            }
            return vref;
        }

        private object GetValueOf(XzaarRuntimeVariable v)
        {
            return v.Value;
        }

        private object GetValueOf(XzaarRuntime rt, Constant c)
        {
            if (c.ArrayIndex != null)
            {
                try
                {
                    var index = int.Parse((GetValueOf(rt, c.ArrayIndex as MemberReference) ?? "0").ToString());
                    switch (c.Type.Name)
                    {
                        case "string": return (c.Value + "")[index];
                        case "number":
                            {
                                // TODO: return bit
                            }
                            return c.Value;
                    }
                }
                catch (Exception exc)
                {
                    throw new XzaarRuntimeException(exc.Message);
                }
            }
            return c.Value;
        }

        private void AssertVariableFound(XzaarRuntimeVariable targetVariable, string varName)
        {
            if (targetVariable == null) throw new XzaarRuntimeException("The target variable '" + varName + "' could not be found.");
        }

        internal enum ArithmeticOperation
        {
            Add = 1,
            Sub = 2,
            Mul = 3,
            Div = 4,
            Mod = 5
        }
    }
}