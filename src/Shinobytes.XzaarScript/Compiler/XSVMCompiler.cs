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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Utilities;
using Label = Shinobytes.XzaarScript.Assembly.Label;

namespace Shinobytes.XzaarScript.Compiler
{
    public class XSVMCompiler : IScriptCompilerVisitor
    {
        private readonly List<string> errors = new List<string>();
        private readonly ScriptCompilerContext ctx;
        private ExpressionType lastBinaryOperationType;
        private bool insideBinaryOperation;
        private VariableReference lastMemberAccessChainResult;

        internal XSVMCompiler(ScriptCompilerContext ctx)
        {
            this.ctx = ctx;
        }

        public bool HasErrors => errors.Count > 0;

        public IList<string> Errors => errors;

        public static XzaarAssembly Compile(XzaarExpression expression)
        {
            return Compile(expression, out _);
        }

        public static XzaarAssembly Compile(XzaarExpression expression, out IList<string> errors)
        {
            var compilerContext = new ScriptCompilerContext(Guid.NewGuid().ToString(), expression);
            var compiler = new XSVMCompiler(compilerContext);
            var discovery = new ScriptDiscoveryVisitor(compilerContext);
            discovery.Visit(expression);
            compiler.Visit(expression);
            foreach (var v in compilerContext.GlobalTempVariables) compilerContext.Assembly.GlobalVariables.Add(v);
            foreach (var i in compilerContext.GlobalInstructions) compilerContext.Assembly.GlobalInstructions.Add(i);
            // last one should be global instructors            
            foreach (var i in compilerContext.MethodInstructions) compilerContext.Assembly.GlobalInstructions.Add(i);
            errors = compiler.errors;
            return compilerContext.Assembly;
        }

        public object Visit(XzaarExpression expression)
        {
            if (expression == null || expression is ErrorExpression)
                return null;

            if (expression.NodeType == ExpressionType.Block && expression is BlockExpression block)
            {
                foreach (var item in block.Expressions)
                {
                    Visit(item);
                }
                return null;
            }

//#if UNITY
            if (expression is LambdaExpression lambda) return Visit(lambda);

            if (expression is BinaryExpression binaryExpression) return Visit(binaryExpression);
            if (expression is IfElseExpression elseExpression) return Visit(elseExpression);
            if (expression is ConditionalExpression conditionalExpression) return Visit(conditionalExpression);
            if (expression is MemberExpression memberExpression) return Visit(memberExpression);
            if (expression is MemberAccessChainExpression chainExpression) return Visit(chainExpression);
            if (expression is GotoExpression gotoExpression) return Visit(gotoExpression);
            if (expression is SwitchExpression switchExpression) return Visit(switchExpression);
            if (expression is SwitchCaseExpression caseExpression) return Visit(caseExpression);
            if (expression is UnaryExpression unaryExpression) return Visit(unaryExpression);
            if (expression is BlockExpression blockExpression) return Visit(blockExpression);
            if (expression is ForExpression forExpression) return Visit(forExpression);
            if (expression is ForEachExpression eachExpression) return Visit(eachExpression);
            if (expression is DoWhileExpression whileExpression) return Visit(whileExpression);
            if (expression is WhileExpression expression1) return Visit(expression1);
            if (expression is LoopExpression loopExpression) return Visit(loopExpression);
            if (expression is DefaultExpression defaultExpression) return Visit(defaultExpression);
            if (expression is FunctionCallExpression callExpression) return Visit(callExpression);
            if (expression is ConstantExpression constantExpression) return Visit(constantExpression);
            if (expression is NegateExpression negateExpression) return Visit(negateExpression);
            if (expression is VariableDefinitionExpression definitionExpression) return Visit(definitionExpression);
            if (expression is LabelExpression labelExpression) return Visit(labelExpression);
            if (expression is ParameterExpression parameterExpression) return Visit(parameterExpression);
            if (expression is FunctionExpression functionExpression) return Visit(functionExpression);
            if (expression is StructExpression structExpression) return Visit(structExpression);
            if (expression is FieldExpression fieldExpression) return Visit(fieldExpression);
            if (expression is LogicalNotExpression notExpression) return Visit(notExpression);
            return Visit(expression);
//#else
//            return Visit((dynamic)expression);
//#endif
        }

        private VariableReference BinaryOp(BinaryExpression binaryOp, OpCode oper, XzaarType tempVariableType)
        {
            var left = Visit(binaryOp.Left) as VariableReference;
            var right = Visit(binaryOp.Right) as VariableReference;
            var createTempVariableLeft = !(oper == OpCode.CmpEq || oper == OpCode.CmpGt || oper == OpCode.CmpLt || oper == OpCode.CmpGte || oper == OpCode.CmpLte);
            var createTempVariableRight = createTempVariableLeft;
            if (!createTempVariableLeft && left is FieldReference) createTempVariableLeft = true;
            if (!createTempVariableLeft && right is FieldReference) createTempVariableRight = true;
            return BinaryOp(left, right, oper, tempVariableType, createTempVariableLeft, createTempVariableRight); // true, true
        }

        private VariableReference BinaryOp(VariableReference left, VariableReference right, OpCode oper, XzaarType tempVariableType, bool createTempVariableLeft, bool createTempVariableRight)
        {
            var lv = left;
            var rv = right;

            if (left == null || right == null)
            {
                insideBinaryOperation = false;
                return null;
            }

            if (createTempVariableLeft) lv = TempVariable(left.Type);
            if (createTempVariableRight) rv = TempVariable(right.Type);
            var cv = TempVariable(tempVariableType);
            if (createTempVariableLeft)
            {
                if (left is FieldReference fRef)
                {
                    ctx.AddInstruction(left.ArrayIndex != null
                        ? Instruction.Create(OpCode.StructGet, lv, fRef.Instance, fRef, left.ArrayIndex as VariableReference)
                        : Instruction.Create(OpCode.StructGet, lv, fRef.Instance, fRef));
                }
                else
                    ctx.AddInstruction(Instruction.Create(OpCode.Assign, lv, left));
            }
            if (createTempVariableRight)
            {
                if (right is FieldReference fRef)
                {
                    ctx.AddInstruction(right.ArrayIndex != null
                        ? Instruction.Create(OpCode.StructGet, rv, fRef.Instance, fRef, right.ArrayIndex as VariableReference)
                        : Instruction.Create(OpCode.StructGet, rv, fRef.Instance, fRef));
                }
                else
                    ctx.AddInstruction(Instruction.Create(OpCode.Assign, rv, right));
            }
            ctx.AddInstruction(Instruction.Create(oper, cv, lv, rv));
            insideBinaryOperation = false;
            return cv; // return the temp variable we used to return the result of the comparison
        }

        private VariableReference Assign(VariableReference varRef, VariableReference value, BinaryExpression binaryOp = null)
        {
            VariableReference returnValue = null;
            if (varRef == null) return Error("Bad binary expression: Left side is missing!");
            var fRef = varRef as FieldReference;
            if (value is Constant constant)
            {
                if (constant.Value is XzaarExpression[] arrayInit)
                {
                    ctx.AddInstruction(Instruction.Create(OpCode.ArrayClearElements, varRef));
                    foreach (var item in arrayInit)
                    {
                        var val = Visit(item) as VariableReference;
                        var inst = Instruction.Create(OpCode.ArrayAddElements, varRef);
                        inst.OperandArguments.Add(val);
                        ctx.AddInstruction(inst);
                    }

                    insideBinaryOperation = false;
                    return varRef;
                }
            }

            if (value is FieldReference fieldRef)
            {
                value = TempVariable(varRef.Type);
                ctx.AddInstruction(fieldRef.ArrayIndex != null
                    ? Instruction.Create(OpCode.StructGet, value, fieldRef.Instance, fieldRef, fieldRef.ArrayIndex as VariableReference)
                    : Instruction.Create(OpCode.StructGet, value, fieldRef.Instance, fieldRef));

                // ctx.AddInstruction(Instruction.Create(OpCode.StructGet, value, fRefRight.Instance, fRefRight));
            }

            if (fRef == null && varRef.IsRef)
            {
                fRef = varRef.Reference as FieldReference;
            }
            if (fRef != null)
            {
                ctx.AddInstruction(varRef.ArrayIndex != null
                    ? Instruction.Create(OpCode.StructSet, fRef.Instance, fRef, varRef.ArrayIndex as VariableReference, value)
                    : Instruction.Create(OpCode.StructSet, fRef.Instance, fRef, value));
                returnValue = fRef;
            }
            else
            {
                if (binaryOp?.Left is MemberAccessChainExpression)
                {
                    ctx.AddInstruction(varRef.ArrayIndex != null
                        ? Instruction.Create(OpCode.StructSet, varRef, varRef.Reference, varRef.ArrayIndex as VariableReference, value)
                        : Instruction.Create(OpCode.StructSet, varRef, varRef.Reference, value));
                }
                else
                {
                    ctx.AddInstruction(varRef.ArrayIndex != null
                        ? Instruction.Create(OpCode.ArraySetElement, varRef, varRef.ArrayIndex as VariableReference, value)
                        : Instruction.Create(OpCode.Assign, varRef, value));
                }
                returnValue = varRef;
            }
            insideBinaryOperation = false;

            return returnValue;
        }

        public object Visit(BinaryExpression binaryOp)
        {
            lastBinaryOperationType = binaryOp.NodeType;
            insideBinaryOperation = true;
            switch (binaryOp.NodeType)
            {
                case ExpressionType.Assign:
                    if (!(Visit(binaryOp.Left) is VariableReference varRef))
                    {
                        return Error("Bad binary expression: Left side is missing!");
                    }
                    var value = Visit(binaryOp.Right) as VariableReference;
                    return Assign(varRef, value, binaryOp);
                case ExpressionType.LessThan:
                    return BinaryOp(binaryOp, OpCode.CmpLt, XzaarBaseTypes.Boolean);
                case ExpressionType.LessThanOrEqual:
                    return BinaryOp(binaryOp, OpCode.CmpLte, XzaarBaseTypes.Boolean);
                case ExpressionType.GreaterThan:
                    return BinaryOp(binaryOp, OpCode.CmpGt, XzaarBaseTypes.Boolean);
                case ExpressionType.GreaterThanOrEqual:
                    return BinaryOp(binaryOp, OpCode.CmpGte, XzaarBaseTypes.Boolean);
                case ExpressionType.Equal:
                    return BinaryOp(binaryOp, OpCode.CmpEq, XzaarBaseTypes.Boolean);
                case ExpressionType.NotEqual:
                    return BinaryOp(binaryOp, OpCode.CmpNotEq, XzaarBaseTypes.Boolean);
                case ExpressionType.Subtract:
                    return BinaryOp(binaryOp, OpCode.Sub, XzaarBaseTypes.Number);
                case ExpressionType.Add:
                    return BinaryOp(binaryOp, OpCode.Add, binaryOp.Type); // XzaarBaseTypes.Number                    
                case ExpressionType.Multiply:
                    return BinaryOp(binaryOp, OpCode.Mul, XzaarBaseTypes.Number);
                case ExpressionType.Divide:
                    return BinaryOp(binaryOp, OpCode.Div, XzaarBaseTypes.Number);
                case ExpressionType.Modulo:
                    return BinaryOp(binaryOp, OpCode.Mod, XzaarBaseTypes.Number);
            }
            return Error(binaryOp.NodeType + " has not been implemented.");
        }


        public object Visit(ConditionalExpression expr)
        {
            if (expr.Type.IsEquivalentTo(XzaarBaseTypes.Void))
            {
                return Error("Type of the conditional expression cannot be a void");
            }

            // if test is true, assign result of 'WhenTrue' to temp and return temp variable
            // if test is false, assign result of 'WhenFalse' to temp and return temp variable

            var endLabel = Instruction.Label();
            var ifTrueLabel = Instruction.Label();
            var ifFalseLabel = Instruction.Label();
            var test = Visit(expr.Test) as VariableReference;
            var t = expr.WhenTrue;
            var f = expr.WhenFalse;

            var result = TempVariable(expr.Type);

            ctx.AddInstruction(Instruction.Create(OpCode.Jmpt, test, ifTrueLabel));
            ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, test, ifFalseLabel));
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, endLabel));
            var startIndex = ctx.InstructionCount;


            Assign(result, Visit(t) as VariableReference);


            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, endLabel));
            ctx.InsertInstruction(startIndex, ifTrueLabel);


            startIndex = ctx.InstructionCount; // ctx.MethodInstructions.Count;

            if (f != null && !f.IsEmpty())
            {
                Assign(result, Visit(f) as VariableReference);
                ctx.AddInstruction(Instruction.Create(OpCode.Jmp, endLabel));
                ctx.InsertInstruction(startIndex, ifFalseLabel);
            }

            ctx.AddInstruction(endLabel);

            return result;
        }

        public object Visit(IfElseExpression ifElse)
        {
            var endLabel = Instruction.Label();
            var ifTrueLabel = Instruction.Label();
            var ifFalseLabel = Instruction.Label();
            var test = Visit(ifElse.Test) as VariableReference;
            var t = ifElse.IfTrue;
            var f = ifElse.IfFalse;
            if (t != null && !t.IsEmpty()) ctx.AddInstruction(Instruction.Create(OpCode.Jmpt, test, ifTrueLabel));
            if (f != null && !f.IsEmpty()) ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, test, ifFalseLabel));
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, endLabel));
            var startIndex = ctx.InstructionCount;
            if (t != null && !t.IsEmpty())
            {
                Visit(t);
                ctx.AddInstruction(Instruction.Create(OpCode.Jmp, endLabel));
                ctx.InsertInstruction(startIndex, ifTrueLabel);
            }

            startIndex = ctx.InstructionCount; // ctx.MethodInstructions.Count;

            if (f != null && !f.IsEmpty())
            {
                Visit(f);
                ctx.AddInstruction(Instruction.Create(OpCode.Jmp, endLabel));
                ctx.InsertInstruction(startIndex, ifFalseLabel);
            }

            ctx.AddInstruction(endLabel);
            return null;
        }

        public object Visit(MemberExpression access)
        {
            // when defining a global variable without an initial value, we end up having a member access instead of define?
            if (access.Expression is ParameterExpression pe)
            {
                var varRef = new VariableReference(pe.Name, new TypeReference(pe.Type));
                if (access.ArrayIndex != null)
                {
                    var arrayIndex = Visit(access.ArrayIndex);
                    varRef.ArrayIndex = arrayIndex;
                }
                //if (ctx.RecordVariableReferencesOnly) ctx.VariableReferences.Add(varRef);
                return varRef;
            }

            if (access.Expression is FieldExpression fe)
            {
                var varRef = new FieldReference
                {
                    Name = fe.Name,
                    Type = new TypeReference(fe.Type)
                };
                if (access.ArrayIndex != null)
                {
                    var arrayIndex = Visit(access.ArrayIndex);
                    varRef.ArrayIndex = arrayIndex;
                }
                return varRef;
            }

            if (access.Expression is ConstantExpression constantAccess)
            {
                // var constantRef = Visit(constantAccess);
                var newVariable = TempVariable(constantAccess);
                if (access.ArrayIndex != null)
                {
                    newVariable.ArrayIndex = Visit(access.ArrayIndex);
                }
                ctx.AddInstruction(Instruction.Create(OpCode.Assign, newVariable, Visit(constantAccess) as VariableReference));
                return newVariable;
            }

            var vrefRaw = Visit(access.Expression);
            if (vrefRaw is FieldReference fieldRef)
            {
                if (access.ArrayIndex != null) fieldRef.ArrayIndex = Visit(access.ArrayIndex);
                return fieldRef;
            }

            if (vrefRaw is VariableReference variableRef)
            {
                var varRef = new VariableReference(variableRef.Name, variableRef.Type);
                if (variableRef.ArrayIndex != null)
                {
                    varRef.ArrayIndex = variableRef.ArrayIndex;
                    if (access.ArrayIndex != null)
                    {
                        // oh schnap! we have an array of arrays (most likely
                        var arrayIndex = Visit(access.ArrayIndex);
                        if (!varRef.Type.IsArray && varRef.Type.Name != "any") return Error("Expected '" + varRef.Name + "' to be an array but is '" + varRef.Type.Name + "'");

                        // TODO: Revise this, because this might not be the correct way of doing it P:

                        var elmType = varRef.Type.ArrayElementType ?? XzaarBaseTypes.Any;
                        var tempVariable = TempVariable(elmType);

                        ctx.AddInstruction(Instruction.Create(OpCode.Assign,
                            tempVariable.Clone(), // clone the temp variable so we do not persist the array index we assign below
                            varRef));

                        tempVariable.ArrayIndex = arrayIndex;
                        return tempVariable;
                        // varRef.Reference = tempVariable;
                    }
                }
                else if (access.ArrayIndex != null)
                {
                    var arrayIndex = Visit(access.ArrayIndex);
                    varRef.ArrayIndex = arrayIndex;
                }
                return varRef;
            }
            return Error("PANIC!! '" + access.Expression + "' cannot be passed as a member");
        }

        private FieldReference FieldReference(string fieldName, VariableReference instance, object initialValue, TypeReference type, object arrayIndex)
        {
            return new FieldReference
            {
                ArrayIndex = arrayIndex,
                Instance = instance,
                Name = fieldName,
                InitialValue = initialValue,
                Type = type
            };
        }

        public object Visit(MemberAccessChainExpression access)
        {
            lastMemberAccessChainResult = null;
            var instance = Visit(access.Left) as VariableReference;
            var targetField = Visit(access.Right) as VariableReference;
            if (targetField is FieldReference) (targetField as FieldReference).Instance = instance;
            if (targetField == null) return null;
            if (instance is FieldReference instanceField)
            {
                // a.b.c
                if (insideBinaryOperation && lastBinaryOperationType == ExpressionType.Assign && !(targetField is FieldReference))
                {
                    // make sure we return
                    //   StructGet
                    //   StructSet
                    // instead of
                    //   StructGet
                    //   StructGet
                    //   Assign

                    var value = TempVariable(instanceField.Instance.Type, targetField);

                    ctx.AddInstruction(instanceField.Instance.ArrayIndex != null
                        ? Instruction.Create(OpCode.StructGet, value, instanceField.Instance, instanceField,
                            instanceField.Instance.ArrayIndex as VariableReference)
                        : Instruction.Create(OpCode.StructGet, value, instanceField.Instance, instanceField));

                    return value;
                }
                else
                {
                    var value = TempVariable(instanceField.Instance.Type);
                    var value2 = targetField is FieldReference
                        ? TempVariable(targetField.Type, targetField)
                        : TempVariable(targetField.Type);

                    ctx.AddInstruction(instanceField.Instance.ArrayIndex != null
                        ? Instruction.Create(OpCode.StructGet, value, instanceField.Instance, instanceField,
                            instanceField.Instance.ArrayIndex as VariableReference)
                        : Instruction.Create(OpCode.StructGet, value, instanceField.Instance, instanceField));

                    ctx.AddInstruction(targetField.ArrayIndex != null
                        ? Instruction.Create(OpCode.StructGet, value2, value, targetField,
                            targetField.ArrayIndex as VariableReference)
                        : Instruction.Create(OpCode.StructGet, value2, value, targetField));
                    lastMemberAccessChainResult = value2;
                    return value2;
                }
            }

            if (instance != null)
            {
                var vref = ctx.VariableReferences.FirstOrDefault(v => v.Name == instance.Name);
                if (vref != null)
                {
                    return lastMemberAccessChainResult = new FieldReference
                    {
                        ArrayIndex = targetField.ArrayIndex,
                        Instance = instance,
                        Name = targetField.Name,
                        InitialValue = targetField.InitialValue,
                        Type = targetField.Type
                    };
                    //  instance.Name
                }

                if (access.Right is FunctionCallExpression)
                {
                    lastMemberAccessChainResult = targetField;
                    return null;
                }

                if (insideBinaryOperation && lastBinaryOperationType == ExpressionType.Assign)
                {
                    // so, we want to assign a field?
                    return lastMemberAccessChainResult = new FieldReference
                    {
                        ArrayIndex = targetField.ArrayIndex,
                        Instance = instance,
                        Name = targetField.Name,
                        InitialValue = targetField.InitialValue,
                        Type = targetField.Type
                    };
                }

                var value = targetField is FieldReference ? TempVariable(targetField.Type, targetField) : TempVariable(targetField.Type);
                ctx.AddInstruction(targetField.ArrayIndex != null
                    ? Instruction.Create(OpCode.StructGet, value, instance, targetField, targetField.ArrayIndex as VariableReference)
                    : Instruction.Create(OpCode.StructGet, value, instance, targetField));
                lastMemberAccessChainResult = value;
                return value;
            }

            // could be other object or even static reference
            return Error("PANIC!! '" + access + "' cannot be passed as a member");
        }

        public object Visit(GotoExpression @goto)
        {
            switch (@goto.Kind)
            {
                case GotoExpressionKind.Return:
                    {
                        if (@goto.Value != null)
                        {
                            var value = Visit(@goto.Value) as VariableReference;
                            if (@goto.Value is MemberAccessChainExpression)
                            {
                                // value == null && 
                                // function call, its the only one that returns null after visitng a memberAccessChain                                
                                value = lastMemberAccessChainResult;
                            }
                            if (value != null)
                            {
                                // if we got any new temp variables, then most likely we want to use that one.
                                ctx.AddInstruction(Instruction.Create(OpCode.Return, value));
                                return null;
                            }
                        }
                        ctx.AddInstruction(Instruction.Create(OpCode.Return));
                        return null;
                    }
                case GotoExpressionKind.Goto:
                    {
                        var target = @goto.Target;
                        var t = ctx.MethodInstructions.FirstOrDefault(l =>
                        {
                            var label = l as Label;
                            return label != null && label.Name == target.Name;
                        }) as Label;

                        if (t == null)
                        {
                            return Error("The label '" + target.Name + "' does not seem to exist");
                        }
                        ctx.AddInstruction(Instruction.Create(OpCode.Jmp, t));
                    }
                    return null;
                case GotoExpressionKind.Break:
                    {
                        ctx.AddInstruction(Instruction.Create(OpCode.Jmp, ctx.CurrentLoopEndLabel));
                        return null;
                    }
                case GotoExpressionKind.Continue:
                    {
                        ctx.AddInstruction(Instruction.Create(OpCode.Jmp, ctx.CurrentLoopStartLabel));
                        return null;
                    }
            }

            return Error("'" + @goto.Kind + "' is not a goto|continue|break or return statement");
        }

        public object Visit(SwitchExpression match)
        {
            // todo: jump to next case when one fails. not to the end
            ctx.BeginControlBlock();
            var value = Visit(match.Value) as VariableReference;
            var endLabel = ctx.CurrentLoopEndLabel;
            Label[] labels = new Label[match.Cases.Length];
            for (var i = 0; i < match.Cases.Length; i++) labels[i] = Instruction.Label();

            for (var i = 0; i < match.Cases.Length; i++)
            {
                if (!(match.Cases[i] is SwitchCaseExpression switchCase))
                {
                    return Error("Invalid switch case");
                }

                ctx.AddInstruction(labels[i]);

                // for each case
                // cmpeq tempBool caseMatch value
                var caseMatch = Visit(switchCase.Match) as VariableReference;
                var tmpVar = BinaryOp(value, caseMatch, OpCode.CmpEq, XzaarBaseTypes.Boolean, false, true);
                if (i == match.Cases.Length - 1)
                {
                    // last one should jump to end
                    // jmpf <out of switch case label> tmpVar
                    ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, tmpVar, endLabel));
                }
                else
                {
                    // jmpf next_label tmpVar
                    ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, tmpVar, labels[i + 1]));
                }

                //// jmpf <out of switch case label> tmpVar
                //ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, tmpVar, endLabel));

                // ...body..
                Visit(switchCase.Body);
            }

            ctx.AddInstruction(endLabel);
            ctx.EndControlBlock();
            return null;
        }

        public object Visit(SwitchCaseExpression matchCase)
        {
            throw new NotImplementedException();
        }

        public object Visit(UnaryExpression unary)
        {
            VariableReference variable = null;
            if (unary.Item is MemberExpression memExpr) variable = Variable(memExpr);
            if (variable == null) variable = Variable(unary.Item as ParameterExpression);
            switch (unary.NodeType)
            {
                case ExpressionType.PostDecrementAssign:
                    // Create a copy of the value and decrement the original                    
                    var beforeDecrement = TempVariable(variable.Type);
                    ctx.AddInstruction(Instruction.Create(OpCode.Assign, beforeDecrement, variable));
                    ctx.AddInstruction(Instruction.Create(OpCode.Sub, variable, variable, Constant(1))); // LateAdd
                    return beforeDecrement;
                case ExpressionType.PostIncrementAssign:
                    // Create a copy of the value and increment the original
                    var beforeIncrement = TempVariable(variable.Type);
                    ctx.AddInstruction(Instruction.Create(OpCode.Assign, beforeIncrement, variable));
                    ctx.AddInstruction(Instruction.Create(OpCode.Add, variable, variable, Constant(1))); // LateAdd
                    return beforeIncrement;
                case ExpressionType.Increment:
                    ctx.AddInstruction(Instruction.Create(OpCode.Add, variable, variable, Constant(1)));
                    return variable;
                case ExpressionType.Decrement:
                    ctx.AddInstruction(Instruction.Create(OpCode.Sub, variable, variable, Constant(1)));
                    return variable;
            }
            return Error("'" + unary.NodeType + "' is not a known unary expression");
        }

        public object Visit(BlockExpression block)
        {
            throw new NotImplementedException();
        }

        public object Visit(ForExpression @for)
        {
            ctx.BeginControlBlock();
            var loopStart = ctx.CurrentLoopStartLabel;
            var loopEnd = ctx.CurrentLoopEndLabel;
            Visit(@for.Initiator);
            ctx.AddInstruction(loopStart);
            var tester = Visit(@for.Test) as VariableReference;
            ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, tester, loopEnd));
            Visit(@for.Body);
            Visit(@for.Incrementor);
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, loopStart));
            ctx.AddInstruction(loopEnd);
            ctx.EndControlBlock();
            return null;
        }

        public object Visit(ForEachExpression @foreach)
        {
            ctx.BeginControlBlock();
            var loopStart = ctx.CurrentLoopStartLabel;
            var loopEnd = ctx.CurrentLoopEndLabel;
            var collection = Visit(@foreach.Collection) as VariableReference;
            var arrayLength = TempVariable(XzaarBaseTypes.Number);
            var itemIndex = TempVariable(0);
            var compareVar = TempVariable(XzaarBaseTypes.Boolean);
            ctx.AddInstruction(Instruction.Create(OpCode.ArrayLength, arrayLength, collection));
            ctx.AddInstruction(loopStart);
            ctx.AddInstruction(Instruction.Create(OpCode.CmpLt, compareVar, itemIndex, arrayLength));
            ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, compareVar, loopEnd));
            var v = Visit(@foreach.Variable) as VariableReference;
            ctx.AddInstruction(Instruction.Create(OpCode.ArrayGetElement, v, collection, itemIndex));
            Visit(@foreach.Body);
            ctx.AddInstruction(Instruction.Create(OpCode.Add, itemIndex, itemIndex, Constant(1)));
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, loopStart));
            ctx.AddInstruction(loopEnd);
            ctx.EndControlBlock();
            return null;
        }

        public object Visit(DoWhileExpression doWhile)
        {
            ctx.BeginControlBlock();
            var loopStart = ctx.CurrentLoopStartLabel;
            var loopEnd = ctx.CurrentLoopEndLabel;
            ctx.AddInstruction(loopStart);
            Visit(doWhile.Body);
            var tester = Visit(doWhile.Test) as VariableReference;
            ctx.AddInstruction(Instruction.Create(OpCode.Jmpt, tester, loopStart));
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, loopEnd));
            ctx.AddInstruction(loopEnd);
            ctx.EndControlBlock();
            return null;
        }

        public object Visit(WhileExpression @while)
        {
            ctx.BeginControlBlock();
            var loopStart = ctx.CurrentLoopStartLabel;
            var loopEnd = ctx.CurrentLoopEndLabel;
            ctx.AddInstruction(loopStart);
            var tester = Visit(@while.Test) as VariableReference;
            ctx.AddInstruction(Instruction.Create(OpCode.Jmpf, tester, loopEnd));
            Visit(@while.Body);
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, loopStart));
            ctx.AddInstruction(loopEnd);
            ctx.EndControlBlock();
            return null;
        }

        public object Visit(LoopExpression loop)
        {
            ctx.BeginControlBlock();
            var loopStart = ctx.CurrentLoopStartLabel;
            var loopEnd = ctx.CurrentLoopEndLabel;
            ctx.AddInstruction(loopStart);
            Visit(loop.Body);
            ctx.AddInstruction(Instruction.Create(OpCode.Jmp, loopStart));
            ctx.AddInstruction(loopEnd);
            ctx.EndControlBlock();
            return null;
        }

        public object Visit(NegateExpression value)
        {
            var target = Visit(value.Expression) as VariableReference;
            var result = TempVariable(XzaarBaseTypes.Number);
            ctx.AddInstruction(Instruction.Create(OpCode.Neg, result, target));
            return result;
        }

        public object Visit(DefaultExpression emptyOrNull)
        {
            return null;
        }

        public object Visit(LogicalNotExpression expr)
        {
            var target = Visit(expr.Expression) as VariableReference;
            var result = TempVariable(XzaarBaseTypes.Boolean);
            ctx.AddInstruction(Instruction.Create(OpCode.Not, result, target));
            return result;
        }

        public object Visit(FunctionCallExpression call)
        {
            var methodName = call.MethodName;
            // check if we are accessing the function by an alias (by reference)
            if (call.RefMethodName != null && call.RefMethodName != call.MethodName)
            {
                methodName = call.RefMethodName;
            }

            var method = ctx.Assembly.GlobalMethods.FirstOrDefault(m => m.Name == methodName);
            var args = new List<VariableReference>();
            foreach (var a in call.Arguments)
            {
                if (Visit(a) is VariableReference vr)
                {
                    args.Add(vr);
                }
            }

            if (method == null)
            {
                // check if this is an direct invocation of a previous one
                // ex: myFunction()();
                if (call.PreviousInvocation != null)
                {
                    // traverse invocation tree and assign result from previous to temp variable
                    // and then call the temp variable
                    // a()()
                    //   ^ ^--- 0                    
                    //   `------1

                    var functionToCall = Visit(call.PreviousInvocation) as VariableReference;
                    var vargs = new List<VariableReference>();
                    var tempReturnVar = TempVariable(XzaarBaseTypes.Any);
                    vargs.Insert(0, tempReturnVar);
                    vargs.Insert(0, functionToCall);
                    var anonCall = Instruction.Create(OpCode.Callunknown, vargs.ToArray());
                    foreach (var a in args) anonCall.OperandArguments.Add(a);
                    ctx.AddInstruction(anonCall);
                    return tempReturnVar;
                }

                // check if this is an anonymous function, if so.
                // use OpCode.Callanonymous and provide the AnonymousFunctionReference as argument
                if (call.AnonymousMethod != null)
                {
                    var anonFuncVar = this.ctx.FindVariable(call.MethodName); // check with aliased name
                    if (anonFuncVar != null && anonFuncVar.Reference is AnonymousFunctionReference funcRef)
                    {
                        var vargs = new List<VariableReference>();
                        var returnValueType = funcRef.ReturnType ?? XzaarBaseTypes.Any;
                        var tempReturnVar = TempVariable(returnValueType);
                        vargs.Insert(0, tempReturnVar);
                        vargs.Insert(0, funcRef);
                        var anonCall = Instruction.Create(OpCode.Callanonymous, vargs.ToArray());
                        foreach (var a in args) anonCall.OperandArguments.Add(a);
                        ctx.AddInstruction(anonCall);
                        return tempReturnVar;
                    }
                }

                VariableReference returnValue = null;
                var instance = call.GetInstance();

                if (instance != null && instance.Type.IsArray && ArrayHelper.IsArrayFunction(methodName))
                {
                    var vargs = new List<VariableReference>();
                    // vargs.Insert(0, anyVar);
                    vargs.Insert(0, Visit(instance) as VariableReference);
                    // vargs.Insert(0, new VariableReference { Name = methodName });
                    Instruction xzaarBinaryCode;
                    if (ArrayHelper.IsArrayClear(methodName))
                        xzaarBinaryCode = Instruction.Create(OpCode.ArrayClearElements, vargs.ToArray());
                    else if (ArrayHelper.IsArrayIndexOf(methodName))
                    {
                        returnValue = TempVariable(XzaarBaseTypes.Number);
                        vargs.Add(returnValue);
                        xzaarBinaryCode = Instruction.Create(OpCode.ArrayIndexOf, vargs.ToArray());
                    }
                    else if (ArrayHelper.IsArrayAdd(methodName)) xzaarBinaryCode = Instruction.Create(OpCode.ArrayAddElements, vargs.ToArray());
                    else if (ArrayHelper.IsArrayInsert(methodName)) xzaarBinaryCode = Instruction.Create(OpCode.ArrayInsertElement, vargs.ToArray());
                    else if (ArrayHelper.IsArrayRemove(methodName)) xzaarBinaryCode = Instruction.Create(OpCode.ArrayRemoveElements, vargs.ToArray());
                    else if (ArrayHelper.IsArrayRemoveLast(methodName)) xzaarBinaryCode = Instruction.Create(OpCode.ArrayRemoveLastElement, vargs.ToArray());
                    else xzaarBinaryCode = Instruction.Create(OpCode.Callmethod, vargs.ToArray());
                    foreach (var a in args) xzaarBinaryCode.OperandArguments.Add(a);
                    ctx.AddInstruction(xzaarBinaryCode);
                    return returnValue;
                }

                if (instance != null && Equals(call.Type, XzaarBaseTypes.Any)) // && Equals(instance.Type, XzaarBaseTypes.Any)
                {
                    // acceptable
                    var vargs = new List<VariableReference>();
                    var anyVar = TempVariable(XzaarBaseTypes.Any);
                    vargs.Insert(0, anyVar);
                    vargs.Insert(0, Visit(instance) as VariableReference);
                    vargs.Insert(0, new VariableReference { Name = methodName });
                    var xzaarBinaryCode = Instruction.Create(OpCode.Callmethod, vargs.ToArray());
                    foreach (var a in args) xzaarBinaryCode.OperandArguments.Add(a);
                    ctx.AddInstruction(xzaarBinaryCode);
                    return anyVar;
                }

                // trying to invoke an "Any" variable.
                // this is fine. It will throw an error on runtime if its invalid.
                if (call.RefMethodName == null && call.Type.IsAny)
                {
                    var anyVar = this.ctx.FindVariable(methodName); // check for a variable.
                    if (anyVar != null)
                    {
                        var vargs = new List<VariableReference>();
                        var tempReturnVar = TempVariable(XzaarBaseTypes.Any);
                        vargs.Insert(0, tempReturnVar);
                        if (anyVar.Type.IsArray && call.Member != null)
                        {
                            // check if we are providing a member access with array index                            
                            var access = Visit(call.Member) as VariableReference;
                            vargs.Insert(0, access);
                        }
                        else
                        {
                            vargs.Insert(0, new VariableReference { Name = methodName });
                        }

                        var unknownCall = Instruction.Create(OpCode.Callunknown, vargs.ToArray());
                        foreach (var a in args) unknownCall.OperandArguments.Add(a);
                        ctx.AddInstruction(unknownCall);
                        return tempReturnVar;
                    }
                }
                return Error("A function with the name '" + methodName + "' could not be found");
            }

            var tmpVar = TempVariable(method.ReturnType);
            args.Insert(0, tmpVar);
            if (method.IsExtern) ctx.AddInstruction(Instruction.Create(OpCode.Callextern, method, args.ToArray()));
            else
            {
                var instance = call.GetInstance();
                if (instance != null)
                {
                    args.Insert(0, Visit(instance) as VariableReference);
                    ctx.AddInstruction(Instruction.Create(OpCode.Callmethod, method, args.ToArray()));
                }
                else
                {
                    ctx.AddInstruction(Instruction.Create(OpCode.Callglobal, method, args.ToArray()));
                }
            }
            return tmpVar;
        }

        public object Visit(ConstantExpression value)
        {
            return Constant(value);
        }

        public object Visit(VariableDefinitionExpression definedVariable)
        {
            var existing = ctx.VariableReferences.FirstOrDefault(v => v.Name == definedVariable.Name);
            if (existing != null) return existing;
            var varRef = new VariableReference(definedVariable.Name, new TypeReference(definedVariable.Type));

            if (ctx.RecordVariableReferencesOnly) ctx.VariableReferences.Add(varRef);
            if (ctx.IsInGlobalScope && !ctx.VariableReferences.Contains(varRef)) ctx.VariableReferences.Add(varRef);
            if (definedVariable.AssignmentExpression != null)
            {
                if (definedVariable.AssignmentExpression.NodeType == ExpressionType.CreateStruct)
                {
                    if (!(definedVariable.AssignmentExpression is CreateStructExpression createStruct))
                    {
                        return Error("Expected assignment to be a struct but it was empty??");
                    }

                    var targetType = ctx.KnownTypes.FirstOrDefault(t => t.Name == createStruct.StructName);
                    ctx.AddInstruction(Instruction.Create(OpCode.StructCreate, varRef, targetType));

                    if (createStruct.FieldInitializers == null)
                    {
                        return varRef;
                    }

                    foreach (var f in createStruct.FieldInitializers)
                    {
                        if (!(f is AssignBinaryExpression assignment))
                        {
                            return Error("Nope! This is suppose to be assigning a struct field... No magic stuff here!");
                        }

                        if (!(assignment.Left is ParameterExpression structField))
                        {
                            return Error("Bad assignment expression");
                        }

                        if (targetType == null) return Error("Bad assignment expression");
                        var arrayIndex = structField.ArrayIndex != null
                            ? Visit(structField.ArrayIndex) as VariableReference : null;

                        var structFieldName = structField.Name;
                        var fieldRef = targetType.Fields.FirstOrDefault(x => x.Name == structFieldName);
                        if (fieldRef == null) return Error("Bad assignment expression");
                        var assignmentExpression = Visit(assignment.Right) as VariableReference;
                        var field = FieldReference(fieldRef.Name, varRef, null, fieldRef.Type, arrayIndex);

                        ctx.AddInstruction(arrayIndex != null
                            ? Instruction.Create(OpCode.StructSet, varRef, field, arrayIndex, assignmentExpression)
                            : Instruction.Create(OpCode.StructSet, varRef, field, assignmentExpression));
                    }
                }
                else
                {
                    var result = Visit(definedVariable.AssignmentExpression) as VariableReference;
                    if (result is Constant constant)
                    {
                        var constantValueArray = constant.Value as object[];
                        var values = constant.Value as XzaarExpression[];
                        if (values == null && constantValueArray != null)
                        {
                            var refs = constantValueArray.Select(x => x as VariableReference);
                            ctx.AddInstruction(Instruction.Create(OpCode.ArrayCreate, varRef));
                            foreach (var value in refs)
                            {
                                var inst = Instruction.Create(OpCode.ArrayAddElements, varRef);
                                inst.OperandArguments.Add(value);
                                ctx.AddInstruction(inst);
                            }
                            return varRef;
                        }
                        if ((constant.Value == null || values != null) && constant.Type.IsArray)
                        {
                            ctx.AddInstruction(Instruction.Create(OpCode.ArrayCreate, varRef));
                            if (values == null) return varRef;
                            foreach (var value in values)
                            {
                                var val = Visit(value) as VariableReference;
                                var inst = Instruction.Create(OpCode.ArrayAddElements, varRef);
                                inst.OperandArguments.Add(val);
                                ctx.AddInstruction(inst);
                            }
                        }
                        else ctx.AddInstruction(Instruction.Create(OpCode.Assign, varRef, constant));
                    }
                    else
                    {
                        if (!(result is FieldReference fieldRef))
                        {
                            ctx.AddInstruction(Instruction.Create(OpCode.Assign, varRef, result));
                            return varRef;
                        }

                        var opCode = OpCode.StructGet;
                        if (fieldRef.Instance.Type.IsArray && ArrayHelper.IsArrayProperty(fieldRef.Name))
                        {
                            // what if we need more properties? then all of them will return the type as number.
                            // TODO: Wrap possible properties into an object that contains both Property Type and Property Name
                            // THEN: var property = ArrayHelper.GetArrayProperty(fieldRef.Name);
                            //       if (varRef.Type.Name != property.Type.Name) varRef.Type = new TypeReference(property.Type);
                            if (varRef.Type.Name != "number") varRef.Type = new TypeReference(XzaarBaseTypes.Number);
                            opCode = OpCode.ArrayLength;
                        }

                        ctx.AddInstruction(Instruction.Create(
                            opCode,
                            varRef,
                            fieldRef.
                            Instance,
                            fieldRef));
                    }
                }
            }
            return varRef;
        }

        public object Visit(LabelExpression label)
        {
            var lbl = Instruction.Label(label.Target.Name);
            ctx.AddInstruction(lbl);
            return lbl;
        }

        public object Visit(ParameterExpression parameter)
        {
            return Variable(parameter);
        }

        public object Visit(LambdaExpression lambda)
        {
            var ofn = ctx.CurrentFunctionName;
            var parameters = lambda.Parameters.Select(x => new ParameterDefinition(Parameter(x))).ToArray();

            ctx.CurrentFunctionName = lambda.AssignmentName;
            ctx.BeginAnonymousFunctionBlock(parameters);

            var methodBody = VisitLambdaBody(lambda.Body);

            var var = ctx.FindVariable(lambda.AssignmentName);
            if (var != null)
            {
                var.Reference = AnonymousFunctionReference(parameters, methodBody);
            }

            ctx.EndAnonymousFunctionBlock();
            ctx.CurrentFunctionName = ofn;

            if (var == null)
            {
                return AnonymousFunctionReference(parameters, methodBody);
            }

            return var;
        }

        public object Visit(FunctionExpression function)
        {
            var method = ctx.Assembly.GlobalMethods.FirstOrDefault(f => f.Name == function.Name);
            ctx.CurrentFunctionName = function.Name;
            if (method == null) return Error("FunctionDefinitionExpression '" + function.Name + "' not found");
            if (method.IsExtern) return method;
            if (ctx.MethodInstructions.Count > 0 && ctx.IsInGlobalScope)
            {
                ctx.GlobalInstructions.AddRange(ctx.MethodInstructions);
                ctx.MethodInstructions.Clear();
            }
            var methodBody = VisitMethodBody(method, function.GetBody());
            if (methodBody == null) return Error("A method body was expected for the function '" + function.Name + "' but was not found");
            method.SetBody(methodBody);
            ctx.CurrentFunctionName = null;
            return method;
        }

        public object Visit(StructExpression node)
        {
            return ctx.Assembly.Types.FirstOrDefault(t => t.Name == node.Name);
        }

        public object Visit(FieldExpression node)
        {
            throw new NotImplementedException();
        }

        public MethodBody VisitLambdaBody(XzaarExpression body)
        {
            ctx.BeginControlBlock();
            var mbody = new MethodBody(null);
            if (body != null)
            {
                var returnValueFromExpression = !(body is BlockExpression); // if its not a blockexpression, we have a one-liner.

                BuildMethodBody(body, mbody);

                if (returnValueFromExpression)
                {
                    // var temp = ctx.TempVariable(body.Type);
                    if (mbody.MethodInstructions.Count > 0)
                    {
                        // ugly hack/fix
                        var instr = mbody.MethodInstructions.Last() as Instruction;

                        var value = GetInstructionOutput(instr);

                        mbody.MethodInstructions.Add(Instruction.Create(OpCode.Return, value));
                    }
                    else
                    {
                        var value = Visit(body) as VariableReference;
                        mbody.MethodInstructions.Add(Instruction.Create(OpCode.Return, value));
                    }
                }
            }
            ctx.EndControlBlock();
            return mbody;
        }

        private VariableReference GetInstructionOutput(Instruction instr)
        {
            switch (instr.OpCode)
            {
                case OpCode.Mod:
                case OpCode.Div:
                case OpCode.Mul:
                case OpCode.Sub:
                case OpCode.Add: return instr.Arguments[0] as VariableReference;
                default: return instr.Arguments.Last() as VariableReference;
            }
        }


        public MethodBody VisitMethodBody(MethodDefinition method, XzaarExpression body)
        {
            ctx.BeginControlBlock();
            var mbody = new MethodBody(method);
            if (body != null) BuildMethodBody(body, mbody);
            ctx.EndControlBlock();
            return mbody;
        }

        private void BuildMethodBody(XzaarExpression expr, MethodBody mbody)
        {
            ctx.VariableReferences.Clear();
            ctx.MethodInstructions.Clear();

            if (expr is BlockExpression block)
            {
                foreach (var c in block.Expressions)
                {
                    FindVariableReferencesAndBuildInstructions(c);
                }
            }
            else
            {
                FindVariableReferencesAndBuildInstructions(expr);
            }

            if (ctx.InsideAnonymousFunctionCount > 0)
            {
                mbody.MethodVariables.AddRange(ctx.CurrentAnonymousFunctionScope.Variables);
                mbody.MethodInstructions.AddRange(ctx.CurrentAnonymousFunctionScope.Instructions);
            }
            else
            {
                foreach (var vr in ctx.VariableReferences)
                {
                    if (!mbody.MethodVariables.ContainsVariable(vr.Name))
                    {
                        mbody.MethodVariables.Add(vr);
                    }
                }

                foreach (var inst in ctx.MethodInstructions)
                {
                    mbody.MethodInstructions.Add(inst);
                }

                ctx.MethodInstructions.Clear();
                ctx.VariableReferences.Clear();
            }
        }

        private void FindVariableReferencesAndBuildInstructions(XzaarExpression xzaarExpression)
        {
            ctx.RecordVariableReferencesOnly = true;
            Visit(xzaarExpression);
            ctx.RecordVariableReferencesOnly = false;
        }

        public VariableReference Constant(int value)
        {
            return ctx.Constant(value);
        }

        public VariableReference Constant(ConstantExpression value)
        {
            var v = value.Value;
            var isArray = false;
            if (value.Value is XzaarExpression[] expressions)
            {
                v = expressions.Select(Visit).ToArray();
                isArray = true;
            }

            return new Constant
            {
                Type = new TypeReference(value.Type),
                Value = v,
                IsArray = isArray,
                ArrayIndex = value.ArrayIndex != null ? Visit(value.ArrayIndex) : null
            };
        }


        public FunctionReference FunctionReference(MethodDefinition method)
        {
            return new FunctionReference(method);
        }

        public AnonymousFunctionReference AnonymousFunctionReference(ParameterDefinition[] parameters, MethodBody body)
        {
            return new AnonymousFunctionReference(parameters, body);
        }

        public VariableReference TempVariable(ConstantExpression constant)
        {
            return ctx.TempVariable(constant);
        }

        public VariableReference TempVariable(int value)
        {
            return ctx.TempVariable(value);
        }

        public VariableReference TempVariable(TypeReference type, VariableReference reference)
        {

            return ctx.TempVariable(type, reference);
        }

        public VariableReference TempVariable(TypeReference type)
        {
            return ctx.TempVariable(type);
        }

        public VariableReference TempVariable(XzaarType type)
        {
            return ctx.TempVariable(type);
        }

        public VariableReference Variable(MemberExpression memberExpression)
        {
            var p = memberExpression.Expression as ParameterExpression;
            return Variable(p);
        }

        private VariableReference Variable(ParameterExpression p)
        {
            if (p != null)
            {
                var vr = ctx.VariableReferences.FirstOrDefault(v => v.Name == p.Name) ?? ctx.FindVariable(p.Name);
                if (vr != null)
                {
                    if (p.ArrayIndex != null) vr.ArrayIndex = Visit(p.ArrayIndex);
                    return vr;
                }

                if (p.Name.StartsWith("$"))
                {
                    return new VariableReference
                    {
                        Name = p.Name,
                        Type = new TypeReference(p.Type),
                        ArrayIndex = p.ArrayIndex != null ? Visit(p.ArrayIndex) : null
                    };
                }


                // check if we are in an anonymous function or not
                if (ctx.InsideAnonymousFunctionCount > 0 && ctx.CurrentAnonymousFunctionScope != null)
                {
                    if (ctx.CurrentAnonymousFunctionScope.TryFind(p.Name, out var param))
                    {
                        return Parameter(p);
                    }
                }

                var currentFunction = ctx.Assembly.GlobalMethods.FirstOrDefault(f => f.Name == ctx.CurrentFunctionName);
                var targetParameter = currentFunction?.Parameters.FirstOrDefault(pa => pa.Name == p.Name);
                if (targetParameter != null)
                {
                    return Parameter(p);
                }

                // if this is a reference to a function, we will have to search for all our methods within scope to see
                // if one of them has a matching name.
                if (p.IsFunctionReference)
                {
                    var func = ctx.Assembly.GlobalMethods.FirstOrDefault(f => f.Name == p.Name);
                    if (func != null)
                    {
                        return FunctionReference(func);
                    }
                }

                return Error("The name '" + p.Name + "' does not exist in the current context");
            }
            return null;
        }

        private VariableReference Parameter(ParameterExpression p)
        {
            return new VariableReference
            {
                Name = p.Name,
                Type = new TypeReference(p.Type),
                ArrayIndex = p.ArrayIndex != null ? Visit(p.ArrayIndex) : null
            };
        }

        private VariableReference Error(string message)
        {
            var msg = "[Error] " + message;
            errors.Add(msg);
            return null;
            // return AstNode.Error(msg);
        }
    }
}
