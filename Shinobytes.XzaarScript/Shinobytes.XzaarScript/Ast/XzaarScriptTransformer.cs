using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast.Nodes;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarScriptTransformer : IXzaarScriptTransformer
    {
        /// <summary>
        ///     Transforms the raw Abstract Syntax Tree into a more "comprehensive" one for the compiler to use.
        /// </summary>
        /// <param name="ast">The ast.</param>
        /// <returns></returns>
        public EntryNode Transform(EntryNode ast)
        {
            var ctx = new XzaarExpressionTransformerContext();
            var nodeCount = ast.Children.Count;
            var programNodes = new List<XzaarNode>();

            while (ctx.CurrentNodeIndex < nodeCount)
            {
                WalkNodes(ctx, ast.Children);
                if (ctx.CurrentNodeIndex >= nodeCount) break;
            }

            var globalVars = ctx.GetGlobalVariables();
            if (globalVars.Count > 0) programNodes.AddRange(globalVars);
            programNodes.AddRange(ctx.GlobalScope.Structs);
            foreach (var fn in ctx.Stack)
            {
                var f = fn as FunctionNode;
                if (f != null)
                {
                    ctx.AddGlobalFunction(f);
                    continue;
                }

            }

            if (ctx.Functions.Count > 0) programNodes.AddRange(ctx.Functions);
            // var reversedStack = new Stack<XzaarNode>(ctx.Stack.Reverse());

            foreach (var fn in ctx.Stack.Reverse())
            {
                if (fn != null && !(fn is FunctionNode)) programNodes.Add(fn);
            }

            //while (ctx.Stack.Count > 0) programNodes.Add(ctx.Stack.Pop());

            // AddScopeVariables(ctx, ref programNodes);

            if (programNodes.Count > 1)
            {
                var blockNode = XzaarNode.Block(XzaarSyntaxTokenPositionInfo.Empty, XzaarNode.Block(XzaarSyntaxTokenPositionInfo.Empty, programNodes.ToArray()));
                return XzaarScriptBlockSorter.SortBlocks(XzaarScriptBlockReducer.ReduceBlocks(new EntryNode(blockNode, XzaarSyntaxTokenPositionInfo.Empty)));
            }
            if (programNodes.Count > 0)
            {
                return XzaarScriptBlockSorter.SortBlocks(XzaarScriptBlockReducer.ReduceBlocks(new EntryNode(programNodes[0], XzaarSyntaxTokenPositionInfo.Empty)));
            }
            return new EntryNode(XzaarNode.Empty(XzaarSyntaxTokenPositionInfo.Empty), new XzaarSyntaxTokenPositionInfo());
        }

        private void AddScopeVariables(XzaarExpressionTransformerContext ctx, ref List<XzaarNode> n)
        {
            var nodes = n.ToList();
            // create a new global scope, we will use this one to identify scope names


            foreach (var node in nodes)
            {
                if (node is FunctionNode)
                {
                    var f = node as FunctionNode;
                    var b = f.Body;

                    var scope = ctx.GetScopeByNode(node);

                    if (scope.Variables.Count > 0)
                    {
                        // add variable in upper local scope of the function
                        // if the function's body is not a block or (body) then we need to move
                        // the child into a block node and insert the Variable Definition at the top.
                        throw new NotImplementedException();
                    }

                    AddScopeVariables(ctx, ref b);
                }
            }
        }

        private void AddScopeVariables(XzaarExpressionTransformerContext ctx, ref XzaarNode node)
        {
            var scope = ctx.GetScopeByNode(node);

            if (scope != null && scope.Variables.Count > 0)
            {
                if (!(node is BlockNode))
                {
                    var newBlockBody = scope.Variables.Cast<XzaarNode>().ToList();
                    newBlockBody.Add(node);
                    node = XzaarNode.Block(node.PositionInfo, newBlockBody.ToArray());
                }
                else
                {
                    for (var i = 0; i < scope.Variables.Count; i++)
                        node.InsertChild(i, scope.Variables[i]);
                }

                scope.Variables.Clear();
            }

            if (node is BlockNode)
            {
                for (var i = 0; i < node.Children.Count; i++)
                {
                    var child = node.Children[i];
                    AddScopeVariables(ctx, ref child);
                    node.Children[i] = child;
                }
            }
            else if (node is ConditionalNode)
            {
                var conditional = node as ConditionalNode;
                var t = conditional.GetTrue();
                var f = conditional.GetFalse();
                // if (!t.IsEmpty() || !f.IsEmpty())
                {
                    AddScopeVariables(ctx, ref t);
                    conditional.SetIfTrue(t);

                    AddScopeVariables(ctx, ref f);
                    conditional.SetIfFalse(f);
                }
            }
            else if (node is LoopNode)
            {
                var loop = node as LoopNode;
            }
        }

        private void Walk(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node, bool expressionOrFunctionCallOnly = false)
        {
            if (ctx.CurrentNodeIndex >= nodes.Count)
            {
                return;
            }

            if (expressionOrFunctionCallOnly)
            {
                if ((node.NodeType != XzaarNodeTypes.CALL || node.NodeType != XzaarNodeTypes.EXPRESSION) &&
                   !(node.NodeType == XzaarNodeTypes.LITERAL && ctx.CurrentNodeIndex + 1 < nodes.Count && nodes[ctx.CurrentNodeIndex + 1].NodeType == XzaarNodeTypes.EXPRESSION))
                    return;
            }

            node.Walked = true;
            switch (node.NodeType)
            {
                case XzaarNodeTypes.ACCESSOR:
                    WalkAccessor(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.CASE:
                    WalkSwitchCase(ctx, nodes, node);
                    return;
                case XzaarNodeTypes.ARRAYINDEX:
                case XzaarNodeTypes.BRACKET:
                    WalkArrayIndexer(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.GOTO:
                    WalkGoTo(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.CONTINUE:
                    WalkContinue(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.BREAK:
                    WalkBreak(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.RETURN:
                    WalkReturn(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.COLON:
                    WalkColon(ctx);
                    return;

                case XzaarNodeTypes.LITERAL:
                    WalkLiteral(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.DECLARATION:
                    WalkDeclaration(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.LOOP:
                    WalkLoop(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.CONDITION:
                    WalkCondition(ctx, nodes);
                    return;

                case XzaarNodeTypes.MATCH:
                    WalkMatch(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.BODY:
                    WalkBody(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.EXPRESSION:
                    WalkExpression(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.ASSIGNMENT_OPERATOR:
                    WalkAssignment(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.UNARY_OPERATOR:
                    WalkUnary(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.CONDITIONAL_OPERATOR:
                    WalkConditionalOperator(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.EQUALITY_OPERATOR:
                    WalkEquality(ctx, nodes, node);
                    return;

                case XzaarNodeTypes.ARITHMETIC_OPERATOR:
                    WalkArithmetic(ctx, nodes, node);
                    return;
                case XzaarNodeTypes.NEGATE_OPERATOR:
                    ctx.NegateNext = !ctx.NegateNext;
                    ctx.CurrentNodeIndex++;
                    break;
            }
        }

        private void WalkAccessor(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // a.b.c.d.e = something    
            // accessors can be chained infinite amount of times. herpaderpa        
            // build from stack here.
            // first one will be normal member access
            // we should probably let this function walk through all accessors, meaning we should keep walking the nodes until we can find a possible end            
            // build accessor chain
            node.Walked = true;
            var lastAccessorItem = ctx.Stack.Peek();
            lastAccessorItem.Walked = true;
            // find next accessor member until no accessor type can be found
            var isExternalVariable = false;
            var isVariable = false;
            var isAcceptableConstant = false;
            if (lastAccessorItem.NodeType == XzaarNodeTypes.LITERAL)
            {
                // literals are not acceptable as-is, but this could be pointing to a parameter. So we gotta do a lookup
                var parameter = ctx.FindParameterInScope(lastAccessorItem.Value + "");
                if (parameter != null)
                {
                    isExternalVariable = parameter.IsExtern;
                    ctx.Stack.Pop();
                    ctx.Stack.Push(XzaarNode.MemberAccess(node.PositionInfo, lastAccessorItem, null, parameter.Type));
                }
                if (parameter == null)
                {
                    var variable = ctx.FindVariableInScope(lastAccessorItem.Value + "");

                    isVariable = variable != null;
                }
                // check if this is a string constant
                if (lastAccessorItem.NodeName == "STRING")
                {
                    ctx.Stack.Pop();
                    ctx.Stack.Push(XzaarNode.MemberAccess(node.PositionInfo, lastAccessorItem, null, XzaarBaseTypes.String.Name));
                    isAcceptableConstant = true;
                }
                // lastAccessorItem.Value
            }
            if (!isExternalVariable && !isVariable && !isAcceptableConstant)
            {
                if (lastAccessorItem.NodeType != XzaarNodeTypes.ACCESS && lastAccessorItem.NodeType != XzaarNodeTypes.CALL && lastAccessorItem.NodeName != "ARRAY")
                    throw new XzaarExpressionTransformerException(
                        "Uh nuh! I don't think the item you're trying to use has any properties or available functions. What is a '" +
                        lastAccessorItem.Value + "' anyway?");

                if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                    throw new XzaarExpressionTransformerException(
                        "Look, if you're going to try and access a member of the target instanced item then you better let us know exactly what you want.");
            }

            var oldTypeRestriction = ctx.IgnoreTypeRestrictions;
            while (ctx.CurrentNodeIndex < nodes.Count)
            {
                lastAccessorItem = ctx.Stack.Pop();
                lastAccessorItem.Walked = true;
                // check if accessor item is of type 'any' because then the next item can be an unknown function call or member access
                if (lastAccessorItem.NodeType == XzaarNodeTypes.ACCESS)
                {
                    var a = lastAccessorItem as MemberAccessNode;
                    if (a != null)
                    {
                        ctx.IgnoreTypeRestrictions = a.MemberType == "any";
                        if (a.MemberType == "array" || a.MemberType.EndsWith("[]"))
                        {
                            var next = nodes[ctx.CurrentNodeIndex + 1];
                            var target = next.Value + "";
                            if (ArrayHelper.IsArrayFunction(target))
                            {
                                ctx.IgnoreTypeRestrictions = true;
                            }
                        }
                        else if (a.MemberType == "string")
                        {
                            var next = nodes[ctx.CurrentNodeIndex + 1];
                            var target = next.Value + "";
                            if (StringHelper.IsStringFunction(target))
                            {
                                ctx.IgnoreTypeRestrictions = true;
                            }
                        }
                    }
                }
                else
                {
                    if (lastAccessorItem.NodeName == "ARRAY")
                        ctx.IgnoreTypeRestrictions = true;
                }
                var nextAccessorItem = nodes[++ctx.CurrentNodeIndex];
                //if (nextAccessorItem.NodeType == XzaarNodeTypes.ACCESSOR)
                //    throw new XzaarExpressionTransformerException(
                //        "Invalid accessor expression: You cannot access an accessor, typo or did you forget to enter a member? Anyway, this is just weird. '" +
                //        lastAccessorItem.Value + "..'");

                var previousStackCount = ctx.Stack.Count;
                Walk(ctx, nodes, nextAccessorItem);
                var newStackItems = ctx.Stack.Count - previousStackCount;

                // if no new stack items were added in previous steps, then we need to add it now
                var memAcc = lastAccessorItem as MemberAccessNode;
                if (newStackItems == 0 && memAcc != null && ctx.IgnoreTypeRestrictions)
                {
                    if (memAcc.MemberType == "any")
                    {
                        newStackItems++;

                        // nodes.Count
                    }
                    // 
                }

                if (newStackItems > 0)
                {
                    nextAccessorItem = ctx.Stack.Pop();
                    if (nextAccessorItem.NodeType == XzaarNodeTypes.LITERAL)
                    {
                        // check if we want to access an array..
                        // -- but not now..
                        // access a variable/field/property member



                        var lastMemberAccess = lastAccessorItem as MemberAccessNode;
                        if (lastMemberAccess != null)
                        {
                            var memberType = FindMemberType(ctx, lastMemberAccess.MemberType, nextAccessorItem);

                            if (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.ARRAYINDEX)
                            {
                                // we want to access an array item, so we will need to supply the array indexer
                                ctx.Stack.Push(nextAccessorItem);
                                WalkArrayIndexer(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                var popped = ctx.Stack.Pop();
                                var item = popped as MemberAccessNode;
                                if (item != null)
                                {
                                    var memberAccess = XzaarNode.MemberAccess(item.PositionInfo, item.Member,
                                        item.ArrayIndex, lastMemberAccess.MemberType, memberType);
                                    var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo,
                                        lastAccessorItem, memberAccess);
                                    ctx.Stack.Push(memberAccessChain);
                                }
                                else
                                {
                                    if (popped.NodeType == XzaarNodeTypes.ASSIGN)
                                    {
                                        var assign = popped as AssignNode;
                                        var access = assign.Left as MemberAccessNode;
                                        var memberAccess = XzaarNode.MemberAccess(access.PositionInfo, access.Member, access.ArrayIndex, lastMemberAccess.MemberType, memberType);
                                        var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, memberAccess);
                                        var newAssign = XzaarNode.Assign(lastAccessorItem.PositionInfo,
                                            memberAccessChain, assign.Right
                                        );
                                        ctx.Stack.Push(newAssign);
                                    }
                                    else
                                    {
                                        throw new XzaarExpressionTransformerException("Not implemented unfortunately");
                                    }
                                }
                            }
                            else
                            {

                                var memberAccess = XzaarNode.MemberAccess(nextAccessorItem.PositionInfo, nextAccessorItem, lastMemberAccess.MemberType, memberType);
                                var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, memberAccess);
                                ctx.Stack.Push(memberAccessChain);
                            }
                        }
                        else
                        {
                            var accessChain = lastAccessorItem as MemberAccessChainNode;
                            if (accessChain != null)
                            {
                                var memberType = FindMemberType(ctx, accessChain.ResultType, nextAccessorItem);
                                var memberAccess = XzaarNode.MemberAccess(nextAccessorItem.PositionInfo, nextAccessorItem, accessChain.ResultType,
                                    memberType);
                                var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, memberAccess);
                                ctx.Stack.Push(memberAccessChain);
                            }
                            else
                            {
                                var fncCall = lastAccessorItem as FunctionCallNode;
                                if (fncCall != null)
                                {
                                    var fnc = ctx.Functions.FirstOrDefault(f => f.Name == fncCall.Function.Value + "");
                                    if (fnc != null)
                                    {
                                        var retType = fnc.GetReturnType();
                                        var memberAccess = XzaarNode.MemberAccess(nextAccessorItem.PositionInfo, nextAccessorItem, retType.ToString(), retType.ToString());
                                        var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, memberAccess);
                                        ctx.Stack.Push(memberAccessChain);
                                    }
                                    // TODO: try get return type of the target function to determine whether this accessor is valid or not

                                }
                                else
                                {
                                    if (ctx.IgnoreTypeRestrictions)
                                    {
                                        var memberAccess = XzaarNode.MemberAccess(nextAccessorItem.PositionInfo, nextAccessorItem, "any", "any");
                                        var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, memberAccess);
                                        ctx.Stack.Push(memberAccessChain);
                                    }
                                    else
                                    {
                                        throw new XzaarExpressionTransformerException("The object '" + lastAccessorItem.Value + "' does not expose any properties or functions you can use. At line " + lastAccessorItem.PositionInfo.Line);
                                    }
                                }
                            }
                        }
                    }
                    else if (nextAccessorItem.NodeType == XzaarNodeTypes.CALL)
                    {
                        // access a function/method member                        
                        var memberAccess = nextAccessorItem as FunctionCallNode;
                        var returnType = "any";
                        // TODO: find a way to get the return type of a function more accurately. Dynamically determing the return type just wont do... God damnit!
                        var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, memberAccess, returnType);
                        ctx.Stack.Push(memberAccessChain);
                    }
                    else
                    {
                        // access a what?... unfortunately, this is somewhat unhandled but there are cases that are valid
                        // for instance, if the nextAccessorItem is a Math type, it is completely valid

                        if (nextAccessorItem is BinaryOperatorNode)
                        {
                            // we need to break out the binary operation
                            // and re-assign new nodes
                            // from:
                            //   Access: <lastAccessor> <Math: <targetAccessor> op <expression>>
                            // to
                            //   Math: <Access: <lastAccessor> <targetAccessor>> op <expression>
                            var binop = nextAccessorItem as BinaryOperatorNode;

                            var access = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, binop.Left, "any");
                            var math = XzaarNode.BinaryOperator(lastAccessorItem.PositionInfo, binop.OperatingOrder,
                                access, binop.Op, binop.Right);
                            ctx.Stack.Push(math);
                        }
                        else
                        {
                            var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem.PositionInfo, lastAccessorItem, nextAccessorItem, "any");
                            ctx.Stack.Push(memberAccessChain);
                        }


                        //throw new XzaarExpressionTransformerException("Bad accessor expression: I don't even... '" +
                        //                                              nextAccessorItem.Value +
                        //                                              " is not a valid member of '" +
                        //                                              lastAccessorItem.Value + "'");
                    }
                }
                else
                {
                    // not sure if this is acceptable or not..
                    throw new XzaarExpressionTransformerException("Bad accessor expression: I don't even... '" +
                                              nextAccessorItem.Value +
                                              "' is not a valid member of '" +
                                              lastAccessorItem.Value + "'");
                }

                if (ctx.CurrentNodeIndex >= nodes.Count || nodes[ctx.CurrentNodeIndex].NodeType != XzaarNodeTypes.ACCESSOR)
                {
                    var the = ctx.Stack.Peek();
                    ctx.IgnoreTypeRestrictions = oldTypeRestriction;
                    if (ctx.CurrentNodeIndex < nodes.Count && CanBeChained(the, nodes[ctx.CurrentNodeIndex]))
                    {
                        Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    }

                    return;
                }
            }



            throw new NotImplementedException();
        }

        private static string FindMemberType(
            XzaarExpressionTransformerContext ctx,
            string lastMemberType,
            XzaarNode nextAccessorItem)
        {

            if (lastMemberType == "string")
            {
                var targetField = nextAccessorItem.Value + "";
                if (StringHelper.IsStringProperty(targetField))
                {
                    if (StringHelper.IsLengthProperty(targetField))
                        return "number";
                    return "any";
                }
                if (StringHelper.IsStringFunction(targetField))
                {
                    if (StringHelper.IsCharAt(targetField)) return "string";
                    if (StringHelper.IsIndexOf(targetField)) return "number";
                    return "any";
                }
            }

            if (lastMemberType == "array" || lastMemberType.EndsWith("[]"))
            {
                var targetField = nextAccessorItem.Value + "";
                if (ArrayHelper.IsArrayProperty(targetField))
                {
                    if (ArrayHelper.IsArrayLengthProperty(targetField))
                        return "number";
                    return "any";
                }
                if (ArrayHelper.IsArrayFunction(targetField))
                {
                    if (ArrayHelper.IsArrayAdd(targetField)) return "void";
                    if (ArrayHelper.IsArrayRemove(targetField)) return "void";
                    if (ArrayHelper.IsArrayRemoveLast(targetField)) return "void";
                    if (ArrayHelper.IsArrayClear(targetField)) return "void";
                    if (ArrayHelper.IsArrayInsert(targetField)) return "void";
                    if (ArrayHelper.IsIndexOf(targetField)) return "number";
                    return "any";
                }
            }

            var memberType = "";
            var existingStruct = ctx.Structs.FirstOrDefault(s => s.Name == lastMemberType);
            if (existingStruct != null)
            {
                var field =
                    existingStruct.Fields.Cast<FieldNode>()
                        .FirstOrDefault(f => f.Name == nextAccessorItem.Value + "");
                if (field != null)
                {
                    memberType = field.Type;
                }
            }
            return memberType;
        }

        private void WalkNodes(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes)
        {
            if (ctx.CurrentNodeIndex >= nodes.Count)
            {
                return;
            }

            var node = nodes[ctx.CurrentNodeIndex];

            if (node.IsTransformed)
            {
                ctx.CurrentNodeIndex++;
                return;
            }
            //if (node.Walked)
            //{
            //    ctx.CurrentNodeIndex++;
            //    return;
            //}

            Walk(ctx, nodes, node);
            // node.Walked = true;
        }

        private void WalkArrayIndexer(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // previous item should be the variable (from stack)
            // this one is '['
            // next one is Indexer (can be expression)
            // last one is ']'
            // finally, this should be pushed as member access

            var isArrayIndex = node is ArrayIndexNode;
            if (ctx.Stack.Count == 0)
            {
                // array initializer

                var values = new List<XzaarNode>();
                var arrayIndex = node as ArrayIndexNode;
                if (arrayIndex != null && arrayIndex.Children.Count > 0)
                {
                    values.AddRange(WalkArrayInitializerValues(ctx, nodes, arrayIndex));
                    var newValue = XzaarNode.NewArrayInstance(node.PositionInfo, values);
                    ctx.Stack.Push(newValue);
                    ctx.CurrentNodeIndex += 1;
                    return;
                }
                else
                {
                    ctx.Stack.Push(XzaarNode.NewArrayInstance(node.PositionInfo, new List<XzaarNode>()));
                    ctx.CurrentNodeIndex += 1;
                    return;
                }
            }
            else
            {
                // array indexer
                var variable = ctx.Stack.Pop();
                var before = ctx.CurrentNodeIndex;
                var arrayIndex = isArrayIndex ? nodes[ctx.CurrentNodeIndex] : nodes[++ctx.CurrentNodeIndex];

                arrayIndex = TraverseRightSide(ctx, nodes, arrayIndex);

                // Checke if we progressed in our token list or not
                if (before == ctx.CurrentNodeIndex)
                {
                    ctx.CurrentNodeIndex++;
                }

                ctx.Stack.Push(XzaarNode.VariableAccess(node.PositionInfo, variable, arrayIndex));

                if (ctx.CurrentNodeIndex < nodes.Count)
                {
                    var next = nodes[ctx.CurrentNodeIndex];
                    var current = ctx.Stack.Peek();
                    if (CanBeChained(current, next))
                    {
                        Walk(ctx, nodes, next);
                    }
                }


                if (!isArrayIndex) ctx.CurrentNodeIndex++;
            }
        }

        private void WalkReturn(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            node.Walked = true;
            AdvanceToNextUnwalked(ctx, nodes);
            // ctx.CurrentNodeIndex++;

            // check if there is a possible 'return' expression on the next node. otherwise just ignore it

            if (ctx.CurrentNodeIndex < nodes.Count)
            {
                var nextNode = nodes[ctx.CurrentNodeIndex];
                var nextNodeBefore = nodes[ctx.CurrentNodeIndex];

                if (CanBeChained(node, nextNode))
                {
                    if (nextNode.NodeName == "NUMBER")
                    {
                        nextNode = TraverseRightSide(ctx, nodes, nextNode);
                    }
                    else
                    {
                        nextNode = TraverseExtractAndAddDeclaredVariables(ctx, nodes, nextNode);
                    }

                    AdvanceToNextUnwalked(ctx, nodes);

                    //if (ctx.Stack.Count > 0 && ctx.Stack.Peek() == nextNodeBefore)
                    //{
                    //    // ctx.Stack.Pop();
                    //    Walk(ctx, nodes, nextNodeBefore);
                    //}


                    if (nextNode != null)
                    {
                        nextNode.Walked = true;
                        ctx.Stack.Push(XzaarNode.Return(node.PositionInfo, nextNode));
                        AdvanceToNextUnwalked(ctx, nodes);
                        return;
                    }
                }
            }
            ctx.Stack.Push(XzaarNode.Return(node.PositionInfo));
            AdvanceToNextUnwalked(ctx, nodes);
        }

        private void WalkBreak(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            ctx.CurrentNodeIndex++;

            ctx.Stack.Push(XzaarNode.Break(node.PositionInfo));
        }

        private void WalkContinue(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            ctx.CurrentNodeIndex++;

            ctx.Stack.Push(XzaarNode.Continue(node.PositionInfo));
        }

        private void WalkGoTo(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // goto <label_name>

            var labelTarget = nodes[++ctx.CurrentNodeIndex];
            var literal = labelTarget as LiteralNode;
            if (literal == null)
                throw new XzaarExpressionTransformerException("Expected a literal label name after the 'goto' keyword.");

            ctx.Stack.Push(XzaarNode.Goto(node.PositionInfo, literal.Value + ""));
            ctx.CurrentNodeIndex++;
        }
        private void WalkDeclaration(
                                     XzaarExpressionTransformerContext ctx,
                                     IList<XzaarNode> nodes,
                                     XzaarNode node)
        {
            // the of independent
            switch (node.NodeName)
            {
                case "STRUCT":
                    WalkStruct(ctx, nodes, node);
                    break;
                case "VARIABLE":
                    WalkVariable(ctx, nodes);
                    break;
                case "EXTERN":
                    WalkExternMember(ctx, nodes);
                    break;
                case "FUNCTION":
                    WalkFunction(ctx, nodes);
                    break;
                default:
                    ctx.CurrentNodeIndex++;
                    break;
            }
        }

        private void WalkStruct(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            /*
                struct MyAwesomeStruct {
                  number myNumberA
                  string myStringB
                  object myObjectC
                }             
             */

            if (nodes[ctx.CurrentNodeIndex + 1].NodeType != XzaarNodeTypes.LITERAL)
                throw new XzaarExpressionTransformerException("Expected a name for the beautiful newly defined struct... But no such thing was found.");

            if (!(nodes[ctx.CurrentNodeIndex + 2] is BlockNode))
                throw new XzaarExpressionTransformerException("Hey wait! No 'body' was found for your struct.. Take another look so you havnt missed anything.");

            var structNameNode = nodes[++ctx.CurrentNodeIndex];
            var structBody = nodes[++ctx.CurrentNodeIndex] as BlockNode;
            var structName = structNameNode.Value + "";

            var structFields = new List<XzaarNode>();
            if (structBody.Children.Count > 0)
            {
                // yay
                // if one field is an array then the following check is no longer valid
                //if (structBody.Children.Count % 2 != 0)
                //    throw new XzaarExpressionTransformerException("Seems like you missed something in your struct '" + structName + "' body.");

                for (var i = 0; i < structBody.Children.Count;)
                {

                    var type = structBody.Children[i++].Value + "";
                    var nameOrArrayNode = structBody.Children[i];
                    if (nameOrArrayNode.NodeType == XzaarNodeTypes.ARRAYINDEX)
                    {
                        type += "[]";
                        i++;
                    }
                    var name = structBody.Children[i++].Value + "";

                    structFields.Add(XzaarNode.Field(node.PositionInfo, type, name, structName));

                    // skip next node if its a semicolon or comma as they are both acceptable deliminators for the struct fields
                    if (i < structBody.Children.Count)
                    {
                        var peek = structBody.Children[i];
                        var strValue = peek.Value + "";
                        if (strValue == ";" || strValue == ",") i++;
                    }
                }
            }

            var s = XzaarNode.Struct(node.PositionInfo, structName, structFields.ToArray());
            ctx.AddGlobalStruct(s);
            ctx.SetScopeNode(s);
            ctx.CurrentNodeIndex++;
        }

        private void WalkVariable(
                              XzaarExpressionTransformerContext ctx,
                              IList<XzaarNode> nodes)
        {
            var node = nodes[ctx.CurrentNodeIndex];
            var next = nodes[++ctx.CurrentNodeIndex];
            node.Walked = true;
            var negate = ctx.NegateNext;
            // we expect the next node to be a name node as we want to define the name of the variable
            if (next.NodeName == "NAME")
            {
                var variableName = next.Value?.ToString();
                if (string.IsNullOrEmpty(variableName))
                    throw new XzaarExpressionTransformerException(
                        "Variable cannot have a empty name. How the heck did this happen anyway?");

                next.Walked = true;

                // check if the one after the name is an assignment or not.
                // the assignment is kinda important if we want to be aware of the type right away.

                string variableType = null;
                if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                {
                    var peeked = nodes[ctx.CurrentNodeIndex + 1];
                    var assignValue = peeked;
                    if ((peeked.NodeType == XzaarNodeTypes.ASSIGNMENT_OPERATOR ||
                         peeked.NodeType == XzaarNodeTypes.ASSIGN)
                        && peeked.NodeName == "ASSIGN")
                    {
                        ctx.CurrentNodeIndex++;
                        // assume we are talking about direct assignment "=" and not +=, /=, etc                        
                        // peek once more to see if we can get a value we want to assign with
                        // be it a function call, a constant, another variable. mehps :)


                        if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                        {
                            peeked = nodes[++ctx.CurrentNodeIndex];

                            assignValue = peeked;

                            while (peeked.NodeType == XzaarNodeTypes.NEGATE_OPERATOR)
                            {
                                // advance to next node as we want to negate anything that comes after                                
                                if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                                    throw new XzaarExpressionTransformerException("Unexpected end of file.");
                                peeked = nodes[++ctx.CurrentNodeIndex];
                                negate = !negate;
                            }
                            assignValue = peeked;

                            if (ctx.NegateNext != negate)
                                ctx.NegateNext = !negate;

                            var arrayIndex = peeked as ArrayIndexNode;
                            var isArrayIndex = arrayIndex != null;

                            if (peeked.NodeType == XzaarNodeTypes.BRACKET && peeked.NodeName == "LBRACKET" ||
                                isArrayIndex)
                            {
                                assignValue.Walked = true;
                                // define array variable
                                var endingBracket = nodes[++ctx.CurrentNodeIndex];
                                if ((endingBracket.NodeType != XzaarNodeTypes.BRACKET ||
                                     endingBracket.NodeName != "RBRACKET") && !isArrayIndex)
                                {
                                    // since we are guaranteeed to have a matching ending bracket
                                    // (the parser checks for it)
                                    // we can assume that this is an array initializer
                                    // let myArray = [ "string", "string1", "string2", "..." ]
                                    // TODO: Implement
                                    throw new XzaarExpressionTransformerException(
                                        "Awwes! We havn't implemented array initializers yet! You will have to push each individual item for now");
                                }

                                var values = new List<XzaarNode>();
                                if (arrayIndex != null && arrayIndex.Children.Count > 0)
                                {
                                    values.AddRange(WalkArrayInitializerValues(ctx, nodes, arrayIndex));
                                }

                                if (endingBracket.NodeType == XzaarNodeTypes.BRACKET)
                                    endingBracket.Walked = true;
                                variableType = "array";
                                var dvn = XzaarNode.DefineVariable(node.PositionInfo, variableType, variableName, XzaarNode.NewArrayInstance(node.PositionInfo, values));
                                ctx.AddVariableToScope(dvn);
                                if (!isArrayIndex) ctx.CurrentNodeIndex++; // skip the RBRACKET
                                return;
                            }

                            if (assignValue.NodeType == XzaarNodeTypes.EXPRESSION)
                            {
                                assignValue.Walked = true;
                                assignValue = TraverseRightSide(ctx, nodes, assignValue);
                                assignValue.Walked = true;
                                peeked.Walked = true;

                                AdvanceToNextUnwalked(ctx, nodes);
                                if (ctx.CurrentNodeIndex < nodes.Count)
                                {
                                    while (CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                    {
                                        ctx.Stack.Push(assignValue);
                                        Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                        assignValue = ctx.Stack.Pop();
                                        if (ctx.CurrentNodeIndex >= nodes.Count) break;
                                    }
                                }

                            }

                            if (peeked.NodeType == XzaarNodeTypes.UNARY_OPERATOR)
                            {
                                WalkUnary(ctx, nodes, peeked);
                                assignValue = ctx.Stack.Pop();
                                assignValue.Walked = true;
                                peeked.Walked = true;

                                if (ctx.CurrentNodeIndex < nodes.Count)
                                {
                                    while (CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                    {
                                        ctx.Stack.Push(assignValue);
                                        Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                        assignValue = ctx.Stack.Pop();
                                    }
                                }

                                var newVariable = XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Number.Name, variableName, assignValue);
                                ctx.AddVariableToScope(newVariable);
                                return;
                            }

                            if (peeked.NodeType == XzaarNodeTypes.LITERAL)
                            {
                                // check if the node afterwards is any indication of expression
                                // for instance (MATH) or other Binary Op
                                if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                                {
                                    var nextNode = nodes[ctx.CurrentNodeIndex + 1];
                                    if (nextNode.NodeType == XzaarNodeTypes.UNARY_OPERATOR)
                                    {
                                        assignValue.Walked = true;
                                        ctx.Stack.Push(assignValue);
                                        WalkUnary(ctx, nodes, nextNode);
                                        assignValue = ctx.Stack.Pop();
                                        assignValue.Walked = true;
                                        if (ctx.CurrentNodeIndex < nodes.Count &&
                                            CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                        {
                                            // nodes[ctx.CurrentNodeIndex].Walked = true;
                                            ctx.CurrentNodeIndex++;
                                        }
                                        assignValue.Walked = true;
                                        ctx.AddVariableToScope(XzaarNode.DefineVariable(node.PositionInfo, "NUMBER", variableName, assignValue));
                                        AdvanceToNextUnwalked(ctx, nodes);

                                        // if (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].Walked) ctx.CurrentNodeIndex++;

                                        // if (moveNextLast) ctx.CurrentNodeIndex++;
                                        return;
                                    }

                                    if (nextNode.NodeType == XzaarNodeTypes.ASSIGNMENT_OPERATOR
                                        // && nextNode.NodeName.EndsWith("_EQUALS")
                                        )
                                    {

                                        assignValue.Walked = true;
                                        ctx.Stack.Push(assignValue);
                                        WalkAssignment(ctx, nodes, nextNode);

                                        assignValue = ctx.Stack.Pop();
                                        assignValue.Walked = true;


                                        if (ctx.CurrentNodeIndex < nodes.Count)
                                        {
                                            while (CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                            {
                                                ctx.Stack.Push(assignValue);
                                                Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                                assignValue = ctx.Stack.Pop();
                                            }
                                        }

                                        var newVariable = XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Number.Name, variableName, assignValue);
                                        ctx.AddVariableToScope(newVariable);
                                        return;
                                    }

                                    if (IsExpression(nextNode.NodeType))
                                    {
                                        var moveNextLast = ctx.MoveNext;
                                        ctx.MoveNext = false;
                                        var pos = ctx.CurrentNodeIndex;
                                        assignValue = TraverseExtractAndAddDeclaredVariables(ctx, nodes, assignValue);
                                        var posNew = ctx.CurrentNodeIndex;
                                        if (ctx.CurrentNodeIndex < nodes.Count && posNew == pos + 1)
                                        {
                                            if (nodes[ctx.CurrentNodeIndex] is ExpressionNode)
                                            {
                                                ctx.CurrentNodeIndex++;
                                            }
                                        }

                                        if (ctx.CurrentNodeIndex < nodes.Count && CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                        {
                                            ctx.Stack.Push(assignValue);
                                            Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                            assignValue = ctx.Stack.Pop();
                                            var posChange = ctx.CurrentNodeIndex - pos;
                                            if (posChange == 0 && ctx.CurrentNodeIndex < nodes.Count &&
                                                CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                            {
                                                ctx.CurrentNodeIndex--; // ctx.CurrentNodeIndex++;
                                            }
                                        }

                                        assignValue.Walked = true;
                                        var varType = "ANY";
                                        if (assignValue.NodeType == XzaarNodeTypes.MATH) varType = "NUMBER";
                                        if (assignValue.NodeType == XzaarNodeTypes.CONDITIONAL || negate) varType = "BOOLEAN";

                                        ctx.AddVariableToScope(XzaarNode.DefineVariable(node.PositionInfo, varType, variableName,
                                            negate ? XzaarNode.Negate(assignValue.PositionInfo, assignValue) : assignValue));
                                        ctx.MoveNext = moveNextLast;
                                        // if (moveNextLast) ctx.CurrentNodeIndex++;
                                        return;
                                    }

                                    // check if next one after this is an accessor or not
                                    if (nextNode.NodeType == XzaarNodeTypes.ACCESSOR)
                                    {
                                        var before = ctx.Stack.Count;
                                        var variables = ctx.GetVariablesInScope();
                                        var targetVariable = variables.FirstOrDefault(v => v.Name == peeked.Value + "");
                                        var variableTypeName = "";

                                        if (targetVariable != null)
                                        {
                                            variableTypeName = targetVariable.Type;
                                        }

                                        if (targetVariable == null)
                                        {
                                            // chain access on constant.    
                                            switch (peeked.NodeName)
                                            {
                                                case "BOOLEAN":
                                                case "ANY":
                                                case "STRING":
                                                case "NUMBER":
                                                case "DATETIME":
                                                case "CHAR":
                                                    variableTypeName = peeked.NodeName.ToLower();
                                                    break;
                                            }

                                        }

                                        ctx.Stack.Push(XzaarNode.VariableAccess(node.PositionInfo, peeked, variableTypeName));
                                        ctx.CurrentNodeIndex++;
                                        this.WalkAccessor(ctx, nodes, nextNode);
                                        var newInStack = ctx.Stack.Count - before;
                                        if (newInStack > 0)
                                        {
                                            VariableNode newVariable = null;
                                            var resultNode = ctx.Stack.Pop();
                                            var chain = resultNode as MemberAccessChainNode;
                                            if (chain != null)
                                            {
                                                newVariable = XzaarNode.DefineVariable(node.PositionInfo,
                                                    chain.ResultType, variableName, chain);
                                            }
                                            else
                                            {
                                                var binop = resultNode as BinaryOperatorNode;
                                                if (binop != null)
                                                {
                                                    newVariable = XzaarNode.DefineVariable(node.PositionInfo,
                                                        TryGetReturnType(binop), variableName, binop);
                                                }
                                                else
                                                {
                                                    throw new XzaarExpressionTransformerException("Burp");
                                                }
                                                // var math = resultNode. as 
                                            }
                                            ctx.AddVariableToScope(newVariable);
                                            return;
                                        }
                                    }

                                    if (nextNode.NodeType == XzaarNodeTypes.EQUALITY ||
                                        nextNode.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR)
                                    {
                                        // equality operation
                                        assignValue = TraverseExtractAndAddDeclaredVariables(ctx, nodes, assignValue);



                                        //var p = ctx.CurrentNodeIndex;                                        
                                        //if (p != ctx.CurrentNodeIndex)
                                        //    ctx.CurrentNodeIndex--;

                                        if (assignValue.IsTransformed && ctx.CurrentNodeIndex < nodes.Count)
                                        {
                                            while (ctx.CurrentNodeIndex < nodes.Count && CanBeChained(assignValue, nodes[ctx.CurrentNodeIndex]))
                                            {
                                                ctx.Stack.Push(assignValue);
                                                Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                                assignValue = ctx.Stack.Pop();
                                            }
                                        }





                                        assignValue = WalkArithmeticExpression(ctx, nodes, assignValue);

                                        assignValue.Walked = true;
                                        ctx.AddVariableToScope(XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Boolean.Name, variableName, assignValue));
                                        return;
                                    }

                                    if (nextNode.NodeType == XzaarNodeTypes.ARRAYINDEX)
                                    {
                                        ctx.Stack.Push(assignValue);
                                        ctx.CurrentNodeIndex++;
                                        WalkArrayIndexer(ctx, nodes, nextNode);
                                        assignValue = ctx.Stack.Pop();
                                        assignValue.Walked = true;
                                        ctx.AddVariableToScope(XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Boolean.Name, variableName, assignValue));
                                        return;
                                    }

                                }

                                switch (assignValue.NodeName)
                                {
                                    case "BOOLEAN":
                                    case "NUMBER":
                                    case "STRING":
                                    case "DATETIME":
                                    case "CHAR":
                                        variableType = assignValue.NodeName.ToLower();
                                        break;
                                }
                                if (assignValue.NodeName == "NAME")
                                {
                                    // could be a struct
                                    // let a = STRUCT_NAME
                                    var strName = assignValue.Value + "";
                                    var str = ctx.Structs.FirstOrDefault(s => s.Name == strName);
                                    if (str != null)
                                    {
                                        // Yep! its a struct :)   
                                        assignValue = XzaarNode.CreateStruct(node.PositionInfo, str);
                                        variableType = str.Name;
                                    }
                                }
                            }
                        }

                        ctx.CurrentNodeIndex++;
                        assignValue.Walked = true;

                        var defineVariableNode = XzaarNode.DefineVariable(node.PositionInfo, variableType, variableName, negate ? XzaarNode.Negate(assignValue.PositionInfo, assignValue) : assignValue);
                        ctx.AddVariableToScope(defineVariableNode);
                    }
                    else
                    {
                        if (peeked.NodeType == XzaarNodeTypes.LITERAL && peeked.NodeName == "IN")
                        {
                            ctx.AddVariableToScope(XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Any.Name, variableName, null));
                            // i'm so pretty sure we're inside a foreach loop expression right now. so lets unwind by setting our current node index to the node count and return
                            ctx.State = ctx.CurrentNodeIndex;
                            ctx.CurrentNodeIndex = nodes.Count;
                            return;
                        }

                        var defineVariableNode = XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Any.Name, variableName, null);
                        ctx.AddVariableToScope(defineVariableNode);
                    }

                    //ctx.Stack.Push(defineVariableNode);
                }
                else
                {
                    var defineVariableNode = XzaarNode.DefineVariable(node.PositionInfo, XzaarBaseTypes.Any.Name, variableName, null);
                    ctx.AddVariableToScope(defineVariableNode);
                    //ctx.Stack.Push(defineVariableNode);
                }
            }
            else
            {
                throw new XzaarExpressionTransformerException("Expected a variable name but got a '" + next.NodeType
                    + "':'" + next.NodeName + "' instead.");
            }
        }

        private IEnumerable<XzaarNode> WalkArrayInitializerValues(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, ArrayIndexNode arrayIndex)
        {
            var values = new List<XzaarNode>();
            var oldIndex = ctx.CurrentNodeIndex;
            var tmpCtx = ctx.With(currentNodeIndex: 0);

            while (tmpCtx.CurrentNodeIndex < arrayIndex.Children.Count)
            {
                var child = arrayIndex.Children[tmpCtx.CurrentNodeIndex];
                if (child.NodeType == XzaarNodeTypes.SEPARATOR)
                {
                    tmpCtx.CurrentNodeIndex++;
                    continue;
                }

                Walk(tmpCtx, arrayIndex.Children, child);


                var value = ctx.Stack.Pop();
                if (tmpCtx.CurrentNodeIndex < arrayIndex.Children.Count)
                {
                    var next = arrayIndex.Children[tmpCtx.CurrentNodeIndex];
                    if (CanBeChained(value, next))
                    {
                        ctx.Stack.Push(value);
                        continue;
                    }
                }

                values.Add(value);
            }

            ctx.From(tmpCtx, oldIndex);

            return values;
        }

        private XzaarNode WalkArithmeticExpression(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode assignValue)
        {
            if (nodes.Count > ctx.CurrentNodeIndex)
            {
                var p1 = nodes[ctx.CurrentNodeIndex];
                if (p1.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR)
                {
                    ctx.Stack.Push(assignValue);

                    while (p1.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR)
                    {
                        // not end of expression

                        Walk(ctx, nodes, p1);
                        if (ctx.Stack.Count > 0)
                        {
                            p1 = ctx.Stack.Pop();
                        }
                        if (nodes.Count > ctx.CurrentNodeIndex &&
                            nodes[ctx.CurrentNodeIndex].NodeType ==
                            XzaarNodeTypes.ARITHMETIC_OPERATOR)
                        {
                            ctx.Stack.Push(p1);
                            p1 = nodes[ctx.CurrentNodeIndex];
                        }
                    }
                    ctx.CurrentNodeIndex++;
                    assignValue = p1;
                }
            }
            return assignValue;
        }

        private bool IsExpression(XzaarNodeTypes nodeType)
        {
            switch (nodeType)
            {
                case XzaarNodeTypes.EXPRESSION:
                case XzaarNodeTypes.ARITHMETIC_OPERATOR:
                case XzaarNodeTypes.CONDITIONAL_OPERATOR:
                case XzaarNodeTypes.CALL:
                    return true;
            }
            return false;
        }


        private void WalkExternMember(
                                      XzaarExpressionTransformerContext ctx,
                                      IList<XzaarNode> nodes)
        {
            var next = nodes[++ctx.CurrentNodeIndex];
            if (next.NodeName == "FUNCTION")
            {
                WalkFunction(ctx, nodes, true);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void WalkFunction(
                                  XzaarExpressionTransformerContext ctx,
                                  IList<XzaarNode> nodes,
                                  bool isExtern = false)
        {
            FunctionNode result = null;
            var positionInfo = nodes[ctx.CurrentNodeIndex].PositionInfo;

            var targetFunctionName = nodes[++ctx.CurrentNodeIndex];
            if (targetFunctionName.NodeType != XzaarNodeTypes.LITERAL)
            {
                throw new XzaarExpressionTransformerException("This ain't Game of Thrones, a function needs a name!");
            }

            var fnName = targetFunctionName.Value?.ToString();
            var targetFunctionArguments = XzaarNode.Parameters(positionInfo, nodes[++ctx.CurrentNodeIndex]);

            if (isExtern)
            {
                var returnTypeValue = TryGetReturnType(ctx, nodes, fnName);

                var terminate = nodes[++ctx.CurrentNodeIndex];
                if (terminate.Value?.ToString() != ";")
                {
                    throw new XzaarExpressionTransformerException("Missing expected ';' after extern function declaration.");
                }

                result = returnTypeValue != null
                    ? XzaarNode.ExternFunction(positionInfo, fnName, returnTypeValue, targetFunctionArguments)
                    : XzaarNode.ExternFunction(positionInfo, fnName, targetFunctionArguments);

                ctx.In(fnName, result);
            }
            else
            {
                var returnTypeValue = TryGetReturnType(ctx, nodes, fnName);

                ctx.In(fnName);

                var targetFunctionBody = nodes[++ctx.CurrentNodeIndex];
                var funcBody = targetFunctionBody;

                var args = XzaarNode.Parameters(positionInfo, targetFunctionArguments);

                foreach (var a in args.Parameters) ctx.AddParameterToScope(a);

                targetFunctionBody = TraverseExtractAndAddDeclaredVariables(ctx, nodes, targetFunctionBody, false);

                funcBody.Walked = true;

                result = returnTypeValue != null
                    ? XzaarNode.Function(positionInfo, fnName, returnTypeValue, args, targetFunctionBody)
                    : XzaarNode.Function(positionInfo, fnName, args, targetFunctionBody);

                ctx.SetScopeNode(result);
            }

            if (result.Parameters != null)
            {
                foreach (var arg in result.Parameters.Parameters)
                {
                    ctx.AddParameterToScope(result.Name, arg);
                }
            }

            ctx.Out();

            ctx.AddGlobalFunction(result);

            //stack.Push(result);

            AdvanceToNextUnwalked(ctx, nodes);
        }

        private static string TryGetReturnType(BinaryOperatorNode node)
        {
            return "any";
        }

        private static string TryGetReturnType(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, string fnName)
        {
            var peekReturnType = nodes[ctx.CurrentNodeIndex + 1];
            if (peekReturnType.NodeType == XzaarNodeTypes.ACCESSOR && peekReturnType.Value == "->")
            {
                ctx.CurrentNodeIndex++;
                var returnType = nodes[++ctx.CurrentNodeIndex];
                if (returnType.NodeType == XzaarNodeTypes.BODY || returnType.NodeType == XzaarNodeTypes.BLOCK)
                    throw new XzaarExpressionTransformerException("The function '" + fnName +
                                                                  "' expected a return type after the use of '->', but got a body instead. Did you forget to add it?");
                return returnType.Value.ToString();
            }
            return null;
        }

        private void WalkColon(XzaarExpressionTransformerContext ctx)
        {
            // do nothing for now.
            ctx.CurrentNodeIndex++;
        }


        private void WalkCondition(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes)
        {
            var node = nodes[ctx.CurrentNodeIndex];
            var condition = nodes[++ctx.CurrentNodeIndex];

            condition = TraverseRightSide(ctx, nodes, condition);
            // -- if we want to allow using if statements without paranthesis, we need to uncomment the line below. (not properly tested)
            // condition = ExtractExpression(ref nodeIndex, nodes, stack, condition);

            var ifTrue = nodes[++ctx.CurrentNodeIndex];

            ifTrue = TraverseExtractAndAddDeclaredVariables(ctx, nodes, ifTrue);

            AdvanceToNextUnwalked(ctx, nodes);

            if (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].NodeName == "ELSE" && nodes.Count > ctx.CurrentNodeIndex + 1)
            {
                var ifFalse = nodes[++ctx.CurrentNodeIndex];

                if (ifFalse.NodeType == XzaarNodeTypes.CONDITION)
                {
                    WalkCondition(ctx, nodes);
                    ifFalse = ctx.Stack.Pop();
                }
                else
                {
                    ifFalse = TraverseExtractAndAddDeclaredVariables(ctx, nodes, ifFalse);
                }

                var ifElseThen = XzaarNode.IfElseThen(node.PositionInfo, condition, ifTrue, ifFalse);
                ctx.Stack.Push(ifElseThen);
                ctx.SetScopeNode(ifElseThen);
                ctx.SetChildScopeNode(0, condition);
                ctx.SetChildScopeNode(1, ifTrue);
                ctx.SetChildScopeNode(2, ifFalse);
                // ctx.CurrentNodeIndex++;
            }
            else
            {

                // 
                var conditionalNode = XzaarNode.IfThen(node.PositionInfo, condition, ifTrue);
                ctx.Stack.Push(conditionalNode);
                ctx.SetScopeNode(conditionalNode);
                ctx.SetChildScopeNode(0, condition);
                ctx.SetChildScopeNode(1, ifTrue);

                // ctx.CurrentNodeIndex++;
            }
        }

        private XzaarNode TraverseExtractAndAddDeclaredVariables(XzaarExpressionTransformerContext ctx,
            IList<XzaarNode> nodes, XzaarNode node, bool makeNewScope = true)
        {
            if (makeNewScope) ctx.In();

            var oldVarCount = ctx.GetVariablesInScope().Count;
            ctx.State = ctx.Stack.Count;
            var nodeBefore = node;
            node = TraverseRightSide(ctx, nodes, node, false);
            node = ExtractExpressionBlock(ctx, nodes, node);
            if (node.IsTransformed)
                nodeBefore.Walked = true;


            if (!node.Walked && !node.IsTransformed)
            {
                XzaarNode lastStackItem = null;
                if (ctx.Stack.Count > 0)
                    lastStackItem = ctx.Stack.Peek();
                AdvanceToNode(ctx, nodes, node);
                Walk(ctx, nodes, node);
                if (ctx.Stack.Count > 0 && lastStackItem != ctx.Stack.Peek())
                    node = ctx.Stack.Pop();
            }

            var newVariables = ctx.GetVariablesInScope().Where(i => !i.IsDeclared()).OrderBy(j => j.Index).Skip(oldVarCount).ToList();

            if (newVariables.Count > 0)
            {
                if (node is BlockNode)
                {
                    var i = 0;
                    foreach (var v in newVariables)
                        node.InsertChild(i++, v);
                }
                else
                {
                    node = XzaarNode.Block(node.PositionInfo, node);
                    var i = 0;
                    foreach (var v in newVariables)
                        node.InsertChild(i++, v);
                }

                node.RemoveUntransformedVariableDeclarations();
            }

            node.SortChildren();

            if (ctx.CurrentNodeIndex < nodes.Count)
            {
                if ((node.NodeType == XzaarNodeTypes.LITERAL || node.NodeType == XzaarNodeTypes.ACCESS) && nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.ARRAYINDEX)
                {
                    ctx.Stack.Push(node);
                    WalkArrayIndexer(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    node = ctx.Stack.Pop();
                }

                if (node.NodeType == XzaarNodeTypes.UNARY_OPERATOR && nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.LITERAL)
                {
                    ctx.Stack.Push(node);
                    WalkUnary(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    node = ctx.Stack.Pop();
                }

                if (node.NodeType == XzaarNodeTypes.LITERAL && nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.UNARY_OPERATOR)
                {
                    ctx.Stack.Push(node);
                    WalkUnary(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    node = ctx.Stack.Pop();
                }
            }


            if (makeNewScope) ctx.Out();

            AdvanceToNextUnwalked(ctx, nodes);

            return node;
        }

        private XzaarNode ExtractExpressionBlock(
            XzaarExpressionTransformerContext ctx,
            IList<XzaarNode> nodes,
            XzaarNode ifTrue, int maxPop = 9999)
        {
            if (ctx.State != ctx.Stack.Count)
            {
                if (ctx.Stack.Count - ctx.State > 0)
                {
                    var items = new List<XzaarNode>();
                    var popCount = 0;
                    while (ctx.Stack.Count - ctx.State > 0)
                    {
                        if (popCount >= maxPop) break;
                        items.Add(ctx.Stack.Pop());
                        popCount++;
                    }
                    items.Add(ifTrue);

                    ifTrue = XzaarNode.Block(ifTrue.PositionInfo, items.ToArray());
                }
            }
            ctx.State = 0;

            ifTrue = ExtractExpression(ctx, nodes, ifTrue);
            ifTrue.SortChildren();
            ctx.SetScopeNode(ifTrue);

            return ifTrue;
        }


        private void WalkLoop(
                              XzaarExpressionTransformerContext ctx,
                              IList<XzaarNode> nodes,
                              XzaarNode node)
        {

            // can be LOOP, WHILE, FOREACH, FOR, DO


            switch (node.NodeName)
            {
                case "LOOP":
                    WalkBasicLoop(ctx, nodes, node);
                    break;
                case "WHILE":
                    WalkWhileLoop(ctx, nodes, node);
                    break;
                case "DO":
                    WalkDoWhileLoop(ctx, nodes, node); break;
                case "FOR":
                    WalkForLoop(ctx, nodes, node);
                    break;
                case "FOREACH":
                    WalkForeachLoop(ctx, nodes, node);
                    break;
                default:
                    throw new NotSupportedException(node.NodeName + " is not a supported loop keyword");
            }
        }

        private void WalkMatch(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // initially, I thought the 'match' would be like the way Rust does it
            // but instead, it will just be a normal switch | case with the only difference 
            // that you can actually use 'match(x) {...}' same as 'switch(x) {...}' this may change later.

            //
            // switch|match <expr> {
            //    case <value1 or expr>:
            //        break|return|continue(if inside looped scope);
            //    case <value2 or expr>:
            //        ....
            //    case <valueN>:
            //        ....
            //    default:
            //        ....
            // }
            // 
            // example using expressions:
            // switch (true != false) {
            //   case true || false: // same as 'default:'
            //       doSomething()
            //      return;
            // }
            AdvanceToNode(ctx, nodes, node);
            // AdvanceToNextUnwalked(ctx, nodes);

            var ctxStart = ctx.CurrentNodeIndex;
            var valueExprNode = nodes[ctx.CurrentNodeIndex + 1] as ExpressionNode;
            var valueExpression = TraverseExtractAndAddDeclaredVariables(ctx, nodes, valueExprNode);
            valueExprNode.Walked = true;
            AdvanceToNextUnwalked(ctx, nodes);

            if (ctx.CurrentNodeIndex >= nodes.Count)
                throw new XzaarExpressionTransformerException(
                    "So no switch/match body? Hmm.. Did your refactoring go wrong or you're not done yet?");

            var bodyNode = nodes[ctx.CurrentNodeIndex];
            var bodyNodeBefore = bodyNode;
            var oldStackCount = ctx.Stack.Count;
            var oldNodeIndex = ctx.CurrentNodeIndex;
            var tmpCtx = ctx.With(currentNodeIndex: 0);

            //if (bodyNode.Children.Count == 0 && ctx.Stack.Count > 0)
            //{
            //    while (ctx.Stack.Count > 0 && ctx.Stack.Peek().NodeType == XzaarNodeTypes.CASE)
            //    {
            //        bodyNode.Children.Add(ctx.Stack.Pop());
            //    }
            //}

            while (tmpCtx.CurrentNodeIndex < bodyNode.Children.Count)
            {
                if (bodyNode.Children[tmpCtx.CurrentNodeIndex].NodeType != XzaarNodeTypes.CASE) break;
                WalkSwitchCase(tmpCtx, bodyNode.Children, bodyNode.Children[tmpCtx.CurrentNodeIndex]);
                AdvanceToNextUnwalked(tmpCtx, bodyNode.Children);
            }

            bodyNodeBefore.Walked = true;
            ctx.From(tmpCtx, oldNodeIndex);
            var cases = new List<CaseNode>();
            var newCount = ctx.Stack.Count - oldStackCount;
            for (var i = 0; i < newCount; i++) cases.Add(ctx.Stack.Pop() as CaseNode);
            cases.Reverse(); // the cases are added in a reversed order, so lets reverse em :)

            var switchMatchNode = XzaarNode.Switch(
                    node.PositionInfo,
                    valueExpression,
                    cases.ToArray()
                );

            ctx.Stack.Push(switchMatchNode);
            ctx.SetScopeNode(switchMatchNode);
            AdvanceToNextUnwalked(ctx, nodes);
            if (ctxStart == ctx.CurrentNodeIndex || ctxStart == ctx.CurrentNodeIndex - 1) ctx.CurrentNodeIndex++;
        }


        private void WalkSwitchCase(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // case <value or expr>:
            //   <body>
            //   break | return | continue

            var matchNode = nodes[++ctx.CurrentNodeIndex];
            var matchExpression = TraverseExtractAndAddDeclaredVariables(ctx, nodes, matchNode);

            var singleRowExpression = false;
            XzaarNode body = null;

            //if (ctx.CurrentNodeIndex >= nodes.Count)
            //{
            //    // most likely we only have a return case without body (with expr?)
            //    if (ctx.CurrentNodeIndex - 2 > 0)
            //    {
            //        var prevPrev = nodes[ctx.CurrentNodeIndex - 2];
            //        if (prevPrev.NodeType == XzaarNodeTypes.RETURN)
            //        {
            //            ctx.CurrentNodeIndex -= 2;
            //            body = nodes[ctx.CurrentNodeIndex];
            //        }
            //    }

            //}
            //else
            body = nodes[++ctx.CurrentNodeIndex];


            if (body == null)
            {
                // maybe we only had a direct return or break inside our case?
                // either way, not handled.
                throw new NotImplementedException();
            }

            ctx.MoveNext = false;
            var before = ctx.Stack.Count;
            var bodyBefore = body;
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body, false);
            bodyBefore.Walked = true;
            if (ctx.Stack.Count - before > 0)
            {
                var items = new List<XzaarNode>();
                for (var i = 0; i < ctx.Stack.Count - before; i++)
                {
                    items.Add(ctx.Stack.Pop());
                }
                items.Add(body);
                body = XzaarNode.Body(node.PositionInfo, items.ToArray());
            }

            var cfNode = body as ControlFlowNode;
            if (cfNode != null && ctx.CurrentNodeIndex + 1 < nodes.Count && nodes[ctx.CurrentNodeIndex + 1].NodeType != XzaarNodeTypes.CASE)
            {
                Walk(ctx, nodes, cfNode);
                body = ctx.Stack.Pop();
            }
            else
            {
                body.Walked = true;
                if (ctx.CurrentNodeIndex < nodes.Count)
                {
                    // if (nodes[ctx.CurrentNodeIndex + 1].NodeType == XzaarNodeTypes.CASE)
                    //if (nodes[ctx.CurrentNodeIndex].Walked)
                    //ctx.CurrentNodeIndex++;
                    AdvanceToNextUnwalked(ctx, nodes);

                    ctx.MoveNext = true;

                    if (ctx.CurrentNodeIndex < nodes.Count)
                    {
                        var peekNode = nodes[ctx.CurrentNodeIndex];
                        if (peekNode is ControlFlowNode && peekNode.NodeName != "CASE")
                        {
                            // ctx.CurrentNodeIndex++;          
                            peekNode.Walked = true;
                            if (peekNode.NodeType == XzaarNodeTypes.RETURN)
                            {
                                WalkReturn(ctx, nodes, peekNode);
                                body = XzaarNode.Body(node.PositionInfo, body, ctx.Stack.Pop());
                            }
                            else
                            {
                                body = XzaarNode.Body(node.PositionInfo, body, peekNode);
                            }
                        }
                        else
                        {
                            // get all until a <return | continue | break | goto> is found
                            var bodyNodes = new List<XzaarNode>();
                            while (!(peekNode is ControlFlowNode) && peekNode.NodeName != "CASE")
                            {
                                peekNode.Walked = true;
                                ctx.MoveNext = false;
                                peekNode = TraverseExtractAndAddDeclaredVariables(ctx, nodes, peekNode, false);
                                ctx.MoveNext = true;

                                AdvanceToNextUnwalked(ctx, nodes);

                                bodyNodes.Add(peekNode);
                                if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                                    break;


                                peekNode = nodes[ctx.CurrentNodeIndex];
                                peekNode.Walked = true;
                            }
                            if (ctx.CurrentNodeIndex < nodes.Count)
                            {
                                peekNode = nodes[ctx.CurrentNodeIndex];
                                peekNode.Walked = true;
                            }
                            var cfn = peekNode as ControlFlowNode;
                            if (cfn != null && peekNode.NodeName != "CASE")
                            {
                                if (cfn.NodeType == XzaarNodeTypes.RETURN)
                                {
                                    Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                                    bodyNodes.Add(ctx.Stack.Pop());
                                }
                                else
                                {
                                    bodyNodes.Add(cfn);
                                }
                            }

                            if (bodyNodes.Count > 0)
                            {
                                if (body.IsTransformed) bodyNodes.Insert(0, body);
                                body = XzaarNode.Body(node.PositionInfo, bodyNodes.ToArray());
                            }

                        }
                    }
                }
            }


            if (!(body is BodyNode))
            {
                body = XzaarNode.Body(node.PositionInfo, body);
                singleRowExpression = true;
            }

            var caseNode = XzaarNode.Case(node.PositionInfo, matchExpression, body);

            ctx.Stack.Push(caseNode);
            ctx.SetScopeNode(caseNode);

            AdvanceToNextUnwalked(ctx, nodes);
            if (ctx.CurrentNodeIndex < nodes.Count)
            {
                if (nodes[ctx.CurrentNodeIndex - 1].NodeType == XzaarNodeTypes.CASE)
                {
                    ctx.CurrentNodeIndex--;
                    nodes[ctx.CurrentNodeIndex].Walked = false;
                }
            }
            //if (!singleRowExpression) ctx.CurrentNodeIndex++;
        }


        private void WalkLabel(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // similar to defining a variable but.. a bit simpler?
            // <label_name>:

            var labelName = node.Value + "";
            if (labelName.EndsWith(":"))
            {
                // well thats it, our job is done.
                ctx.Stack.Push(XzaarNode.Label(node.PositionInfo, labelName));
                ctx.CurrentNodeIndex++;
                return;
            }

            throw new NotImplementedException();
        }

        private void WalkForeachLoop(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // super special case coming up!
            // foreach (let|var <var_name1> in <var_name2>) { <body> }
            // <var_name1> needs to be unique
            // <var_name2> have to be an array, but does not require to exist prior to the foreach
            //             ex: foreach(let j in []) is acceptable, even though the array is empty.
            var expr = nodes[++ctx.CurrentNodeIndex];
            expr.Walked = true;

            expr = TraverseExtractAndAddDeclaredVariables(ctx, nodes, expr);
            expr.Walked = true;

            var cExpr = expr.Children.FirstOrDefault(c => c.NodeType == XzaarNodeTypes.EXPRESSION);
            var inIndex = IndexOfForeachIn(cExpr);
            if (inIndex == -1) throw new XzaarExpressionTransformerException("Somethings missing in your foreach loop. Perhaps the 'in' keyword?");
            if (inIndex + 1 > cExpr.Children.Count) throw new XzaarExpressionTransformerException("Somethings missing in your foreach loop. Perhaps the collection variable?");

            var collectionExpression = cExpr.Children[inIndex + 1];
            collectionExpression = TraverseRightSide(ctx, cExpr.Children, collectionExpression, false);
            var variableDefinition = expr.Children.FirstOrDefault(c => c.NodeType != XzaarNodeTypes.EXPRESSION);

            var body = nodes[ctx.CurrentNodeIndex];
            body.Walked = true;
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body);

            var foreachLoop = XzaarNode.Foreach(node.PositionInfo, variableDefinition, collectionExpression, body);
            ctx.Stack.Push(foreachLoop);
            ctx.SetScopeNode(foreachLoop);
            // ctx.CurrentNodeIndex++;
        }

        private int IndexOfForeachIn(XzaarNode node)
        {
            var collectionExpression = node.Children;

            for (var i = 0; i < collectionExpression.Count; i++)
            {
                if (collectionExpression[i].NodeType == XzaarNodeTypes.LITERAL &&
                    collectionExpression[i].NodeName == "IN")
                {
                    return i;
                }
            }
            return -1;
        }

        private void WalkForLoop(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // unlike a while and do-while a for loop has 3 expressions and a body
            // for (<init_expr>; <test_expr>; <incr_expr>) { <body> }

            // we will always expect at least a ';' to break between the expressions
            // making for (;;) { } a valid for loop (infinite loop)

            // first off. we have expression inside expressions
            // so the first node will be an expression containing our 3 sub expressions

            var forExpr = nodes[++ctx.CurrentNodeIndex];
            forExpr = TraverseExtractAndAddDeclaredVariables(ctx, nodes, forExpr) as BlockNode;



            if (forExpr == null)
            {
                throw new XzaarExpressionTransformerException("What kind of for loop is this?");
            }

            if (forExpr.Children.Count != 3)
            {
                throw new XzaarExpressionTransformerException("Hey man, this for loop is missing something. Please take another look!");
            }

            var initExpr = forExpr[0];
            var testExpr = forExpr[1];
            var incrExpr = forExpr[2];



            var body = nodes[ctx.CurrentNodeIndex];
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body);

            var forLoop = XzaarNode.For(node.PositionInfo, initExpr, testExpr, incrExpr, body);
            ctx.Stack.Push(forLoop);
            ctx.SetScopeNode(forLoop);
        }

        private void WalkDoWhileLoop(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // do { <body> } while(<condition>)

            // we expect current node to obviously be our 'do'
            // so the next node will be our body

            var body = nodes[++ctx.CurrentNodeIndex];
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body);

            // afterwards it should be a 'while' keyword
            var @while = nodes[ctx.CurrentNodeIndex];

            if (@while.NodeType == XzaarNodeTypes.LOOP && @while.NodeName == "WHILE")
            {
                var condition = nodes[++ctx.CurrentNodeIndex];

                condition = TraverseRightSide(ctx, nodes, condition);
                ctx.CurrentNodeIndex++;

                var conditionalNode = XzaarNode.DoWhile(node.PositionInfo, condition, body);
                ctx.Stack.Push(conditionalNode);
                ctx.SetScopeNode(conditionalNode);
                ctx.SetChildScopeNode(0, condition);
                ctx.SetChildScopeNode(1, body);
                return;
            }

            throw new XzaarExpressionTransformerException("The keyword 'while' was expected after the 'do' body");
        }

        private void WalkWhileLoop(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // first node is an expression (hopefully)
            // second node is a body
            var condition = nodes[++ctx.CurrentNodeIndex];
            condition = TraverseRightSide(ctx, nodes, condition);

            var body = nodes[++ctx.CurrentNodeIndex];
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body);

            var conditionalNode = XzaarNode.While(node.PositionInfo, condition, body);
            ctx.Stack.Push(conditionalNode);
            ctx.SetScopeNode(conditionalNode);
            ctx.SetChildScopeNode(0, condition);
            ctx.SetChildScopeNode(1, body);

        }

        private void WalkBasicLoop(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // we need a loop start (label)
            // a loop body
            // and a goto loop start (at the end)

            // loop { << start_label:
            //       // body
            // }     << goto start_label;            

            var body = nodes[++ctx.CurrentNodeIndex];
            var bodyBefore = body;
            if (!(body is BlockNode))
                throw new XzaarScriptTransformerException("A body/block was expected after the keyword 'loop'");

            var stackCount = ctx.Stack.Count;
            body = TraverseRightSide(ctx, nodes, body);

            var newStackCount = ctx.Stack.Count - stackCount;
            body = ExtractExpressionBlock(ctx, nodes, body, newStackCount);

            var blocky = body as BlockNode;
            XzaarNode loopNode;
            if (blocky == null)
            {
                loopNode = XzaarNode.Loop(node.PositionInfo, XzaarNode.Block(node.PositionInfo, body));
                ctx.Stack.Push(loopNode);
            }
            else
            {
                loopNode = XzaarNode.Loop(node.PositionInfo, blocky);
                ctx.Stack.Push(loopNode);
            }

            ctx.SetScopeNode(loopNode);
            bodyBefore.Walked = true;
            AdvanceToNextUnwalked(ctx, nodes);
            // ctx.CurrentNodeIndex++;
        }

        private void WalkLiteral(
                                 XzaarExpressionTransformerContext ctx,
                                IList<XzaarNode> nodes,
                                XzaarNode node1)
        {
            var node = nodes[ctx.CurrentNodeIndex];
            AdvanceToNode(ctx, nodes, node);
            node.Walked = true;
            var str = node.Value + "";
            if (str.EndsWith(":"))
            {
                WalkLabel(ctx, nodes, node);
                return;
            }

            var func = ctx.Functions.FirstOrDefault(i => i.Value?.ToString() == str);
            if (func != null)
            {
                WalkFunctionCall(ctx, nodes, func);
            }
            else
            {
                if (ctx.CurrentNodeIndex + 1 < nodes.Count && nodes[ctx.CurrentNodeIndex + 1].NodeType == XzaarNodeTypes.EXPRESSION)
                {

                    // this is most likely a function call towards an undefined function. It is possible that it is defined later on.
                    WalkFunctionCall(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    return;
                }
                else
                {
                    var vars = ctx.GetVariablesInScope();
                    var variable = vars.FirstOrDefault(v => v.Name == str);

                    if (variable != null)
                    {
                        ctx.Stack.Push(XzaarNode.VariableAccess(node.PositionInfo, node, variable.Type));
                    }
                    else
                    {
                        var pars = ctx.GetParametersInScope();
                        var parm = pars.FirstOrDefault(v => v.Name == str);
                        if (parm != null)
                        {
                            ctx.Stack.Push(XzaarNode.ParameterAccess(node.PositionInfo, node, parm.Type));
                        }
                        else
                        {
                            if (ctx.NegateNext)
                            {
                                ctx.NegateNext = false;
                                ctx.Stack.Push(XzaarNode.Negate(node.PositionInfo, node));
                            }
                            else
                                ctx.Stack.Push(node);
                        }
                    }
                }
                ctx.CurrentNodeIndex++;
            }
        }

        private void WalkFunctionCall(
                                    XzaarExpressionTransformerContext ctx,
                                    IList<XzaarNode> nodes,
                                    XzaarNode function)
        {
            var node = nodes[ctx.CurrentNodeIndex];
            var arguments = nodes[++ctx.CurrentNodeIndex];
            var argsNode = arguments;
            var argumentIndex = ctx.CurrentNodeIndex;
            var accessorPeekOffset = 3;
            arguments = TraverseRightSide(ctx, nodes, arguments);
            if (!(arguments is ExpressionNode) || (arguments.Children.Count > 0))
            {
                arguments = ExtractExpression(ctx, nodes, arguments);
            }

            argsNode.Walked = true;
            arguments.Walked = true;
            XzaarNode instanceNode = null;

            // if (ctx.IgnoreTypeRestrictions)
            // {
            // this only happens from inside an accessor chain, so lets rewind and find our instance

            if (argumentIndex - 2 >= 0 && nodes[argumentIndex - 2].NodeType == XzaarNodeTypes.ACCESSOR)
            {
                var instance = nodes[argumentIndex - 3];
                if (instance.NodeType == XzaarNodeTypes.LITERAL)
                {
                    instance.Walked = true;
                    // easy peasy, variable name here
                    instanceNode = XzaarNode.VariableAccess(node.PositionInfo, instance);
                }
                else
                {
                    throw new XzaarExpressionTransformerException("Oh darnit! We havn't implemented this kind of function call just yet! Please stay tuned for the future!");
                    // not so easy peasy as its most likely a function().function() expression or (a+b).function()
                    if (instance.NodeType == XzaarNodeTypes.EXPRESSION)
                    {

                    }
                    else
                    {

                    }
                }
            }
            // }

            if (instanceNode != null) instanceNode.Walked = true;

            FunctionCallNode call;
            if (arguments is ExpressionNode && arguments.Children.Count == 0)
            {
                // ctx.CurrentNodeIndex--;
                call = instanceNode != null
                    ? XzaarNode.Call(node.PositionInfo, instanceNode, function, new ArgumentNode[0])
                    : XzaarNode.Call(node.PositionInfo, function, new ArgumentNode[0]);
            }
            else
            {
                var argumentNodes = new ArgumentNode[1];
                if (arguments is BlockNode)
                    argumentNodes = arguments.Children.Select((c, i) => XzaarNode.Argument(node.PositionInfo, c, i)).ToArray();
                else
                    argumentNodes[0] = XzaarNode.Argument(node.PositionInfo, arguments, 0);

                call = instanceNode != null
                    ? XzaarNode.Call(node.PositionInfo, instanceNode, function, argumentNodes)
                    : XzaarNode.Call(node.PositionInfo, function, argumentNodes);
            }

            ctx.Stack.Push(call);

            if (ctx.CurrentNodeIndex < nodes.Count)
            {
                if ((call.Arguments == null || call.Arguments.Length == 0) && ctx.CurrentNodeIndex + 1 < nodes.Count)
                {
                    var peek = nodes[ctx.CurrentNodeIndex + 1];
                    if (CanBeChained(call, peek))
                    {
                        ctx.CurrentNodeIndex++;
                        Walk(ctx, nodes, peek);
                    }
                }
                else if (CanBeChained(call, nodes[ctx.CurrentNodeIndex]))
                {
                }
            }

            if (argumentIndex == ctx.CurrentNodeIndex)
            {
                AdvanceToNextUnwalked(ctx, nodes);
            }
        }

        private void WalkAssignment(
                                    XzaarExpressionTransformerContext ctx,
                                    IList<XzaarNode> nodes,
                                    XzaarNode node)
        {
            XzaarNode left = ctx.Stack.Peek();

            AdvanceToNode(ctx, nodes, node);


            XzaarNode right = nodes[ctx.CurrentNodeIndex + 1];// nodes[ctx.CurrentNodeIndex + 1];
            var negate = ctx.NegateNext;

            while (right.NodeType == XzaarNodeTypes.NEGATE_OPERATOR)
            {
                ctx.CurrentNodeIndex++;
                // advance to next node as we want to negate anything that comes after                
                if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                    throw new XzaarExpressionTransformerException("Unexpected end of file.");
                right = nodes[ctx.CurrentNodeIndex + 1];
                negate = !negate;
            }

            if (ctx.NegateNext != negate)
                ctx.NegateNext = !negate;

            if (right.NodeType == XzaarNodeTypes.ARRAYINDEX)
            {
                ctx.Stack.Pop();

                node.Walked = true;
                left.Walked = true;
                right.Walked = true;

                var values = new List<XzaarNode>();
                var arrayIndex = right as ArrayIndexNode;
                if (arrayIndex != null && arrayIndex.Children.Count > 0)
                {
                    values.AddRange(WalkArrayInitializerValues(ctx, nodes, arrayIndex));
                    var newValue = XzaarNode.NewArrayInstance(node.PositionInfo, values);
                    ctx.Stack.Push(XzaarNode.Assign(left.PositionInfo, left, negate ? XzaarNode.Negate(left.PositionInfo, newValue) : newValue));
                    ctx.CurrentNodeIndex += 2;
                }
                else
                {
                    var newValue = XzaarNode.NewArrayInstance(node.PositionInfo, new List<XzaarNode>());
                    ctx.Stack.Push(XzaarNode.Assign(left.PositionInfo, left, negate ? XzaarNode.Negate(left.PositionInfo, newValue) : newValue));
                    ctx.CurrentNodeIndex += 2;

                }
                AdvanceToNextUnwalked(ctx, nodes);
                return;
            }

            var before1 = ctx.CurrentNodeIndex;
            GetLeftAndRightNodes(ctx, out left, out right, nodes, node);

            AdvanceToNextUnwalked(ctx, nodes);

            var after = ctx.CurrentNodeIndex;
            var nextNodeIndex = before1 == after ? before1 + 1 : after;

            node.Walked = true;
            left.Walked = true;
            right.Walked = true;
            // check if next token (if one exists) is an accessor or not
            var walked = false;
            if (nodes.Count > nextNodeIndex)
            {
                var peeked = nodes[nextNodeIndex];
                if (peeked.NodeType == XzaarNodeTypes.ACCESSOR)
                {

                    var before = ctx.Stack.Count;
                    var variables = ctx.GetVariablesInScope();
                    var targetVariable = variables.FirstOrDefault(v => v.Name == right.Value + "");
                    ctx.Stack.Push(XzaarNode.VariableAccess(right.PositionInfo, right, targetVariable.Type));
                    ctx.CurrentNodeIndex++;
                    this.WalkAccessor(ctx, nodes, peeked);
                    var newInStack = ctx.Stack.Count - before;
                    if (newInStack > 0)
                    {
                        var newValue = ctx.Stack.Pop();
                        ctx.Stack.Push(XzaarNode.Assign(left.PositionInfo, left, negate ? XzaarNode.Negate(left.PositionInfo, newValue) : newValue));
                        AdvanceToNextUnwalked(ctx, nodes);
                        return;
                    }

                }

                var peekIndex = -1;
                while (CanBeChained(right, peeked))
                {
                    XzaarNode peekedAfter = null;
                    if (ctx.CurrentNodeIndex == peekIndex)
                        ctx.CurrentNodeIndex++;
                    ctx.Stack.Push(right);


                    if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                    {
                        peekedAfter = nodes[ctx.CurrentNodeIndex + 1];
                        peekIndex = ctx.CurrentNodeIndex + 1;
                    }

                    Walk(ctx, nodes, peeked);
                    right = ctx.Stack.Pop();
                    walked = true;

                    if (ctx.CurrentNodeIndex > nodes.Count)
                    {
                        var last = nodes[nodes.Count - 1];
                        var skip = last.Index - right.Index;
                        var testIndex = ctx.CurrentNodeIndex - skip;
                        while (testIndex > 0 && nodes[testIndex].Walked)
                        {
                            testIndex++;
                            if (testIndex >= nodes.Count) break;
                        }
                        if (testIndex < nodes.Count)
                        {
                            ctx.CurrentNodeIndex = testIndex;
                        }
                    }
                    if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                    {
                        peeked = nodes[ctx.CurrentNodeIndex];
                        if (peekedAfter == peeked || peekIndex < ctx.CurrentNodeIndex)
                        {
                            if (CanBeChained(right, nodes[ctx.CurrentNodeIndex - 1]))
                                ctx.CurrentNodeIndex--;
                        }
                        peeked = nodes[ctx.CurrentNodeIndex];
                        continue;
                    }
                    break;
                }
            }
            //if (!walked)
            //    ctx.CurrentNodeIndex++;



            var op = node.Value.ToString();
            if (op.Length > 1)
            {
                var op0 = op[0];
                var order = GetOperatingOrderWeight(op0.ToString());
                if (op0 == '+' || op0 == '/' || op0 == '*' || op0 == '-' || op0 == '%' || op0 == '&' || op0 == '|')
                {
                    ctx.Stack.Push(XzaarNode.Assign(left.PositionInfo, left,
                        XzaarNode.BinaryOperator(left.PositionInfo, order, left, op0,
                            negate ? XzaarNode.Negate(left.PositionInfo, right) : right)));
                }
            }
            else
            {
                ctx.Stack.Push(XzaarNode.Assign(left.PositionInfo, left,
                    negate ? XzaarNode.Negate(left.PositionInfo, right) : right));
            }

            AdvanceToNextUnwalked(ctx, nodes);
        }

        private void AdvanceToNode(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            var index = nodes.IndexOf(node);
            if (index >= 0)
                ctx.CurrentNodeIndex = index;
        }

        private static void AdvanceToNextUnwalked(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes)
        {
            while (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].Walked)
            {
                while (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].Walked)
                    ctx.CurrentNodeIndex++;
            }
        }

        private void WalkArithmetic(
                                    XzaarExpressionTransformerContext ctx,
                                    IList<XzaarNode> nodes,
                                    XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;

            GetLeftAndRightNodes(ctx, out left, out right, nodes, node);

            left.Walked = true;
            right.Walked = true;

            var currentNode = ctx.CurrentNodeIndex < nodes.Count ? nodes[ctx.CurrentNodeIndex] : null;
            if (currentNode != null && currentNode.Walked)
            {
                ctx.CurrentNodeIndex++;
            }

            while (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.UNARY_OPERATOR)
            {
                // our 'right' value isnt done yet.
                ctx.Stack.Push(right);
                Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                right = ctx.Stack.Pop();
                right.Walked = true;
            }


            var order = GetOperatingOrderWeight(node.Value.ToString());

            var leftMath = left as BinaryOperatorNode;
            if (left.OperatingOrder < order && leftMath != null)
            {
                // rebuild statement
                // EX: (25 + 10) * 100 <- Right
                //     ^- LEFT   ^- this

                // Should become:
                //      (25 + (10 * 100))
                //       ^- push new statement to stack and return null

                // take the `Right´ value (10) from our Left, and use it as our new left.

                var oldLeft = leftMath;
                left = oldLeft.RightParams[0];

                var newRight = XzaarNode.BinaryOperator(left.PositionInfo, order, left, node.Value.ToString(), right);
                ctx.Stack.Push(XzaarNode.BinaryOperator(oldLeft.PositionInfo, oldLeft.OperatingOrder, oldLeft.LeftParams[0], oldLeft.Op,
                    newRight));
                return;
            }
            ctx.Stack.Push(XzaarNode.BinaryOperator(left.PositionInfo, order, left, node.Value.ToString(), right));
        }

        private void WalkEquality(
                                  XzaarExpressionTransformerContext ctx,
                                  IList<XzaarNode> nodes,
                                  XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;
            GetLeftAndRightNodes(ctx, out left, out right, nodes, node);

            // ctx.CurrentNodeIndex++;

            left.Walked = true;
            right.Walked = true;

            var currentNode = ctx.CurrentNodeIndex < nodes.Count ? nodes[ctx.CurrentNodeIndex] : null;
            if (currentNode != null && currentNode.Walked)
            {
                ctx.CurrentNodeIndex++;
            }

            while (ctx.CurrentNodeIndex < nodes.Count && nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.UNARY_OPERATOR)
            {
                // our 'right' value isnt done yet.
                ctx.Stack.Push(right);
                Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                right = ctx.Stack.Pop();
                right.Walked = true;
            }


            ctx.Stack.Push(XzaarNode.EqualityOperator(left.PositionInfo, left, node.Value.ToString(), right));
        }


        private void WalkBody(
                                XzaarExpressionTransformerContext ctx,
                                IList<XzaarNode> nodes,
                                XzaarNode node)
        {
            TraverseExpression(ctx, nodes, node, false);
            ctx.CurrentNodeIndex++;
        }

        private void WalkExpression(
                                    XzaarExpressionTransformerContext ctx,
                                    IList<XzaarNode> nodes,
                                    XzaarNode node)
        {

            TraverseExpression(ctx, nodes, node, true);
            node.Walked = true;
            ctx.CurrentNodeIndex++;
        }

        private void WalkUnary(
                                      XzaarExpressionTransformerContext ctx,
                                      IList<XzaarNode> nodes,
                                      XzaarNode node)
        {
            // this can be a post decrementor | decrementor | post incrementor | incrementor
            XzaarNode item = null;
            nodes[ctx.CurrentNodeIndex].Walked = true;
            node.Walked = true;
            var isPost = true;
            if (ctx.Stack.Count == 0 || ctx.CurrentNodeIndex == 0 ||
                (ctx.CurrentNodeIndex > 0 && nodes[ctx.CurrentNodeIndex - 1].NodeType != XzaarNodeTypes.LITERAL
                && nodes[ctx.CurrentNodeIndex - 1].NodeType != XzaarNodeTypes.ASSIGN
                && nodes[ctx.CurrentNodeIndex - 1].NodeType != XzaarNodeTypes.ASSIGNMENT_OPERATOR))
            {
                if (nodes[ctx.CurrentNodeIndex].NodeType != XzaarNodeTypes.UNARY_OPERATOR &&
                    nodes[ctx.CurrentNodeIndex - 1].NodeType == XzaarNodeTypes.UNARY_OPERATOR)
                {
                    var prevItem = ctx.Stack.Peek();
                    if (prevItem.NodeType == XzaarNodeTypes.UNARY_OPERATOR && !prevItem.IsTransformed)
                        node = ctx.Stack.Pop();
                    item = TraverseRightSide(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    isPost = false;
                }
                else
                {
                    item = TraverseRightSide(ctx, nodes, nodes[++ctx.CurrentNodeIndex]);
                    isPost = false;
                }
            }
            else
            {
                item = ctx.Stack.Pop();
            }

            ctx.CurrentNodeIndex++;

            var unaryNode = node.NodeName == "PLUS_PLUS"
                ? isPost ? XzaarNode.PostIncrementor(item.PositionInfo, item) : XzaarNode.Incrementor(item.PositionInfo, item)
                : isPost ? XzaarNode.PostDecrementor(item.PositionInfo, item) : XzaarNode.Decrementor(item.PositionInfo, item);

            //var assignNode = XzaarNode.Assign(item, node.NodeName == "PLUS_PLUS"
            //    ? XzaarNode.BinaryOperator(0, item, '+', XzaarNode.Number(1))
            //    : XzaarNode.BinaryOperator(0, item, '-', XzaarNode.Number(1)));

            ctx.SetScopeNode(unaryNode);
            ctx.Stack.Push(unaryNode);

            // peek next node to see if we should end here or not.
            // because if the next node is arithmetic then this is part of an expression, so we should keep going


            if (ctx.CurrentNodeIndex + 1 < nodes.Count)
            {
                if (CanBeChained(unaryNode, nodes[ctx.CurrentNodeIndex + 1]))
                {
                    // ctx.Stack.Push(unaryNode);
                    // ctx.CurrentNodeIndex++;                    
                    Walk(ctx, nodes, nodes[ctx.CurrentNodeIndex + 1]);
                }
                else
                {
                    if (nodes[ctx.CurrentNodeIndex + 1].NodeType == XzaarNodeTypes.RETURN)
                        ctx.CurrentNodeIndex++;
                }
            }

        }

        private void WalkConditionalOperator(
                                     XzaarExpressionTransformerContext ctx,
                                     IList<XzaarNode> nodes,
                                     XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;
            GetLeftAndRightNodes(ctx, out left, out right, nodes, node);

            var tmpContext = ctx.With();
            XzaarNode lastStackItem = null;

            do
            {
                WalkNodes(tmpContext, nodes);
                if (tmpContext.CurrentNodeIndex > nodes.Count || ctx.Stack.Count == 0) break;
                lastStackItem = ctx.Stack.Peek();
            } while (tmpContext.CurrentNodeIndex < nodes.Count
                     && lastStackItem?.NodeType != XzaarNodeTypes.EQUALITY
                     && lastStackItem?.NodeType != XzaarNodeTypes.CONDITIONAL);

            AdvanceToNextUnwalked(ctx, nodes);

            var eqNode = lastStackItem as EqualityOperatorNode;
            if (eqNode != null && ctx.CurrentNodeIndex < nodes.Count)
            {
                var next = nodes[ctx.CurrentNodeIndex];
                var r = eqNode.Right;
                while (CanBeChained(r, next))
                {
                    ctx.Stack.Push(r);
                    Walk(ctx, nodes, next);
                    r = ctx.Stack.Pop();
                    if (ctx.CurrentNodeIndex >= nodes.Count || next == nodes[ctx.CurrentNodeIndex])
                        break;
                    next = nodes[ctx.CurrentNodeIndex];
                }
                eqNode.SetRight(r);
            }
            if (ctx.Stack.Count > 0)
            {
                var tempRight = ctx.Stack.Pop();
                if (!right.IsTransformed) right = tempRight;

            }
            //right = ctx.Stack.Count > 0 ? ctx.Stack.Pop() : right;

            ctx.From(tmpContext);

            var order = GetOperatingOrderWeight(node.Value.ToString());
            var rightConditional = right as ConditionalOperatorNode;
            if (right.OperatingOrder < order && rightConditional != null)
            {
                // rebuild right statement?
                // ex input:  a && (b || c)
                // result:   (a && b) || c

                // 1. take Left as L
                // 2. take Right's Left as RL
                // 3. take Right's Right as RR
                // 4. make new And with L, RL as LC
                // 5. make new Or  with LC, RR as RC
                // 6. push RC to stack

                var l = left;
                var rl = right.LeftParams[0];
                var rr = right.RightParams[0];
                var lc = XzaarNode.ConditionalOperator(l.PositionInfo, order, l, node.Value.ToString(), rl);
                var rc = XzaarNode.ConditionalOperator(right.PositionInfo, right.OperatingOrder, lc, rightConditional.Op, rr);
                ctx.SetScopeNode(rc);
                ctx.Stack.Push(rc);
                return;
            }

            var conditionalOperatorNode = XzaarNode.ConditionalOperator(left.PositionInfo, order, left, node.Value.ToString(), right);
            ctx.SetScopeNode(conditionalOperatorNode);
            ctx.Stack.Push(conditionalOperatorNode);
        }

        private XzaarNode ExtractExpression(XzaarExpressionTransformerContext ctx,
                                                      IList<XzaarNode> nodes,
                                                      XzaarNode expr)
        {
            var popped = false;
            int stackSize = ctx.Stack.Count;
            int lastIndex = ctx.CurrentNodeIndex;
            var startPos = ctx.CurrentNodeIndex;
            var negate = ctx.NegateNext;
            while (expr.NodeType == XzaarNodeTypes.NEGATE_OPERATOR)
            {
                if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                    throw new XzaarExpressionTransformerException("Unexpected end of file");

                expr = nodes[++ctx.CurrentNodeIndex];
                negate = !negate;
            }
            if (ctx.NegateNext != negate)
                ctx.NegateNext = !negate;

            if (expr.NodeType != XzaarNodeTypes.ARRAYINDEX && expr.NodeType != XzaarNodeTypes.BODY && expr.NodeType != XzaarNodeTypes.EXPRESSION && expr is LiteralNode)
            {
                ctx.Stack.Push(expr);
                ctx.CurrentNodeIndex++;
                // build body with the statement ahead.
                // so go through all nodes until we can determine a end of expression by any of the following ';' ',' or 'ELSE'

                do
                {
                    if (ctx.CurrentNodeIndex < nodes.Count)
                    {
                        var current = nodes[ctx.CurrentNodeIndex];
                        var strValueCurrent = current.Value?.ToString();
                        if (strValueCurrent == "," || strValueCurrent == ";" || strValueCurrent == ":" || current.NodeName == "ELSE")
                        {
                            expr = ctx.Stack.Pop();
                            // nodeIndex++;
                            break;
                        }
                        var ss1 = ctx.Stack.Count;
                        var peeked = ctx.Stack.Peek();

                        // check if the item from the stack can be chained with this one to build a larger expression
                        // if not, then we should break.
                        if (!CanBeChained(peeked, current))
                        {
                            expr = ctx.Stack.Pop();
                            AdvanceToNextUnwalked(ctx, nodes);
                            //if (!ctx.MoveNext && current.NodeType == XzaarNodeTypes.RETURN && !current.Walked)//ctx.CurrentNodeIndex > startPos + 1)
                            //{
                            //    ctx.CurrentNodeIndex--;
                            //}
                            break;
                        }

                        WalkNodes(ctx, nodes);



                        if (ctx.Stack.Count != ss1 && ctx.Stack.Count > 0)
                        {
                            var item = ctx.Stack.Peek();
                            if (!(item is LiteralNode))
                            {
                                // anything but literal node is a valid statement
                                expr = ctx.Stack.Pop();
                                //nodeIndex++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (ctx.Stack.Count > stackSize)
                            expr = ctx.Stack.Pop();
                    }
                    if (lastIndex == ctx.CurrentNodeIndex)
                    {
                        ctx.CurrentNodeIndex++;
                        lastIndex = ctx.CurrentNodeIndex;
                    }
                } while (ctx.CurrentNodeIndex + 1 < nodes.Count);
            }
            else if (expr.NodeType == XzaarNodeTypes.BODY || expr.NodeType == XzaarNodeTypes.EXPRESSION || expr.NodeType == XzaarNodeTypes.ARRAYINDEX)
            {
                if (ctx.MoveNext) ctx.CurrentNodeIndex++;
            }
            //else if (expr.NodeType == XzaarNodeTypes.RETURN)
            //{
            //    WalkReturn(ctx, nodes, expr);
            //    ctx.CurrentNodeIndex++;
            //}
            if (ctx.Stack.Count > stackSize && !popped)
                expr = ctx.Stack.Pop();


            if (ctx.MoveNext)
            {
                while (ctx.CurrentNodeIndex < nodes.Count && IsEndOfExpression(nodes[ctx.CurrentNodeIndex].Value?.ToString()))
                    ctx.CurrentNodeIndex++;
            }

            AdvanceToNextUnwalked(ctx, nodes);
            // if (lastIndex == ctx.CurrentNodeIndex && ctx.MoveNext) ctx.CurrentNodeIndex++;

            ctx.SetScopeNode(expr);
            return expr;
        }

        private bool CanBeChained(XzaarNode a, XzaarNode b)
        {
            string leftType = null;
            string rightType = null;
            if (a is MemberAccessChainNode) leftType = (a as MemberAccessChainNode).ResultType;
            if (b is MemberAccessChainNode) rightType = (b as MemberAccessChainNode).ResultType;

            // LEFT -> RIGHT
            if (leftType == "string" && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (leftType == "string" && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (leftType == "string" && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (leftType == "number" && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (leftType == "number" && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (leftType == "number" && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (leftType == "any" && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (leftType == "any" && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (leftType == "any" && b.NodeType == XzaarNodeTypes.EQUALITY) return true;

            // RIGHT -> LEFT
            if (rightType == "string" && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (rightType == "string" && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (rightType == "string" && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (rightType == "number" && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (rightType == "number" && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (rightType == "number" && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (rightType == "any" && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (rightType == "any" && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (rightType == "any" && a.NodeType == XzaarNodeTypes.EQUALITY) return true;


            if (a.NodeType == XzaarNodeTypes.RETURN)
            {
                if (b.NodeType == XzaarNodeTypes.LITERAL) return true;
                if (b.NodeType == XzaarNodeTypes.ACCESS) return true;
                if (b.NodeType == XzaarNodeTypes.CALL) return true;
                if (b.NodeType == XzaarNodeTypes.EXPRESSION) return true;
                if (b.NodeType == XzaarNodeTypes.ACCESSOR_CHAIN) return true;
                if (b.NodeType == XzaarNodeTypes.ASSIGN) return true;
                if (b.NodeType == XzaarNodeTypes.UNARY_OPERATOR) return true;
                if (b.NodeType == XzaarNodeTypes.MATH) return true;
                if (b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            }

            if (a.NodeType == XzaarNodeTypes.EQUALITY)
            {
                if (b.NodeType == XzaarNodeTypes.CONDITIONAL_OPERATOR) return true; // a == b && c
                if (b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;  // a == b + ""
            }

            // A -> B
            if (a.NodeType == XzaarNodeTypes.EXPRESSION && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.EXPRESSION && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.EXPRESSION && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.EXPRESSION && b.NodeType == XzaarNodeTypes.CONDITIONAL_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.EXPRESSION && b.NodeType == XzaarNodeTypes.CONDITIONAL) return true;
            if (a.NodeType == XzaarNodeTypes.EXPRESSION && b.NodeType == XzaarNodeTypes.ACCESSOR) return true;

            if (a.NodeType == XzaarNodeTypes.NEGATE && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.NEGATE && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.NEGATE && b.NodeType == XzaarNodeTypes.CONDITIONAL_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.NEGATE && b.NodeType == XzaarNodeTypes.CONDITIONAL) return true;


            if (a.NodeType == XzaarNodeTypes.UNARY_OPERATOR && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.UNARY_OPERATOR && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.UNARY_OPERATOR && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.LITERAL && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.LITERAL && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.LITERAL && b.NodeType == XzaarNodeTypes.ASSIGN) return true;
            if (a.NodeType == XzaarNodeTypes.LITERAL && b.NodeType == XzaarNodeTypes.ASSIGNMENT_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.LITERAL && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.LITERAL && b.NodeType == XzaarNodeTypes.UNARY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.ASSIGN) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.ASSIGNMENT_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.UNARY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.ACCESS && b.NodeType == XzaarNodeTypes.ARRAYINDEX) return true;

            if (a.NodeType == XzaarNodeTypes.MATH && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.MATH && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.MATH && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;

            if (a.NodeType == XzaarNodeTypes.CALL && b.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.CALL && b.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (a.NodeType == XzaarNodeTypes.CALL && b.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.CALL && b.NodeType == XzaarNodeTypes.CONDITIONAL_OPERATOR) return true;
            if (a.NodeType == XzaarNodeTypes.CALL && b.NodeType == XzaarNodeTypes.CONDITIONAL) return true;
            if (a.NodeType == XzaarNodeTypes.CALL && b.NodeType == XzaarNodeTypes.ACCESSOR) return true;

            // B -> A
            if (b.NodeType == XzaarNodeTypes.UNARY_OPERATOR && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (b.NodeType == XzaarNodeTypes.UNARY_OPERATOR && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.UNARY_OPERATOR && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.LITERAL && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (b.NodeType == XzaarNodeTypes.LITERAL && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.LITERAL && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.ACCESS && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (b.NodeType == XzaarNodeTypes.ACCESS && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.ACCESS && a.NodeType == XzaarNodeTypes.ASSIGN) return true;
            if (b.NodeType == XzaarNodeTypes.ACCESS && a.NodeType == XzaarNodeTypes.ASSIGNMENT_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.ACCESS && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.ACCESS && a.NodeType == XzaarNodeTypes.UNARY_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.MATH && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.MATH && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (b.NodeType == XzaarNodeTypes.MATH && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.CALL && a.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR) return true;
            if (b.NodeType == XzaarNodeTypes.CALL && a.NodeType == XzaarNodeTypes.EQUALITY) return true;
            if (b.NodeType == XzaarNodeTypes.CALL && a.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR) return true;

            return false;
        }

        private bool IsEndOfExpression(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value == "," || value == ";" || value == "else";
        }

        private int GetOperatingOrderWeight(string op)
        {
            op = op.ToLower();
            switch (op)
            {
                case "&&":
                case "and":
                    return 5;

                case "||":
                case "or":
                    return 4;

                case "/": return 3;
                case "*": return 2;
                case "%": return 1;

                case "-":
                case "+":
                    return 0;
            }
            return 0;
        }

        private void GetLeftAndRightNodes(
            XzaarExpressionTransformerContext ctx,
            out XzaarNode left,
            out XzaarNode right,
            IList<XzaarNode> nodes, XzaarNode node, bool makeNewScope = true)
        {
            left = ctx.Stack.Pop();

            right = nodes[++ctx.CurrentNodeIndex];

            var negate = ctx.NegateNext;
            while (right.NodeType == XzaarNodeTypes.NEGATE_OPERATOR)
            {
                if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                    throw new XzaarExpressionTransformerException("Unexpected end of file");

                right = nodes[++ctx.CurrentNodeIndex];
                negate = !negate;
            }
            if (ctx.NegateNext != negate)
                ctx.NegateNext = !negate;

            while (right.Walked && ctx.CurrentNodeIndex + 1 < nodes.Count) right = nodes[++ctx.CurrentNodeIndex];
            if (node == right) if (ctx.CurrentNodeIndex + 1 < nodes.Count) right = nodes[++ctx.CurrentNodeIndex];

            var before = ctx.Stack.ToList();
            right.Walked = true;
            TraverseExpression(ctx, nodes, left, makeNewScope);
            XzaarNode peeked = null;
            if (ctx.Stack.Count > before.Count)
            {
                // peeked = ctx.Stack.Peek();
                right = ctx.Stack.Peek();
            }
            // if (peeked == null || CanBeChained(right, peeked))
            if (!right.IsTransformed)
            {
                right = TraverseRightSide(ctx, nodes, right);
            }

            var after = ctx.Stack.ToList();
            if (before.Count != after.Count)
            {
                var newerRight = ctx.Stack.Peek();
                if (newerRight.Index >= right.Index)
                {
                    right = ctx.Stack.Pop();
                }
                else
                {
                    // toss it
                    ctx.Stack.Pop();
                }
            }

            if (negate)
            {
                right = XzaarNode.Negate(right.PositionInfo, right);
            }
        }

        private XzaarNode TraverseRightSide(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode right, bool makeNewScope = true)
        {
            XzaarNode right1 = null;
            var stackBefore = ctx.Stack.Count;
            TraverseExpression(ctx, nodes, right, makeNewScope);
            if (stackBefore != ctx.Stack.Count) right1 = ctx.Stack.Pop();
            if (right1 != null) right = right1;
            // if (right)
            if (right.NodeType == XzaarNodeTypes.BLOCK || right.NodeType == XzaarNodeTypes.BODY)
            {
                var b = right as BlockNode;
                if (b != null && b.Children != null)
                {
                    var unhandledChildren = b.Children.Where(c => !c.IsTransformed).ToArray();
                    //var unhandledChildren = b.Children.Where(c => c.Index == -1 || c.Walked
                    //    || c.NodeType == XzaarNodeTypes.ARITHMETIC_OPERATOR || c.NodeType == XzaarNodeTypes.UNARY_OPERATOR || c.NodeType == XzaarNodeTypes.ASSIGNMENT_OPERATOR
                    //    || c.NodeType == XzaarNodeTypes.EQUALITY_OPERATOR || c.NodeType == XzaarNodeTypes.CONDITIONAL_OPERATOR || c.NodeType == XzaarNodeTypes.NEGATE_OPERATOR
                    //    || c.NodeType == XzaarNodeTypes.LAMBDA_OPERATOR || c.NodeType == XzaarNodeTypes.NULL_COALESCING_OPERATOR || c.NodeType == XzaarNodeTypes.NULL_EMPTY
                    //).ToArray();
                    if (unhandledChildren.Length == b.Children.Count)
                        foreach (var c in unhandledChildren)
                            b.RemoveChild(c);
                }
            }
            // right.Walked = true;
            return right;
        }

        private void TraverseExpression(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode expr, bool makeNewScope)
        {
            if (!ctx.Stack.Contains(expr) && expr.NodeType == XzaarNodeTypes.LITERAL)
            {
                if (ctx.CurrentNodeIndex + 1 < nodes.Count &&
                    nodes[ctx.CurrentNodeIndex + 1].NodeType == XzaarNodeTypes.ACCESSOR)
                {
                    Walk(ctx, nodes, expr);
                    if (nodes[ctx.CurrentNodeIndex].NodeType == XzaarNodeTypes.ACCESSOR)
                    {
                        WalkAccessor(ctx, nodes, nodes[ctx.CurrentNodeIndex]);
                    }
                }
                else
                {
                    Walk(ctx, nodes, expr, true);
                }
            }

            var children = expr.Children;
            var oldIndex = ctx.CurrentNodeIndex;
            var tempContext = ctx.With(currentNodeIndex: 0);
            tempContext.SetScopeNode(expr);
            if (makeNewScope) tempContext.In();
            while (tempContext.CurrentNodeIndex < children.Count)
            {
                WalkNodes(tempContext, children);
                AdvanceToNextUnwalked(tempContext, children);
            }
            if (makeNewScope) tempContext.Out();
            ctx.From(tempContext, oldIndex);
        }
    }
}
