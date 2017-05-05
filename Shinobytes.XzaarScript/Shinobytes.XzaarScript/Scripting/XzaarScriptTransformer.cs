using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarScriptTransformer
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
                if (f != null) ctx.AddGlobalFunction(f);
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
                return XzaarScriptBlockSorter.SortBlocks(XzaarScriptBlockReducer.ReduceBlocks(new EntryNode(XzaarNode.Block(XzaarNode.Block(programNodes.ToArray())))));
            }
            if (programNodes.Count > 0)
            {
                return XzaarScriptBlockSorter.SortBlocks(XzaarScriptBlockReducer.ReduceBlocks(new EntryNode(programNodes[0])));
            }
            return new EntryNode(XzaarNode.Empty());
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
                    node = XzaarNode.Block(newBlockBody.ToArray());
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

            var lastAccessorItem = ctx.Stack.Peek();

            // find next accessor member until no accessor type can be found

            if (lastAccessorItem.NodeType != XzaarNodeTypes.ACCESS && lastAccessorItem.NodeType != XzaarNodeTypes.CALL)
                throw new XzaarExpressionTransformerException(
                    "Unexpected item used with an accessor expression: Uh nuh! I don't think the item you're trying to use has any properties or available functions. What is a '" +
                    lastAccessorItem.Value + "' anyway?");

            if (ctx.CurrentNodeIndex + 1 >= nodes.Count)
                throw new XzaarExpressionTransformerException(
                    "Unexpected end of accessor expression: Look, if you're going to try and access a member of the target instanced item then you better let us know exactly what you want.");



            while (ctx.CurrentNodeIndex < nodes.Count)
            {
                lastAccessorItem = ctx.Stack.Pop();

                var nextAccessorItem = nodes[++ctx.CurrentNodeIndex];
                //if (nextAccessorItem.NodeType == XzaarNodeTypes.ACCESSOR)
                //    throw new XzaarExpressionTransformerException(
                //        "Invalid accessor expression: You cannot access an accessor, typo or did you forget to enter a member? Anyway, this is just weird. '" +
                //        lastAccessorItem.Value + "..'");

                var previousStackCount = ctx.Stack.Count;
                Walk(ctx, nodes, nextAccessorItem);
                var newStackItems = ctx.Stack.Count - previousStackCount;

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
                                var item = ctx.Stack.Pop() as MemberAccessNode;
                                var memberAccess = XzaarNode.MemberAccess(item.Member, item.ArrayIndex, lastMemberAccess.MemberType, memberType);
                                var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem, memberAccess);
                                ctx.Stack.Push(memberAccessChain);
                            }
                            else
                            {

                                var memberAccess = XzaarNode.MemberAccess(nextAccessorItem, lastMemberAccess.MemberType, memberType);
                                var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem, memberAccess);
                                ctx.Stack.Push(memberAccessChain);
                            }
                        }
                        else
                        {
                            var accessChain = lastAccessorItem as MemberAccessChainNode;
                            if (accessChain != null)
                            {
                                var memberType = FindMemberType(ctx, accessChain.ResultType, nextAccessorItem);
                                var memberAccess = XzaarNode.MemberAccess(nextAccessorItem, accessChain.ResultType,
                                    memberType);
                                var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem, memberAccess);
                                ctx.Stack.Push(memberAccessChain);
                            }
                            else
                            {
                                throw new XzaarExpressionTransformerException("Nope. This accessor wont do!");
                            }
                        }
                    }
                    else if (nextAccessorItem.NodeType == XzaarNodeTypes.CALL)
                    {
                        // access a function/method member                        
                        var memberAccess = nextAccessorItem as FunctionCallNode;
                        var returnType = "any";
                        // TODO: find a way to get the return type of a function more accurately. Dynamically determing the return type just wont do... God damnit!
                        var memberAccessChain = XzaarNode.MemberAccessChain(lastAccessorItem, memberAccess, returnType);
                        ctx.Stack.Push(memberAccessChain);
                    }
                    else
                    {
                        // access a what?
                        throw new XzaarExpressionTransformerException("Bad accessor expression: I don't even... '" +
                                                                      nextAccessorItem.Value +
                                                                      " is not a valid member of '" +
                                                                      lastAccessorItem.Value + "'");
                    }
                }
                else
                {
                    // not sure if this is acceptable or not..
                    throw new XzaarExpressionTransformerException("Bad accessor expression: I don't even... '" +
                                              nextAccessorItem.Value +
                                              " is not a valid member of '" +
                                              lastAccessorItem.Value + "'");
                }

                if (ctx.CurrentNodeIndex >= nodes.Count || nodes[ctx.CurrentNodeIndex].NodeType != XzaarNodeTypes.ACCESSOR)
                {
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

            Walk(ctx, nodes, node);
        }

        private void WalkArrayIndexer(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // previous item should be the variable (from stack)
            // this one is '['
            // next one is Indexer (can be expression)
            // last one is ']'
            // finally, this should be pushed as member access

            var isArrayIndex = node is ArrayIndexNode;
            var variable = ctx.Stack.Pop();

            var arrayIndex = isArrayIndex ? nodes[ctx.CurrentNodeIndex] : nodes[++ctx.CurrentNodeIndex];

            arrayIndex = TraverseRightSide(ctx, nodes, arrayIndex);

            ctx.CurrentNodeIndex++;

            ctx.Stack.Push(XzaarNode.VariableAccess(
                variable, arrayIndex
            ));

            if (!isArrayIndex) ctx.CurrentNodeIndex++;
        }

        private void WalkReturn(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            ctx.CurrentNodeIndex++;

            // check if there is a possible 'return' expression on the next node. otherwise just ignore it

            if (ctx.CurrentNodeIndex < nodes.Count)
            {
                var nextNode = nodes[ctx.CurrentNodeIndex];

                nextNode = TraverseExtractAndAddDeclaredVariables(ctx, nodes, nextNode);

                if (nextNode != null)
                {
                    ctx.Stack.Push(XzaarNode.Return(nextNode));
                    return;
                }
            }
            ctx.Stack.Push(XzaarNode.Return());
        }

        private void WalkBreak(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            ctx.CurrentNodeIndex++;

            ctx.Stack.Push(XzaarNode.Break());
        }

        private void WalkContinue(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            ctx.CurrentNodeIndex++;

            ctx.Stack.Push(XzaarNode.Continue());
        }

        private void WalkGoTo(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // goto <label_name>

            var labelTarget = nodes[++ctx.CurrentNodeIndex];
            var literal = labelTarget as LiteralNode;
            if (literal == null)
                throw new XzaarExpressionTransformerException("Expected a literal label name after the 'goto' keyword.");

            ctx.Stack.Push(XzaarNode.Goto(literal.Value + ""));
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

                    structFields.Add(XzaarNode.Field(type, name, structName));

                    // skip next node if its a semicolon or comma as they are both acceptable deliminators for the struct fields
                    if (i < structBody.Children.Count)
                    {
                        var peek = structBody.Children[i];
                        var strValue = peek.Value + "";
                        if (strValue == ";" || strValue == ",") i++;
                    }
                }
            }

            var s = XzaarNode.Struct(structName, structFields.ToArray());
            ctx.AddGlobalStruct(s);
            ctx.SetScopeNode(s);
            ctx.CurrentNodeIndex++;
        }

        private void WalkVariable(
                              XzaarExpressionTransformerContext ctx,
                              IList<XzaarNode> nodes)
        {
            var next = nodes[++ctx.CurrentNodeIndex];
            // we expect the next node to be a name node as we want to define the name of the variable
            if (next.NodeName == "NAME")
            {
                var variableName = next.Value?.ToString();
                if (string.IsNullOrEmpty(variableName))
                    throw new XzaarExpressionTransformerException(
                        "Variable cannot have a empty name. How the heck did this happen anyway?");


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

                            var isArrayIndex = peeked is ArrayIndexNode;
                            if (peeked.NodeType == XzaarNodeTypes.BRACKET && peeked.NodeName == "LBRACKET" ||
                                isArrayIndex)
                            {
                                // define array variable
                                var endingBracket = nodes[++ctx.CurrentNodeIndex];
                                if ((endingBracket.NodeType != XzaarNodeTypes.BRACKET ||
                                     endingBracket.NodeName != "RBRACKET") && !isArrayIndex)
                                {
                                    throw new XzaarExpressionTransformerException(
                                        "Did you try to define an array but completely failed?");
                                }

                                variableType = "array";
                                var dvn = XzaarNode.DefineVariable(variableType, variableName,
                                    XzaarNode.NewArrayInstance());
                                ctx.AddVariableToScope(dvn);
                                if (!isArrayIndex) ctx.CurrentNodeIndex++; // skip the RBRACKET
                                return;
                            }

                            if (assignValue.NodeType == XzaarNodeTypes.EXPRESSION)
                            {
                                assignValue = TraverseRightSide(ctx, nodes, assignValue);
                            }

                            if (peeked.NodeType == XzaarNodeTypes.LITERAL)
                            {
                                // check if the node afterwards is any indication of expression
                                // for instance (MATH) or other Binary Op
                                if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                                {
                                    var nextNode = nodes[ctx.CurrentNodeIndex + 1];
                                    if (IsExpression(nextNode.NodeType))
                                    {
                                        assignValue = TraverseExtractAndAddDeclaredVariables(ctx, nodes, assignValue);
                                        ctx.CurrentNodeIndex--;
                                        ctx.AddVariableToScope(XzaarNode.DefineVariable("NUMBER", variableName, assignValue));
                                        return;
                                    }
                                    // check if next one after this is an accessor or not
                                    if (nextNode.NodeType == XzaarNodeTypes.ACCESSOR)
                                    {
                                        var before = ctx.Stack.Count;
                                        var variables = ctx.GetVariablesInScope();
                                        var targetVariable = variables.FirstOrDefault(v => v.Name == peeked.Value + "");
                                        ctx.Stack.Push(XzaarNode.VariableAccess(peeked, targetVariable.Type));
                                        ctx.CurrentNodeIndex++;
                                        this.WalkAccessor(ctx, nodes, nextNode);
                                        var newInStack = ctx.Stack.Count - before;
                                        if (newInStack > 0)
                                        {
                                            var target = ctx.Stack.Pop() as MemberAccessChainNode;
                                            var newVariable = XzaarNode.DefineVariable(target.ResultType, variableName, target);
                                            ctx.AddVariableToScope(newVariable);
                                            return;
                                        }
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
                                        assignValue = XzaarNode.CreateStruct(str);
                                        variableType = str.Name;
                                    }
                                }
                            }

                        }

                        ctx.CurrentNodeIndex++;
                        var defineVariableNode = XzaarNode.DefineVariable(variableType, variableName, assignValue);
                        ctx.AddVariableToScope(defineVariableNode);
                    }
                    else
                    {
                        if (peeked.NodeType == XzaarNodeTypes.LITERAL && peeked.NodeName == "IN")
                        {
                            ctx.AddVariableToScope(XzaarNode.DefineVariable(XzaarBaseTypes.Any.Name, variableName, null));
                            // i'm so pretty sure we're inside a foreach loop expression right now. so lets unwind by setting our current node index to the node count and return
                            ctx.State = ctx.CurrentNodeIndex;
                            ctx.CurrentNodeIndex = nodes.Count;
                            return;
                        }

                        var defineVariableNode = XzaarNode.DefineVariable(XzaarBaseTypes.Any.Name, variableName, null);
                        ctx.AddVariableToScope(defineVariableNode);
                    }

                    //ctx.Stack.Push(defineVariableNode);
                }
                else
                {
                    var defineVariableNode = XzaarNode.DefineVariable(XzaarBaseTypes.Any.Name, variableName, null);
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

            var targetFunctionName = nodes[++ctx.CurrentNodeIndex];
            if (targetFunctionName.NodeType != XzaarNodeTypes.LITERAL)
            {
                throw new XzaarExpressionTransformerException("This ain't Game of Thrones, a function needs a name!");
            }

            var fnName = targetFunctionName.Value?.ToString();
            var targetFunctionArguments = nodes[++ctx.CurrentNodeIndex];

            if (isExtern)
            {
                var returnTypeValue = TryGetReturnType(ctx, nodes, fnName);

                var terminate = nodes[++ctx.CurrentNodeIndex];
                if (terminate.Value?.ToString() != ";")
                {
                    throw new XzaarExpressionTransformerException("Missing expected ';' after extern function declaration.");
                }

                result = returnTypeValue != null
                    ? XzaarNode.ExternFunction(fnName, targetFunctionArguments)
                    : XzaarNode.ExternFunction(fnName, returnTypeValue, targetFunctionArguments);

                ctx.In(fnName, result);
            }
            else
            {
                var returnTypeValue = TryGetReturnType(ctx, nodes, fnName);

                ctx.In(fnName);

                var targetFunctionBody = nodes[++ctx.CurrentNodeIndex];

                targetFunctionBody = TraverseExtractAndAddDeclaredVariables(ctx, nodes, targetFunctionBody, false);

                result = returnTypeValue != null
                    ? XzaarNode.Function(fnName, returnTypeValue, targetFunctionArguments, targetFunctionBody)
                    : XzaarNode.Function(fnName, targetFunctionArguments, targetFunctionBody);

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
            var condition = nodes[++ctx.CurrentNodeIndex];

            condition = TraverseRightSide(ctx, nodes, condition);
            // -- if we want to allow using if statements without paranthesis, we need to uncomment the line below. (not properly tested)
            // condition = ExtractExpression(ref nodeIndex, nodes, stack, condition);

            var ifTrue = nodes[++ctx.CurrentNodeIndex];

            ifTrue = TraverseExtractAndAddDeclaredVariables(ctx, nodes, ifTrue);

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

                var ifElseThen = XzaarNode.IfElseThen(condition, ifTrue, ifFalse);
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
                var conditionalNode = XzaarNode.IfThen(condition, ifTrue);
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
            node = TraverseRightSide(ctx, nodes, node, false);
            node = ExtractExpressionBlock(ctx, nodes, node);

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
                    node = XzaarNode.Block(node);
                    var i = 0;
                    foreach (var v in newVariables)
                        node.InsertChild(i++, v);
                }
            }

            node.SortChildren();

            if (makeNewScope) ctx.Out();
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
                    ifTrue = XzaarNode.Block(items.ToArray());
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

            var valueExpression = TraverseExtractAndAddDeclaredVariables(ctx, nodes, nodes[++ctx.CurrentNodeIndex]);

            if (ctx.CurrentNodeIndex >= nodes.Count)
                throw new XzaarExpressionTransformerException(
                    "So no switch/match body? Hmm.. Did your refactoring go wrong or you're not done yet?");

            var bodyNode = nodes[ctx.CurrentNodeIndex];
            var oldStackCount = ctx.Stack.Count;
            var oldNodeIndex = ctx.CurrentNodeIndex;
            var tmpCtx = ctx.With(currentNodeIndex: 0);

            while (tmpCtx.CurrentNodeIndex < bodyNode.Children.Count)
            {
                WalkSwitchCase(tmpCtx, bodyNode.Children, bodyNode.Children[tmpCtx.CurrentNodeIndex]);
            }

            ctx.From(tmpCtx, oldNodeIndex);
            var cases = new List<CaseNode>();
            var newCount = ctx.Stack.Count - oldStackCount;
            for (var i = 0; i < newCount; i++) cases.Add(ctx.Stack.Pop() as CaseNode);
            cases.Reverse(); // the cases are added in a reversed order, so lets reverse em :)

            var switchMatchNode = XzaarNode.Switch(
                    valueExpression,
                    cases.ToArray()
                );

            ctx.Stack.Push(switchMatchNode);
            ctx.SetScopeNode(switchMatchNode);
            ctx.CurrentNodeIndex++;
        }


        private void WalkSwitchCase(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // case <value or expr>:
            //   <body>
            //   break | return | continue

            var matchNode = nodes[++ctx.CurrentNodeIndex];
            var matchExpression = TraverseExtractAndAddDeclaredVariables(ctx, nodes, matchNode);

            var singleRowExpression = false;
            var body = nodes[++ctx.CurrentNodeIndex];

            ctx.MoveNext = false;
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body, false);
            ctx.MoveNext = true;
            ctx.CurrentNodeIndex++;
            if (ctx.CurrentNodeIndex < nodes.Count)
            {
                var peekNode = nodes[ctx.CurrentNodeIndex];
                if (peekNode is ControlFlowNode && peekNode.NodeName != "CASE")
                {
                    // ctx.CurrentNodeIndex++;                                        
                    body = XzaarNode.Body(body, peekNode);
                }
                else
                {
                    // get all until a <return | continue | break | goto> is found
                    var bodyNodes = new List<XzaarNode>();
                    while (!(peekNode is ControlFlowNode) && peekNode.NodeName != "CASE")
                    {
                        ctx.MoveNext = false;
                        peekNode = TraverseExtractAndAddDeclaredVariables(ctx, nodes, peekNode, false);
                        ctx.MoveNext = true;

                        bodyNodes.Add(peekNode);
                        peekNode = nodes[++ctx.CurrentNodeIndex];
                    }

                    if (peekNode is ControlFlowNode && peekNode.NodeName != "CASE")
                    {
                        bodyNodes.Add(peekNode);
                    }

                    if (bodyNodes.Count > 0)
                    {
                        bodyNodes.Insert(0, body);
                        body = XzaarNode.Body(bodyNodes.ToArray());
                    }

                }
            }

            if (!(body is BodyNode))
            {
                body = XzaarNode.Body(body);
                singleRowExpression = true;
            }

            var caseNode = XzaarNode.Case(matchExpression, body);

            ctx.Stack.Push(caseNode);
            ctx.SetScopeNode(caseNode);
            if (!singleRowExpression) ctx.CurrentNodeIndex++;
        }


        private void WalkLabel(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode node)
        {
            // similar to defining a variable but.. a bit simpler?
            // <label_name>:

            var labelName = node.Value + "";
            if (labelName.EndsWith(":"))
            {
                // well thats it, our job is done.
                ctx.Stack.Push(XzaarNode.Label(labelName));
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

            expr = TraverseExtractAndAddDeclaredVariables(ctx, nodes, expr);

            var cExpr = expr.Children.First();
            var inIndex = IndexOfForeachIn(cExpr);
            if (inIndex == -1) throw new XzaarExpressionTransformerException("Somethings missing in your foreach loop. Perhaps the 'in' keyword?");
            if (inIndex + 1 > cExpr.Children.Count) throw new XzaarExpressionTransformerException("Somethings missing in your foreach loop. Perhaps the collection variable?");

            var collectionExpression = cExpr.Children[inIndex + 1];
            collectionExpression = TraverseRightSide(ctx, cExpr.Children, collectionExpression, false);
            var variableDefinition = expr.Children.Last();

            var body = nodes[ctx.CurrentNodeIndex];
            body = TraverseExtractAndAddDeclaredVariables(ctx, nodes, body);

            var foreachLoop = XzaarNode.Foreach(variableDefinition, collectionExpression, body);
            ctx.Stack.Push(foreachLoop);
            ctx.SetScopeNode(foreachLoop);
            ctx.CurrentNodeIndex++;
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

            var forLoop = XzaarNode.For(initExpr, testExpr, incrExpr, body);
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

                var conditionalNode = XzaarNode.DoWhile(condition, body);
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

            var conditionalNode = XzaarNode.While(condition, body);
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
                loopNode = XzaarNode.Loop(XzaarNode.Block(body));
                ctx.Stack.Push(loopNode);
            }
            else
            {
                loopNode = XzaarNode.Loop(blocky);
                ctx.Stack.Push(loopNode);
            }

            ctx.SetScopeNode(loopNode);
            // ctx.CurrentNodeIndex++;
        }

        private void WalkLiteral(
                                 XzaarExpressionTransformerContext ctx,
                                IList<XzaarNode> nodes,
                                XzaarNode node)
        {
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
                        ctx.Stack.Push(XzaarNode.VariableAccess(node, variable.Type));
                    }
                    else
                    {
                        var pars = ctx.GetParametersInScope();
                        var parm = pars.FirstOrDefault(v => v.Name == str);
                        if (parm != null)
                        {
                            ctx.Stack.Push(XzaarNode.ParameterAccess(node, parm.Type));
                        }
                        else
                        {

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

            var arguments = nodes[++ctx.CurrentNodeIndex];

            arguments = TraverseRightSide(ctx, nodes, arguments);
            arguments = ExtractExpression(ctx, nodes, arguments);

            FunctionCallNode call;
            if (arguments is ExpressionNode && arguments.Children.Count == 0)
            {
                // ctx.CurrentNodeIndex--;
                call = XzaarNode.Call(function, new ArgumentNode[0]);
            }
            else
            {
                var argumentNodes = new ArgumentNode[1];
                if (arguments is BlockNode)
                    argumentNodes = arguments.Children.Select(XzaarNode.Argument).ToArray();
                else
                    argumentNodes[0] = XzaarNode.Argument(arguments, 0);
                call = XzaarNode.Call(function, argumentNodes);
            }

            ctx.Stack.Push(call);
        }

        private void WalkAssignment(
                                    XzaarExpressionTransformerContext ctx,
                                    IList<XzaarNode> nodes,
                                    XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;
            GetLeftAndRightNodes(ctx, out left, out right, nodes);

            // check if next token (if one exists) is an accessor or not
            if (nodes.Count > ctx.CurrentNodeIndex + 1)
            {
                var peeked = nodes[ctx.CurrentNodeIndex + 1];
                if (peeked.NodeType == XzaarNodeTypes.ACCESSOR)
                {                   

                    var before = ctx.Stack.Count;
                    var variables = ctx.GetVariablesInScope();
                    var targetVariable = variables.FirstOrDefault(v => v.Name == right.Value + "");
                    ctx.Stack.Push(XzaarNode.VariableAccess(right, targetVariable.Type));
                    ctx.CurrentNodeIndex++;
                    this.WalkAccessor(ctx, nodes, peeked);
                    var newInStack = ctx.Stack.Count - before;
                    if (newInStack > 0)
                    {
                        var target = ctx.Stack.Pop() as MemberAccessChainNode;
                        ctx.Stack.Push(XzaarNode.Assign(left, target));                  
                        return;
                    }

                }

            }

            ctx.CurrentNodeIndex++;

            var op = node.Value.ToString();
            if (op.Length > 1)
            {
                var op0 = op[0];
                var order = GetOperatingOrderWeight(op0.ToString());
                if (op0 == '+' || op0 == '/' || op0 == '*' || op0 == '-' || op0 == '%' || op0 == '&' || op0 == '|')
                {
                    ctx.Stack.Push(XzaarNode.Assign(left,
                        XzaarNode.BinaryOperator(order, left, op0, right)));
                    return;
                }
            }

            ctx.Stack.Push(XzaarNode.Assign(left, right));
        }

        private void WalkArithmetic(
                                    XzaarExpressionTransformerContext ctx,
                                    IList<XzaarNode> nodes,
                                    XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;

            GetLeftAndRightNodes(ctx, out left, out right, nodes);

            ctx.CurrentNodeIndex++;

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

                var newRight = XzaarNode.BinaryOperator(order, left, node.Value.ToString(), right);
                ctx.Stack.Push(XzaarNode.BinaryOperator(oldLeft.OperatingOrder, oldLeft.LeftParams[0], oldLeft.Op,
                    newRight));
                return;
            }
            ctx.Stack.Push(XzaarNode.BinaryOperator(order, left, node.Value.ToString(), right));
        }

        private void WalkEquality(
                                  XzaarExpressionTransformerContext ctx,
                                  IList<XzaarNode> nodes,
                                  XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;
            GetLeftAndRightNodes(ctx, out left, out right, nodes);

            ctx.CurrentNodeIndex++;

            ctx.Stack.Push(XzaarNode.EqualityOperator(left, node.Value.ToString(), right));
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
            ctx.CurrentNodeIndex++;
        }

        private void WalkUnary(
                                      XzaarExpressionTransformerContext ctx,
                                      IList<XzaarNode> nodes,
                                      XzaarNode node)
        {
            // this can be a post decrementor | decrementor | post incrementor | incrementor
            XzaarNode item = null;
            var isPost = true;
            if (ctx.Stack.Count == 0 || ctx.CurrentNodeIndex == 0 ||
                (ctx.CurrentNodeIndex > 0 && nodes[ctx.CurrentNodeIndex - 1].NodeType != XzaarNodeTypes.LITERAL))
            {
                item = TraverseRightSide(ctx, nodes, nodes[++ctx.CurrentNodeIndex]);
                isPost = false;
            }
            else
            {
                item = ctx.Stack.Pop();
            }

            ctx.CurrentNodeIndex++;

            var unaryNode = node.NodeName == "PLUS_PLUS"
                ? isPost ? XzaarNode.PostIncrementor(item) : XzaarNode.Incrementor(item)
                : isPost ? XzaarNode.PostDecrementor(item) : XzaarNode.Decrementor(item);

            //var assignNode = XzaarNode.Assign(item, node.NodeName == "PLUS_PLUS"
            //    ? XzaarNode.BinaryOperator(0, item, '+', XzaarNode.Number(1))
            //    : XzaarNode.BinaryOperator(0, item, '-', XzaarNode.Number(1)));

            ctx.SetScopeNode(unaryNode);
            ctx.Stack.Push(unaryNode);
        }

        private void WalkConditionalOperator(
                                     XzaarExpressionTransformerContext ctx,
                                     IList<XzaarNode> nodes,
                                     XzaarNode node)
        {
            XzaarNode left;
            XzaarNode right;
            GetLeftAndRightNodes(ctx, out left, out right, nodes);

            var tmpContext = ctx.With();
            XzaarNode lastStackItem;

            do
            {
                WalkNodes(tmpContext, nodes);
                lastStackItem = ctx.Stack.Peek();
            } while (tmpContext.CurrentNodeIndex < nodes.Count
                     && lastStackItem?.NodeType != XzaarNodeTypes.EQUALITY
                     && lastStackItem?.NodeType != XzaarNodeTypes.CONDITIONAL);
            right = ctx.Stack.Pop();

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
                var lc = XzaarNode.ConditionalOperator(order, l, node.Value.ToString(), rl);
                var rc = XzaarNode.ConditionalOperator(right.OperatingOrder, lc, rightConditional.Op, rr);
                ctx.SetScopeNode(rc);
                ctx.Stack.Push(rc);
                return;
            }

            var conditionalOperatorNode = XzaarNode.ConditionalOperator(order, left, node.Value.ToString(), right);
            ctx.SetScopeNode(conditionalOperatorNode);
            ctx.Stack.Push(conditionalOperatorNode);
        }

        private XzaarNode ExtractExpression(XzaarExpressionTransformerContext ctx,
                                                      IList<XzaarNode> nodes,
                                                      XzaarNode expr)
        {
            int stackSize = ctx.Stack.Count;
            int lastIndex = ctx.CurrentNodeIndex;
            if (expr.NodeType != XzaarNodeTypes.ARRAYINDEX && expr.NodeType != XzaarNodeTypes.BODY && expr.NodeType != XzaarNodeTypes.EXPRESSION && expr is LiteralNode)
            {
                ctx.Stack.Push(expr);
                ctx.CurrentNodeIndex++;
                // build body with the statement ahead.
                // so go through all nodes until we can determine a end of expression by any of the following ';' ',' or 'ELSE'

                do
                {
                    if (ctx.CurrentNodeIndex + 1 < nodes.Count)
                    {
                        var current = nodes[ctx.CurrentNodeIndex];
                        var strValueCurrent = current.Value?.ToString();
                        if (strValueCurrent == "," || strValueCurrent == ";" || strValueCurrent == ":" || current.NodeName == "ELSE")
                        {
                            expr = ctx.Stack.Pop();
                            // nodeIndex++;
                            break;
                        }

                        WalkNodes(ctx, nodes);

                        if (ctx.Stack.Count > 0)
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
            if (ctx.Stack.Count > stackSize)
                expr = ctx.Stack.Pop();


            if (ctx.MoveNext)
            {
                while (ctx.CurrentNodeIndex < nodes.Count && IsEndOfExpression(nodes[ctx.CurrentNodeIndex].Value?.ToString()))
                    ctx.CurrentNodeIndex++;
            }

            if (lastIndex == ctx.CurrentNodeIndex && ctx.MoveNext) ctx.CurrentNodeIndex++;

            ctx.SetScopeNode(expr);
            return expr;
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
            IList<XzaarNode> nodes, bool makeNewScope = true)
        {
            left = ctx.Stack.Pop();

            right = nodes[++ctx.CurrentNodeIndex];

            var before = ctx.Stack.ToList();

            TraverseExpression(ctx, nodes, left, makeNewScope);
            right = TraverseRightSide(ctx, nodes, right);

            var after = ctx.Stack.ToList();
            if (before.Count != after.Count)
            {
                right = ctx.Stack.Pop();
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
                    var unhandledChildren = b.Children.Where(c => c.Index == -1).ToArray();
                    if (unhandledChildren.Length == b.Children.Count)
                        foreach (var c in unhandledChildren)
                            b.RemoveChild(c);
                }
            }

            return right;
        }

        private void TraverseExpression(XzaarExpressionTransformerContext ctx, IList<XzaarNode> nodes, XzaarNode expr, bool makeNewScope)
        {
            if (!ctx.Stack.Contains(expr) && expr.NodeType == XzaarNodeTypes.LITERAL)
            {
                Walk(ctx, nodes, expr, true);
            }

            var children = expr.Children;
            var oldIndex = ctx.CurrentNodeIndex;
            var tempContext = ctx.With(currentNodeIndex: 0);
            tempContext.SetScopeNode(expr);
            if (makeNewScope) tempContext.In();
            while (tempContext.CurrentNodeIndex < children.Count)
            {
                WalkNodes(tempContext, children);
            }
            if (makeNewScope) tempContext.Out();
            ctx.From(tempContext, oldIndex);
        }
    }
}