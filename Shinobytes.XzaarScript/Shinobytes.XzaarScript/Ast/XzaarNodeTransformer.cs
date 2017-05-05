using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Ast.Nodes;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarNodeTransformer : INodeTransformer
    {
        private readonly XzaarSyntaxNode ast;
        private TransformerScope currentScope;
        private readonly List<string> errors = new List<string>();
        private readonly List<ErrorNode> errorNodes = new List<ErrorNode>();

        private readonly List<LabelNode> labels = new List<LabelNode>();
        private readonly List<ParameterNode> currentParameters = new List<ParameterNode>();
        private readonly Dictionary<string, FunctionNode> definedFunctions = new Dictionary<string, FunctionNode>();
        private readonly Dictionary<string, StructNode> definedStructs = new Dictionary<string, StructNode>();
        private XzaarAstNode lastWalkedExpression;

        public XzaarNodeTransformer(XzaarSyntaxNode ast)
        {
            this.ast = ast;
            this.currentScope = new TransformerScope();
        }

        public XzaarAstNode Transform()
        {
            return new EntryNode(WalkAllNodes());
        }

        public bool HasErrors { get { return errors.Count > 0; } }

        public IList<string> Errors { get { return errors; } }

        private XzaarAstNode WalkAllNodes()
        {
            var nodes = new List<XzaarAstNode>();
            this.BeginScope(new XzaarNodeStream(ast.Children));

            // var currentNode = this.Nodes.Current;
            while (!this.EndOfStream())
            {
                try
                {
                    var astNode = Walk();
                    if (astNode != null)
                        nodes.Add(astNode);
                }
                catch (Exception exc)
                {
                    return Error(exc.Message, Nodes.Current);
                }
                // currentNode = this.Nodes.Next();
            }

            this.EndScope(XzaarSyntaxKind.Scope);
            return XzaarAstNode.Block(nodes.ToArray());
        }

        private XzaarAstNode Walk()
        {

            // NOTE: A node must always be returned unless error
            //       if the next nodes require the previous node
            //       then it has to parse the same node once more
            //       but when it has been used by another node, 
            //       the previous node should be replaced with the new node
            //       This is so we can avoid using a stack and never
            //       care to know whether our resulting node can be chained 
            //       with the next. It saves us alot of headache and code maintenance

            Nodes.Consume(n => n.Type == XzaarSyntaxKind.StatementTerminator);

            if (Nodes.EndOfStream()) return null;

            switch (Nodes.Current.Type)
            {
                case XzaarSyntaxKind.Identifier:
                    {
                        if (Nodes.PeekNext() != null && Nodes.PeekNext().Kind == XzaarSyntaxKind.Colon)
                            return lastWalkedExpression = WalkLabel();
                        return lastWalkedExpression = WalkExpressionStatement(); // return WalkIdentifier();
                    }
                case XzaarSyntaxKind.Keyword: return lastWalkedExpression = WalkKeyword();
                case XzaarSyntaxKind.UnaryOperator: return lastWalkedExpression = WalkUnaryOperator();
                case XzaarSyntaxKind.AggregateObjectIndex:
                //case XzaarSyntaxKind.ArrayIndexExpression:
                //    return WalkObjectIndex();
                case XzaarSyntaxKind.ArithmeticOperator: return lastWalkedExpression = WalkArithmeticOperator();
                case XzaarSyntaxKind.EqualityOperator: return lastWalkedExpression = WalkEqualityOperator();
                case XzaarSyntaxKind.LogicalConditionalOperator: return lastWalkedExpression = WalkLogicalConditionalOperator();
                case XzaarSyntaxKind.ConditionalOperator: return lastWalkedExpression = WalkConditionalOperator();
                case XzaarSyntaxKind.AssignmentOperator: return lastWalkedExpression = WalkAssignmentOperator();
                case XzaarSyntaxKind.Constant: return lastWalkedExpression = WalkConstantValue();
                case XzaarSyntaxKind.Literal:
                    return lastWalkedExpression = Nodes.Current.Kind == XzaarSyntaxKind.LiteralNumber
                        ? WalkNumberLiteral()
                        : WalkStringLiteral();
                // case XzaarSyntaxKind.Expression: return WalkExpression();
                case XzaarSyntaxKind.Scope:
                    return lastWalkedExpression = WalkScope();
                //case XzaarSyntaxKind.MemberAccess: return WalkMemberAccess();
                default:
                    {
                        return lastWalkedExpression = Error("Unexpected node type '" + Nodes.Current.Type + "' found.", Nodes.Current);
                    }
            }
        }

        private XzaarAstNode WalkLabel()
        {
            var labelName = WalkIdentifier();
            var colon = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Colon);

            var label = XzaarAstNode.Label(labelName.ValueText);

            this.labels.Add(label);

            return label;
        }

        private XzaarAstNode WalkGoto()
        {
            if (Nodes.PeekNext().Kind == XzaarSyntaxKind.KeywordCase)
            {
                return Error("Goto case has not been implemented yet.", Nodes.Current);
            }
            var before = Nodes.Current;
            var target = WalkIdentifier();
            var label = labels.FirstOrDefault(l => l.Name == target.ValueText);
            if (label == null)
                return Error("No labels with the name '" + target.ValueText + "' could be found.", before);

            return XzaarAstNode.Goto(target.ValueText);
        }

        private XzaarAstNode WalkExpressionStatement()
        {
            return WalkExpressionStatement(this.WalkExpressionCore());
        }

        private XzaarAstNode WalkExpressionStatement(XzaarAstNode expression)
        {

            if (this.Nodes.Current != null && this.Nodes.Current.Kind == XzaarSyntaxKind.Semicolon)
            {
                this.Nodes.Consume();
            }

            return expression;

            //if (expression.NodeType == XzaarAstNodeTypes.EXPRESSION)
            //    return expression;

            //return XzaarAstNode.Expression(expression);


            // return _syntaxFactory.ExpressionStatement(expression, semicolon);
        }

        private XzaarAstNode WalkKeyword()
        {
            var keyword = Nodes.Current.StringValue.ToLower();



            switch (keyword)
            {
                case "fn": return WalkFunction();
                case "let":
                case "var": return WalkVariable();
                case "if": return WalkIf();
                case "else": return WalkElse();
                case "while": return WalkWhileLoop();
                case "loop": return WalkLoop();
                case "do": return WalkDoWhileLoop();
                case "for": return WalkForLoop();
                case "foreach": return WalkForeachLoop();
                case "switch": return WalkSwitch();
                case "case": return WalkSwitchCase();
                case "struct": return WalkStruct();
                case "class": return WalkClass();
                case "enum": return WalkEnum();
                case "return": return WalkReturn();
                case "break": return WalkBreak();
                case "continue": return WalkContinue();
                case "goto": return WalkGoto();
                case "null":
                case "true":
                case "false":
                    return WalkKnownConstant();
                case "string":
                case "boolean":
                case "bool":
                case "number":
                case "date":
                case "any":
                    return WalkType();
                default:
                    return Error("Unexpected keyword '" + keyword + "' found.", Nodes.Current);
            }
        }

        private XzaarAstNode WalkContinue()
        {
            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordContinue);
            return XzaarAstNode.Continue();
        }

        private XzaarAstNode WalkBreak()
        {
            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordBreak);
            return XzaarAstNode.Break();
        }

        private XzaarAstNode WalkKnownConstant()
        {
            var value = this.Nodes.Consume().StringValue;
            return XzaarAstNode.Identifier(value);
        }

        private XzaarAstNode WalkType()
        {
            var isArray = false;
            var current = Nodes.Consume();
            if (Nodes.Current != null)
            {
                if (Nodes.Current.Kind == XzaarSyntaxKind.AggregateObjectIndex ||
                    Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
                {
                    isArray = true;
                    Nodes.Consume();
                }
            }
            return XzaarAstNode.Identifier(current.StringValue + (isArray ? "[]" : ""));
        }

        private XzaarAstNode WalkReturn()
        {
            Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordReturn);

            var nextNode = Nodes.Current;
            if (nextNode != null && nextNode.Kind != XzaarSyntaxKind.KeywordCase)
            {
                var result = WalkSubExpression(Precedence.Expression);
                return XzaarAstNode.Return(result);
            }
            return XzaarAstNode.Return();
        }

        private XzaarAstNode WalkNumberLiteral()
        {
            var value = XzaarAstNode.NumberLiteral(Nodes.Consume(x => x.Kind == XzaarSyntaxKind.LiteralNumber).Value);

            return WalkPostFixExpression(value);
        }

        private XzaarAstNode WalkStringLiteral()
        {
            var value = XzaarAstNode.StringLiteral(Nodes.Consume(x => x.Kind == XzaarSyntaxKind.LiteralString).StringValue);
            return WalkPostFixExpression(value);
        }

        private string FindMemberType(
            string lastMemberName,
            string lastMemberType,
            XzaarAstNode nextAccessorItem)
        {
            if (lastMemberName != null)
            {
                var variable = this.currentScope.FindVariable(lastMemberName);
                if (variable != null)
                {
                    lastMemberType = variable.Type;
                }
                else
                {
                    var parameter = this.FindParameter(lastMemberName);
                    if (parameter != null)
                    {
                        lastMemberType = parameter.Type;
                    }
                }
            }
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

            if (lastMemberType == "array" || (lastMemberType != null && lastMemberType.EndsWith("[]")))
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
            var existingStruct = definedStructs.Values.FirstOrDefault(s => s.Name == lastMemberType);
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
            if (string.IsNullOrEmpty(memberType) && lastMemberType == "any")
                return lastMemberType;
            return memberType;
        }

        private bool CanBeChained(XzaarAstNodeTypes nType, XzaarSyntaxKind currentKind)
        {
            if (nType == XzaarAstNodeTypes.ACCESS && currentKind == XzaarSyntaxKind.MemberAccess) return true;
            if (nType == XzaarAstNodeTypes.ACCESS && currentKind == XzaarSyntaxKind.ArrayIndexExpression) return true;
            if (nType == XzaarAstNodeTypes.ACCESS && currentKind == XzaarSyntaxKind.ArithmeticOperator) return true;
            return true;
        }

        private ArgumentNode[] WalkArrayArgumentList()
        {
            return WalkArgumentList(XzaarSyntaxKind.ArrayIndexExpression);
        }


        private ArgumentNode[] WalkArgumentList()
        {
            return WalkArgumentList(XzaarSyntaxKind.Expression);
        }

        private ArgumentNode[] WalkArgumentList(XzaarSyntaxKind kind)
        {
            var args = new List<ArgumentNode>();
            if (PrepareScope())
            {
                int index = 0;
                while (!this.EndOfStream())
                {
                    if (!IsPossibleExpression() && !IsPossibleStatement())
                    {
                        var errNode = "";
                        if (Nodes.Current != null) errNode = Nodes.Current.StringValue;
                        Error("'" + errNode + "' is not a valid argument.", Nodes.Current);
                        return null;
                    }
                    var astNode = WalkSubExpression(Precedence.Expression);
                    if (astNode != null && astNode.NodeType != XzaarAstNodeTypes.SEPARATOR || Nodes.Current == null)
                        args.Add(XzaarAstNode.Argument(astNode, index++));

                    if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.Separator)
                        Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Separator);
                }
                EndScope(kind);
            }
            return args.ToArray();
        }


        private XzaarAstNode WalkParameterList()
        {
            var args = new List<ParameterNode>();
            if (PrepareScope())
            {

                int index = 0;
                while (!this.EndOfStream())
                {
                    var before = this.Nodes.Current;

                    var possibleIdentifier = this.Nodes.PeekAt(1);
                    if (possibleIdentifier != null && possibleIdentifier.Kind == XzaarSyntaxKind.Colon)
                    {
                        // typescript/rust style
                        var name = WalkIdentifier();
                        var colon = Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Colon);
                        var type = WalkType();
                        args.Add(XzaarAstNode.Parameter(name, type));
                    }
                    else
                    {
                        // javascript/java/c#/c/c++, list goes long - style
                        var type = WalkType();
                        var name = WalkIdentifier();
                        args.Add(XzaarAstNode.Parameter(name, type));
                    }

                    //this.Nodes.Next(); 
                    if (before == this.Nodes.Current || (this.Nodes.Current != null && this.Nodes.Current.Type == XzaarSyntaxKind.Separator)) Nodes.Consume();
                }
                EndScope(XzaarSyntaxKind.Expression);
            }
            return XzaarAstNode.Parameters(args.ToArray());
        }


        private bool PrepareScope(IList<XzaarSyntaxNode> nodes = null)
        {
            if ((nodes != null) || (this.Nodes.Current != null))
            {
                BeginScope(new XzaarNodeStream(nodes ?? this.Nodes.Current.Children));

                return true;
            }
            return false;
        }

        private XzaarAstNode WalkScope()
        {
            var node = Nodes.Current;
            XzaarAstNode scopeNode = null;
            var nodes = new List<XzaarAstNode>();
            if (node.HasChildren)
            {
                BeginScope(new XzaarNodeStream(node.Children));

                while (!this.EndOfStream())
                {
                    var astNode = Walk();
                    if (astNode != null) nodes.Add(astNode);
                    //this.Nodes.Next();
                }
                EndScope(XzaarSyntaxKind.Scope);
            }


            return XzaarAstNode.Block(nodes.ToArray());
        }

        private XzaarAstNode WalkExpression()
        {
            var node = Nodes.Current;
            XzaarAstNode scopeNode = null;
            var nodes = new List<XzaarAstNode>();

            if (PrepareScope())
            {
                while (!this.EndOfStream())
                {
                    var astNode = WalkExpressionCore();
                    if (astNode != null) nodes.Add(astNode);
                    //this.Nodes.Next();
                }
                EndScope(XzaarSyntaxKind.Expression);
            }

            if (nodes.Count > 1)
            {
                // merge expressions since we should only return one
                return MergeExpressions(nodes);
            }

            if (nodes.Count == 0)
                return Error("Empty expressions '()' are not allowed.", node);

            Debug.Assert(nodes.Count == 1);

            return nodes[0];
        }

        private XzaarAstNode MergeExpressions(List<XzaarAstNode> nodes)
        {
            // if its only one node, return that one.
            if (nodes.Count == 1) return nodes[0];

            var expressions = new List<XzaarAstNode>();
            // we will merge left with next one, we expect to have an even amount of nodes
            if (nodes.Count % 2 == 0)
            {
                // (a && b) (&& c)
                // (a ||) (b && c)

                for (var i = 0; i < nodes.Count; i += 2)
                {
                    var left = nodes[i];
                    var right = nodes[i + 1];
                    var exprLeft = MergeExpressionsLeft(left, right);
                    if (exprLeft != null)
                        expressions.Add(exprLeft);
                    else
                    {
                        var exprRight = MergeExpressionsRight(left, right);
                        if (exprRight != null)
                            expressions.Add(exprRight);
                    }
                }
            }
            else if (nodes.Count % 3 == 0)
            {
                // (a && b) (||) (c)

                for (var i = 0; i < nodes.Count; i += 3)
                {
                    var left = nodes[i];
                    var mid = nodes[i + 1];
                    var right = nodes[i + 2];

                    var newLeft = MergeExpressionsRight(left, mid);
                    if (newLeft != null)
                    {
                        var expr = MergeExpressionsLeft(newLeft, right);
                        if (expr != null)
                            expressions.Add(expr);
                    }
                }

            }
            else
            {
                throw new NotImplementedException();
            }

            return MergeExpressions(expressions);
        }

        private XzaarAstNode MergeExpressionsRight(XzaarAstNode left, XzaarAstNode right)
        {
            var binOpLeft = left as BinaryOperatorNode;
            var binOpRight = right as BinaryOperatorNode;
            if (binOpLeft != null && binOpRight != null)
            {
                // binOpLeft:  a || b
                // binOpRight  &&
                if (binOpLeft.Left != null && binOpLeft.Right != null && binOpRight.Left == null && binOpRight.Right == null)
                {
                    binOpRight.SetLeft(binOpLeft);
                    return binOpRight;
                }

                // binOpLeft: a || true
                // binOpRight: (&& a) || (b > 0)
                if (binOpLeft.Left != null && binOpLeft.Right != null && binOpRight.Left != null &&
                    binOpRight.Right != null)
                {
                    var rightLeft = binOpRight.Left as BinaryOperatorNode;
                    if (rightLeft != null)
                    {
                        if (rightLeft.Left == null)
                        {
                            rightLeft.SetLeft(left);
                            return binOpRight;
                        }
                        else if (rightLeft.Right == null)
                        {
                            // binOpRight: (a &&) || (b > 0)) <<woot??
                            throw new NotImplementedException();
                        }
                    }

                }
            }
            return null;
        }

        private XzaarAstNode MergeExpressionsLeft(XzaarAstNode left, XzaarAstNode right)
        {
            var binOpLeft = left as BinaryOperatorNode;
            var binOpRight = right as BinaryOperatorNode;
            if (binOpLeft != null && binOpRight != null)
            {
                // binOpLeft:  a ||
                // binOpRight  b && c
                if (binOpLeft.Right == null && binOpRight.Left != null && binOpRight.Right != null)
                {
                    binOpLeft.SetRight(binOpRight);
                    return binOpLeft;
                }

                // binOpLeft:  a || b
                // binOpRight  && c
                if (binOpRight.Left == null && binOpLeft.Left != null && binOpLeft.Right != null)
                {
                    binOpRight.SetLeft(binOpLeft);
                    return binOpRight;
                }
            }
            return null;
        }

        private XzaarAstNode ReplaceDeepLeft(XzaarAstNode op, XzaarAstNode expr)
        {
            var binOp = op as BinaryOperatorNode;
            if (binOp == null) return expr;
            while (true)
            {
                if (binOp == null)
                    return expr;

                if (binOp.Left == null)
                {
                    binOp.SetLeft(expr);
                    return op;
                }

                binOp = binOp.Left as BinaryOperatorNode;
            }
        }

        private XzaarAstNode WalkConstantValue()
        {
            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkConditionalOperator()
        {
            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next
            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkAssignmentOperator()
        {
            var assign = Nodes.Consume(n => n.Type == XzaarSyntaxKind.AssignmentOperator);
            // go back once if we are currently on the assignment
            // if (XzaarSyntaxFacts.IsAssignmentExpression(Nodes.Current.Kind) || XzaarSyntaxFacts.IsExpectedAssignmentOperator(Nodes.Current.Kind)) Nodes.Previous();

            return WalkExpressionCore();
        }

        private XzaarAstNode WalkNewArrayInstance()
        {
            var old = Nodes.Current;
            if (Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
            {
                // array initializer
                var args = WalkArgumentList();

                if (Nodes.Current == old) Nodes.Consume(x => x.Kind == XzaarSyntaxKind.ArrayIndexExpression);

                // Nodes.Consume(i => i.Kind == XzaarSyntaxKind.ArrayIndexExpression);

                return XzaarAstNode.NewArrayInstance(args);
            }
            return Error("Unexpected type '" + Nodes.Current?.Kind + "' found. We expected a new array 'a = []' expression.", Nodes.Current);
        }

        private XzaarAstNode WalkLogicalConditionalOperator()
        {
            // I was hoping that we never needed this one.
            // but it cannot be helped... 

            var leftSide = lastWalkedExpression;

            if (leftSide == null)
                return Error("We were unable to build the logical expression. Perhaps it is a bit too complex? Try and simplify it and avoid using paranthesis if possible!", Nodes.Current);

            var rightSide = WalkSubExpression(Precedence.Expression);

            var assignNode = leftSide as AssignNode;
            var variableDefinitionNode = leftSide as DefineVariableNode;
            var binopNode = leftSide as BinaryOperatorNode;

            XzaarAstNode valueNode = null;

            if (assignNode != null)
            {
                valueNode = assignNode.Right;
            }

            if (variableDefinitionNode != null)
            {
                valueNode = variableDefinitionNode.AssignmentExpression;
            }

            if (binopNode != null)
            {
                valueNode = binopNode;
            }

            if (valueNode != null)
            {
                rightSide = ReplaceDeepLeft(rightSide, valueNode);

                if (variableDefinitionNode != null)
                {
                    variableDefinitionNode.SetValue(rightSide);
                }
                else if (assignNode != null)
                {
                    assignNode.SetAssignmentValue(rightSide);
                }
                else
                {
                    binopNode.SetRight(rightSide);
                }
                return null;
            }

            // 

            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkEqualityOperator()
        {
            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next

            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkArithmeticOperator()
        {
            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next

            return WalkExpression();

            throw new System.NotImplementedException();
        }

        //private XzaarAstNode WalkObjectIndex()
        //{
        //    // if previous is assignment, it is an array initializer            
        //    // otherwise a normal object index

        //    var item = this.Nodes.Previous();

        //    if (item == null && this.Nodes.Current != null && this.Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
        //    {
        //        var result = WalkNewArrayInstance();

        //        return result;
        //    }

        //    var xzaarAstNode = Walk();

        //    return WalkElementAccess(xzaarAstNode);
        //}

        private XzaarAstNode WalkUnaryOperator()
        {
            AssertMinExpectedNodeCount(2);
            // |++|+|--|-|!| (expr)

            var unary = this.Nodes.Consume();

            var expr = this.Walk();

            if (expr.NodeType == XzaarAstNodeTypes.UNARY_OPERATOR)
            {
                // oh no! we need to split these up and go back one step
                // we are about to do a unary operation on a unary operation.
                // ex:
                // --i ++i becomes --i++ i
                // that is baadururur!
                var unary2 = (UnaryNode)expr;
                expr = unary2.Item;
                var prev = Nodes.Previous(); // go back one step, so we can forget even touching this unary
                Debug.Assert(prev.Index == unary.Index + 2); // make sure we are recovering from the correct node.
            }

            // var type = XzaarSyntaxFacts.GetPrefixUnaryExpression(unary.Kind);

            return XzaarAstNode.PrefixUnary(unary, expr);
        }

        private XzaarAstNode WalkEnum()
        {
            AssertMinExpectedNodeCount(3);
            // enum name { body }

            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkClass()
        {
            AssertMinExpectedNodeCount(3);
            // class name { body }

            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkStruct()
        {
            AssertMinExpectedNodeCount(3);
            // struct name { body }

            var @struct = Nodes.Consume(n => n.Kind == XzaarSyntaxKind.KeywordStruct);

            var name = WalkIdentifier();

            var fields = WalkStructFields(name.ValueText);

            var str = XzaarAstNode.Struct(name.ValueText, fields);

            this.definedStructs.Add(name.ValueText, str);

            return str;
        }

        private XzaarAstNode[] WalkStructFields(string declaringTypeName)
        {
            var fields = new List<XzaarAstNode>();
            if (PrepareScope(Nodes.Current.Children))
            {
                while (!EndOfStream())
                {
                    var possibleIdentifier = Nodes.PeekNext();
                    if (possibleIdentifier != null && possibleIdentifier.Kind == XzaarSyntaxKind.Colon)
                    {
                        // tsstyle/rust-style
                        // name : type
                        var name = WalkIdentifier();
                        var colon = Nodes.Consume();
                        var type = WalkType();
                        fields.Add(XzaarAstNode.Field(type.ValueText, name.ValueText, declaringTypeName));
                    }
                    else
                    {
                        // c#/java/cc/c++/... style
                        // type name                                                
                        var type = WalkType();
                        var name = WalkIdentifier();
                        fields.Add(XzaarAstNode.Field(type.ValueText, name.ValueText, declaringTypeName));
                    }
                    if (Nodes.Current != null &&
                        (Nodes.Current.Kind == XzaarSyntaxKind.Separator ||
                         Nodes.Current.Kind == XzaarSyntaxKind.StatementTerminator))
                    {
                        Nodes.Consume();
                    }
                }

                EndScope(XzaarSyntaxKind.Scope);
            }

            return fields.ToArray();
        }

        private XzaarAstNode[] WalkStructInitializer()
        {
            // similar to WalkStructFields, but here we expect to assign the values of the target struct and separate the expressions by a comma
            var fieldAssignments = new List<XzaarAstNode>();
            if (PrepareScope(Nodes.Current.Children))
            {
                while (!EndOfStream())
                {
                    var assignment = Walk();
                    if (assignment.NodeType != XzaarAstNodeTypes.ASSIGN)
                    {
                        return new[] { Error("You're suppose to assign the values here", Nodes.Current) };
                    }
                    fieldAssignments.Add(assignment);
                    Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Separator);
                }

                EndScope(XzaarSyntaxKind.Scope);
            }

            return fieldAssignments.ToArray();
        }

        private XzaarAstNode WalkSwitchCase()
        {
            Debug.Assert(this.IsPossibleSwitchCase());

            // case (expr) : { body }
            // case (expr) : { body } break|continue|return


            if (Nodes.Current.Kind == XzaarSyntaxKind.KeywordCase)
            {
                AssertMinExpectedNodeCount(4);

                var @case = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordCase);
                Debug.Assert(@case != null);
                var test = WalkExpressionStatement();
                var colon = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Colon);
                var switchCaseBody = WalkSwitchCaseBody();
                Debug.Assert(switchCaseBody != null);

                return XzaarAstNode.Case(test, switchCaseBody);
            }
            else
            {
                AssertMinExpectedNodeCount(3);

                var defaultCase = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordDefault);
                Debug.Assert(defaultCase != null);
                var colon = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Colon);
                var switchCaseBody = WalkSwitchCaseBody();
                Debug.Assert(switchCaseBody != null);

                return XzaarAstNode.DefaultCase(switchCaseBody);
            }



            throw new System.NotImplementedException();
        }

        private XzaarAstNode WalkSwitchCaseBody()
        {
            if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.KeywordCase)
                return Error("Multiple switch case labels are not supported at this time! Please make sure you add a 'break' or 'return' after declaring your switch case!", Nodes.Current);

            if (Nodes.Current == null)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);


            XzaarAstNode result = null;

            var childStatements = new List<XzaarAstNode>();

            if (Nodes.Current != null)
            {
                if (Nodes.Current.Kind == XzaarSyntaxKind.Scope)
                {
                    var scope = Nodes.Current;
                    if (PrepareScope(scope.Children))
                    {
                        while (!EndOfStream())
                        {
                            if (Nodes.Current.Kind == XzaarSyntaxKind.KeywordReturn ||
                                Nodes.Current.Kind == XzaarSyntaxKind.KeywordBreak ||
                                Nodes.Current.Kind == XzaarSyntaxKind.KeywordContinue)
                            {
                                childStatements.Add(Walk());
                                break;
                            }
                            childStatements.Add(WalkExpressionStatement());
                        }

                        EndScope(XzaarSyntaxKind.Scope);
                    }
                }
                else
                {
                    while (Nodes.Current != null)
                    {
                        if (Nodes.Current.Kind == XzaarSyntaxKind.KeywordReturn ||
                            Nodes.Current.Kind == XzaarSyntaxKind.KeywordBreak ||
                            Nodes.Current.Kind == XzaarSyntaxKind.KeywordContinue)
                        {
                            childStatements.Add(Walk());
                            break;
                        }
                        childStatements.Add(WalkExpressionStatement());
                    }
                }

                if (Nodes.Current != null && (Nodes.Current.Kind == XzaarSyntaxKind.KeywordReturn ||
                    Nodes.Current.Kind == XzaarSyntaxKind.KeywordBreak ||
                    Nodes.Current.Kind == XzaarSyntaxKind.KeywordContinue))
                {
                    childStatements.Add(Walk());
                }

                result = XzaarAstNode.Block(childStatements.ToArray());
                // result
            }

            if (Nodes.Current != null && (Nodes.Current.Kind != XzaarSyntaxKind.KeywordReturn &&
                Nodes.Current.Kind != XzaarSyntaxKind.KeywordBreak &&
                Nodes.Current.Kind != XzaarSyntaxKind.KeywordContinue))
            {
                if (childStatements.Count > 0)
                {
                    var lastChildStatement = childStatements[childStatements.Count - 1];
                    if (lastChildStatement.NodeType == XzaarAstNodeTypes.RETURN || lastChildStatement.NodeType == XzaarAstNodeTypes.BREAK || lastChildStatement.NodeType == XzaarAstNodeTypes.CONTINUE)
                        return result;
                }
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);
            }

            if (Nodes.Current == null && childStatements.Count == 0)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);

            var child = childStatements[childStatements.Count - 1];
            if (child.NodeType != XzaarAstNodeTypes.RETURN && child.NodeType != XzaarAstNodeTypes.BREAK && child.NodeType != XzaarAstNodeTypes.CONTINUE)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);

            return result;
        }

        private XzaarAstNode WalkSwitch()
        {
            AssertMinExpectedNodeCount(3);
            // switch (expr) { body }

            var @switch = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordSwitch);

            var expr = this.WalkExpression();

            var scope = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Scope);

            Debug.Assert(scope != null);

            var switchCases = new List<CaseNode>();

            if (PrepareScope(scope.Children))
            {
                while (this.IsPossibleSwitchCase())
                {
                    var swcase = this.WalkSwitchCase() as CaseNode;
                    if (swcase != null)
                        switchCases.Add(swcase);
                }
                EndScope(XzaarSyntaxKind.Scope);
            }


            return XzaarAstNode.Switch(expr, switchCases.ToArray());
        }

        private bool IsPossibleSwitchCase()
        {
            return this.Nodes.Current != null && ((this.Nodes.Current.Kind == XzaarSyntaxKind.KeywordCase) || (this.Nodes.Current.Kind == XzaarSyntaxKind.KeywordDefault));
        }

        private XzaarAstNode WalkForeachLoop()
        {
            AssertMinExpectedNodeCount(3);
            // foreach (expr) { body }

            var @foreach = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordForEach);

            var exprList = this.WalkForEachStatementExpressionList();

            Debug.Assert(exprList.Children.Count == 2);

            var body = this.WalkStatementOrBody();

            return XzaarAstNode.Foreach(exprList[0], exprList[1], body);
        }

        private XzaarAstNode WalkForLoop()
        {
            AssertMinExpectedNodeCount(3);
            // for (expr; expr; expr) { body }
            // while we only expect 3 items, we expect the second item to have 3 nodes
            // this wont be tested until later

            var @for = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordFor);

            var exprList = this.WalkForStatementExpressionList();

            Debug.Assert(exprList.Children.Count == 3);

            var body = this.WalkStatementOrBody();


            return XzaarAstNode.For(exprList[0], exprList[1], exprList[2], body);
        }

        private XzaarAstNode WalkForStatementExpressionList()
        {
            return WalkStatementExpressionList(XzaarSyntaxKind.StatementTerminator);
        }

        private XzaarAstNode WalkForEachStatementExpressionList()
        {
            return WalkStatementExpressionList(XzaarSyntaxKind.KeywordIn);
        }


        private XzaarAstNode WalkStatementExpressionList(XzaarSyntaxKind separator)
        {
            var forExprHolder = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Expression);
            if (!forExprHolder.HasChildren)
                return Error("Well, this is awkward. Are you sure that you know how to declare a for statement?", Nodes.Current);

            var expressions = SplitExpression(forExprHolder, separator);

            var exprList = new List<XzaarAstNode>();

            // 0: variable declaration/instantiation
            // 1: conditional test
            // 2: incrementor
            for (var i = 0; i < expressions.Count; i++)
            {
                if (PrepareScope(expressions[i]))
                {

                    var before = this.Nodes.Current;
                    var astNode = Walk();
                    if (astNode != null) exprList.Add(astNode);
                    if (before == this.Nodes.Current) Nodes.Consume();

                    EndScope(XzaarSyntaxKind.Expression);
                }
            }

            return XzaarAstNode.Expression(exprList.ToArray());
        }

        private List<List<XzaarSyntaxNode>> SplitExpression(XzaarSyntaxNode expr, XzaarSyntaxKind separator)
        {
            var expressions = new List<XzaarSyntaxNode>();
            var finalExpressions = new List<List<XzaarSyntaxNode>>();
            foreach (var item in expr.Children)
            {
                if (item.Kind == separator)
                {
                    finalExpressions.Add(new List<XzaarSyntaxNode>(expressions.ToArray()));
                    expressions.Clear();
                    continue;
                }
                expressions.Add(item);
            }
            if (expressions.Count > 0)
            {
                finalExpressions.Add(new List<XzaarSyntaxNode>(expressions.ToArray()));
            }
            return finalExpressions;
        }

        private XzaarAstNode WalkDoWhileLoop()
        {
            AssertMinExpectedNodeCount(4);
            // do { body } while (expr)

            XzaarAstNode result = null;

            var @do = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordDo);

            var body = this.WalkStatementOrBody();

            var @while = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordWhile);

            var condition = this.WalkExpression();

            result = XzaarAstNode.DoWhile(condition, body);

            Nodes.Consume();

            return result;
        }

        private XzaarAstNode WalkWhileLoop()
        {
            AssertMinExpectedNodeCount(3);
            // while (expr) { body }

            XzaarAstNode result = null;

            var @while = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordWhile);

            var condition = this.WalkExpression();

            var body = this.WalkStatementOrBody();

            result = XzaarAstNode.While(condition, body);

            Nodes.Consume();

            return result;
        }


        private XzaarAstNode WalkLoop()
        {
            AssertMinExpectedNodeCount(2);
            // loop { body }

            var loop = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordLoop);
            var body = this.WalkStatementOrBody();
            return XzaarAstNode.Loop(body);
        }


        private XzaarAstNode WalkIf()
        {
            AssertMinExpectedNodeCount(3);

            // if (expr) { body }         
            XzaarAstNode result = null;

            var @if = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordIf);

            var condition = this.WalkExpression(); // this.ParseExpressionCore();

            var ifTrue = this.WalkStatementOrBody();

            var ifFalse = WalkElseClause();

            if (ifFalse != null)
            {
                result = XzaarAstNode.IfElseThen(condition, ifTrue, ifFalse);
            }
            else
            {
                result = XzaarAstNode.IfThen(condition, ifTrue);
            }

            return result;
        }

        private XzaarAstNode WalkElseClause()
        {
            if (this.Nodes.Current == null || this.Nodes.Current.Kind != XzaarSyntaxKind.KeywordElse)
            {
                return null;
            }

            return WalkElse();
        }

        private XzaarAstNode WalkElse()
        {
            AssertMinExpectedNodeCount(2);
            // else <statement: { body }>
            // else <statement: if (expr) { body }>

            var @else = this.Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordElse);

            var statement = this.WalkStatementOrBody();

            return statement;
        }

        private XzaarAstNode WalkStatement()
        {
            return Walk();
        }


        private XzaarAstNode WalkVariable()
        {
            AssertMinExpectedNodeCount(2);
            // let|var name
            // let|var name = expr
            // let|var name : type = expr

            var varTypeExplicit = false;
            var varType = "any";
            var letVar = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordVar);
            var name = WalkIdentifier();

            if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.Colon)
            {
                var colon = Nodes.Consume();
                varTypeExplicit = true;
                varType = WalkType().ValueText;
            }

            var isAssign = Nodes.Current != null && XzaarSyntaxFacts.IsAssignmentExpression(Nodes.Current.Type);
            if (isAssign)
            {
                var assign = Nodes.Consume();
                var before = Nodes.Current;
                var valueAssignment = WalkAssignmentOperator();
                if (before == Nodes.Current) Nodes.Consume();

                if (valueAssignment == null && HasErrors)
                    return errorNodes.Last();

                if ((valueAssignment is ArrayNode || valueAssignment.NodeName == "ARRAY") && !varTypeExplicit)
                    varType = "array";

                if (valueAssignment is CreateStructNode)
                    varType = (valueAssignment as CreateStructNode).StructNode.Name;

                if (!varTypeExplicit)
                {
                    varType = GetExpressionResultType(valueAssignment);
                }

                // variables can NEVER be of type void
                if (varType == "void") varType = "any";

                return AddVariable(XzaarAstNode.DefineVariable(varType, name.ValueText, valueAssignment));
            }
            // Nodes.Consume();
            return AddVariable(XzaarAstNode.DefineVariable(varType, name.ValueText, null));
        }

        private string GetExpressionResultType(XzaarAstNode expr)
        {
            var call = expr as FunctionCallNode;
            if (call != null)
            {
                if (!definedFunctions.ContainsKey(call.Function.ValueText))
                {
                    return "any";
                }
                var func = definedFunctions[call.Function.ValueText];
                return func.ReturnType?.Name ?? "any";
            }

            var createStruct = expr as CreateStructNode;
            if (createStruct != null)
            {
                return createStruct.StructNode.Name;
            }

            var literal = expr as LiteralNode;
            if (literal != null)
            {
                if (literal.NodeName == "NUMBER") return "number";
                if (literal.NodeName == "STRING") return "string";
                if (literal.NodeName == "ARRAY") return "array";
                if (literal.NodeName == "NAME")
                {
                    if (literal.ValueText == "false" || literal.ValueText == "true") return XzaarBaseTypes.Boolean.Name;
                    var p = FindParameter(literal.ValueText);
                    if (p != null) return p.Type;
                    var v = currentScope.FindVariable(literal.ValueText);
                    if (v != null) return v.Type;
                    if (this.definedStructs.ContainsKey(literal.ValueText)) return literal.ValueText;
                }
            }

            var field = expr as FieldNode;
            if (field != null) return field.Type;

            var variableDef = expr as DefineVariableNode;
            if (variableDef != null) return variableDef.Type;

            var variable = expr as VariableNode;
            if (variable != null) return variable.Type;

            var unary = expr as UnaryNode;
            if (unary != null)
            {
                var op = unary.Operator;
                if (op == "++" || op == "+" || op == "-" || op == "--") return XzaarBaseTypes.Number.Name;
                if (op == "!") return XzaarBaseTypes.Boolean.Name;
            }

            var binOp = expr as BinaryOperatorNode;
            if (binOp != null)
            {
                var op = binOp.Op;

                if (op == "+")
                {
                    if (binOp.Left?.Type == "string" || binOp.Right?.Type == "string")
                        return XzaarBaseTypes.String.Name;
                    string leftCallType = null, rightCallType = null;
                    if (binOp.Left is FunctionCallNode fc1) leftCallType = GetExpressionResultType(fc1);
                    if (binOp.Right is FunctionCallNode fc2) rightCallType = GetExpressionResultType(fc2);
                    if (leftCallType == "any" || rightCallType == "any") return XzaarBaseTypes.Any.Name;
                    if (leftCallType == "string" || rightCallType == "string") return XzaarBaseTypes.String.Name;
                }

                if (op == "++" || op == "+" || op == "-" || op == "--" || op == "*" || op == "%" || op == "/")
                    return XzaarBaseTypes.Number.Name;
                if (op == "||" || op == "&&" || op == ">" || op == ">=" || op == "<" || op == "<=" || op == "==" || op == "!=")
                    return XzaarBaseTypes.Boolean.Name;
            }

            return "any";
        }

        private DefineVariableNode AddVariable(DefineVariableNode defineVariable)
        {
            currentScope.AddVariable(defineVariable);

            return defineVariable;
        }

        private ParameterNode FindParameter(string name)
        {
            return currentParameters.FirstOrDefault(n => n.Name == name);
        }

        private XzaarAstNode WalkFunction()
        {
            AssertMinExpectedNodeCount(4);

            this.currentParameters.Clear();

            // fn name (expr) { body }
            // fn name (expr) -> type { body }

            var fn = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.KeywordFn);
            var name = WalkIdentifier();
            var parameterList = WalkParameterList();
            var parameters = XzaarAstNode.Parameters(parameterList);

            currentParameters.AddRange(parameters.Parameters);

            var functionName = name.ValueText;
            if (Nodes.Current.Type == XzaarSyntaxKind.PointerMemberAccess || Nodes.Current.Type == XzaarSyntaxKind.Colon)
            {
                AssertMinExpectedNodeCount(3);
                var access = Nodes.Consume();

                var returnType = WalkType().ValueText;
                var function = XzaarAstNode.Function(functionName, returnType, parameters);
                definedFunctions.Add(functionName, function);

                var body = WalkBody(functionName);
                if (body != null)
                    function.SetBody(body);
                return function;
            }
            else
            {

                var function = XzaarAstNode.Function(functionName, parameters);
                if (definedFunctions.ContainsKey(functionName)) return Error($"A function with the same name '{function.Name}' already exists", Nodes.Current);
                definedFunctions.Add(functionName, function);

                var body = WalkBody(functionName);
                if (body != null)
                    function.SetBody(body);
                return function;
            }
        }

        private XzaarAstNode WalkStatementOrBody()
        {
            var stmts = new List<XzaarAstNode>();
            if (this.Nodes.Current != null && this.Nodes.Current.Kind == XzaarSyntaxKind.Scope)
            {
                return WalkBody();
            }
            else
            {
                var statement = WalkStatement();
                if (statement != null)
                    stmts.Add(statement);
            }
            return XzaarAstNode.Body(stmts.ToArray());
        }

        private XzaarAstNode WalkBody(string name = null)
        {
            if (Nodes.Current == null || Nodes.Current.Kind != XzaarSyntaxKind.Scope)
            {
                if (!string.IsNullOrEmpty(name))
                    return Error("The function '" + name + "' is clearly missing a body!!", Nodes.Current);
                return Error("We expected a new scope or body to be declared.. But it wasnt?", Nodes.Current);
            }
            var stmts = new List<XzaarAstNode>();
            if (PrepareScope())
            {
                while (!this.EndOfStream())
                {

                    var statement = WalkStatement();
                    if (statement != null)
                        stmts.Add(statement);

                    //var astNode = WalkSubExpression(Precedence.Expression);
                    //if (astNode != null && astNode.NodeType != XzaarAstNodeTypes.SEPARATOR || Nodes.Current == null)
                    //    args.Add(XzaarAstNode.Argument(astNode, index++));

                    //if (Nodes.Current != null && Nodes.Current.Kind == XzaarSyntaxKind.Separator)
                    //    Nodes.Consume(n => n.Kind == XzaarSyntaxKind.Separator);
                }
                EndScope(XzaarSyntaxKind.Scope);
            }
            return XzaarAstNode.Body(stmts.ToArray());
        }

        private XzaarAstNode WalkIdentifier()
        {
            XzaarAstNode item = null;

            if (Nodes.Current.Kind != XzaarSyntaxKind.Identifier
                && Nodes.PeekNext() != null && Nodes.PeekNext().Kind == XzaarSyntaxKind.Identifier)
                Nodes.Consume();

            var identifier = Nodes.Consume(x => x.Kind == XzaarSyntaxKind.Identifier);
            if (identifier == null)
                return Error("Identifier expected", Nodes.Current);

            var stringValue = identifier.StringValue;


            if (definedStructs.ContainsKey(stringValue))
            {
                var structDefinition = definedStructs[stringValue];
                if (Nodes.CurrentIs(n => n.Kind == XzaarSyntaxKind.Scope))
                {
                    var structInit = WalkStructInitializer();

                    return XzaarAstNode.CreateStruct(structDefinition, structInit);
                }
                return XzaarAstNode.CreateStruct(structDefinition);
            }


            item = XzaarAstNode.Identifier(stringValue);

            return item;
        }



        private XzaarAstNode WalkExpressionCore()
        {
            XzaarAstNode subExpr = null;
            var before = this.Nodes.Current;
            var isExpression = before.Type == XzaarSyntaxKind.Expression;
            if (isExpression)
            {
                subExpr = this.WalkExpression();
            }
            else
            {
                subExpr = this.WalkSubExpression(Precedence.Expression);
            }

            //if (isExpression && subExpr.NodeType != XzaarAstNodeTypes.EXPRESSION)
            //{
            //    return XzaarAstNode.Expression(subExpr);
            //}

            return subExpr;
        }


        private XzaarAstNode WalkSubExpression(Precedence precedence)
        {
            return ParseSubExpressionCore(precedence);
        }

        private XzaarAstNode WalkTerm(Precedence precedence)
        {
            XzaarAstNode expr = null;

            var tk = this.Nodes.Current.Kind;
            switch (tk)
            {
                case XzaarSyntaxKind.Identifier:
                    expr = WalkIdentifier();
                    break;

                // case XzaarSyntaxKind.ArgListKeyword:
                case XzaarSyntaxKind.KeywordFalse:
                case XzaarSyntaxKind.KeywordTrue:
                case XzaarSyntaxKind.KeywordNull:
                    expr = WalkKnownConstant();
                    break;
                case XzaarSyntaxKind.LiteralNumber:
                case XzaarSyntaxKind.LiteralString:
                case XzaarSyntaxKind.Literal:
                    expr = Walk();
                    break;
                //case XzaarSyntaxKind.KeywordNew:
                //    expr = this.ParseNewExpression();
                //    break;
                default:
                    // check for intrinsic type followed by '.'
                    if (XzaarSyntaxFacts.IsPredefinedType(tk))
                    {
                        expr = Walk();
                    }
                    break;
            }

            return this.WalkPostFixExpression(expr);
        }

        private XzaarAstNode WalkPostFixExpression(XzaarAstNode expr)
        {
            // Debug.Assert(expr != null);
            if (expr == null && this.Nodes.Current != null)
            {
                if (this.Nodes.Current.Kind == XzaarSyntaxKind.ArrayIndexExpression)
                {
                    expr = WalkNewArrayInstance();
                    // array initializer, no need for expr to have a value.
                }
            }
            if (expr == null) return null;

            while (!this.HasErrors)
            {
                if (this.Nodes.Current == null) return expr;
                XzaarSyntaxKind tk = this.Nodes.Current.Kind;
                switch (tk)
                {
                    case XzaarSyntaxKind.Expression:
                        expr = XzaarAstNode.Call(expr, this.WalkArgumentList());
                        break;

                    case XzaarSyntaxKind.AggregateObjectIndex:
                    case XzaarSyntaxKind.ArrayIndexExpression:
                        var walkArrayArgumentList = WalkArrayArgumentList();
                        if (walkArrayArgumentList == null) return errorNodes.LastOrDefault();
                        expr = XzaarAstNode.MemberAccess(expr, walkArrayArgumentList.FirstOrDefault()); // we only support one item for now                        
                        break;
                    case XzaarSyntaxKind.UnaryIncrement:
                    case XzaarSyntaxKind.UnaryDecrement:
                    case XzaarSyntaxKind.PostfixDecrement:
                    case XzaarSyntaxKind.PostfixIncrement:
                    case XzaarSyntaxKind.PlusPlus:
                    case XzaarSyntaxKind.MinusMinus:

                        var type = XzaarSyntaxFacts.GetPostfixUnaryExpression(tk);

                        expr = XzaarAstNode.PostfixUnary(Nodes.Consume(), expr, type);
                        // expr = _syntaxFactory.PostfixUnaryExpression(SyntaxFacts.GetPostfixUnaryExpression(tk), expr, this.EatToken());
                        break;

                    case XzaarSyntaxKind.MemberAccess:
                    case XzaarSyntaxKind.Dot:
                        var identifier = WalkIdentifier();
                        expr = XzaarAstNode.MemberAccessChain(expr, XzaarAstNode.MemberAccess(identifier, expr.Type, FindMemberType(expr.ValueText, expr.Type, identifier)));
                        break;
                    case XzaarSyntaxKind.PointerMemberAccess:
                    case XzaarSyntaxKind.MinusGreater:
                        // expr = _syntaxFactory.MemberAccessExpression(XzaarSyntaxKind.PointerMemberAccess, expr, this.Nodes.Next(), Walk(this.Nodes.Next()));
                        throw new Exception("pointer member access");
                        break;
                        //case XzaarSyntaxKind.Dot:
                        //    // if we have the error situation:
                        //    //
                        //    //      expr.
                        //    //      X Y
                        //    //
                        //    // Then we don't want to parse this out as "Expr.X"
                        //    //
                        //    // It's far more likely the member access expression is simply incomplete and
                        //    // there is a new declaration on the next line.
                        //    if (this.CurrentToken.TrailingTrivia.Any((int)SyntaxKind.EndOfLineTrivia) &&
                        //        this.PeekToken(1).Kind == SyntaxKind.IdentifierToken &&
                        //        this.PeekToken(2).Kind == SyntaxKind.IdentifierToken)
                        //    {
                        //        expr = _syntaxFactory.MemberAccessExpression(
                        //            SyntaxKind.SimpleMemberAccessExpression, expr, this.EatToken(),
                        //            this.AddError(this.CreateMissingIdentifierName(), ErrorCode.ERR_IdentifierExpected));

                        //        return expr;
                        //    }

                        //    expr = _syntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expr, this.EatToken(), this.ParseSimpleName(NameOptions.InExpression));
                        //    break;

                        //case XzaarSyntaxKind.Question:
                        //    if (CanStartConsequenceExpression(this.PeekToken(1).Kind))
                        //    {
                        //        var qToken = this.EatToken();
                        //        var consequence = ParseConsequenceSyntax();
                        //        expr = _syntaxFactory.ConditionalAccessExpression(expr, qToken, consequence);
                        //        expr = CheckFeatureAvailability(expr, MessageID.IDS_FeatureNullPropagatingOperator);
                        //        break;
                        //    }

                        goto default;
                    default:
                        return expr;
                }
            }
            return null;
        }

        private XzaarAstNode WalkPostfixUnary(XzaarSyntaxKind type, XzaarAstNode item)
        {
            var token = this.Nodes.Consume();
            return XzaarAstNode.PostfixUnary(token, item);
        }


        private XzaarAstNode ParseSubExpressionCore(Precedence precedence)
        {
            XzaarAstNode leftOperand = null;
            Precedence newPrecedence = 0;
            XzaarSyntaxKind opKind = XzaarSyntaxKind.None;

            // all of these are tokens that start statements and are invalid
            // to start a expression with. if we see one, then we must have
            // something like:
            //
            // return
            // if (...
            // parse out a missing name node for the expression, and keep on going
            var tk = this.Nodes.Current.Kind;
            if (XzaarSyntaxFacts.IsInvalidSubExpression(tk))
            {
                return this.Error(tk + " is not a valid sub expression", Nodes.Current);
            }


            if (tk == XzaarSyntaxKind.Expression)
            {
                leftOperand = WalkExpression();
            }


            // No left operand, so we need to parse one -- possibly preceded by a
            // unary operator.
            else if (XzaarSyntaxFacts.IsExpectedPrefixUnaryOperator(tk))
            {
                opKind = XzaarSyntaxFacts.GetPrefixUnaryExpression(tk);
                newPrecedence = XzaarSyntaxFacts.GetPrecedence(opKind);
                var opToken = this.Nodes.Consume();
                var operand = this.WalkSubExpression(newPrecedence);
                //if (XzaarSyntaxFacts.IsIncrementOrDecrementOperator(opToken.Kind))
                //{
                //    operand = CheckValidLvalue(operand);
                //}

                leftOperand =
                    XzaarAstNode.PrefixUnary(opToken, operand);
                // XzaarAstNode.Incrementor(operand);
                // _syntaxFactory.PrefixUnaryExpression(opKind, opToken, operand);
            }
            else
            {
                // Not a unary operator - get a primary expression.
                leftOperand = this.WalkTerm(precedence);
            }

            if (HasErrors)
                return null;

            while (true)
            {
                // We either have a binary or assignment operator here, or we're finished.
                if (this.Nodes.Current == null) // check for end of expression
                    break;

                tk = this.Nodes.Current.Kind;

                bool isAssignmentOperator = false;
                if (XzaarSyntaxFacts.IsExpectedBinaryOperator(tk) || XzaarSyntaxFacts.IsBinaryExpression(tk))
                {
                    if (XzaarSyntaxFacts.IsExpectedBinaryOperator(tk))
                        opKind = XzaarSyntaxFacts.GetBinaryExpression(tk);
                    else
                        opKind = tk;
                }
                else if (XzaarSyntaxFacts.IsExpectedAssignmentOperator(tk) || XzaarSyntaxFacts.IsAssignmentExpression(tk))
                {
                    if (XzaarSyntaxFacts.IsExpectedAssignmentOperator(tk))
                        opKind = XzaarSyntaxFacts.GetAssignmentExpression(tk);
                    else
                        opKind = tk;
                    isAssignmentOperator = true;
                }
                else
                {
                    if (Nodes.Current.Type == XzaarSyntaxKind.StatementTerminator)
                        Nodes.Consume();
                    break;
                }

                newPrecedence = XzaarSyntaxFacts.GetPrecedence(opKind);

                Debug.Assert(newPrecedence > 0);      // All binary operators must have precedence > 0!

                // check for >> or >>=
                bool doubleOp = false;
                //if (tk == XzaarSyntaxKind.GreaterEquals
                //    && (this.Nodes.PeekNext().Kind == XzaarSyntaxKind.Greater || this.Nodes.PeekNext().Kind == XzaarSyntaxKind.GreaterEquals))
                //{
                //    // check to see if they really are adjacent
                //    if (this.Nodes.Current.GetTrailingTriviaWidth() == 0 && this.Nodes.PeekNext().GetLeadingTriviaWidth() == 0)
                //    {
                //        if (this.Nodes.PeekNext().Kind == XzaarSyntaxKind.Greater)
                //        {
                //            opKind = XzaarSyntaxFacts.GetBinaryExpression(XzaarSyntaxKind.GreaterGreater);
                //        }
                //        else
                //        {
                //            opKind = XzaarSyntaxFacts.GetAssignmentExpression(XzaarSyntaxKind.GreaterGreaterEquals);
                //            isAssignmentOperator = true;
                //        }
                //        newPrecedence = XzaarSyntaxFacts.GetPrecedence(opKind);
                //        doubleOp = true;
                //    }
                //}

                // Check the precedence to see if we should "take" this operator
                if (newPrecedence < precedence)
                {
                    break;
                }

                // Same precedence, but not right-associative -- deal with this "later"
                if ((newPrecedence == precedence) && !XzaarSyntaxFacts.IsRightAssociative(opKind))
                {
                    break;
                }

                // Precedence is okay, so we'll "take" this operator.
                var opToken = Nodes.Consume(); // this.EatContextualToken(tk);
                //if (doubleOp)
                //{
                //    // combine tokens into a single token
                //    var opToken2 = this.EatToken();
                //    var kind = opToken2.Kind == XzaarSyntaxKind.GreaterThanToken ? XzaarSyntaxKind.GreaterThanGreaterThanToken : XzaarSyntaxKind.GreaterThanGreaterThanEqualsToken;
                //    opToken = SyntaxFactory.Token(opToken.GetLeadingTrivia(), kind, opToken2.GetTrailingTrivia());
                //}


                if (isAssignmentOperator)
                {
                    // leftOperand = CheckValidLvalue(leftOperand);
                    var rightOperand = this.WalkSubExpression(newPrecedence);
                    switch (opKind)
                    {
                        case XzaarSyntaxKind.Assign:
                            leftOperand = XzaarAstNode.Assign(leftOperand, rightOperand);
                            break;
                        case XzaarSyntaxKind.AssignAnd:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '&', rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignOr:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '|', rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignMultiply:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '*', rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignDivide:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '/', rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignMinus:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '-', rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignPlus:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '+', rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignLeftShift:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, "<<", rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignRightShift:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, ">>", rightOperand));
                            break;
                        case XzaarSyntaxKind.AssignModulo:
                            leftOperand = XzaarAstNode.Assign(leftOperand, XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, '%', rightOperand));
                            break;
                    }

                    //  _syntaxFactory.AssignmentExpression(opKind, leftOperand, opToken,);
                }
                else
                {
                    leftOperand = XzaarAstNode.BinaryOperator((int)newPrecedence, leftOperand, opToken.StringValue, this.WalkSubExpression(newPrecedence));
                }
            }


            // From the language spec:
            //
            // conditional-expression:
            //  null-coalescing-expression
            //  null-coalescing-expression   ?   expression   :   expression
            //
            // Only take the ternary if we're at a precedence less than the null coalescing
            // expression.

            //if (tk == XzaarSyntaxKind.Question && precedence <= Precedence.Ternary)
            //{
            //    var questionToken = this.Nodes.Next();
            //    var colonLeft = this.ParseExpressionCore();
            //    var colon = this.Nodes.Next(); // XzaarSyntaxKind.Colon
            //    var colonRight = this.ParseExpressionCore();
            //    leftOperand = XzaarAstNode.ConditionalExpression(leftOperand, questionToken, colonLeft, colon, colonRight);
            //}

            return leftOperand;
        }


        private bool IsPossibleStatement()
        {
            var tk = this.Nodes.Current.Kind;
            switch (tk)
            {
                case XzaarSyntaxKind.KeywordBreak:
                case XzaarSyntaxKind.KeywordContinue:
                case XzaarSyntaxKind.KeywordDo:
                case XzaarSyntaxKind.KeywordFor:
                case XzaarSyntaxKind.KeywordForEach:
                case XzaarSyntaxKind.KeywordGoto:
                case XzaarSyntaxKind.KeywordLoop:
                case XzaarSyntaxKind.KeywordIf:
                case XzaarSyntaxKind.KeywordReturn:
                case XzaarSyntaxKind.KeywordSwitch:
                case XzaarSyntaxKind.KeywordWhile:
                case XzaarSyntaxKind.Scope:
                case XzaarSyntaxKind.Semicolon:
                    return true;

                case XzaarSyntaxKind.Identifier:
                    return true;

                // Accessibility modifiers are not legal in a statement,
                // but a common mistake for local functions. Parse to give a
                // better error message.

                default:
                    return XzaarSyntaxFacts.IsPredefinedType(tk)
                           || IsPossibleExpression();
            }
        }

        private bool IsPossibleExpression()
        {
            var tk = this.Nodes.Current.Kind;
            switch (tk)
            {

                case XzaarSyntaxKind.ArgList:
                case XzaarSyntaxKind.Constant:

                case XzaarSyntaxKind.Expression:
                case XzaarSyntaxKind.ArrayIndexExpression:
                case XzaarSyntaxKind.LiteralNumber:
                case XzaarSyntaxKind.LiteralString:
                case XzaarSyntaxKind.Literal:
                case XzaarSyntaxKind.KeywordNew:
                case XzaarSyntaxKind.KeywordTrue:
                case XzaarSyntaxKind.KeywordFalse:
                case XzaarSyntaxKind.KeywordNull:
                case XzaarSyntaxKind.ColonColon: // bad aliased name                
                    return true;
                case XzaarSyntaxKind.Identifier:
                    // Specifically allow the from contextual keyword, because it can always be the start of an
                    // expression (whether it is used as an identifier or a keyword).
                    return true;
                default:
                    return XzaarSyntaxFacts.IsExpectedPrefixUnaryOperator(tk)
                           || (XzaarSyntaxFacts.IsPredefinedType(tk) && tk != XzaarSyntaxKind.KeywordVoid)
                           || XzaarSyntaxFacts.IsAnyUnaryExpression(tk)
                           || XzaarSyntaxFacts.IsBinaryExpression(tk)
                           || XzaarSyntaxFacts.IsAssignmentExpressionOperatorToken(tk);
            }
        }

        private void AssertMinExpectedNodeCount(int count)
        {
            if (count > Nodes.Available)
                Error("Oh no! Unexpected end of script. We expected "
                                                    + count + " more nodes, but there are only " + Nodes.Available + " left.", Nodes.Current);
        }


        //private XzaarAstNode Error(string message)
        //{
        //    var msg = "[Error] " + message;
        //    this.errors.Add(msg);
        //    this.Nodes.Interrupted = true;
        //    return XzaarAstNode.Error(message);
        //}

        private XzaarAstNode Error(string message, XzaarSyntaxNode token = null)
        {
            var msg = "[Error] " + message;
            if (token != null)
            {
                if (token.TrailingToken != null)
                {
                    msg += ". At line " + token.TrailingToken.Line;
                }
            }
            var errorNode = XzaarAstNode.Error(message, token);

            this.errors.Add(msg);
            this.Nodes.Interrupted = true;
            this.errorNodes.Add(errorNode);
            return errorNode;
        }

        private bool EndOfStream()
        {
            return this.HasErrors || Nodes.EndOfStream();
        }

        private XzaarNodeStream Nodes
        {
            get { return this.currentScope.Nodes; }
        }

        private void BeginScope(XzaarNodeStream scopeNodes)
        {
            this.currentScope = currentScope.BeginScope(scopeNodes);
        }

        private void EndScope(XzaarSyntaxKind endType)
        {
            this.currentScope = currentScope.EndScope();

            if (Nodes.Current != null)
            {
                Nodes.Consume(n => n.Kind == endType);
            }

            // Debug.Assert(node != null);
        }
    }
}