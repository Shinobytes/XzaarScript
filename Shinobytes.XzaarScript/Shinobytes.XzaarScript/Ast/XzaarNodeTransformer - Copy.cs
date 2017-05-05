//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Linq.Expressions;
//using Shinobytes.XzaarScript.Ast.Nodes;
//using Shinobytes.XzaarScript.Ast.NodesOld;
//using Shinobytes.XzaarScript.Utilities;

//namespace Shinobytes.XzaarScript.Ast
//{
//    public class XzaarNodeTransformer : INodeTransformer
//    {
//        private readonly XzaarSyntaxNode ast;
//        private TransformerScope currentScope;
//        private readonly List<string> errors = new List<string>();

//        private Dictionary<string, FunctionNode> definedFunctions = new Dictionary<string, FunctionNode>();
//        private Dictionary<string, StructNode> definedStructs = new Dictionary<string, StructNode>();

//        public XzaarNodeTransformer(XzaarSyntaxNode ast)
//        {
//            this.ast = ast;
//            this.currentScope = new TransformerScope();
//        }

//        public XzaarAstNode Transform()
//        {
//            return new EntryNode(WalkAllNodes());
//        }

//        public bool HasErrors { get { return errors.Count > 0; } }

//        public IList<string> Errors { get { return errors; } }

//        private XzaarAstNode WalkAllNodes()
//        {
//            var nodes = new List<XzaarAstNode>();
//            this.BeginScope(new XzaarNodeStream(ast.Children));

//            // var currentNode = this.Nodes.Current;
//            while (!this.Nodes.EndOfStream())
//            {
//                var astNode = Walk();
//                if (astNode != null)
//                    nodes.Add(astNode);
//                // currentNode = this.Nodes.Next();
//            }

//            this.EndScope();
//            return XzaarAstNode.Block(nodes.ToArray());
//        }

//        private XzaarAstNode Walk()
//        {

//            // NOTE: A node must always be returned unless error
//            //       if the next nodes require the previous node
//            //       then it has to parse the same node once more
//            //       but when it has been used by another node, 
//            //       the previous node should be replaced with the new node
//            //       This is so we can avoid using a stack and never
//            //       care to know whether our resulting node can be chained 
//            //       with the next. It saves us alot of headache and code maintenance

//            try
//            {
//                switch (Nodes.Current.Type)
//                {
//                    case XzaarSyntaxKind.Identifier:
//                        return WalkExpressionStatement(); // return WalkIdentifier();
//                    case XzaarSyntaxKind.Keyword: return WalkKeyword();
//                    case XzaarSyntaxKind.UnaryOperator: return WalkUnaryOperator();
//                    case XzaarSyntaxKind.AggregateObjectIndex:
//                    case XzaarSyntaxKind.ArrayIndexExpression:
//                        return WalkObjectIndex();
//                    case XzaarSyntaxKind.ArithmeticOperator: return WalkArithmeticOperator();
//                    case XzaarSyntaxKind.EqualityOperator: return WalkEqualityOperator();
//                    case XzaarSyntaxKind.LogicalConditionalOperator: return WalkLogicalConditionalOperator();
//                    case XzaarSyntaxKind.ConditionalOperator: return WalkConditionalOperator();
//                    case XzaarSyntaxKind.AssignmentOperator: return WalkAssignmentOperator();
//                    case XzaarSyntaxKind.Constant: return WalkConstantValue();
//                    case XzaarSyntaxKind.Literal:
//                        return Nodes.Current.Kind == XzaarSyntaxKind.LiteralNumber
//                            ? WalkNumberLiteral()
//                            : WalkStringLiteral();
//                    case XzaarSyntaxKind.Expression: return WalkExpression();
//                    case XzaarSyntaxKind.Scope: return WalkScope();
//                    case XzaarSyntaxKind.PostfixDecrement: return WalkPostDecrement();
//                    case XzaarSyntaxKind.PostfixIncrement: return WalkPostIncrement();
//                    case XzaarSyntaxKind.LogicalBitOperator: return WalkLogicalBitOperator();
//                    //case XzaarSyntaxKind.MemberAccess: return WalkMemberAccess();
//                    default:
//                        return Error("Unexpected node type '" + Nodes.Current.Type + "' found.");
//                }
//            }
//            catch (Exception exc)
//            {
//                throw exc;
//                // return Error(exc.Message);
//            }
//        }

//        private XzaarAstNode WalkExpressionStatement()
//        {
//            return WalkExpressionStatement(this.WalkExpressionCore());
//        }

//        private XzaarAstNode WalkExpressionStatement(XzaarAstNode expression)
//        {

//            if (this.Nodes.Current != null && this.Nodes.Current.Kind == XzaarSyntaxKind.Semicolon)
//            {
//                this.Nodes.Consume();
//            }

//            return expression;

//            //if (expression.NodeType == XzaarAstNodeTypes.EXPRESSION)
//            //    return expression;

//            //return XzaarAstNode.Expression(expression);


//            // return _syntaxFactory.ExpressionStatement(expression, semicolon);
//        }

//        private XzaarAstNode WalkKeyword()
//        {
//            var keyword = Nodes.Current.StringValue.ToLower();



//            switch (keyword)
//            {
//                case "fn": return WalkFunction();
//                case "let":
//                case "var": return WalkVariable();
//                case "if": return WalkIf();
//                case "else": return WalkElse();
//                case "while": return WalkWhileLoop();
//                case "loop": return WalkLoop();
//                case "do": return WalkDoWhileLoop();
//                case "for": return WalkForLoop();
//                case "foreach": return WalkForeachLoop();
//                case "switch": return WalkSwitch();
//                case "case": return WalkSwitchCase();
//                case "struct": return WalkStruct();
//                case "class": return WalkClass();
//                case "enum": return WalkEnum();
//                case "return": return WalkReturn();
//                case "break": return WalkBreak();
//                case "continue": return WalkContinue();
//                case "null":
//                case "true":
//                case "false":
//                    return WalkKnownConstant();
//                case "string":
//                case "boolean":
//                case "number":
//                case "date":
//                case "any":
//                    return WalkType();
//                default:
//                    return Error("Unexpected keyword '" + keyword + "' found.");
//            }
//        }

//        private XzaarAstNode WalkContinue()
//        {
//            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordContinue);
//            return XzaarAstNode.Continue();
//        }

//        private XzaarAstNode WalkBreak()
//        {
//            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordBreak);
//            return XzaarAstNode.Break();
//        }

//        private XzaarAstNode WalkKnownConstant()
//        {
//            var value = this.Nodes.Consume().StringValue;
//            return XzaarAstNode.Identifier(value);
//        }

//        private XzaarAstNode WalkType()
//        {
//            var isArray = false;
//            var current = Nodes.Consume();
//            if (Nodes.Current != null)
//            {
//                if (Nodes.Current.Kind == XzaarSyntaxKind.AggregateObjectIndex ||
//                    Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
//                {
//                    isArray = true;
//                    Nodes.Consume();
//                }
//            }
//            return XzaarAstNode.Identifier(current.StringValue);
//        }

//        private XzaarAstNode WalkReturn()
//        {
//            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordReturn);

//            var nextNode = Nodes.Current;
//            if (nextNode != null && nextNode.Kind != XzaarSyntaxKind.KeywordCase)
//            {
//                return XzaarAstNode.Return(Walk());
//            }
//            return XzaarAstNode.Return();
//        }

//        private XzaarAstNode WalkNumberLiteral()
//        {
//            var value = XzaarAstNode.NumberLiteral(Nodes.Consume().Value);
//            return WalkPostFixExpression(value);
//        }

//        private XzaarAstNode WalkStringLiteral()
//        {
//            var value = XzaarAstNode.StringLiteral(Nodes.Consume().StringValue);
//            return WalkPostFixExpression(value);
//        }

//        private XzaarAstNode WalkMemberAccess(XzaarAstNode leftSide)
//        {
//            var accessor = Nodes.Consume(n => n.Kind == XzaarSyntaxKind.MemberAccess);

//            leftSide = MemberAccess(leftSide);

//            var varOldIndex = Nodes.Index;

//            var memberWeWantToAccess = MemberAccess(Walk());

//            var indexAfter = Nodes.Index;

//            if (memberWeWantToAccess is FunctionCallNode)
//            {
//                (memberWeWantToAccess as FunctionCallNode).Instance = leftSide;

//            }

//            if (memberWeWantToAccess is BinaryOperatorNode)
//            {
//                var binOp = memberWeWantToAccess as BinaryOperatorNode;

//                var nextLeft = ExtractDeepLeft(binOp);

//                Nodes.SetPosition(varOldIndex + 1);

//                // var realMember = MemberAccess(binOp.Left);

//                // XzaarAstNode newChain;



//                //if (!ShiftChainAccess(leftSide, realMember, out newChain))
//                //{
//                //    var access = XzaarAstNode.MemberAccess(realMember, "any", "any");
//                //    newChain = XzaarAstNode.MemberAccessChain(leftSide, access);
//                //}


//                var memberAccessChain2 = XzaarAstNode.MemberAccessChain(leftSide, MemberAccess(nextLeft) as MemberAccessNode);
//                // this.Nodes.Consume();
//                return memberAccessChain2;


//                //Nodes.SetPosition(indexAfter);

//                //return XzaarAstNode.BinaryOperator(binOp.OperatingOrder, newChain, binOp.Op, binOp.Right);
//            }


//            if (memberWeWantToAccess is AssignNode)
//            {
//                // whopes! a bit to far.
//                var assign = memberWeWantToAccess as AssignNode;

//                Nodes.SetPosition(varOldIndex);

//                var consumeNext = Nodes.PeekNext().Kind == XzaarSyntaxKind.Assign;

//                var realMember = MemberAccess(assign.Left);

//                XzaarAstNode newChain;
//                if (!ShiftChainAccess(leftSide, realMember, out newChain))
//                {
//                    var access = XzaarAstNode.MemberAccess(realMember, "any", "any");
//                    newChain = XzaarAstNode.MemberAccessChain(leftSide, access);
//                }

//                if (consumeNext)
//                {
//                    var indexOf = Nodes.IndexOf(i => i.Kind == XzaarSyntaxKind.Assign);
//                    Nodes.SetPosition(indexOf + 2);
//                }

//                return XzaarAstNode.Assign(newChain, assign.Right);

//                //if (consumeNext)
//                //{
//                //    this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.Identifier);
//                //}
//                //else
//                //{
//                // var indexOf = Nodes.IndexOf(i => i.Kind == XzaarSyntaxKind.Assign);
//                // Nodes.SetPosition(indexOf);
//                // }
//                // return ChainExpression(newChain);
//            }

//            XzaarAstNode xzaarAstNode;
//            if (ShiftChainAccess(leftSide, memberWeWantToAccess, out xzaarAstNode))
//                return xzaarAstNode;


//            if (leftSide is MemberAccessNode)
//                return WawlkSimpleMemberAccess(leftSide as MemberAccessNode, memberWeWantToAccess);

//            if (leftSide is MemberAccessChainNode)
//                return WawlkChainMemberAccess(leftSide as MemberAccessChainNode, memberWeWantToAccess);

//            if (leftSide is FunctionCallNode)
//                return WawlkFunctionCallAccess(leftSide as FunctionCallNode, memberWeWantToAccess);


//            var memberAccess = XzaarAstNode.MemberAccess(memberWeWantToAccess, "any", "any");
//            var memberAccessChain = XzaarAstNode.MemberAccessChain(leftSide, memberAccess);
//            // this.Nodes.Consume();
//            return memberAccessChain;
//        }

//        private XzaarAstNode MemberAccess(XzaarAstNode assignLeft)
//        {
//            if (assignLeft is LiteralNode && assignLeft.NodeName == "NAME")
//            {
//                return XzaarAstNode.MemberAccess(assignLeft, "any", "any");
//            }
//            return assignLeft;
//        }

//        private bool ShiftChainAccess(XzaarAstNode leftSide, XzaarAstNode memberWeWantToAccess, out XzaarAstNode xzaarAstNode)
//        {
//            xzaarAstNode = null;
//            // the compiler expects us to have the last accessor first, so we have to move
//            // it around so we get it all in correct order
//            if (memberWeWantToAccess is MemberAccessChainNode)
//            {
//                // Expr: a.b.c
//                //    l ---------- r
//                //   /              \
//                //  (a) first      (b) next
//                //         \        /  | \
//                //         (a) First   |  (c)
//                //                     | /   \
//                //                    (b)     end

//                // we need to reverse the order to have the last (c) -> b, -> end into 
//                // our first occurence

//                var memAccess = memberWeWantToAccess as MemberAccessChainNode;
//                var r = memAccess.Accessor;
//                var l = memAccess.LastAccessor;

//                var leftMemAccess = leftSide as MemberAccessNode;
//                if (leftMemAccess != null)
//                {

//                    if (l is MemberAccessChainNode)
//                    {
//                        memAccess = (MemberAccessChainNode)WawlkChainMemberAccess(l as MemberAccessChainNode, r);
//                    }
//                    else
//                    {
//                        var leftChain = XzaarAstNode.MemberAccessChain(leftMemAccess, l as MemberAccessNode);

//                        leftSide = leftChain;
//                        memAccess = XzaarAstNode.MemberAccessChain(leftSide, r, leftChain.ResultType);
//                        //   this.Nodes.Consume();
//                    }


//                    {
//                        xzaarAstNode = memAccess;
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }

//        private XzaarAstNode WawlkSimpleMemberAccess(MemberAccessNode last, XzaarAstNode nextAccessorItem)
//        {
//            var memberType = FindMemberType(last.MemberType, nextAccessorItem);
//            var access = XzaarAstNode.MemberAccessChain(last,
//                XzaarAstNode.MemberAccess(nextAccessorItem, last.MemberType, memberType));
//            // this.Nodes.Consume();
//            return access;
//        }

//        private XzaarAstNode WawlkChainMemberAccess(MemberAccessChainNode last, XzaarAstNode nextAccessorItem)
//        {
//            var memberType = FindMemberType(last.ResultType, nextAccessorItem);
//            var access = XzaarAstNode.MemberAccessChain(last,
//                XzaarAstNode.MemberAccess(nextAccessorItem, last.ResultType, memberType));
//            // this.Nodes.Consume();
//            return access;
//        }

//        private XzaarAstNode WawlkFunctionCallAccess(FunctionCallNode last, XzaarAstNode nextAccessorItem)
//        {
//            var fnc = definedFunctions.FirstOrDefault(f => f.Key == last.Function.NodeName + "").Value;
//            var access = XzaarAstNode.MemberAccessChain(last,
//                XzaarAstNode.MemberAccess(nextAccessorItem, fnc.ReturnType.ToString(), fnc.ReturnType.ToString()));
//            // this.Nodes.Consume();
//            return access;
//        }

//        private string FindMemberType(
//            string lastMemberType,
//            XzaarAstNode nextAccessorItem)
//        {

//            if (lastMemberType == "string")
//            {
//                var targetField = nextAccessorItem.Value + "";
//                if (StringHelper.IsStringProperty(targetField))
//                {
//                    if (StringHelper.IsLengthProperty(targetField))
//                        return "number";
//                    return "any";
//                }
//                if (StringHelper.IsStringFunction(targetField))
//                {
//                    if (StringHelper.IsCharAt(targetField)) return "string";
//                    if (StringHelper.IsIndexOf(targetField)) return "number";
//                    return "any";
//                }
//            }

//            if (lastMemberType == "array" || (lastMemberType != null && lastMemberType.EndsWith("[]")))
//            {
//                var targetField = nextAccessorItem.Value + "";
//                if (ArrayHelper.IsArrayProperty(targetField))
//                {
//                    if (ArrayHelper.IsArrayLengthProperty(targetField))
//                        return "number";
//                    return "any";
//                }
//                if (ArrayHelper.IsArrayFunction(targetField))
//                {
//                    if (ArrayHelper.IsArrayAdd(targetField)) return "void";
//                    if (ArrayHelper.IsArrayRemove(targetField)) return "void";
//                    if (ArrayHelper.IsArrayRemoveLast(targetField)) return "void";
//                    if (ArrayHelper.IsArrayClear(targetField)) return "void";
//                    if (ArrayHelper.IsArrayInsert(targetField)) return "void";
//                    if (ArrayHelper.IsIndexOf(targetField)) return "number";
//                    return "any";
//                }
//            }

//            var memberType = "";
//            var existingStruct = definedStructs.Values.FirstOrDefault(s => s.Name == lastMemberType);
//            if (existingStruct != null)
//            {
//                var field =
//                    existingStruct.Fields.Cast<FieldNode>()
//                        .FirstOrDefault(f => f.Name == nextAccessorItem.Value + "");
//                if (field != null)
//                {
//                    memberType = field.Type;
//                }
//            }
//            return memberType;
//        }

//        private XzaarAstNode WalkLogicalBitOperator()
//        {
//            AssertMinExpectedNodeCount(1);
//            // a & | b
//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkPostIncrement()
//        {
//            AssertMinExpectedNodeCount(1);
//            // a++
//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkPostDecrement()
//        {
//            AssertMinExpectedNodeCount(1);
//            // a--            
//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkFunctionCall(XzaarAstNode functionName)
//        {
//            var args = this.WalkArgumentList();

//            var result = XzaarAstNode.Call(functionName, args);

//            var expr = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.Expression);

//            return result;

//            //var name = Walk(currentNode);
//            //var args = WalkArgumentNodes(Nodes.Next());
//            //return XzaarAstNode.Call(name, args);           
//        }

//        private XzaarAstNode WalkElementAccess(XzaarAstNode member)
//        {
//            // expr = _syntaxFactory.ElementAccessExpression(expr, this.ParseBracketedArgumentList());
//            var arrayArguments = WalkArgumentList();

//            XzaarAstNode arrayIndex = arrayArguments.Length > 1
//                ? (XzaarAstNode)XzaarAstNode.Expression(arrayArguments)
//                : arrayArguments.Length > 0 ? arrayArguments[0] : null;

//            Debug.Assert(arrayIndex != null);

//            XzaarAstNode item = XzaarAstNode.MemberAccess(member, arrayIndex);

//            return item;
//        }


//        private XzaarAstNode ChainExpression(XzaarAstNode item)
//        {
//            var nType = item.NodeType;

//            while (this.Nodes.Current != null)
//            {
//                var lastChild = this.Nodes.Current;
//                if (!CanBeChained(nType, this.Nodes.Current.Kind) && !CanBeChained(nType, this.Nodes.Current.Type)) break;
//                if (this.Nodes.Current.Type == XzaarSyntaxKind.UnaryOperator)
//                {
//                    // can only be postfix here
//                    var nextItem = WalkPostfixUnary(this.Nodes.Current.Kind, item);
//                    if (nextItem != null)
//                        item = nextItem;
//                    if (nextItem == null) break;
//                }
//                else if (this.Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
//                {
//                    var nextItem = WalkElementAccess(item);
//                    if (nextItem != null)
//                        item = nextItem;
//                    if (nextItem == null) break;
//                }
//                else if (this.Nodes.Current.Kind == XzaarSyntaxKind.MemberAccess)
//                {
//                    var nextItem = WalkMemberAccess(item);
//                    if (nextItem != null)
//                        item = nextItem;
//                    if (nextItem == null) break;
//                }
//                else if (XzaarSyntaxFacts.IsBinaryExpression(this.Nodes.Current.Kind))
//                {
//                    var nextItem = WalkSubExpression(Precedence.Expression);
//                    if (nextItem != null)
//                    {
//                        item = ReplaceDeepLeft(nextItem, item);
//                    }
//                    if (nextItem == null) break;
//                }
//                //else if (XzaarSyntaxFacts.IsAssignmentExpression(this.Nodes.Current.Kind))
//                //{
//                //    var nextItem = WalkAssignmentOperator();
//                //    if (nextItem != null)
//                //        item = nextItem;
//                //    if (nextItem == null) break;
//                //}
//                else
//                {
//                    break;
//                }
//            }
//            return item;
//        }

//        private bool CanBeChained(XzaarAstNodeTypes nType, XzaarSyntaxKind currentKind)
//        {
//            if (nType == XzaarAstNodeTypes.ACCESS && currentKind == XzaarSyntaxKind.MemberAccess) return true;
//            if (nType == XzaarAstNodeTypes.ACCESS && currentKind == XzaarSyntaxKind.ArrayIndexExpression) return true;
//            if (nType == XzaarAstNodeTypes.ACCESS && currentKind == XzaarSyntaxKind.ArithmeticOperator) return true;
//            return true;
//        }


//        private ArgumentNode[] WalkArgumentList()
//        {
//            var args = new List<ArgumentNode>();
//            if (PrepareScope())
//            {
//                int index = 0;
//                while (!this.Nodes.EndOfStream())
//                {
//                    var before = this.Nodes.Current;
//                    var astNode = WalkSubExpression(Precedence.Expression);
//                    if (astNode != null && astNode.NodeType != XzaarAstNodeTypes.SEPARATOR || Nodes.Current == null)
//                        args.Add(XzaarAstNode.Argument(astNode, index++));

//                    if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.Separator)
//                        Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Separator);

//                    //this.Nodes.Next(); 
//                    if (before == this.Nodes.Current) Nodes.Consume();
//                }
//                EndScope();
//            }
//            return args.ToArray();
//        }


//        private XzaarAstNode WalkParameterList()
//        {
//            var before1 = this.Nodes.Current;
//            var args = new List<ParameterNode>();
//            if (PrepareScope())
//            {

//                int index = 0;
//                while (!this.Nodes.EndOfStream())
//                {
//                    var before = this.Nodes.Current;
//                    ParameterNode p = null;
//                    var possibleIdentifier = this.Nodes.PeekAt(1);
//                    if (possibleIdentifier != null && possibleIdentifier.Kind == XzaarSyntaxKind.Colon)
//                    {
//                        // typescript/rust style
//                        var name = WalkIdentifier();
//                        var colon = Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Colon);
//                        var type = WalkType();
//                        p = XzaarAstNode.Parameter(name, type);
//                    }
//                    else
//                    {
//                        // javascript/java/c#/c/c++, list goes long - style
//                        var type = WalkType();
//                        var name = WalkIdentifier();
//                        p = XzaarAstNode.Parameter(name, type);
//                    }



//                    if (p != null && (Nodes.Current == null || Nodes.Current.Kind != XzaarSyntaxKind.Separator))
//                        args.Add(p);
//                    //this.Nodes.Next(); 
//                    if (before == this.Nodes.Current || (this.Nodes.Current != null && this.Nodes.Current.Type == XzaarSyntaxKind.Separator)) Nodes.Consume();
//                }
//                EndScope();

//            }
//            if (before1 == this.Nodes.Current) Nodes.Consume();
//            return XzaarAstNode.Parameters(args.ToArray());
//        }


//        private bool PrepareScope(IReadOnlyList<XzaarSyntaxNode> nodes = null)
//        {
//            if ((nodes != null && nodes.Count > 0) || (this.Nodes.Current != null && this.Nodes.Current.HasChildren))
//            {
//                BeginScope(new XzaarNodeStream(nodes ?? this.Nodes.Consume().Children));

//                return true;
//            }
//            return false;
//        }

//        private XzaarAstNode WalkScope()
//        {
//            var node = Nodes.Current;
//            XzaarAstNode scopeNode = null;
//            var nodes = new List<XzaarAstNode>();
//            if (node.HasChildren)
//            {
//                BeginScope(new XzaarNodeStream(node.Children));

//                while (!this.Nodes.EndOfStream())
//                {
//                    var astNode = Walk();
//                    if (astNode != null) nodes.Add(astNode);
//                    //this.Nodes.Next();
//                }
//                EndScope();
//            }

//            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Scope);

//            return XzaarAstNode.Block(nodes.ToArray());
//        }

//        //private ArgumentNode[] WalkArgumentNodes(XzaarSyntaxNode node)
//        //{
//        //    if (node == null || node.Children.Count == 0) return new ArgumentNode[0];

//        //    BeginScope(new XzaarNodeStream(node.Children));

//        //    while (!this.Nodes.EndOfStream())
//        //    {
//        //        var expr = WalkSubExpression(Precedence.Expression);
//        //    }

//        //    EndScope();

//        //    throw new System.NotImplementedException();
//        //}

//        //private ArgumentNode[] WalkArgumentNodes(XzaarAstNode args)
//        //{
//        //    if (args == null || args.Children.Count == 0) return new ArgumentNode[0];
//        //    var argList = new List<ArgumentNode>();
//        //    int index = 0;
//        //    foreach (var child in args.Children)
//        //    {
//        //        argList.Add(XzaarAstNode.Argument(child, index++));
//        //    }
//        //    return argList.ToArray();
//        //}

//        private XzaarAstNode WalkExpression()
//        {
//            XzaarAstNode expr = null;
//            var before = Nodes.Current;
//            if (Nodes.Current.HasChildren)
//            {
//                BeginScope(new XzaarNodeStream(Nodes.Current.Children));

//                expr = WalkExpressionCore();

//                var binOp = expr as BinaryOperatorNode;
//                if (binOp != null && binOp.Right == null)
//                {
//                    // the expression isnt complete. Lets complete it!                    
//                    if (Nodes.Current != null)
//                    {
//                        var rightOperator = WalkExpression();
//                        expr = ReplaceDeepRight(expr, rightOperator);
//                    }
//                }

//                EndScope();
//            }

//            if (Nodes.Current == before) Nodes.Consume();

//            // try and merge the expression

//            if (Nodes.Current != null && before.Kind == XzaarSyntaxKind.Expression)
//            {
//                XzaarAstNode rightOperator = null;
//                while (Nodes.Current != null)
//                {
//                    var tk = Nodes.Current.Kind;
//                    if (XzaarSyntaxFacts.IsExpectedBinaryOperator(tk) || XzaarSyntaxFacts.IsBinaryExpression(tk))
//                    {
//                        rightOperator = WalkSubExpression(Precedence.Expression);
//                    }
//                    else
//                    {
//                        break;
//                    }
//                    expr = ReplaceDeepLeft(rightOperator, expr);
//                }
//            }

//            if (expr != null)
//            {
//                return expr;
//            }

//            return XzaarAstNode.Expression();
//        }

//        private XzaarAstNode ReplaceDeepRight(XzaarAstNode op, XzaarAstNode expr)
//        {
//            var binOp = op as BinaryOperatorNode;
//            if (binOp == null) return expr;
//            while (true)
//            {
//                if (binOp == null)
//                    return expr;

//                if (binOp.Right == null)
//                {
//                    binOp.SetRight(expr);
//                    return op;
//                }

//                binOp = binOp.Left as BinaryOperatorNode;
//            }
//        }

//        private XzaarAstNode ExtractDeepLeft(XzaarAstNode node)
//        {
//            if (node is BinaryOperatorNode)
//            {
//                return ExtractDeepLeft(node as BinaryOperatorNode);
//            }

//            return node;
//        }

//        private XzaarAstNode ExtractDeepLeft(BinaryOperatorNode op)
//        {
//            var left = op.Left;

//            return ExtractDeepLeft(left);
//        }


//        private XzaarAstNode ReplaceDeepLeft(XzaarAstNode op, XzaarAstNode expr)
//        {
//            var binOp = op as BinaryOperatorNode;
//            if (binOp == null) return expr;
//            while (true)
//            {
//                if (binOp == null)
//                    return expr;

//                if (binOp.Left == null)
//                {
//                    binOp.SetLeft(expr);
//                    return op;
//                }

//                binOp = binOp.Left as BinaryOperatorNode;
//            }
//        }

//        private XzaarAstNode WalkConstantValue()
//        {
//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkConditionalOperator()
//        {
//            AssertMinExpectedNodeCount(1);
//            // get previous
//            // op
//            // get next
//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkAssignmentOperator()
//        {
//            var assign = Nodes.Consume(n => n.Type == XzaarSyntaxKind.AssignmentOperator);
//            // go back once if we are currently on the assignment
//            // if (XzaarSyntaxFacts.IsAssignmentExpression(Nodes.Current.Kind) || XzaarSyntaxFacts.IsExpectedAssignmentOperator(Nodes.Current.Kind)) Nodes.Previous();

//            if (Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
//            {
//                return WalkNewArrayInstance();
//            }

//            return WalkExpressionCore();
//        }

//        private XzaarAstNode WalkNewArrayInstance()
//        {
//            var old = Nodes.Current;
//            if (Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
//            {
//                // array initializer
//                var args = WalkArgumentList();

//                if (Nodes.Current == old) Nodes.Consume(i => i.Kind == XzaarSyntaxKind.ArrayIndexExpression);

//                // Nodes.Consume(i => i.Kind == XzaarSyntaxKind.ArrayIndexExpression);

//                return ChainExpression(XzaarAstNode.NewArrayInstance(args));
//            }
//            return Error("Unexpected type '" + Nodes.Current?.Kind + "' found. We expected a new array 'a = []' expression.");
//        }

//        private XzaarAstNode WalkLogicalConditionalOperator()
//        {
//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkEqualityOperator()
//        {
//            AssertMinExpectedNodeCount(1);
//            // get previous
//            // op
//            // get next

//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkArithmeticOperator()
//        {
//            AssertMinExpectedNodeCount(1);
//            // get previous
//            // op
//            // get next

//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkObjectIndex()
//        {
//            // if previous is assignment, it is an array initializer            
//            // otherwise a normal object index

//            var item = this.Nodes.Previous();

//            if (item == null && this.Nodes.Current != null && this.Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
//            {
//                var result = WalkNewArrayInstance();

//                return result;
//            }

//            var xzaarAstNode = Walk();

//            return WalkElementAccess(xzaarAstNode);
//        }

//        private XzaarAstNode WalkUnaryOperator()
//        {
//            AssertMinExpectedNodeCount(2);
//            // |++|+|--|-|!| (expr)

//            var unary = this.Nodes.Consume();

//            var expr = this.Walk();

//            if (expr.NodeType == XzaarAstNodeTypes.UNARY_OPERATOR)
//            {
//                // oh no! we need to split these up and go back one step
//                // we are about to do a unary operation on a unary operation.
//                // ex:
//                // --i ++i becomes --i++ i
//                // that is baadururur!
//                var unary2 = expr as UnaryNode;
//                expr = unary2.Item;
//                var prev = Nodes.Previous(); // go back one step, so we can forget even touching this unary
//                Debug.Assert(prev.Index == unary.Index + 2); // make sure we are recovering from the correct node.
//            }

//            // var type = XzaarSyntaxFacts.GetPrefixUnaryExpression(unary.Kind);

//            return XzaarAstNode.PrefixUnary(unary, expr);
//        }

//        private XzaarAstNode WalkEnum()
//        {
//            AssertMinExpectedNodeCount(3);
//            // enum name { body }

//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkClass()
//        {
//            AssertMinExpectedNodeCount(3);
//            // class name { body }

//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkStruct()
//        {
//            AssertMinExpectedNodeCount(3);
//            // struct name { body }

//            var @struct = Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordStruct);

//            var name = WalkIdentifier();

//            var fields = WalkStructFields(name.ValueText);

//            var str = XzaarAstNode.Struct(name.ValueText, fields);

//            this.definedStructs.Add(name.ValueText, str);

//            return str;
//        }

//        private XzaarAstNode[] WalkStructFields(string declaringTypeName)
//        {
//            var scope = Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Scope);
//            var fields = new List<XzaarAstNode>();
//            if (PrepareScope(scope.Children))
//            {
//                while (!Nodes.EndOfStream())
//                {
//                    var possibleIdentifier = Nodes.PeekNext();
//                    if (possibleIdentifier != null && possibleIdentifier.Kind == XzaarSyntaxKind.Colon)
//                    {
//                        // tsstyle/rust-style
//                        // name : type
//                        var name = WalkIdentifier();
//                        var colon = Nodes.Consume();
//                        var type = WalkType();
//                        fields.Add(XzaarAstNode.Field(type.ValueText, name.ValueText, declaringTypeName));
//                    }
//                    else
//                    {
//                        // c#/java/cc/c++/... style
//                        // type name                                                
//                        var type = WalkType();
//                        var name = WalkIdentifier();
//                        fields.Add(XzaarAstNode.Field(type.ValueText, name.ValueText, declaringTypeName));
//                    }
//                    if (Nodes.Current != null &&
//                        (Nodes.Current.Kind == XzaarSyntaxKind.Separator ||
//                         Nodes.Current.Kind == XzaarSyntaxKind.StatementTerminator))
//                    {
//                        Nodes.Consume();
//                    }
//                }

//                EndScope();
//            }

//            return fields.ToArray();
//        }

//        private XzaarAstNode WalkSwitchCase()
//        {
//            AssertMinExpectedNodeCount(4);
//            Debug.Assert(this.IsPossibleSwitchCase());

//            // case (expr) : { body }
//            // case (expr) : { body } break|continue|return


//            if (Nodes.Current.Kind == XzaarSyntaxKind.KeywordCase)
//            {
//                var @case = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordCase);
//                Debug.Assert(@case != null);
//                var test = WalkExpressionStatement();
//                var colon = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.Colon);
//                var switchCaseBody = WalkSwitchCaseBody();
//                Debug.Assert(switchCaseBody != null);

//                return XzaarAstNode.Case(test, switchCaseBody);
//            }
//            else
//            {
//                var defaultCase = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordDefault);
//                Debug.Assert(defaultCase != null);
//                var colon = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.Colon);
//                var switchCaseBody = WalkSwitchCaseBody();
//                Debug.Assert(switchCaseBody != null);

//                return XzaarAstNode.DefaultCase(switchCaseBody);
//            }



//            throw new System.NotImplementedException();
//        }

//        private XzaarAstNode WalkSwitchCaseBody()
//        {
//            if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.KeywordCase)
//                return Error("Multiple switch case labels are not supported at this time! Please make sure you add a 'break' or 'return' after declaring your switch case!");

//            if (Nodes.Current == null)
//                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!");


//            XzaarAstNode result = null;

//            var childStatements = new List<XzaarAstNode>();

//            if (Nodes.Current != null)
//            {
//                if (Nodes.Current.Kind == XzaarSyntaxKind.Scope)
//                {
//                    var scope = Nodes.Current;
//                    if (PrepareScope(scope.Children))
//                    {
//                        while (!Nodes.EndOfStream())
//                        {
//                            if (Nodes.Current.Kind == XzaarSyntaxKind.KeywordReturn ||
//                                Nodes.Current.Kind == XzaarSyntaxKind.KeywordBreak ||
//                                Nodes.Current.Kind == XzaarSyntaxKind.KeywordContinue)
//                            {
//                                childStatements.Add(Walk());
//                                break;
//                            }
//                            childStatements.Add(WalkExpressionStatement());
//                        }

//                        EndScope();
//                    }
//                }
//                else
//                {
//                    while (Nodes.Current != null)
//                    {
//                        if (Nodes.Current.Kind == XzaarSyntaxKind.KeywordReturn ||
//                            Nodes.Current.Kind == XzaarSyntaxKind.KeywordBreak ||
//                            Nodes.Current.Kind == XzaarSyntaxKind.KeywordContinue)
//                        {
//                            childStatements.Add(Walk());
//                            break;
//                        }
//                        childStatements.Add(WalkExpressionStatement());
//                    }
//                }

//                result = XzaarAstNode.Block(childStatements.ToArray());
//                // result
//            }

//            if (Nodes.Current != null && (Nodes.Current.Kind != XzaarSyntaxKind.KeywordReturn ||
//                Nodes.Current.Kind != XzaarSyntaxKind.KeywordBreak ||
//                Nodes.Current.Kind != XzaarSyntaxKind.KeywordContinue))
//            {
//                if (childStatements.Count > 0)
//                {
//                    var lastChildStatement = childStatements[childStatements.Count - 1];
//                    if (lastChildStatement.NodeType == XzaarAstNodeTypes.RETURN || lastChildStatement.NodeType == XzaarAstNodeTypes.BREAK || lastChildStatement.NodeType == XzaarAstNodeTypes.CONTINUE)
//                        return result;
//                }
//                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!");
//            }

//            if (Nodes.Current == null && childStatements.Count == 0)
//                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!");

//            var child = childStatements[childStatements.Count - 1];
//            if (child.NodeType != XzaarAstNodeTypes.RETURN && child.NodeType != XzaarAstNodeTypes.BREAK && child.NodeType != XzaarAstNodeTypes.CONTINUE)
//                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!");

//            return result;
//        }

//        private XzaarAstNode WalkSwitch()
//        {
//            AssertMinExpectedNodeCount(3);
//            // switch (expr) { body }

//            var @switch = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordSwitch);

//            var expr = this.WalkExpression();

//            var scope = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.Scope);

//            Debug.Assert(scope != null);

//            var switchCases = new List<CaseNode>();

//            if (PrepareScope(scope.Children))
//            {
//                while (this.IsPossibleSwitchCase())
//                {
//                    var swcase = this.WalkSwitchCase() as CaseNode;
//                    if (swcase != null)
//                        switchCases.Add(swcase);
//                }
//                EndScope();
//            }


//            return XzaarAstNode.Switch(expr, switchCases.ToArray());
//        }

//        private bool IsPossibleSwitchCase()
//        {
//            return this.Nodes.Current != null && ((this.Nodes.Current.Kind == XzaarSyntaxKind.KeywordCase) || (this.Nodes.Current.Kind == XzaarSyntaxKind.KeywordDefault));
//        }

//        private XzaarAstNode WalkForeachLoop()
//        {
//            AssertMinExpectedNodeCount(3);
//            // foreach (expr) { body }

//            var @foreach = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordForEach);

//            var exprList = this.WalkForEachStatementExpressionList();

//            Debug.Assert(exprList.Children.Count == 2);

//            var body = this.WalkStatement();

//            return XzaarAstNode.Foreach(exprList[0], exprList[1], body);
//        }

//        private XzaarAstNode WalkForLoop()
//        {
//            AssertMinExpectedNodeCount(3);
//            // for (expr; expr; expr) { body }
//            // while we only expect 3 items, we expect the second item to have 3 nodes
//            // this wont be tested until later

//            var @for = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordFor);

//            var exprList = this.WalkForStatementExpressionList();

//            Debug.Assert(exprList.Children.Count == 3);

//            var body = this.WalkStatement();


//            return XzaarAstNode.For(exprList[0], exprList[1], exprList[2], body);
//        }

//        private XzaarAstNode WalkForStatementExpressionList()
//        {
//            return WalkStatementExpressionList(XzaarSyntaxKind.StatementTerminator);
//        }

//        private XzaarAstNode WalkForEachStatementExpressionList()
//        {
//            return WalkStatementExpressionList(XzaarSyntaxKind.KeywordIn);
//        }


//        private XzaarAstNode WalkStatementExpressionList(XzaarSyntaxKind separator)
//        {
//            var forExprHolder = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.Expression);
//            if (!forExprHolder.HasChildren)
//                return Error("Well, this is awkward. Are you sure that you know how to declare a for statement?");

//            var expressions = SplitExpression(forExprHolder, separator);

//            var exprList = new List<XzaarAstNode>();

//            // 0: variable declaration/instantiation
//            // 1: conditional test
//            // 2: incrementor
//            for (var i = 0; i < expressions.Count; i++)
//            {
//                if (PrepareScope(expressions[i]))
//                {

//                    var before = this.Nodes.Current;
//                    var astNode = Walk();
//                    if (astNode != null) exprList.Add(astNode);
//                    if (before == this.Nodes.Current) Nodes.Consume();

//                    EndScope();
//                }
//            }

//            return XzaarAstNode.Expression(exprList.ToArray());
//        }

//        private List<List<XzaarSyntaxNode>> SplitExpression(XzaarSyntaxNode expr, XzaarSyntaxKind separator)
//        {
//            var expressions = new List<XzaarSyntaxNode>();
//            var finalExpressions = new List<List<XzaarSyntaxNode>>();
//            foreach (var item in expr.Children)
//            {
//                if (item.Kind == separator)
//                {
//                    finalExpressions.Add(new List<XzaarSyntaxNode>(expressions.ToArray()));
//                    expressions.Clear();
//                    continue;
//                }
//                expressions.Add(item);
//            }
//            if (expressions.Count > 0)
//            {
//                finalExpressions.Add(new List<XzaarSyntaxNode>(expressions.ToArray()));
//            }
//            return finalExpressions;
//        }

//        private XzaarAstNode WalkDoWhileLoop()
//        {
//            AssertMinExpectedNodeCount(4);
//            // do { body } while (expr)

//            XzaarAstNode result = null;

//            var @do = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordDo);

//            var body = this.WalkStatement();

//            var @while = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordWhile);

//            var condition = this.WalkExpression();

//            result = XzaarAstNode.DoWhile(condition, body);

//            Nodes.Consume();

//            return result;
//        }

//        private XzaarAstNode WalkWhileLoop()
//        {
//            AssertMinExpectedNodeCount(3);
//            // while (expr) { body }

//            XzaarAstNode result = null;

//            var @while = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordWhile);

//            var condition = this.WalkExpression();

//            var body = this.WalkStatement();

//            result = XzaarAstNode.While(condition, body);

//            Nodes.Consume();

//            return result;
//        }


//        private XzaarAstNode WalkLoop()
//        {
//            AssertMinExpectedNodeCount(2);
//            // loop { body }

//            var loop = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordLoop);
//            var body = this.WalkStatement();
//            return XzaarAstNode.Loop(body);
//        }


//        private XzaarAstNode WalkIf()
//        {
//            AssertMinExpectedNodeCount(3);
//            // if (expr) { body }         
//            XzaarAstNode result = null;

//            var @if = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordIf);

//            var condition = this.WalkExpression(); // this.ParseExpressionCore();

//            var ifTrue = this.WalkStatement();

//            var ifFalse = WalkElseClause();

//            if (ifFalse != null)
//            {
//                result = XzaarAstNode.IfElseThen(condition, ifTrue, ifFalse);
//                Nodes.Consume();
//            }
//            else
//            {

//                result = XzaarAstNode.IfThen(condition, ifTrue);
//                Nodes.Consume();

//            }

//            return result;
//        }

//        private XzaarAstNode WalkElseClause()
//        {
//            if (this.Nodes.Current == null || this.Nodes.Current.Kind != XzaarSyntaxKind.KeywordElse)
//            {
//                return null;
//            }

//            return WalkElse();
//        }

//        private XzaarAstNode WalkElse()
//        {
//            AssertMinExpectedNodeCount(2);
//            // else <statement: { body }>
//            // else <statement: if (expr) { body }>

//            var @else = this.Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordElse);

//            var statement = this.WalkStatement();

//            return statement;
//        }

//        private XzaarAstNode WalkStatement()
//        {
//            return Walk();
//        }


//        private XzaarAstNode WalkVariable()
//        {
//            AssertMinExpectedNodeCount(2);
//            // let|var name
//            // let|var name = expr
//            // let|var name : type = expr

//            var varTypeExplicit = false;
//            var varType = "any";
//            var letVar = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordVar);
//            var name = WalkIdentifier();

//            if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.Colon)
//            {
//                var colon = Nodes.Consume();
//                varTypeExplicit = true;
//                varType = WalkType().ValueText;
//            }

//            var isAssign = Nodes.Current != null && XzaarSyntaxFacts.IsAssignmentExpression(Nodes.Current.Type);
//            if (isAssign)
//            {
//                var assign = Nodes.Consume();
//                var before = Nodes.Current;
//                var valueAssignment = WalkAssignmentOperator();
//                if (before == Nodes.Current) Nodes.Consume();

//                if (valueAssignment is ArrayNode && !varTypeExplicit)
//                    varType = "array";

//                if (valueAssignment is CreateStructNode)
//                    varType = (valueAssignment as CreateStructNode).StructNode.Name;

//                return XzaarAstNode.DefineVariable(varType, name.ValueText, valueAssignment);
//            }
//            Nodes.Consume();
//            return XzaarAstNode.DefineVariable(varType, name.ValueText, null);
//        }

//        private XzaarAstNode WalkFunction()
//        {
//            AssertMinExpectedNodeCount(4);
//            // fn name (expr) { body }
//            // fn name (expr) -> type { body }

//            var fn = Nodes.Consume(i => i.Kind == XzaarSyntaxKind.KeywordFn);
//            var name = WalkIdentifier();
//            var parameterList = WalkParameterList();
//            var parameters = XzaarAstNode.Parameters(parameterList);

//            var functionName = name.ValueText;
//            if (Nodes.Current.Type == XzaarSyntaxKind.PointerMemberAccess || Nodes.Current.Type == XzaarSyntaxKind.Colon)
//            {
//                AssertMinExpectedNodeCount(3);
//                Nodes.Consume();

//                var function = XzaarAstNode.Function(functionName, WalkType().ValueText, parameters);
//                definedFunctions.Add(functionName, function);

//                var body = Walk();
//                Nodes.Consume(k => k.Kind == XzaarSyntaxKind.Scope);

//                function.SetBody(body);
//                return function;
//            }
//            else
//            {
//                var function = XzaarAstNode.Function(functionName, parameters);
//                definedFunctions.Add(functionName, function);

//                var body = Walk();
//                Nodes.Consume(k => k.Kind == XzaarSyntaxKind.Scope);

//                function.SetBody(body);
//                return function;
//            }
//        }

//        private XzaarAstNode WalkIdentifier()
//        {
//            XzaarAstNode item = null;
//            if (Nodes.Current.Kind != XzaarSyntaxKind.Identifier
//                && Nodes.PeekNext().Kind == XzaarSyntaxKind.Identifier)
//                Nodes.Consume();

//            var stringValue = Nodes.Consume().StringValue;


//            if (definedStructs.ContainsKey(stringValue))
//            {
//                return XzaarAstNode.CreateStruct(definedStructs[stringValue]);
//            }


//            item = XzaarAstNode.Identifier(stringValue);

//            return item;
//        }



//        private XzaarAstNode WalkExpressionCore()
//        {
//            XzaarAstNode subExpr = null;
//            var before = this.Nodes.Current;
//            var isExpression = before.Type == XzaarSyntaxKind.Expression;
//            if (isExpression)
//            {
//                subExpr = this.WalkExpression();
//            }
//            else
//            {
//                subExpr = this.WalkSubExpression(Precedence.Expression);
//            }

//            if (before == this.Nodes.Current)
//                this.Nodes.Consume();

//            //if (isExpression && subExpr.NodeType != XzaarAstNodeTypes.EXPRESSION)
//            //{
//            //    return XzaarAstNode.Expression(subExpr);
//            //}

//            return subExpr;
//        }


//        private XzaarAstNode WalkSubExpression(Precedence precedence)
//        {
//            return ParseSubExpressionCore(precedence);
//        }

//        private XzaarAstNode WalkTerm(Precedence precedence)
//        {
//            XzaarAstNode expr = null;

//            var tk = this.Nodes.Current.Kind;
//            switch (tk)
//            {
//                case XzaarSyntaxKind.Identifier:
//                    expr = WalkIdentifier();
//                    break;

//                // case XzaarSyntaxKind.ArgListKeyword:
//                case XzaarSyntaxKind.KeywordFalse:
//                case XzaarSyntaxKind.KeywordTrue:
//                case XzaarSyntaxKind.KeywordNull:
//                    expr = WalkKnownConstant();
//                    break;
//                case XzaarSyntaxKind.LiteralNumber:
//                case XzaarSyntaxKind.LiteralString:
//                case XzaarSyntaxKind.Literal:
//                    expr = Walk();
//                    break;
//                //case XzaarSyntaxKind.KeywordNew:
//                //    expr = this.ParseNewExpression();
//                //    break;
//                default:
//                    // check for intrinsic type followed by '.'
//                    if (XzaarSyntaxFacts.IsPredefinedType(tk))
//                    {
//                        expr = Walk();
//                    }
//                    break;
//            }

//            return this.WalkPostFixExpression(expr);
//        }

//        private XzaarAstNode WalkPostFixExpression(XzaarAstNode expr)
//        {
//            // Debug.Assert(expr != null);
//            if (expr == null && this.Nodes.Current != null)
//            {
//                if (this.Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
//                {
//                    return WalkNewArrayInstance();
//                    // array initializer, no need for expr to have a value.
//                }
//            }
//            if (expr == null) return null;

//            while (true)
//            {
//                if (this.Nodes.Current == null) return expr;
//                XzaarSyntaxKind tk = this.Nodes.Current.Kind;
//                switch (tk)
//                {
//                    case XzaarSyntaxKind.Expression:
//                        return WalkFunctionCall(expr);

//                    case XzaarSyntaxKind.AggregateObjectIndex:
//                    case XzaarSyntaxKind.ArrayIndexExpression:
//                        return WalkElementAccess(expr);

//                    case XzaarSyntaxKind.UnaryIncrement:
//                    case XzaarSyntaxKind.UnaryDecrement:
//                    case XzaarSyntaxKind.PostfixDecrement:
//                    case XzaarSyntaxKind.PostfixIncrement:
//                    case XzaarSyntaxKind.PlusPlus:
//                    case XzaarSyntaxKind.MinusMinus:

//                        var type = XzaarSyntaxFacts.GetPostfixUnaryExpression(tk);
//                        return WalkPostfixUnary(type, expr);
//                        throw new Exception("postfix unary");
//                        // expr = _syntaxFactory.PostfixUnaryExpression(SyntaxFacts.GetPostfixUnaryExpression(tk), expr, this.EatToken());
//                        break;

//                    case XzaarSyntaxKind.MemberAccess:
//                    case XzaarSyntaxKind.Dot:
//                        if (this.Nodes.PeekNext().Kind == XzaarSyntaxKind.Identifier)
//                        {
//                            var dotToken = this.Nodes.Consume();
//                            //ccToken = this.AddError(ccToken, ErrorCode.ERR_UnexpectedAliasedName);
//                            //var dotToken = this.ConvertToMissingWithTrailingTrivia(ccToken, SyntaxKind.DotToken);                            
//                            return WalkMemberAccess(expr);
//                        }
//                        break;
//                    case XzaarSyntaxKind.PointerMemberAccess:
//                    case XzaarSyntaxKind.MinusGreater:
//                        // expr = _syntaxFactory.MemberAccessExpression(XzaarSyntaxKind.PointerMemberAccess, expr, this.Nodes.Next(), Walk(this.Nodes.Next()));
//                        throw new Exception("pointer member access");
//                        break;
//                        //case XzaarSyntaxKind.Dot:
//                        //    // if we have the error situation:
//                        //    //
//                        //    //      expr.
//                        //    //      X Y
//                        //    //
//                        //    // Then we don't want to parse this out as "Expr.X"
//                        //    //
//                        //    // It's far more likely the member access expression is simply incomplete and
//                        //    // there is a new declaration on the next line.
//                        //    if (this.CurrentToken.TrailingTrivia.Any((int)SyntaxKind.EndOfLineTrivia) &&
//                        //        this.PeekToken(1).Kind == SyntaxKind.IdentifierToken &&
//                        //        this.PeekToken(2).Kind == SyntaxKind.IdentifierToken)
//                        //    {
//                        //        expr = _syntaxFactory.MemberAccessExpression(
//                        //            SyntaxKind.SimpleMemberAccessExpression, expr, this.EatToken(),
//                        //            this.AddError(this.CreateMissingIdentifierName(), ErrorCode.ERR_IdentifierExpected));

//                        //        return expr;
//                        //    }

//                        //    expr = _syntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expr, this.EatToken(), this.ParseSimpleName(NameOptions.InExpression));
//                        //    break;

//                        //case XzaarSyntaxKind.Question:
//                        //    if (CanStartConsequenceExpression(this.PeekToken(1).Kind))
//                        //    {
//                        //        var qToken = this.EatToken();
//                        //        var consequence = ParseConsequenceSyntax();
//                        //        expr = _syntaxFactory.ConditionalAccessExpression(expr, qToken, consequence);
//                        //        expr = CheckFeatureAvailability(expr, MessageID.IDS_FeatureNullPropagatingOperator);
//                        //        break;
//                        //    }

//                        goto default;
//                    default:
//                        return expr;
//                }
//            }
//        }

//        private XzaarAstNode WalkPostfixUnary(XzaarSyntaxKind type, XzaarAstNode item)
//        {
//            var token = this.Nodes.Consume();
//            return XzaarAstNode.PostfixUnary(token, item);
//        }


//        private XzaarAstNode ParseSubExpressionCore(Precedence precedence)
//        {
//            XzaarAstNode leftOperand = null;
//            Precedence newPrecedence = 0;
//            XzaarSyntaxKind opKind = XzaarSyntaxKind.None;

//            // all of these are tokens that start statements and are invalid
//            // to start a expression with. if we see one, then we must have
//            // something like:
//            //
//            // return
//            // if (...
//            // parse out a missing name node for the expression, and keep on going
//            var tk = this.Nodes.Current.Kind;
//            if (XzaarSyntaxFacts.IsInvalidSubExpression(tk))
//            {
//                return this.Error(tk + " is not a valid sub expression");
//            }

//            // No left operand, so we need to parse one -- possibly preceded by a
//            // unary operator.
//            if (XzaarSyntaxFacts.IsExpectedPrefixUnaryOperator(tk))
//            {
//                opKind = XzaarSyntaxFacts.GetPrefixUnaryExpression(tk);
//                newPrecedence = XzaarSyntaxFacts.GetPrecedence(opKind);
//                var opToken = this.Nodes.Consume();
//                var operand = this.WalkSubExpression(newPrecedence);
//                //if (XzaarSyntaxFacts.IsIncrementOrDecrementOperator(opToken.Kind))
//                //{
//                //    operand = CheckValidLvalue(operand);
//                //}

//                leftOperand =
//                    XzaarAstNode.PrefixUnary(opToken, operand);
//                // XzaarAstNode.Incrementor(operand);
//                // _syntaxFactory.PrefixUnaryExpression(opKind, opToken, operand);
//            }
//            else
//            {
//                // Not a unary operator - get a primary expression.
//                leftOperand = this.WalkTerm(precedence);
//            }

//            while (true)
//            {
//                // We either have a binary or assignment operator here, or we're finished.
//                if (this.Nodes.Current == null) // check for end of expression
//                    break;

//                tk = this.Nodes.Current.Kind;

//                bool isAssignmentOperator = false;
//                if (XzaarSyntaxFacts.IsExpectedBinaryOperator(tk) || XzaarSyntaxFacts.IsBinaryExpression(tk))
//                {
//                    if (XzaarSyntaxFacts.IsExpectedBinaryOperator(tk))
//                        opKind = XzaarSyntaxFacts.GetBinaryExpression(tk);
//                    else
//                        opKind = tk;
//                }
//                else if (XzaarSyntaxFacts.IsExpectedAssignmentOperator(tk) || XzaarSyntaxFacts.IsAssignmentExpression(tk))
//                {
//                    if (XzaarSyntaxFacts.IsExpectedAssignmentOperator(tk))
//                        opKind = XzaarSyntaxFacts.GetAssignmentExpression(tk);
//                    else
//                        opKind = tk;
//                    isAssignmentOperator = true;
//                }
//                else
//                {
//                    if (Nodes.Current.Type == XzaarSyntaxKind.StatementTerminator)
//                        Nodes.Consume();
//                    break;
//                }

//                newPrecedence = XzaarSyntaxFacts.GetPrecedence(opKind);

//                Debug.Assert(newPrecedence > 0);      // All binary operators must have precedence > 0!

//                // check for >> or >>=
//                bool doubleOp = false;
//                //if (tk == XzaarSyntaxKind.GreaterEquals
//                //    && (this.Nodes.PeekNext().Kind == XzaarSyntaxKind.Greater || this.Nodes.PeekNext().Kind == XzaarSyntaxKind.GreaterEquals))
//                //{
//                //    // check to see if they really are adjacent
//                //    if (this.Nodes.Current.GetTrailingTriviaWidth() == 0 && this.Nodes.PeekNext().GetLeadingTriviaWidth() == 0)
//                //    {
//                //        if (this.Nodes.PeekNext().Kind == XzaarSyntaxKind.Greater)
//                //        {
//                //            opKind = XzaarSyntaxFacts.GetBinaryExpression(XzaarSyntaxKind.GreaterGreater);
//                //        }
//                //        else
//                //        {
//                //            opKind = XzaarSyntaxFacts.GetAssignmentExpression(XzaarSyntaxKind.GreaterGreaterEquals);
//                //            isAssignmentOperator = true;
//                //        }
//                //        newPrecedence = XzaarSyntaxFacts.GetPrecedence(opKind);
//                //        doubleOp = true;
//                //    }
//                //}

//                // Check the precedence to see if we should "take" this operator
//                if (newPrecedence < precedence)
//                {
//                    break;
//                }

//                // Same precedence, but not right-associative -- deal with this "later"
//                if ((newPrecedence == precedence) && !XzaarSyntaxFacts.IsRightAssociative(opKind))
//                {
//                    break;
//                }

//                // Precedence is okay, so we'll "take" this operator.
//                var opToken = Nodes.Consume(); // this.EatContextualToken(tk);
//                //if (doubleOp)
//                //{
//                //    // combine tokens into a single token
//                //    var opToken2 = this.EatToken();
//                //    var kind = opToken2.Kind == XzaarSyntaxKind.GreaterThanToken ? XzaarSyntaxKind.GreaterThanGreaterThanToken : XzaarSyntaxKind.GreaterThanGreaterThanEqualsToken;
//                //    opToken = SyntaxFactory.Token(opToken.GetLeadingTrivia(), kind, opToken2.GetTrailingTrivia());
//                //}


//                if (isAssignmentOperator)
//                {
//                    // leftOperand = CheckValidLvalue(leftOperand);
//                    var rightOperand = this.WalkSubExpression(newPrecedence);
//                    switch (opKind)
//                    {
//                        case XzaarSyntaxKind.Assign:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, rightOperand);
//                            break;
//                        case XzaarSyntaxKind.AssignAnd:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '&', rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignOr:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '|', rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignMultiply:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '*', rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignDivide:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '/', rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignMinus:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '-', rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignPlus:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '+', rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignLeftShift:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, "<<", rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignRightShift:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, ">>", rightOperand));
//                            break;
//                        case XzaarSyntaxKind.AssignModulo:
//                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '%', rightOperand));
//                            break;
//                    }

//                    //  _syntaxFactory.AssignmentExpression(opKind, leftOperand, opToken,);
//                }
//                else
//                {
//                    leftOperand = XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, opToken.StringValue, this.WalkSubExpression(newPrecedence));
//                }
//            }


//            // From the language spec:
//            //
//            // conditional-expression:
//            //  null-coalescing-expression
//            //  null-coalescing-expression   ?   expression   :   expression
//            //
//            // Only take the ternary if we're at a precedence less than the null coalescing
//            // expression.

//            //if (tk == XzaarSyntaxKind.Question && precedence <= Precedence.Ternary)
//            //{
//            //    var questionToken = this.Nodes.Next();
//            //    var colonLeft = this.ParseExpressionCore();
//            //    var colon = this.Nodes.Next(); // XzaarSyntaxKind.Colon
//            //    var colonRight = this.ParseExpressionCore();
//            //    leftOperand = XzaarAstNode.ConditionalExpression(leftOperand, questionToken, colonLeft, colon, colonRight);
//            //}

//            return leftOperand;
//        }


//        private bool IsPossibleStatement()
//        {
//            var tk = this.Nodes.Current.Kind;
//            switch (tk)
//            {
//                case XzaarSyntaxKind.KeywordBreak:
//                case XzaarSyntaxKind.KeywordContinue:
//                case XzaarSyntaxKind.KeywordDo:
//                case XzaarSyntaxKind.KeywordFor:
//                case XzaarSyntaxKind.KeywordForEach:
//                case XzaarSyntaxKind.KeywordGoto:
//                case XzaarSyntaxKind.KeywordLoop:
//                case XzaarSyntaxKind.KeywordIf:
//                case XzaarSyntaxKind.KeywordReturn:
//                case XzaarSyntaxKind.KeywordSwitch:
//                case XzaarSyntaxKind.KeywordWhile:
//                case XzaarSyntaxKind.Scope:
//                case XzaarSyntaxKind.Semicolon:
//                    return true;

//                case XzaarSyntaxKind.Identifier:
//                    return true;

//                // Accessibility modifiers are not legal in a statement,
//                // but a common mistake for local functions. Parse to give a
//                // better error message.

//                default:
//                    return XzaarSyntaxFacts.IsPredefinedType(tk)
//                           || IsPossibleExpression();
//            }
//        }

//        private bool IsPossibleExpression()
//        {
//            var tk = this.Nodes.Current.Kind;
//            switch (tk)
//            {

//                case XzaarSyntaxKind.ArgList:
//                case XzaarSyntaxKind.Constant:

//                case XzaarSyntaxKind.Expression:
//                case XzaarSyntaxKind.LiteralNumber:
//                case XzaarSyntaxKind.LiteralString:
//                case XzaarSyntaxKind.Literal:
//                case XzaarSyntaxKind.KeywordNew:
//                case XzaarSyntaxKind.ColonColon: // bad aliased name                
//                    return true;
//                case XzaarSyntaxKind.Identifier:
//                    // Specifically allow the from contextual keyword, because it can always be the start of an
//                    // expression (whether it is used as an identifier or a keyword).
//                    return true;
//                default:
//                    return XzaarSyntaxFacts.IsExpectedPrefixUnaryOperator(tk)
//                           || (XzaarSyntaxFacts.IsPredefinedType(tk) && tk != XzaarSyntaxKind.KeywordVoid)
//                           || XzaarSyntaxFacts.IsAnyUnaryExpression(tk)
//                           || XzaarSyntaxFacts.IsBinaryExpression(tk)
//                           || XzaarSyntaxFacts.IsAssignmentExpressionOperatorToken(tk);
//            }
//        }

//        private void AssertMinExpectedNodeCount(int count)
//        {
//            if (count > Nodes.Available)
//                throw new XzaarTransformerException("Oh no! Unexpected end of script. We expected "
//                                                    + count + " more nodes, but there are only " + Nodes.Available + " left.");
//        }


//        private XzaarAstNode Error(string message)
//        {
//            this.errors.Add("[Error] " + message);
//            throw new Exception(this.errors.Last());
//            return null;
//        }

//        private XzaarNodeStream Nodes
//        {
//            get { return this.currentScope.Nodes; }
//        }

//        private void BeginScope(XzaarNodeStream scopeNodes)
//        {
//            this.currentScope = currentScope.BeginScope(scopeNodes);
//        }

//        private void EndScope()
//        {
//            this.currentScope = currentScope.EndScope();
//        }
//    }
//}