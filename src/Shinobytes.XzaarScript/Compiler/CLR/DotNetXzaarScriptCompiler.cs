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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Extensions;
using Shinobytes.XzaarScript.Compiler.Types;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    public class DotNetXzaarScriptCompiler : IScriptCompilerVisitor
    {
        private readonly DotNetXzaarScriptCompilerContext ctx;

        internal DotNetXzaarScriptCompiler(DotNetXzaarScriptCompilerContext ctx)
        {
            this.ctx = ctx;
        }

        public static Delegate Compile(XzaarExpression body)
        {
            var ctx = new DotNetXzaarScriptCompilerContext();
            var discovery = new DotNetXzaarScriptDiscoveryVisitor(ctx);
            var compiler = new DotNetXzaarScriptCompiler(ctx);
            discovery.Visit(body);
            compiler.Visit(body);

            return ctx.CreateDelegate();
        }

        public object Visit(XzaarExpression expression)
        {
            if (expression == null || expression is ErrorExpression)
                return null;

            if (expression.NodeType == ExpressionType.Block)
            {
                object finalItem = null;

                if (!(expression is BlockExpression block))
                {
                    return null;
                }
                foreach (var item in block.Expressions)
                {
                    finalItem = Visit(item);
                }
                return finalItem;
            }

#if UNITY
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
#else
            return Visit((dynamic)expression);
#endif
        }

        public object Visit(ConditionalExpression expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(LogicalNotExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(BinaryExpression binaryOp)
        {
            ctx.LastBinaryOperationType = binaryOp.NodeType;
            ctx.InsideBinaryOperation = true;
            switch (binaryOp.NodeType)
            {
                case ExpressionType.Assign:
                    object returnValue = null;
                    {
                        // if (vRef == null) return Error("Bad binary expression: Left side is missing!");

                        var il = ctx.GetILGenerator();
                        var vRef = Visit(binaryOp.Left);
                        if (vRef is XsField field)
                        {
                            il.LoadThis();
                            var value = Visit(binaryOp.Right); // value
                            TryLoadReference(value, il);
                            il.StoreInField(field);
                            returnValue = value;
                        }
                        else if (vRef is XsVariable variable)
                        {
                            var value = Visit(binaryOp.Right); // value
                            TryLoadReference(value, il);
                            il.StoreInLocal(variable);
                            returnValue = value;
                        }
                        else if (vRef is XsParameter param)
                        {
                            var value = Visit(binaryOp.Right); // value
                            TryLoadReference(value, il);
                            il.StoreInArgument(param);
                            returnValue = value;
                        }



                        //var fRef = vRef as FieldReference;
                        //var fRefRight = value as FieldReference;
                        //if (value is Constant)
                        //{
                        //    var c = value as Constant;
                        //    var arrayInit = c.Value as XzaarExpression[];
                        //    if (arrayInit != null)
                        //    {
                        //        ctx.AddInstruction(Instruction.Create(OpCode.ArrayClearElements, vRef));
                        //        foreach (var item in arrayInit)
                        //        {
                        //            var val = Visit(item) as VariableReference;
                        //            var inst = Instruction.Create(OpCode.ArrayAddElements, vRef);
                        //            inst.OperandArguments.Add(val);
                        //            ctx.AddInstruction(inst);
                        //        }

                        //        insideBinaryOperation = false;
                        //        return vRef;
                        //    }
                        //}

                        //if (fRefRight != null)
                        //{
                        //    value = TempVariable(vRef.Type);
                        //    ctx.AddInstruction(fRefRight.ArrayIndex != null
                        //        ? Instruction.Create(OpCode.StructGet, value, fRefRight.Instance, fRefRight, fRefRight.ArrayIndex as VariableReference)
                        //        : Instruction.Create(OpCode.StructGet, value, fRefRight.Instance, fRefRight));

                        //    // ctx.AddInstruction(Instruction.Create(OpCode.StructGet, value, fRefRight.Instance, fRefRight));
                        //}

                        //if (fRef == null && vRef.IsRef) fRef = vRef.Reference as FieldReference;
                        //if (fRef != null)
                        //{
                        //    ctx.AddInstruction(vRef.ArrayIndex != null
                        //        ? Instruction.Create(OpCode.StructSet, fRef.Instance, fRef, vRef.ArrayIndex as VariableReference, value)
                        //        : Instruction.Create(OpCode.StructSet, fRef.Instance, fRef, value));
                        //    value = fRef;
                        //}
                        //else
                        //{
                        //    if (binaryOp.Left is MemberAccessChainExpression)
                        //    {
                        //        ctx.AddInstruction(vRef.ArrayIndex != null
                        //            ? Instruction.Create(OpCode.StructSet, vRef, vRef.Reference, vRef.ArrayIndex as VariableReference, value)
                        //            : Instruction.Create(OpCode.StructSet, vRef, vRef.Reference, value));
                        //    }
                        //    else
                        //    {
                        //        ctx.AddInstruction(vRef.ArrayIndex != null
                        //            ? Instruction.Create(OpCode.ArraySetElement, vRef, vRef.ArrayIndex as VariableReference, value)
                        //            : Instruction.Create(OpCode.Assign, vRef, value));
                        //    }
                        //    value = vRef;
                        //}

                        ctx.InsideBinaryOperation = false;
                    }
                    return returnValue;

                case ExpressionType.AndAlso:
                    return LogicalAndAlso(binaryOp);
                case ExpressionType.OrElse:
                    return LogicalOrElse(binaryOp);

                case ExpressionType.LessThan:
                    return CompareLessThan(binaryOp);
                case ExpressionType.LessThanOrEqual:
                    return CompareLessThanOrEquals(binaryOp);
                case ExpressionType.GreaterThan:
                    return CompareGreaterThan(binaryOp);
                case ExpressionType.GreaterThanOrEqual:
                    return CompareGreaterThanOrEquals(binaryOp);
                case ExpressionType.Equal:
                    return CompareEquals(binaryOp);
                case ExpressionType.NotEqual:
                    return CompareNotEquals(binaryOp);

                case ExpressionType.Subtract:
                    return ArithmethicOperation(binaryOp, OpCodes.Sub);
                case ExpressionType.Add:
                    return ArithmethicOperation(binaryOp, OpCodes.Add, binaryOp.Type); // XzaarBaseTypes.Number                    
                case ExpressionType.Multiply:
                    return ArithmethicOperation(binaryOp, OpCodes.Mul);
                case ExpressionType.Divide:
                    return ArithmethicOperation(binaryOp, OpCodes.Div);
                case ExpressionType.Modulo:
                    return ArithmethicOperation(binaryOp, OpCodes.Rem);
            }
            return Error(binaryOp.NodeType + " has not been implemented.");
        }

        private object LogicalOrElse(BinaryExpression binaryOp)
        {
            var il = ctx.GetILGenerator();
            var label_afterRight = il.DefineLabel();
            var label_final = il.DefineLabel();
            // <left_expr> || <right_expr>
            // left and right must be both booleans
            // if left is false then we want to test right
            // then return result of right to a temp variable or just as is
            // finally, return the temp variable here

            var left = Visit(binaryOp.Left);
            TryLoadReference(left, il);

            il.BranchIfTrue(label_afterRight);

            var right = Visit(binaryOp.Right);
            TryLoadReference(right, il);

            il.BranchToShortForm(label_final);

            il.MarkLabel(label_afterRight);
            il.Emit(OpCodes.Ldc_I4_1);
            il.MarkLabel(label_final);

            // il.BranchIfTrue()

            // this function should return either a 'ceq' or: 'ldc.i4.1' if any of the statements are true or 'ldc.i4.0' if none are.
            return true;
        }

        private object LogicalAndAlso(BinaryExpression binaryOp)
        {
            // this function should return ceq or: 'ldc.i4.1' if BOTH statements are true. Otherwise just 'ldc.i4.0'
            var il = ctx.GetILGenerator();
            // var label_false = il.DefineLabel();
            var label_final = il.DefineLabel();
            var label_false = il.DefineLabel();

            // visit left, if this one is false. we will need to return false, this can be done by branching directly to the end
            var left = Visit(binaryOp.Left);
            TryLoadReference(left, il);

            // il.Duplicate(); // duplicate our result so we can return it after its consumed by our branch if false
            // ^-- The dup didnt work. maybe I was doing it the wrong way?

            il.BranchIfFalse(label_false);

            var right = Visit(binaryOp.Right);
            TryLoadReference(right, il); // if we got this far, we should return the result from this one.
            il.BranchTo(label_final);

            il.MarkLabel(label_false);
            il.Emit(OpCodes.Ldc_I4_0);

            // before we return, mark the label_final
            il.MarkLabel(label_final);

            return true;
        }

        private object CompareGreaterThan(BinaryExpression binaryOp)
        {
            throw new NotImplementedException();
        }

        private object CompareLessThanOrEquals(BinaryExpression binaryOp)
        {
            throw new NotImplementedException();
        }

        private object CompareLessThan(BinaryExpression binaryOp)
        {
            throw new NotImplementedException();
        }

        private object CompareGreaterThanOrEquals(BinaryExpression binaryOp)
        {
            throw new NotImplementedException();
        }

        private object CompareNotEquals(BinaryExpression binaryOp)
        {
            var il = ctx.GetILGenerator();

            var left = Visit(binaryOp.Left);
            TryLoadReference(left, il);

            var right = Visit(binaryOp.Right);
            TryLoadReference(right, il);

            il.CompareNotEqual();

            ctx.InsideBinaryOperation = false;
            return true;
        }

        private object CompareEquals(BinaryExpression binaryOp)
        {
            var il = ctx.GetILGenerator();

            var left = Visit(binaryOp.Left);
            TryLoadReference(left, il);

            var right = Visit(binaryOp.Right);
            TryLoadReference(right, il);

            il.CompareEqual();

            ctx.InsideBinaryOperation = false;
            return true;
        }

        private object ArithmethicOperation(BinaryExpression binaryOp, OpCode op, XzaarType resultType = null)
        {
            if (resultType == null) resultType = XzaarBaseTypes.Number;

            var il = ctx.GetILGenerator();

            if (resultType.Name == "string")
            {
                // determine which side is a string
                var leftType = binaryOp.Left.Type;
                var rightType = binaryOp.Right.Type;
                var boxLeft = false;
                var boxRight = false;
                // if both are strings, we can just find the normal string,string concat function and call that one.
                // if one of the types are a valuetype (not a class or 'any'), then that type needs to be boxed
                //     then use the object,object concat function instead
                MethodInfo targetConcatMethod = typeof(string).GetMethod("Concat", new[] { typeof(object), typeof(object) });
                if (leftType.Name == "string")
                {
                    if (rightType.Name == "string")
                    {
                        targetConcatMethod = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });
                    }
                    else
                    {
#warning Concating string requires struct's to be boxed. TODO: Add boxing for structs!
                        boxRight = rightType.IsNumeric;
                        // numbers and structs needs to be boxed, right now we don't know if one is a struct or not
                        // check if boxing is necessary on the 'right' type.
                    }
                }
                else
                {
                    boxLeft = leftType.IsNumeric;
                    // check if boxing is necessary on the 'left' type.
                }

                var l = Visit(binaryOp.Left);
                TryLoadReference(l, il);

                if (boxLeft)
                {
                    il.Box(ctx.GetClrType(leftType));
                }

                var r = Visit(binaryOp.Right);
                TryLoadReference(r, il);

                if (boxRight)
                {
                    il.Box(ctx.GetClrType(rightType));
                }

                il.Call(targetConcatMethod);

                return true;
            }



            // NOTE: arithmetic operations does NOT necessarily have to introduce a 
            // temporary variable, but COULD. I decided to skip it for simplicity

            var left = Visit(binaryOp.Left);

            TryLoadReference(left, il);
            // if its not any of the ones above, it must be either a function call or constant

            var right = Visit(binaryOp.Right);

            TryLoadReference(right, il);

            il.Emit(op);

            return true;
        }


        /*
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
                var fRef = left as FieldReference;
                if (fRef != null)
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
                var fRef = right as FieldReference;
                if (fRef != null)
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
             */

        public object Visit(IfElseExpression ifElse)
        {
            var il = ctx.GetILGenerator();
            var test = Visit(ifElse.Test);
            TryLoadReference(test, il);

            // 1. create temp variable
            var tempVariable = ctx.CurrentMethod.DefineVariable(typeof(bool));

            // 2. assign temp variable with value
            il.StoreInLocal(tempVariable);

            // 3. load temp variable
            il.LoadLocal(tempVariable);

            // 4. create a label <end_label> that we want to mark last
            var endOfTrue = il.DefineLabel();

            // 5. brfalse.s <end_label> ---- branch to end label
            il.BranchIfFalse(endOfTrue);

            // 6. Visit IfTrue
            if (ifElse.IfTrue != null)
                Visit(ifElse.IfTrue);

            Label? endOfFalse = null;
            if (ifElse.IfFalse != null)
            {
                // if we do have an 'else'
                // then we want to branch out to end of conditional

                endOfFalse = il.DefineLabel();
                il.BranchToShortForm(endOfFalse.Value);
            }

            // 8. Mark label here
            il.MarkLabel(endOfTrue);


            // 9. Visit IfFalse (if not null)
            if (ifElse.IfFalse != null)
            {
                Visit(ifElse.IfFalse);

                // 10. mark the endOfFalse label
                // ReSharper disable once PossibleInvalidOperationException
                il.MarkLabel(endOfFalse.Value);
            }

            return true;
        }

        public object Visit(MemberExpression access)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(MemberAccessChainExpression access)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(GotoExpression @goto)
        {
            switch (@goto.Kind)
            {
                case GotoExpressionKind.Return:
                    {
                        var returnValue = Visit(@goto.Value);

                        var il = ctx.GetILGenerator();

                        TryLoadReference(returnValue, il);

                        if (ctx.CurrentMethod.MethodInfo.ReturnType == typeof(object))
                        {

                        }

                        il.Return();
                        return null;
                    }                    
                case GotoExpressionKind.Goto:
                    throw new System.NotImplementedException();
                case GotoExpressionKind.Break:
                    {
                        var il = ctx.GetILGenerator();
                        il.BranchToShortForm(ctx.CurrentScope.CurrentEndLabel);
                        return null;
                    }
                case GotoExpressionKind.Continue:
                    {
                        var il = ctx.GetILGenerator();
                        il.BranchToShortForm(ctx.CurrentScope.CurrentStartLabel);
                        return null;
                    }
            }
            throw new System.NotImplementedException();
        }

        public object Visit(SwitchExpression match)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(SwitchCaseExpression matchCase)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(UnaryExpression unary)
        {
            switch (unary.NodeType)
            {

                case ExpressionType.PostDecrementAssign: return PostDecrementAssign(unary);
                case ExpressionType.Decrement:
                case ExpressionType.PreDecrementAssign: return PreDecrementAssign(unary);
                case ExpressionType.PostIncrementAssign: return PostIncrementAssign(unary);
                case ExpressionType.Increment:
                case ExpressionType.PreIncrementAssign: return PreIncrementAssign(unary);
                default:
                    throw new System.NotImplementedException(unary.NodeType + " has not been implemented as a valid UnaryExpression");
            }
        }

        private object PreIncrementAssign(UnaryExpression unary)
        {

            /*
                L_0009: ldloc.0                 
                L_000b: ldc.i4.1 
                L_000c: add 
                L_000d: stloc.0 
                L_000a: ldloc.0
             */
            var il = ctx.GetILGenerator();
            var varRef = Visit(unary.Item);

            if (IsField(varRef)) il.LoadThis();

            TryLoadReference(varRef, il);

            var itemType = unary.Item.Type;
            var itl = itemType.Name.ToLower();
            il.LoadConstantOne(itl);

            il.Add();

            // il.Duplicate();

            TryStoreReference(varRef);
            TryLoadReference(varRef, il);

            return varRef;
        }

        private object PostIncrementAssign(UnaryExpression unary)
        {
            var il = ctx.GetILGenerator();
            var varRef = Visit(unary.Item);
            var itemType = unary.Item.Type;
            var itl = itemType.Name.ToLower();
            var tmpVar = ctx.CurrentMethod.DefineVariable(ctx.GetClrType(itemType));

            if (IsField(varRef)) il.LoadThis();

            TryLoadReference(varRef, il);


            il.StoreInLocal(tmpVar);
            il.LoadLocal(tmpVar);


            il.LoadConstantOne(itl);

            il.Add();

            TryStoreReference(varRef);

            // il.LoadLocal(tmpVar);

            //TryLoadReference(varRef, il);



            return varRef;

            /*
                L_0009: ldloc.0 
                L_000a: dup 
                L_000b: ldc.i4.1 
                L_000c: add 
                L_000d: stloc.0              
             */
        }

        private bool IsField(object varRef)
        {
            return varRef is XsField;
        }

        private object PreDecrementAssign(UnaryExpression unary)
        {
            /*
                L_0009: ldloc.0                 
                L_000b: ldc.i4.1 
                L_000c: sub 
                L_000d: stloc.0 
                L_000a: ldloc.0
             */
            throw new NotImplementedException();
        }

        private object PostDecrementAssign(UnaryExpression unary)
        {            /*
                L_0009: ldloc.0 
                L_000a: dup 
                L_000b: ldc.i4.1 
                L_000c: sub 
                L_000d: stloc.0              
             */
            throw new NotImplementedException();
        }

        public object Visit(BlockExpression block)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(ForExpression @for)
        {

            // 

            throw new System.NotImplementedException();
        }

        public object Visit(ForEachExpression @foreach)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(DoWhileExpression doWhile)
        {

            throw new System.NotImplementedException();
        }

        public object Visit(WhileExpression @while)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(LoopExpression loop)
        {

            // loop {
            //  body
            // }

            // start_label
            //  body
            // goto start_label

            var scope = ctx.BeginControlBlock();

            var il = ctx.GetILGenerator();
            var startLabel = scope.CurrentStartLabel;
            var endLabel = scope.CurrentEndLabel;

            il.MarkLabel(startLabel);

            if (loop.Body != null && !loop.Body.IsEmpty())
            {
                var varRef = Visit(loop.Body);
                // TryLoadReference(varRef);
            }

            il.BranchToShortForm(startLabel);
            il.MarkLabel(endLabel);

            return true;
        }

        public object Visit(DefaultExpression emptyOrNull)
        {
            return ctx.GetILGenerator().Nop();
        }

        public object Visit(FunctionCallExpression call)
        {
            var il = ctx.CurrentMethod.GetILGenerator();
            var methods = ctx.GetCurrentTypeMethods();// ctx.CurrentType.GetMethods();

            var method = methods.FirstOrDefault(m => m.Name == call.MethodName);
            if (method != null)
            {
                il.LoadThis();

                foreach (var a in call.Arguments) Visit(a); // args

                il.CallVirtual(method);

                return null;
            }
            else
            {
                // only functions within the same class is allowed right now
                throw new System.NotImplementedException();
            }
        }

        public object Visit(ConstantExpression constant)
        {
            var type = ctx.GetClrType(constant.Type);

            var value = Convert.ChangeType(constant.Value, type);

            var il = ctx.CurrentMethod.GetILGenerator();

            if (value == null)
            {
                if (type == typeof(object))
                {
                    il.LoadNull();
                    return true;
                }

                return null;
            }

            if (constant.Type.Name.ToLower() == "number")
            {
                il.Emit(OpCodes.Ldc_R8, (double)value);
                return value;
            }

            if (constant.Type.Name.ToLower() == "bool" || constant.Type.Name.ToLower() == "boolean")
            {
                var boolValue = (bool)value;
                il.Emit(boolValue ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                return value;
            }

            if (constant.Type.Name.ToLower() == "string" || constant.Type.Name.ToLower() == "char")
            {
                il.Emit(OpCodes.Ldstr, value?.ToString() ?? "");
                return value;
            }

            throw new System.NotImplementedException();
        }

        public object Visit(NegateExpression value)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(VariableDefinitionExpression definedVariable)
        {
            // 1. check if we can find a reference to a variable with the same name in the same scope
            //    throw if it exists
            var clrType = ctx.GetClrType(definedVariable.Type);
            if (ctx.IsInGlobalScope)
            {
                var field = ctx.DefineField(definedVariable.Name, clrType);
                if (definedVariable.AssignmentExpression != null)
                {
                    var il = ctx.Global.GetILGenerator();
                    il.LoadThis();

                    var valueReference = Visit(definedVariable.AssignmentExpression); // value
                                                                                      // if (valueReference != null)
                                                                                      // {

                    TryLoadReference(valueReference);

                    il.StoreInField(field);
                    // }
                    // field
                }

                return field;
            }

            // define local variable
            var variable = ctx.CurrentMethod.DefineVariable(definedVariable.Name, clrType);
            if (definedVariable.AssignmentExpression != null)
            {
                var il = ctx.CurrentMethod.GetILGenerator();
                var valueReference = Visit(definedVariable.AssignmentExpression); // value
                if (valueReference != null)
                {
                    il.StoreInLocal(variable);
                }
            }

            return variable;
        }

        public object Visit(LabelExpression label)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(ParameterExpression parameter)
        {
            var fields = ctx.GetCurrentTypeFields();
            var field = fields.FirstOrDefault(x => x.Name == parameter.Name);

            // if we are not in the global scope, lets also check for method parameters and locals
            if (!ctx.IsInGlobalScope)
            {
                // ...

                // locals first, as they should "override" the existence of the parameter since they are declared later

                var v = ctx.CurrentMethod.Variables.FirstOrDefault(x => x.Name == parameter.Name);
                if (v != null) return v;

                var p = ctx.CurrentMethod.Parameters.FirstOrDefault(x => x.Name == parameter.Name);
                if (p != null) return p;
            }

            if (field != null)
            {
                //var il = ctx.CurrentMethod.GetILGenerator();
                //il.Emit(OpCodes.Ldarg_0);
                //il.Emit(OpCodes.Ldfld, field);
                return field;
            }

            throw new MissingMemberException(parameter.Name);
        }

        public object Visit(FunctionExpression function)
        {
            var methods = ctx.GetCurrentTypeMethods();
            var method = ctx.CurrentMethod = methods.First(x => x.Name == function.Name);

            if (ctx.GetMethodBodyDefined(method))
            {
                throw new InvalidOperationException($"A function with the same name '{function.Name}' already exists");
            }

            // il.Emit(OpCodes.Ldarg_0);

            var body = Visit(function.GetBody());

            var il = method.GetILGenerator();

            // TODO: Check if previous item was a return or not, because we don't want duplicates
            if (il.LastEmittedOpCode != OpCodes.Ret)
                il.Return();

            ctx.SetMethodBodyDefined(method, true);

            ctx.CurrentMethod = ctx.MainMethod;

            return method;
        }

        public object Visit(StructExpression node)
        {
            throw new System.NotImplementedException();
        }

        public object Visit(FieldExpression node)
        {
            throw new System.NotImplementedException();
        }


        private void TryLoadReference(object value, XsILGenerator il = null)
        {
            if (il == null) il = ctx.GetILGenerator();
            if (value is XsVariable variable)
            {
                il.LoadLocal(variable);
            }
            else if (value is XsField field)
            {
                if (!field.FIeldInfo.IsStatic) il.LoadThis();
                il.LoadField(field);
                ctx.LastLoadedReference = value;
            }
            else if (value is XsParameter param)
            {
                il.LoadArgument(param);
            }
        }

        private void TryStoreReference(object varRef, XsILGenerator il = null)
        {
            if (il == null) il = ctx.GetILGenerator();
            if (varRef is XsField field)
            {
                il.StoreInField(field);
                ctx.LastStoredReference = varRef;
            }
            else if (varRef is XsVariable local)
            {
                il.StoreInLocal(local);
            }
            else if (varRef is XsParameter param)
            {
                il.StoreInArgument(param);
            }
        }


        private object Error(string message)
        {
            var msg = "[Error] " + message;
            ctx.Errors.Add(msg);
            return null;
            // return AstNode.Error(msg);
        }
    }
}