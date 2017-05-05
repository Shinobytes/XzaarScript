using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Ast.Expressions;
using Shinobytes.XzaarScript.Compiler.Extensions;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler
{
    public class DotNetXzaarScriptCompiler : IScriptCompilerVisitor
    {
        private DotNetXzaarScriptCompilerContext ctx;

        internal DotNetXzaarScriptCompiler(DotNetXzaarScriptCompilerContext ctx)
        {
            this.ctx = ctx;
        }

        public static Delegate Compile(XzaarAnalyzedTree tree)
        {
            var ctx = new DotNetXzaarScriptCompilerContext(tree);
            var discovery = new DotNetXzaarScriptDiscoveryVisitor(ctx);
            var compiler = new DotNetXzaarScriptCompiler(ctx);
            var body = tree.GetExpression();
            discovery.Visit(body);
            compiler.Visit(body);

            return ctx.CreateDelegate();
        }

        public object Visit(XzaarExpression expression)
        {
            if (expression == null || expression is ErrorExpression)
                return null;

            if (expression.NodeType == XzaarExpressionType.Block)
            {
                var block = expression as BlockExpression;
                object finalItem = null;
                if (block != null)
                    foreach (var item in block.Expressions)
                    {
                        finalItem = Visit(item);
                    }
                return finalItem;
            }

#if UNITY
            if (expression is BinaryExpression) return Visit(expression as BinaryExpression);
            if (expression is ConditionalExpression) return Visit(expression as ConditionalExpression);
            if (expression is MemberExpression) return Visit(expression as MemberExpression);
            if (expression is MemberAccessChainExpression) return Visit(expression as MemberAccessChainExpression);
            if (expression is GotoExpression) return Visit(expression as GotoExpression);
            if (expression is SwitchExpression) return Visit(expression as SwitchExpression);
            if (expression is SwitchCaseExpression) return Visit(expression as SwitchCaseExpression);
            if (expression is UnaryExpression) return Visit(expression as UnaryExpression);
            if (expression is BlockExpression) return Visit(expression as BlockExpression);
            if (expression is ForExpression) return Visit(expression as ForExpression);
            if (expression is ForEachExpression) return Visit(expression as ForEachExpression);
            if (expression is DoWhileExpression) return Visit(expression as DoWhileExpression);
            if (expression is WhileExpression) return Visit(expression as WhileExpression);
            if (expression is LoopExpression) return Visit(expression as LoopExpression);
            if (expression is DefaultExpression) return Visit(expression as DefaultExpression);
            if (expression is FunctionCallExpression) return Visit(expression as FunctionCallExpression);
            if (expression is ConstantExpression) return Visit(expression as ConstantExpression);
            if (expression is NegateExpression) return Visit(expression as NegateExpression);
            if (expression is VariableDefinitionExpression) return Visit(expression as VariableDefinitionExpression);
            if (expression is LabelExpression) return Visit(expression as LabelExpression);
            if (expression is ParameterExpression) return Visit(expression as ParameterExpression);
            if (expression is FunctionExpression) return Visit(expression as FunctionExpression);
            if (expression is StructExpression) return Visit(expression as StructExpression);
            if (expression is FieldExpression) return Visit(expression as FieldExpression);
            if (expression is LogicalNotExpression) return Visit(expression as LogicalNotExpression);
            return Visit(expression);
#else
            return Visit((dynamic)expression);
#endif
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
                case XzaarExpressionType.Assign:
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

                case XzaarExpressionType.AndAlso:
                    return LogicalAndAlso(binaryOp);
                case XzaarExpressionType.OrElse:
                    return LogicalOrElse(binaryOp);

                case XzaarExpressionType.LessThan:
                    return CompareLessThan(binaryOp);
                case XzaarExpressionType.LessThanOrEqual:
                    return CompareLessThanOrEquals(binaryOp);
                case XzaarExpressionType.GreaterThan:
                    return CompareGreaterThan(binaryOp);
                case XzaarExpressionType.GreaterThanOrEqual:
                    return CompareGreaterThanOrEquals(binaryOp);
                case XzaarExpressionType.Equal:
                    return CompareEquals(binaryOp);
                case XzaarExpressionType.NotEqual:
                    return CompareNotEquals(binaryOp);

                case XzaarExpressionType.Subtract:
                    return ArithmethicOperation(binaryOp, OpCodes.Sub);
                case XzaarExpressionType.Add:
                    return ArithmethicOperation(binaryOp, OpCodes.Add, binaryOp.Type); // XzaarBaseTypes.Number                    
                case XzaarExpressionType.Multiply:
                    return ArithmethicOperation(binaryOp, OpCodes.Mul);
                case XzaarExpressionType.Divide:
                    return ArithmethicOperation(binaryOp, OpCodes.Div);
                case XzaarExpressionType.Modulo:
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

        public object Visit(ConditionalExpression conditional)
        {
            var il = ctx.GetILGenerator();
            var test = Visit(conditional.Test);
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
            if (conditional.IfTrue != null)
                Visit(conditional.IfTrue);

            Label? endOfFalse = null;
            if (conditional.IfFalse != null)
            {
                // if we do have an 'else'
                // then we want to branch out to end of conditional

                endOfFalse = il.DefineLabel();
                il.BranchToShortForm(endOfFalse.Value);
            }

            // 8. Mark label here
            il.MarkLabel(endOfTrue);


            // 9. Visit IfFalse (if not null)
            if (conditional.IfFalse != null)
            {
                Visit(conditional.IfFalse);

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
                case XzaarGotoExpressionKind.Return:
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
                    break;
                case XzaarGotoExpressionKind.Goto:
                    throw new System.NotImplementedException();
                    break;
                case XzaarGotoExpressionKind.Break:
                    {
                        var il = ctx.GetILGenerator();
                        il.BranchToShortForm(ctx.CurrentScope.CurrentEndLabel);
                        return null;
                    }
                case XzaarGotoExpressionKind.Continue:
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

                case XzaarExpressionType.PostDecrementAssign: return PostDecrementAssign(unary);
                case XzaarExpressionType.Decrement:
                case XzaarExpressionType.PreDecrementAssign: return PreDecrementAssign(unary);
                case XzaarExpressionType.PostIncrementAssign: return PostIncrementAssign(unary);
                case XzaarExpressionType.Increment:
                case XzaarExpressionType.PreIncrementAssign: return PreIncrementAssign(unary);
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
            // return XzaarAstNode.Error(msg);
        }
    }
}


// interesting code, could be of use in the future.
/*
public delegate TResult BinaryOperator<TLeft, TRight, TResult>(TLeft left, TRight right);

/// <summary>
/// Provide efficient generic access to either native or static operators for the given type combination.
/// </summary>
/// <typeparam name="TLeft">The type of the left operand.</typeparam>
/// <typeparam name="TRight">The type of the right operand.</typeparam>
/// <typeparam name="TResult">The type of the result value.</typeparam>
/// <remarks>Inspired by Keith Farmer's code on CodeProject:<br/>http://www.codeproject.com/KB/cs/genericoperators.aspx</remarks>
public static class Operator<TLeft, TRight, TResult> {
    private static BinaryOperator<TLeft, TRight, TResult> addition;
    private static BinaryOperator<TLeft, TRight, TResult> bitwiseAnd;
    private static BinaryOperator<TLeft, TRight, TResult> bitwiseOr;
    private static BinaryOperator<TLeft, TRight, TResult> division;
    private static BinaryOperator<TLeft, TRight, TResult> exclusiveOr;
    private static BinaryOperator<TLeft, TRight, TResult> leftShift;
    private static BinaryOperator<TLeft, TRight, TResult> modulus;
    private static BinaryOperator<TLeft, TRight, TResult> multiply;
    private static BinaryOperator<TLeft, TRight, TResult> rightShift;
    private static BinaryOperator<TLeft, TRight, TResult> subtraction;

    /// <summary>
    /// Gets the addition operator + (either native or "op_Addition").
    /// </summary>
    /// <value>The addition operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> Addition {
        get {
            if (addition == null) {
                addition = CreateOperator("op_Addition", OpCodes.Add);
            }
            return addition;
        }
    }

    /// <summary>
    /// Gets the modulus operator % (either native or "op_Modulus").
    /// </summary>
    /// <value>The modulus operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> Modulus {
        get {
            if (modulus == null) {
                modulus = CreateOperator("op_Modulus", OpCodes.Rem);
            }
            return modulus;
        }
    }

    /// <summary>
    /// Gets the exclusive or operator ^ (either native or "op_ExclusiveOr").
    /// </summary>
    /// <value>The exclusive or operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> ExclusiveOr {
        get {
            if (exclusiveOr == null) {
                exclusiveOr = CreateOperator("op_ExclusiveOr", OpCodes.Xor);
            }
            return exclusiveOr;
        }
    }

    /// <summary>
    /// Gets the bitwise and operator &amp; (either native or "op_BitwiseAnd").
    /// </summary>
    /// <value>The bitwise and operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> BitwiseAnd {
        get {
            if (bitwiseAnd == null) {
                bitwiseAnd = CreateOperator("op_BitwiseAnd", OpCodes.And);
            }
            return bitwiseAnd;
        }
    }

    /// <summary>
    /// Gets the division operator / (either native or "op_Division").
    /// </summary>
    /// <value>The division operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> Division {
        get {
            if (division == null) {
                division = CreateOperator("op_Division", OpCodes.Div);
            }
            return division;
        }
    }

    /// <summary>
    /// Gets the multiplication operator * (either native or "op_Multiply").
    /// </summary>
    /// <value>The multiplication operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> Multiply {
        get {
            if (multiply == null) {
                multiply = CreateOperator("op_Multiply", OpCodes.Mul);
            }
            return multiply;
        }
    }

    /// <summary>
    /// Gets the bitwise or operator | (either native or "op_BitwiseOr").
    /// </summary>
    /// <value>The bitwise or operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> BitwiseOr {
        get {
            if (bitwiseOr == null) {
                bitwiseOr = CreateOperator("op_BitwiseOr", OpCodes.Or);
            }
            return bitwiseOr;
        }
    }

    /// <summary>
    /// Gets the left shift operator &lt;&lt; (either native or "op_LeftShift").
    /// </summary>
    /// <value>The left shift operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> LeftShift {
        get {
            if (leftShift == null) {
                leftShift = CreateOperator("op_LeftShift", OpCodes.Shl);
            }
            return leftShift;
        }
    }

    /// <summary>
    /// Gets the right shift operator &gt;&gt; (either native or "op_RightShift").
    /// </summary>
    /// <value>The right shift operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> RightShift {
        get {
            if (rightShift == null) {
                rightShift = CreateOperator("op_RightShift", OpCodes.Shr);
            }
            return rightShift;
        }
    }

    /// <summary>
    /// Gets the subtraction operator - (either native or "op_Addition").
    /// </summary>
    /// <value>The subtraction operator.</value>
    public static BinaryOperator<TLeft, TRight, TResult> Subtraction {
        get {
            if (subtraction == null) {
                subtraction = CreateOperator("op_Subtraction", OpCodes.Sub);
            }
            return subtraction;
        }
    }

    private static BinaryOperator<TLeft, TRight, TResult> CreateOperator(string operatorName, OpCode opCode) {
        if (operatorName == null) {
            throw new ArgumentNullException("operatorName");
        }
        bool isPrimitive = true;
        bool isLeftNullable;
        bool isRightNullable = false;
        Type leftType = typeof(TLeft);
        Type rightType = typeof(TRight);
        MethodInfo operatorMethod = LookupOperatorMethod(ref leftType, operatorName, ref isPrimitive, out isLeftNullable) ??
                                    LookupOperatorMethod(ref rightType, operatorName, ref isPrimitive, out isRightNullable);
        DynamicMethod method = new DynamicMethod(string.Format("{0}:{1}:{2}:{3}", operatorName, typeof(TLeft).FullName, typeof(TRight).FullName, typeof(TResult).FullName), typeof(TResult),
                                                 new Type[] {typeof(TLeft), typeof(TRight)});
        Debug.WriteLine(method.Name, "Generating operator method");
        ILGenerator generator = method.GetILGenerator();
        if (isPrimitive) {
            Debug.WriteLine("Primitives using opcode", "Emitting operator code");
            generator.Emit(OpCodes.Ldarg_0);
            if (isLeftNullable) {
                generator.EmitCall(OpCodes.Call, typeof(TLeft).GetMethod("op_Explicit", BindingFlags.Public|BindingFlags.Static), null);
            }
            IlTypeHelper.ILType stackType = IlTypeHelper.EmitWidening(generator, IlTypeHelper.GetILType(leftType), IlTypeHelper.GetILType(rightType));
            generator.Emit(OpCodes.Ldarg_1);
            if (isRightNullable) {
                generator.EmitCall(OpCodes.Call, typeof(TRight).GetMethod("op_Explicit", BindingFlags.Public | BindingFlags.Static), null);
            }
            stackType = IlTypeHelper.EmitWidening(generator, IlTypeHelper.GetILType(rightType), stackType);
            generator.Emit(opCode);
            if (typeof(TResult) == typeof(object)) {
                generator.Emit(OpCodes.Box, IlTypeHelper.GetPrimitiveType(stackType));
            } else {
                Type resultType = typeof(TResult);
                if (IsNullable(ref resultType)) {
                    generator.Emit(OpCodes.Newobj, typeof(TResult).GetConstructor(new Type[] {resultType}));
                } else {
                    IlTypeHelper.EmitExplicit(generator, stackType, IlTypeHelper.GetILType(resultType));
                }
            }
        } else if (operatorMethod != null) {
            Debug.WriteLine("Call to static operator method", "Emitting operator code");
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.EmitCall(OpCodes.Call, operatorMethod, null);
            if (typeof(TResult).IsPrimitive && operatorMethod.ReturnType.IsPrimitive) {
                IlTypeHelper.EmitExplicit(generator, IlTypeHelper.GetILType(operatorMethod.ReturnType), IlTypeHelper.GetILType(typeof(TResult)));
            } else if (!typeof(TResult).IsAssignableFrom(operatorMethod.ReturnType)) {
                Debug.WriteLine("Conversion to return type", "Emitting operator code");
                generator.Emit(OpCodes.Ldtoken, typeof(TResult));
                generator.EmitCall(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] {typeof(RuntimeTypeHandle)}), null);
                generator.EmitCall(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] {typeof(object), typeof(Type)}), null);
            }
        } else {
            Debug.WriteLine("Throw NotSupportedException", "Emitting operator code");
            generator.ThrowException(typeof(NotSupportedException));
        }
        generator.Emit(OpCodes.Ret);
        return (BinaryOperator<TLeft, TRight, TResult>)method.CreateDelegate(typeof(BinaryOperator<TLeft, TRight, TResult>));
    }

    private static bool IsNullable(ref Type type) {
        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>))) {
            type = type.GetGenericArguments()[0];
            return true;
        }
        return false;
    }

    private static MethodInfo LookupOperatorMethod(ref Type type, string operatorName, ref bool isPrimitive, out bool isNullable) {
        isNullable = IsNullable(ref type);
        if (!type.IsPrimitive) {
            isPrimitive = false;
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static|BindingFlags.Public)) {
                if (methodInfo.Name == operatorName) {
                    bool isMatch = true;
                    foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                        switch (parameterInfo.Position) {
                        case 0:
                            if (parameterInfo.ParameterType != typeof(TLeft)) {
                                isMatch = false;
                            }
                            break;
                        case 1:
                            if (parameterInfo.ParameterType != typeof(TRight)) {
                                isMatch = false;
                            }
                            break;
                        default:
                            isMatch = false;
                            break;
                        }
                    }
                    if (isMatch) {
                        if (typeof(TResult).IsAssignableFrom(methodInfo.ReturnType) || typeof(IConvertible).IsAssignableFrom(methodInfo.ReturnType)) {
                            return methodInfo; // full signature match
                        }
                    }
                }
            }
        }
        return null;
    }
}

internal static class IlTypeHelper {
    [Flags]
    public enum ILType {
        None = 0,
        Unsigned = 1,
        B8 = 2,
        B16 = 4,
        B32 = 8,
        B64 = 16,
        Real = 32,
        I1 = B8, // 2
        U1 = B8|Unsigned, // 3
        I2 = B16, // 4
        U2 = B16|Unsigned, // 5
        I4 = B32, // 8
        U4 = B32|Unsigned, // 9
        I8 = B64, //16
        U8 = B64|Unsigned, //17
        R4 = B32|Real, //40
        R8 = B64|Real //48
    }

    public static ILType GetILType(Type type) {
        if (type == null) {
            throw new ArgumentNullException("type");
        }
        if (!type.IsPrimitive) {
            throw new ArgumentException("IL native operations requires primitive types", "type");
        }
        if (type == typeof(double)) {
            return ILType.R8;
        }
        if (type == typeof(float)) {
            return ILType.R4;
        }
        if (type == typeof(ulong)) {
            return ILType.U8;
        }
        if (type == typeof(long)) {
            return ILType.I8;
        }
        if (type == typeof(uint)) {
            return ILType.U4;
        }
        if (type == typeof(int)) {
            return ILType.I4;
        }
        if (type == typeof(short)) {
            return ILType.U2;
        }
        if (type == typeof(ushort)) {
            return ILType.I2;
        }
        if (type == typeof(byte)) {
            return ILType.U1;
        }
        if (type == typeof(sbyte)) {
            return ILType.I1;
        }
        return ILType.None;
    }

    public static Type GetPrimitiveType(ILType iLType) {
        switch (iLType) {
        case ILType.R8:
            return typeof(double);
        case ILType.R4:
            return typeof(float);
        case ILType.U8:
            return typeof(ulong);
        case ILType.I8:
            return typeof(long);
        case ILType.U4:
            return typeof(uint);
        case ILType.I4:
            return typeof(int);
        case ILType.U2:
            return typeof(short);
        case ILType.I2:
            return typeof(ushort);
        case ILType.U1:
            return typeof(byte);
        case ILType.I1:
            return typeof(sbyte);
        }
        throw new ArgumentOutOfRangeException("iLType");
    }

    public static ILType EmitWidening(ILGenerator generator, ILType onStackIL, ILType otherIL) {
        if (generator == null) {
            throw new ArgumentNullException("generator");
        }
        if (onStackIL == ILType.None) {
            throw new ArgumentException("Stack needs a value", "onStackIL");
        }
        if (onStackIL < ILType.I8) {
            onStackIL = ILType.I8;
        }
        if ((onStackIL < otherIL) && (onStackIL != ILType.R4)) {
            switch (otherIL) {
            case ILType.R4:
            case ILType.R8:
                if ((onStackIL&ILType.Unsigned) == ILType.Unsigned) {
                    generator.Emit(OpCodes.Conv_R_Un);
                } else if (onStackIL != ILType.R4) {
                    generator.Emit(OpCodes.Conv_R8);
                } else {
                    return ILType.R4;
                }
                return ILType.R8;
            case ILType.U8:
            case ILType.I8:
                if ((onStackIL&ILType.Unsigned) == ILType.Unsigned) {
                    generator.Emit(OpCodes.Conv_U8);
                    return ILType.U8;
                }
                if (onStackIL != ILType.I8) {
                    generator.Emit(OpCodes.Conv_I8);
                }
                return ILType.I8;
            }
        }
        return onStackIL;
    }

    public static void EmitExplicit(ILGenerator generator, ILType onStackIL, ILType otherIL) {
        if (otherIL != onStackIL) {
            switch (otherIL) {
            case ILType.I1:
                generator.Emit(OpCodes.Conv_I1);
                break;
            case ILType.I2:
                generator.Emit(OpCodes.Conv_I2);
                break;
            case ILType.I4:
                generator.Emit(OpCodes.Conv_I4);
                break;
            case ILType.I8:
                generator.Emit(OpCodes.Conv_I8);
                break;
            case ILType.U1:
                generator.Emit(OpCodes.Conv_U1);
                break;
            case ILType.U2:
                generator.Emit(OpCodes.Conv_U2);
                break;
            case ILType.U4:
                generator.Emit(OpCodes.Conv_U4);
                break;
            case ILType.U8:
                generator.Emit(OpCodes.Conv_U8);
                break;
            case ILType.R4:
                generator.Emit(OpCodes.Conv_R4);
                break;
            case ILType.R8:
                generator.Emit(OpCodes.Conv_R8);
                break;
            }
        }
    }
}
Use like this: int i = Operator.Addition(3, 5); 
*/
