﻿/* 
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
using MemberTypes = Shinobytes.XzaarScript.Assembly.MemberTypes;

namespace Shinobytes.XzaarScript.VM
{
    public class VirtualMachineInstructionInterpreter
    {
        private readonly VirtualMachine vm;
        private readonly ReflectionCache reflectionCache;

        public VirtualMachineInstructionInterpreter(VirtualMachine vm)
        {
            this.vm = vm;
            this.reflectionCache = new ReflectionCache(this, vm);
        }

        internal bool Execute(Runtime rt, Operation op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));
            if (op is Instruction instruction)
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

                    case OpCode.BitwiseAnd:
                    case OpCode.BitwiseOr:
                    case OpCode.BitwiseXor:
                    case OpCode.BitwiseNot:
                    case OpCode.BitwiseRightShift:
                    case OpCode.BitwiseLeftShift:
                        BitwiseValue(rt, instruction, instruction.OpCode);
                        break;

                    case OpCode.And:
                    case OpCode.Or:
                        Conditional(rt, instruction, instruction.OpCode);
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
                    case OpCode.Callanonymous:
                        CallAnonymous(rt, instruction);
                        break;
                    case OpCode.Callunknown:
                        CallUnknown(rt, instruction);
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
                        throw new RuntimeException("Unhandled instruction, OpCode: '" + instruction.OpCode + "'");
                }
            }
            //else
            //{
            //    // will there ever be a null label?? don't think so.
            //    var label = op as Label;
            //    if (label == null) throw new NullReferenceException(nameof(label));
            //}

            rt.CurrentScope.Next();

            if (rt.CurrentScope.Position < rt.CurrentScope.GetOperations().Count)
                return true;

            rt.EndScope();
            return false;
        }

        private void ArrayInsertElement(Runtime rt, Instruction instruction)
        {
            // i have a feeling that this wont work.
            var instance = GetVariable(rt, instruction, 0);
            var index = GetValueOf(rt, instruction.OperandArguments[0]);
            var item = GetValueOf(rt, instruction.OperandArguments[1]);
            instance.InsertToArrray(index, item);
        }

        private void ArrayRemoveElements(Runtime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            var index = GetValueOf(rt, instruction.OperandArguments[0]);
            instance.RemoveFromArray(index);
        }

        private void ArrayIndexOf(Runtime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            var target = GetVariable(rt, instruction, 1);
            var varRef = GetVariableFromOperand(rt, instruction, 0);
            var valueComparison = varRef != null
                ? varRef.Value
                : GetOperandValue(rt, instruction, 0);

            target.SetValue(instance.ArrayIndexOf(valueComparison));
        }


        private void ArrayLength(Runtime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            var instance = GetVariable(rt, instruction, 1);
            target.SetValue(instance.GetArrayLength());
        }

        private void AddArrayElements(Runtime rt, Instruction instruction)
        {
            var instance = GetVariable(rt, instruction, 0);
            var value = GetValueOf(rt, instruction.OperandArguments[0]);
            instance.AddToArray(value);
        }

        private void ClearArrayValues(Runtime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            target.ClearArray();
        }

        private void CreateArrayValue(Runtime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            target.InitArray();

            // target.SetValue(new RuntimeObject(rt, new TypeReference(XzaarBaseTypes.Array)));
            // no need to do anything here really.
        }

        private void SetArrayElementValue(Runtime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            var index = GetArgumentValue(rt, instruction, 1);
            var value = GetArgumentValue(rt, instruction, 2);
            target.SetValue(value, index);
        }

        private void GetArrayElementValue(Runtime rt, Instruction instruction)
        {
            var target = GetVariable(rt, instruction, 0);
            var source = GetVariable(rt, instruction, 1);
            var index = GetArgumentValue(rt, instruction, 2);
            target.SetValue(source.GetValue(index));
        }


        private void ReturnValue(Runtime rt, Instruction instruction)
        {
            object result = null;

            try
            {
                if (instruction.Arguments.Count <= 0)
                {
                    return;
                }

                var returnValueArgument = instruction.Arguments[0];
                if (returnValueArgument is Constant constant)
                {
                    result = GetValueOf(rt, constant);
                    return;
                }

                if (returnValueArgument is AnonymousFunctionReference || returnValueArgument is FunctionReference)
                {
                    result = returnValueArgument;
                    return;
                }

                if (returnValueArgument != null)
                {
                    if (returnValueArgument is FieldReference fieldRef)
                    {
                        var targetVariable = GetVariable(rt, fieldRef);
                        if (targetVariable != null)
                        {
                            result = fieldRef.ArrayIndex != null
                                ? GetValueOf(targetVariable, fieldRef.ArrayIndex)
                                : GetValueOf(targetVariable);
                        }
                        else
                        {
                            var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
                            TryGetClrObject(instanceVariable, fieldRef, out result);
                        }
                    }
                    else
                    {
                        var v = rt.FindVariable(returnValueArgument.Name);
                        if (v == null)
                        {
                            return;
                        }

                        if (returnValueArgument is VariableReference varRef && varRef.ArrayIndex != null)
                        {
                            result = GetValueOf(v, varRef.ArrayIndex);
                        }
                        else
                        {
                            result = GetValueOf(v);
                        }
                    }
                }
                else
                {
                    throw new RuntimeException();
                }
            }
            finally
            {
                rt.CurrentScope.Next();
                rt.EndScope(result);
            }
        }

        private void CallExtern(Runtime rt, Instruction instruction)
        {
            throw new RuntimeException();
        }

        private void CallMethod(Runtime rt, Instruction instruction)
        {
            // lookup target type
            var methodRef = (VariableReference)instruction.Arguments[0];
            var methodName = methodRef.Name;
            var instanceVariable = GetVariable(rt, instruction, 1);
            var targetVar = GetVariable(rt, instruction, 2);

            if (instanceVariable == null) throw new RuntimeException();
            if (instanceVariable.Value == null)
                throw new RuntimeException(instanceVariable.Name + " has not been initialized before use.");

            var instance = GetValueRecursive(instanceVariable);

            var args = instruction.OperandArguments;
            var funcArgs = GetFunctionArgs(rt, args);
            var parameters = funcArgs.ToArray();

            // check if its a xzaar-typed or clr managed typed object
            if (instance is RuntimeObject runtimeObject)
            {
                var invocationMember = runtimeObject.GetFieldValue(methodName);
                if (invocationMember == null)
                    throw new RuntimeException("Target function: '" + methodName + "' is not a valid member of the type '" + runtimeObject.Type.Name);

                if (invocationMember is MethodDefinition funcDef)
                {
                    this.InvokeMethod(rt, funcDef, parameters);
                }
                else if (invocationMember is FunctionReference funcRef)
                {
                    this.InvokeMethod(rt, funcRef, parameters);
                }
                else if (invocationMember is AnonymousFunctionReference anonFuncRef)
                {
                    this.InvokeAnonymousMethod(rt, anonFuncRef, parameters);
                }
                else
                {
                    throw new RuntimeException(invocationMember.GetType().FullName + " is not a supported as an invokable type.");
                }
            }
            else
            {
                // var v = instance.Value;
                var type = instance.GetType();

                var method = reflectionCache.GetMethod(type, methodName);
                if (method == null)
                    throw new RuntimeException("Target function: '" + methodName + "' could not be found.");

                var returnValue = method.Invoke(instance, parameters);

                targetVar.SetValue(returnValue);
            }

        }

        private void CallGlobal(Runtime rt, Instruction instruction)
        {
            if (!(instruction.Arguments[0] is MethodDefinition method) || method.IsExtern)
            {
                throw new RuntimeException("FunctionDefinitionExpression needs to be defined in the script its being used by. External functions not implemented yet");
            }

            var returnVariableRef = instruction.Arguments[1];
            var targetVariable = rt.FindVariable(returnVariableRef.Name);
            var args = instruction.OperandArguments;
            var funcArgs = GetFunctionArgs(rt, args);
            var methodResult = InvokeMethod(rt, method, funcArgs);
            targetVariable.SetValue(methodResult);
            // instruction.
        }

        private void CallUnknown(Runtime rt, Instruction instruction)
        {
            var targetFunctionVarRef = (VariableReference)instruction.Arguments[0];
            var param = rt.FindVariable(targetFunctionVarRef.Name);
            var targetFunctionRef = targetFunctionVarRef.ArrayIndex != null
                ? GetValueOf(rt, targetFunctionVarRef)
                : GetValueOf(rt, param);

            var returnVariableRef = instruction.Arguments[1];
            var targetVariable = rt.FindVariable(returnVariableRef.Name);

            var args = instruction.OperandArguments;
            var funcArgs = GetFunctionArgs(rt, args);

            if (targetFunctionRef is AnonymousFunctionReference anonRef)
            {
                var methodResult = InvokeAnonymousMethod(rt, anonRef, funcArgs);
                targetVariable.SetValue(methodResult);
            }
            else if (targetFunctionRef is FunctionReference funcRef)
            {
                var methodResult = InvokeMethod(rt, funcRef, funcArgs);
                targetVariable.SetValue(methodResult);
            }
            else
            {
                throw new RuntimeException(targetFunctionVarRef.Name + " is not a valid function.");
            }
        }

        private void CallAnonymous(Runtime rt, Instruction instruction)
        {
            if (!(instruction.Arguments[0] is AnonymousFunctionReference function))
            {
                throw new RuntimeException("Unable to invoke anonymous function.");
            }

            var returnVariableRef = instruction.Arguments[1];
            var targetVariable = rt.FindVariable(returnVariableRef.Name);
            var args = instruction.OperandArguments;
            var funcArgs = GetFunctionArgs(rt, args);

            var methodResult = InvokeAnonymousMethod(rt, function, funcArgs);
            targetVariable.SetValue(methodResult);
        }

        private List<object> GetFunctionArgs(Runtime rt, InstructionVariableCollection args)
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
                    if (p is AnonymousFunctionReference anonFuncRef) // lambda: hello(() => ..)
                    {
                        funcArgs.Add(anonFuncRef);

                        // throw new RuntimeException("Lambda as args: Not yet supported");
                    }
                    else if (p is FunctionReference funcRef) // fn test () {} hello(test)
                    {
                        funcArgs.Add(funcRef);

                        // throw new RuntimeException("Fn as args: Not yet supported");
                    }
                    else if (p is FieldReference fieldRef)
                    {
                        var val = GetVariable(rt, fieldRef);
                        if (val == null)
                        {
                            if (TryGetClrObject(rt, fieldRef.Instance, fieldRef, out var outVal))
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

                                throw new RuntimeException("Failed to grab the argument reference :(");
                            }

                        }
                        else
                        {
                            funcArgs.Add(val);
                        }
                    }
                    else
                    {
                        var v = rt.FindVariable(p.Name);
                        var varRef = p as VariableReference;
                        object arrayIndex = null;
                        if (varRef != null) arrayIndex = varRef.ArrayIndex;
                        if (v != null)
                        {
                            if (p is VariableReference varRefRaw && varRefRaw.ArrayIndex != null)
                            {
                                // what was I thinking here?
                            }

                            funcArgs.Add(arrayIndex != null
                                ? GetValueOf(v, arrayIndex)
                                : v);
                        }
                        else
                        {
                            throw new RuntimeException();
                        }
                    }

                }
            }

            var finalArgs = new List<object>();
            foreach (var v in funcArgs)
            {
                var value = v;
                if (value is Constant c)
                {
                    finalArgs.Add(c.Value);
                }
                else
                {
                    if (value is AnonymousFunctionReference || value is FunctionReference)
                    {
                        finalArgs.Add(value);
                        continue;
                    }
                    if (value is VariableReference varRef)
                    {
                        value = rt.FindVariable(varRef.Name);
                    }

                    if (value is RuntimeVariable runtimeVar)
                    {
                        finalArgs.Add(GetValueOf(runtimeVar));
                    }
                    else
                    {
                        finalArgs.Add(value);
                    }
                }
            }
            return finalArgs;
        }

        internal object Invoke(Runtime rt, string functionName, object[] args)
        {
            var method = rt.FindMethod(functionName);
            if (method == null) throw new RuntimeException("Could not found a function with the name '" + functionName + "'.");
            return InvokeMethod(rt, method, args);
        }

        private object InvokeMethod(Runtime rt, FunctionReference method, IList<object> args)
        {
            return InvokeMethod(rt, method.Method, args);
        }

        private object InvokeMethod(Runtime rt, MethodDefinition method, IList<object> args)
        {
            rt.BeginScope();

            var funcArgs = InitMethodLocals(rt, method.Name, method.Body.MethodVariables, method.Parameters, args);
            rt.CurrentScope.AddVariables(funcArgs);
            var ops = method.Body.MethodInstructions;
            rt.CurrentScope.SetOperations(ops);

            if (ops.Count > 0)
            {
                while (Execute(rt, ops[rt.CurrentScope.Position])) { }
            }
            if (rt.LastScope == null)
            {
                rt.EndScope();
            }
            return rt.LastScope.Result;
        }

        private object InvokeAnonymousMethod(Runtime rt, AnonymousFunctionReference function, IList<object> args)
        {
            rt.BeginScope();

            var funcArgs = InitMethodLocals(rt, function.Name, function.Body.MethodVariables, function.Parameters, args);
            rt.CurrentScope.AddVariables(funcArgs);
            var ops = function.Body.MethodInstructions;
            rt.CurrentScope.SetOperations(ops);

            if (ops.Count > 0)
            {
                while (Execute(rt, ops[rt.CurrentScope.Position])) { }
            }
            if (rt.LastScope == null)
            {
                rt.EndScope();
            }
            return rt.LastScope.Result;
        }

        private RuntimeVariable[] InitMethodLocals(Runtime rt, string methodName, MethodVariableCollection variables, ParameterCollection parameters, IList<object> arguments)
        {
            if (parameters.Count != arguments.Count)
            {
                throw new RuntimeException("The function '" + methodName + "' was invoked with wrong amount of parameters. " + parameters.Count + " parameters are expected but " + arguments.Count + " arguments were supplied.");
            }

            var output = new List<RuntimeVariable>();
            for (var i = 0; i < parameters.Count; i++)
            {
                output.Add(new RuntimeVariable(rt, parameters[i], GetValueOf(rt, arguments[i])));
            }

            for (var index = 0; index < variables.Count; index++)
            {
                var varRef = variables[index];
                var tar = rt.FindVariable(varRef.Name);
                if (tar == null)
                {
                    output.Add(new RuntimeVariable(rt, varRef));
                }
            }

            return output.ToArray();
        }

        private void JumpIfTrue(Runtime rt, Instruction instruction)
        {
            var booleanValue = GetArgumentValue(rt, instruction, 0);
            if (booleanValue is bool value)
            {
                if (value)
                {
                    Jump(rt, instruction);
                }
                else
                {
                    rt.CurrentScope.Next();
                }
            }
            else
            {
                rt.CurrentScope.Next();
            }
        }

        private void JumpIfFalse(Runtime rt, Instruction instruction)
        {
            var booleanValue = GetArgumentValue(rt, instruction, 0);
            if (booleanValue is bool)
            {
                if (!(bool)booleanValue) Jump(rt, instruction);
                else
                {
                    rt.CurrentScope.Next();
                }
            }
            else
            {
                rt.CurrentScope.Next();
            }
        }

        private void Jump(Runtime rt, Instruction instruction)
        {
            rt.CurrentScope.Position = instruction.TargetLabel.Offset;
        }


        private bool TrySetClrObject(Instruction instruction, RuntimeVariable instance, object value)
        {
            var fieldRef = instruction.Arguments[1] as VariableReference;
            if (fieldRef == null)
            {
                return false;
            }

            if (instance.Value == null)
            {
                throw new RuntimeException("The field '" + fieldRef.Name + "' has not been initalized before use. It's pretty much null.");
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
                // Cant remember why theres a catch-all here. 
            }
            return false;
        }

        private void SetStructFieldValue(Runtime rt, Instruction instruction)
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

            if (instance.Value != null && !(instance.Value is RuntimeVariable))
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

        private void GetStructFieldValue(Runtime rt, Instruction instruction)
        {
            object clr;
            var variable = GetVariable(rt, instruction, 0);
            var instanceVariable = GetVariable(rt, instruction, 1);
            var fieldVariable = (VariableReference)instruction.Arguments[2];

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
                    throw new RuntimeException("'" + instanceValue + "' does not have a property named '" + fieldVariable.Name + "'");
                }
            }

            // dereference the instance variable by searching for a RuntimeObject,

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
                throw new RuntimeException(instanceVariable.Name + " has not been initialized before use.");
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
        private bool TryGetClrObject(Runtime rt, object instanceVariable, VariableReference fieldVariable, out object clr)
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

        private bool TryGetClrObject(RuntimeVariable instanceVariable, VariableReference fieldVariable, out object clr)
        {
            clr = null;

            if (instanceVariable == null || fieldVariable == null || instanceVariable.Value == null)
                return false;

            var v = instanceVariable.Value;
            return TryGetClrObject(instanceVariable.Runtime, v, fieldVariable, out clr);
        }

        private RuntimeObject FindRuntimeObject(RuntimeVariable instanceVariable)
        {
            if (instanceVariable == null)
            {
                return null;
            }

            if (instanceVariable.Value is RuntimeObject o)
            {
                return o;
            }

            if (instanceVariable.Value is RuntimeVariable)
            {
                object tmp = instanceVariable.Value;
                while (tmp is RuntimeVariable)
                {
                    tmp = (tmp as RuntimeVariable).Value;
                }

                if (tmp is RuntimeObject obj)
                {
                    return obj;
                }
            }

            return null;
        }

        private void CreateStructInstance(Runtime rt, Instruction instruction)
        {
            var variable = GetVariable(rt, instruction, 0);
            var targetType = instruction.Arguments[1] as TypeReference;
            variable.SetValue(new RuntimeObject(rt, targetType));
        }

        private void NotValue(Runtime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueReference = GetVariable(rt, instruction, 1);
            var value = valueReference.GetValue(0);
            targetVariable.SetValue(!bool.Parse(value + ""));
        }

        private void NegateValue(Runtime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueReference = GetVariable(rt, instruction, 1);
            var value = valueReference.GetValue(0);
            targetVariable.SetValue(-double.Parse(value + ""));
        }

        private void Compare(Runtime rt, Instruction instruction)
        {
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueLeft = GetArgumentValue(rt, instruction, 1);
            var valueRight = GetArgumentValue(rt, instruction, 2);
            targetVariable.SetValue(EqualityCondition(instruction.OpCode, valueLeft, valueRight));
        }

        private void ArithmeticValue(Runtime rt, Instruction instruction, OpCode opcode)
        {
            // add target value1 value2
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueLeft = GetArgumentValue(rt, instruction, 1);
            var valueRight = GetArgumentValue(rt, instruction, 2);

            targetVariable.SetValue(Arithmetic((ArithmeticOperation)opcode, valueLeft, valueRight));
        }


        private void BitwiseValue(Runtime rt, Instruction instruction, OpCode opcode)
        {
            // add target value1 value2
            var op = (BitwiseOperation)opcode;
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueLeft = GetArgumentValue(rt, instruction, 1);
            if (op == BitwiseOperation.Not)
            {
                targetVariable.SetValue(BitwiseNot(valueLeft));
            }
            else
            {
                var valueRight = GetArgumentValue(rt, instruction, 2);
                targetVariable.SetValue(Bitwise(op, valueLeft, valueRight));
            }
        }

        /*
         *                     case OpCode.BitwiseAnd:
                    case OpCode.BitwiseOr:
                    case OpCode.BitwiseXor:
                    case OpCode.BitwiseNot:
                    case OpCode.BitwiseRightShift:
                    case OpCode.BitwiseLeftShift:
         */

        private void Conditional(Runtime rt, Instruction instruction, OpCode opcode)
        {
            // add target value1 value2
            var targetVariable = GetVariable(rt, instruction, 0);
            var valueLeft = GetArgumentValue<bool>(rt, instruction, 1);
            var valueRight = GetArgumentValue<bool>(rt, instruction, 2);

            targetVariable.SetValue(
                opcode == OpCode.And
                    ? valueLeft && valueRight
                    : valueLeft || valueRight
                );
        }

        private object GetArgumentValue(Runtime rt, Instruction instruction, int index)
        {
            return GetValueOf(rt, instruction.Arguments[index]);
        }

        private T GetArgumentValue<T>(Runtime rt, Instruction instruction, int index)
        {
            try
            {
                return (T)GetValueOf(rt, instruction.Arguments[index]);
            }
            catch
            {
                return default(T);
            }
        }

        private object GetOperandValue(Runtime rt, Instruction instruction, int index)
        {
            return GetValueOf(rt, instruction.OperandArguments[index]);
        }

        private RuntimeVariable GetVariableFromOperand(Runtime rt, Instruction instruction, int index)
        {
            var targetVariableRef = instruction.OperandArguments[index];
            // check if the variable is a field from a struct or other type
            if (targetVariableRef is FieldReference fieldRef)
            {
                var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
                var instanceObject = FindRuntimeObject(instanceVariable);
                return instanceObject.GetField(fieldRef.Name);
            }

            if (targetVariableRef.MemberType == MemberTypes.Constant)
            {
                return null;
            }

            var targetVariable = rt.FindVariable(targetVariableRef.Name);
            AssertVariableFound(targetVariable, targetVariableRef.Name);
            return targetVariable;
        }



        private RuntimeVariable GetVariable(Runtime rt, Instruction instruction, int index)
        {
            var targetVariableRef = instruction.Arguments[index];
            // check if the variable is a field from a struct or other type
            if (targetVariableRef is FieldReference fieldRef)
            {
                var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
                var instanceObject = FindRuntimeObject(instanceVariable);
                return instanceObject.GetField(fieldRef.Name);
            }

            if (targetVariableRef.MemberType == MemberTypes.Constant)
            {
                if (targetVariableRef is Constant constant)
                {
                    // throw new NotImplementedException("Constant values cannot be accessed as a field or variable yet. This feature has not been implemented.");

                    if (constant.IsArray)
                    {
                        var tempVariable = new RuntimeVariable(rt, constant);
                        rt.CurrentScope.AddVariables(new[] { tempVariable });
                        return tempVariable;
                    }
                }

                return null;
            }

            var targetVariable = rt.FindVariable(targetVariableRef.Name);
            AssertVariableFound(targetVariable, targetVariableRef.Name);
            return targetVariable;
        }


        private RuntimeVariable GetVariable(Runtime rt, FieldReference fieldRef)
        {
            var instanceVariable = rt.FindVariable(fieldRef.Instance.Name);
            var instanceObject = FindRuntimeObject(instanceVariable);
            return instanceObject?.GetField(fieldRef.Name);
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

        private object BitwiseNot(object valueLeft)
        {
            var left = 0;
            if (valueLeft is bool leftBool) left = leftBool ? 1 : 0;
            if (IsNumber(valueLeft)) left = (int)Number(valueLeft);
            return ~left;
        }

        private object Bitwise(BitwiseOperation op, object valueLeft, object valueRight)
        {
            var left = 0;
            var right = 0;
            if (valueLeft is bool leftBool) left = leftBool ? 1 : 0;
            if (valueRight is bool rightBool) left = rightBool ? 1 : 0;
            if (IsNumber(valueLeft)) left = (int)Number(valueLeft);
            if (IsNumber(valueRight)) right = (int)Number(valueRight);
            switch (op)
            {
                case BitwiseOperation.And:
                    return left & right;
                case BitwiseOperation.Or:
                    return left | right;
                case BitwiseOperation.LeftShift:
                    return left << right;
                case BitwiseOperation.RightShift:
                    return left >> right;
                case BitwiseOperation.Xor:
                    return left ^ right;
            }
            return null;
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

        private void AssignValue(Runtime rt, Instruction instruction)
        {
            // assign target value
            var targetVariableRef = instruction.Arguments[0];

            var valueRef = instruction.Arguments[1];

            // we want to reassign a function
            if (targetVariableRef is FunctionReference targetFuncRef)
            {
                ReAssignFunction(rt, targetFuncRef, valueRef);
                return;
            }

            //if (targetVariableRef is AnonymousFunctionReference anonTargetFuncRef)
            //{
            //    ReAssignFunction(rt, anonTargetFuncRef, valueRef);
            //    return;
            //}

            var targetVariable = rt.FindVariable(targetVariableRef.Name);

            object value = null;
            AssertVariableFound(targetVariable, targetVariableRef.Name);

            // we want to assign a function reference to a variable            
            if (valueRef is FunctionReference funcRef)
            {
                value = funcRef;
            }
            else
            {
                value = GetValueOf(rt, valueRef);
                if (value == null)
                {
                    var varRef = GetVariable(rt, instruction, 1);
                    value = GetValueOf(varRef);
                }

            }

            if (value != null)
                targetVariable.SetValue(value);
        }

        private void ReAssignFunction(Runtime rt, FunctionReference funcToReassign, MemberReference newFunction)
        {
            // we must match parameters for this to work. Or it should throw an error
            // we then just "replace" the function body, but this may corrupt the runtime
            // if we access variables outside its possible scope. But we can just fail
            // with a runtime error when that happens.
            // NOTE(zerratar): parameter names does not have to match, as long as they have the same type order, then its fine.

            if (newFunction is AnonymousFunctionReference anonRef)
            {
                if (funcToReassign.Method.Parameters.Count != anonRef.Parameters.Count
                    || !funcToReassign.Method.Parameters.Select(x => x.Type.Name)
                        .SequenceEqual(anonRef.Parameters.Select(x => x.Type.Name)))
                {
                    throw new RuntimeException("Cannot set the target function '"
                                               + funcToReassign.Name + "' with the new '" + anonRef.Name + "' as the parameters does not match.");
                }

                // if anonRef.ReturnType is null, then we were unable to determine returntype
                // so let it be a runtime error later.
                if (anonRef.ReturnType != null && funcToReassign.Method.ReturnType.Name != anonRef.ReturnType.Name)
                {
                    throw new RuntimeException("Cannot set the target function '"
                                               + funcToReassign.Name + "' with the new '" + anonRef.Name + "' as the return type does not match.");
                }

                // TODO: there are no way to reverse this in the script afterwards.
                funcToReassign.Method.SetParameters(anonRef.Parameters.ToArray());
                funcToReassign.Method.SetBody(anonRef.Body);
                return;
            }

            if (newFunction is FunctionReference funcRef)
            {
                if (funcToReassign.Method.Parameters.Count != funcRef.Method.Parameters.Count
                    || !funcToReassign.Method.Parameters.Select(x => x.Type.Name)
                        .SequenceEqual(funcRef.Method.Parameters.Select(x => x.Type.Name)))
                {
                    throw new RuntimeException("Cannot set the target function '"
                                               + funcToReassign.Name + "' with the new '" + funcRef.Method.Name + "' as the parameters does not match.");
                }

                if (funcToReassign.Method.ReturnType != funcRef.Method.ReturnType)
                {
                    throw new RuntimeException("Cannot set the target function '"
                                               + funcToReassign.Name + "' with the new '" + funcRef.Method.Name + "' as the return type does not match.");
                }

                // TODO: there are no way to reverse this in the script afterwards.
                funcToReassign.Method.SetParameters(funcRef.Method.Parameters.ToArray());
                funcToReassign.Method.SetBody(funcRef.Method.Body);
                return;
            }

            throw new RuntimeException("Cannot set the target function '"
                                       + funcToReassign.Name +
                                       "' with a value other than another function or lambda.");
        }

        private object GetValueOf(Runtime rt, object value)
        {
            if (value is object[] array)
            {
                var newArray = new List<object>();
                foreach (var item in array)
                {
                    newArray.Add(GetValueOf(rt, item));
                }
                return newArray.ToArray();
            }

            if (value is Constant cval)
            {
                return GetValueOf(rt, cval);
            }

            if (value is FunctionReference || value is AnonymousFunctionReference)
            {
                return value;
            }

            if (value is VariableReference vval)
            {
                return GetValueOf(rt, vval);
            }

            if (value is RuntimeVariable rtv)
            {
                return rtv.Value;
            }

            return value;
        }

        private object GetValueOf(Runtime rt, MemberReference mRef)
        {
            if (mRef is FunctionReference || mRef is AnonymousFunctionReference)
            {
                return mRef; // just pass them along
            }

            if (mRef is Constant c)
            {
                return GetValueOf(rt, c);
            }

            if (mRef is VariableReference v)
            {
                if (v.IsRef && (v.Reference is AnonymousFunctionReference || v.Reference is FunctionReference))
                {
                    return v.Reference;
                }

                var variable = rt.FindVariable(v.Name);
                if (variable != null)
                {
                    if (v.ArrayIndex != null) return GetValueOf(variable, v.ArrayIndex);
                    return GetValueOf(variable);
                }
            }

            return null;
        }
        private object GetValueOf(RuntimeVariable v, object arrayIndex)
        {
            if (arrayIndex == null) return v.Value;
            var ai = GetValueOf(v.Runtime, arrayIndex as MemberReference);
            return v.GetValue(ai);
        }

        private object GetValueRecursive(RuntimeVariable v)
        {
            object vref = v;
            while (vref is RuntimeVariable)
            {
                var b = vref as RuntimeVariable;
                vref = b.Value;
            }
            return vref;
        }

        private object GetValueOf(RuntimeVariable v)
        {
            return v.Value;
        }

        private object GetValueOf(Runtime rt, Constant c)
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
                    throw new RuntimeException(exc.Message);
                }
            }
            return c.Value;
        }

        private void AssertVariableFound(RuntimeVariable targetVariable, string varName)
        {
            if (targetVariable == null) throw new RuntimeException("The target variable '" + varName + "' could not be found.");
        }

        internal enum ArithmeticOperation
        {
            Add = 1,
            Sub = 2,
            Mul = 3,
            Div = 4,
            Mod = 5
        }

        internal enum BitwiseOperation
        {
            Or,
            Xor,
            Not,
            And,
            LeftShift,
            RightShift
        }
    }
}