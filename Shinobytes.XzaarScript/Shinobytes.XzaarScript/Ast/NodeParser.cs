using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.Utilities;

namespace Shinobytes.XzaarScript.Ast
{
    public class NodeParser
    {
        private readonly SyntaxNode ast;
        private ParserScope currentScope;
        private readonly List<string> errors = new List<string>();
        private readonly List<ErrorNode> errorNodes = new List<ErrorNode>();

        private readonly List<LabelNode> labels = new List<LabelNode>();
        private readonly List<ParameterNode> currentParameters = new List<ParameterNode>();
        private readonly Dictionary<string, FunctionNode> definedFunctions = new Dictionary<string, FunctionNode>();
        private readonly Dictionary<string, StructNode> definedStructs = new Dictionary<string, StructNode>();
        private AstNode lastWalkedExpression;

        public NodeParser(SyntaxNode ast)
        {
            this.ast = ast;
            this.currentScope = new ParserScope();
        }

        public AstNode Transform()
        {
            return new EntryNode(WalkAllNodes());
        }

        public bool HasErrors => errors.Count > 0;

        public IList<string> Errors => errors;

        private AstNode WalkAllNodes()
        {
            var nodes = new List<AstNode>();
            this.BeginScope(new NodeStream(ast.Children));

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

            this.EndScope(SyntaxKind.Scope);
            return AstNode.Block(nodes.ToArray());
        }

        private AstNode Walk()
        {

            // NOTE: A node must always be returned unless error
            //       if the next nodes require the previous node
            //       then it has to parse the same node once more
            //       but when it has been used by another node, 
            //       the previous node should be replaced with the new node
            //       This is so we can avoid using a stack and never
            //       care to know whether our resulting node can be chained 
            //       with the next. It saves us alot of headache and code maintenance

            Nodes.Consume(n => n.Type == SyntaxKind.StatementTerminator);

            if (Nodes.EndOfStream()) return null;

            switch (Nodes.Current.Type)
            {
                case SyntaxKind.Identifier:
                    {
                        if (Nodes.PeekNext() != null && Nodes.PeekNext().Kind == SyntaxKind.Colon)
                            return lastWalkedExpression = WalkLabel();
                        return lastWalkedExpression = WalkExpressionStatement(); // return WalkIdentifier();
                    }
                case SyntaxKind.Keyword: return lastWalkedExpression = WalkKeyword();
                case SyntaxKind.UnaryOperator: return lastWalkedExpression = WalkUnaryOperator();
                case SyntaxKind.AggregateObjectIndex:
                //case SyntaxKind.ArrayIndexExpression:
                //    return WalkObjectIndex();
                case SyntaxKind.ArithmeticOperator: return lastWalkedExpression = WalkArithmeticOperator();
                case SyntaxKind.EqualityOperator: return lastWalkedExpression = WalkEqualityOperator();
                case SyntaxKind.LogicalConditionalOperator: return lastWalkedExpression = WalkLogicalConditionalOperator();
                case SyntaxKind.ConditionalOperator: return lastWalkedExpression = WalkConditionalOperator();
                case SyntaxKind.AssignmentOperator: return lastWalkedExpression = WalkAssignmentOperator();
                case SyntaxKind.Constant: return lastWalkedExpression = WalkConstantValue();
                case SyntaxKind.Literal:
                    return lastWalkedExpression = Nodes.Current.Kind == SyntaxKind.LiteralNumber
                        ? WalkNumberLiteral()
                        : WalkStringLiteral();
                // case SyntaxKind.Expression: return WalkExpression();
                case SyntaxKind.Scope:
                    return lastWalkedExpression = WalkScope();
                //case SyntaxKind.MemberAccess: return WalkMemberAccess();
                default:
                    {
                        return lastWalkedExpression = Error("Unexpected node type '" + Nodes.Current.Type + "' found.", Nodes.Current);
                    }
            }
        }

        private AstNode WalkLabel()
        {
            var labelName = WalkIdentifier();
            var colon = Nodes.Consume(x => x.Kind == SyntaxKind.Colon);

            var label = AstNode.Label(labelName.ValueText);

            this.labels.Add(label);

            return label;
        }

        private AstNode WalkGoto()
        {
            if (Nodes.PeekNext().Kind == SyntaxKind.KeywordCase)
            {
                return Error("Goto case has not been implemented yet.", Nodes.Current);
            }
            var before = Nodes.Current;
            var target = WalkIdentifier();
            var label = labels.FirstOrDefault(l => l.Name == target.ValueText);
            if (label == null)
                return Error("No labels with the name '" + target.ValueText + "' could be found.", before);

            return AstNode.Goto(target.ValueText);
        }

        private AstNode WalkExpressionStatement()
        {
            return WalkExpressionStatement(this.WalkExpressionCore());
        }

        private AstNode WalkExpressionStatement(AstNode expression)
        {

            if (this.Nodes.Current != null && this.Nodes.Current.Kind == SyntaxKind.Semicolon)
            {
                this.Nodes.Consume();
            }

            return expression;

            //if (expression.NodeType == NodeTypes.EXPRESSION)
            //    return expression;

            //return AstNode.Expression(expression);


            // return _syntaxFactory.ExpressionStatement(expression, semicolon);
        }

        private AstNode WalkKeyword()
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

        private AstNode WalkContinue()
        {
            Nodes.Consume(n => n.Kind == SyntaxKind.KeywordContinue);
            return AstNode.Continue();
        }

        private AstNode WalkBreak()
        {
            Nodes.Consume(n => n.Kind == SyntaxKind.KeywordBreak);
            return AstNode.Break();
        }

        private AstNode WalkKnownConstant()
        {
            var value = this.Nodes.Consume().StringValue;
            return AstNode.Identifier(value);
        }

        private AstNode WalkType()
        {
            var isArray = false;
            var current = Nodes.Consume();
            if (Nodes.Current != null)
            {
                if (Nodes.Current.Kind == SyntaxKind.AggregateObjectIndex ||
                    Nodes.Current.Kind == SyntaxKind.ArrayIndexExpression)
                {
                    isArray = true;
                    Nodes.Consume();
                }
            }
            return AstNode.Identifier(current.StringValue + (isArray ? "[]" : ""));
        }

        private AstNode WalkReturn()
        {
            Nodes.Consume(n => n.Kind == SyntaxKind.KeywordReturn);

            var nextNode = Nodes.Current;
            if (nextNode != null && nextNode.Kind != SyntaxKind.KeywordCase)
            {
                var result = WalkSubExpression(Precedence.Expression);
                return AstNode.Return(result);
            }
            return AstNode.Return();
        }

        private AstNode WalkNumberLiteral()
        {
            var value = AstNode.NumberLiteral(Nodes.Consume(x => x.Kind == SyntaxKind.LiteralNumber).Value);

            return WalkPostFixExpression(value);
        }

        private AstNode WalkStringLiteral()
        {
            var value = AstNode.StringLiteral(Nodes.Consume(x => x.Kind == SyntaxKind.LiteralString).StringValue);
            return WalkPostFixExpression(value);
        }

        private string FindMemberType(
            string lastMemberName,
            string lastMemberType,
            AstNode nextAccessorItem)
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

        private bool CanBeChained(NodeTypes nType, SyntaxKind currentKind)
        {
            if (nType == NodeTypes.ACCESS && currentKind == SyntaxKind.MemberAccess) return true;
            if (nType == NodeTypes.ACCESS && currentKind == SyntaxKind.ArrayIndexExpression) return true;
            if (nType == NodeTypes.ACCESS && currentKind == SyntaxKind.ArithmeticOperator) return true;
            return true;
        }

        private ArgumentNode[] WalkArrayArgumentList()
        {
            return WalkArgumentList(SyntaxKind.ArrayIndexExpression);
        }


        private ArgumentNode[] WalkArgumentList()
        {
            return WalkArgumentList(SyntaxKind.Expression);
        }

        private ArgumentNode[] WalkArgumentList(SyntaxKind kind)
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
                    if (astNode != null && astNode.NodeType != NodeTypes.SEPARATOR || Nodes.Current == null)
                        args.Add(AstNode.Argument(astNode, index++));

                    if (Nodes.Current != null && Nodes.Current.Kind == SyntaxKind.Separator)
                        Nodes.Consume(n => n.Kind == SyntaxKind.Separator);
                }
                EndScope(kind);
            }
            return args.ToArray();
        }


        private AstNode WalkParameterList()
        {
            var args = new List<ParameterNode>();
            if (PrepareScope())
            {

                int index = 0;
                while (!this.EndOfStream())
                {
                    var before = this.Nodes.Current;

                    var possibleIdentifier = this.Nodes.PeekAt(1);
                    if (possibleIdentifier != null && possibleIdentifier.Kind == SyntaxKind.Colon)
                    {
                        // typescript/rust style
                        var name = WalkIdentifier();
                        var colon = Nodes.Consume(n => n.Kind == SyntaxKind.Colon);
                        var type = WalkType();
                        args.Add(AstNode.Parameter(name, type));
                    }
                    else
                    {
                        // javascript/java/c#/c/c++, list goes long - style
                        var type = WalkType();
                        var name = WalkIdentifier();
                        args.Add(AstNode.Parameter(name, type));
                    }

                    //this.Nodes.Next(); 
                    if (before == this.Nodes.Current || (this.Nodes.Current != null && this.Nodes.Current.Type == SyntaxKind.Separator)) Nodes.Consume();
                }
                EndScope(SyntaxKind.Expression);
            }
            return AstNode.Parameters(args.ToArray());
        }


        private bool PrepareScope(IList<SyntaxNode> nodes = null)
        {
            if ((nodes != null) || (this.Nodes.Current != null))
            {
                BeginScope(new NodeStream(nodes ?? this.Nodes.Current.Children));

                return true;
            }
            return false;
        }

        private AstNode WalkScope()
        {
            var node = Nodes.Current;
            AstNode scopeNode = null;
            var nodes = new List<AstNode>();
            if (node.HasChildren)
            {
                BeginScope(new NodeStream(node.Children));

                while (!this.EndOfStream())
                {
                    var astNode = Walk();
                    if (astNode != null) nodes.Add(astNode);
                    //this.Nodes.Next();
                }
                EndScope(SyntaxKind.Scope);
            }


            return AstNode.Block(nodes.ToArray());
        }

        private AstNode WalkExpression()
        {
            var node = Nodes.Current;
            AstNode scopeNode = null;
            var nodes = new List<AstNode>();

            if (PrepareScope())
            {
                while (!this.EndOfStream())
                {
                    var astNode = WalkExpressionCore();
                    if (astNode != null) nodes.Add(astNode);
                    //this.Nodes.Next();
                }
                EndScope(SyntaxKind.Expression);
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

        private AstNode MergeExpressions(List<AstNode> nodes)
        {
            // if its only one node, return that one.
            if (nodes.Count == 1) return nodes[0];

            var expressions = new List<AstNode>();
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

        private AstNode MergeExpressionsRight(AstNode left, AstNode right)
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

        private AstNode MergeExpressionsLeft(AstNode left, AstNode right)
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

        private AstNode ReplaceDeepLeft(AstNode op, AstNode expr)
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

        private AstNode WalkConstantValue()
        {
            throw new System.NotImplementedException();
        }

        private AstNode WalkConditionalOperator()
        {
            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next
            throw new System.NotImplementedException();
        }

        private AstNode WalkAssignmentOperator()
        {
            var assign = Nodes.Consume(n => n.Type == SyntaxKind.AssignmentOperator);
            // go back once if we are currently on the assignment
            // if (SyntaxFacts.IsAssignmentExpression(Nodes.Current.Kind) || SyntaxFacts.IsExpectedAssignmentOperator(Nodes.Current.Kind)) Nodes.Previous();

            return WalkExpressionCore();
        }

        private AstNode WalkNewArrayInstance()
        {
            var old = Nodes.Current;
            if (Nodes.Current.Kind == SyntaxKind.ArrayIndexExpression)
            {
                // array initializer
                var args = WalkArgumentList();

                if (Nodes.Current == old) Nodes.Consume(x => x.Kind == SyntaxKind.ArrayIndexExpression);

                // Nodes.Consume(i => i.Kind == SyntaxKind.ArrayIndexExpression);

                return AstNode.NewArrayInstance(args);
            }
            return Error("Unexpected type '" + Nodes.Current?.Kind + "' found. We expected a new array 'a = []' expression.", Nodes.Current);
        }

        private AstNode WalkLogicalConditionalOperator()
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

            AstNode valueNode = null;

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

        private AstNode WalkEqualityOperator()
        {
            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next

            throw new System.NotImplementedException();
        }

        private AstNode WalkArithmeticOperator()
        {
            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next

            return WalkExpression();

            throw new System.NotImplementedException();
        }

        //private AstNode WalkObjectIndex()
        //{
        //    // if previous is assignment, it is an array initializer            
        //    // otherwise a normal object index

        //    var item = this.Nodes.Previous();

        //    if (item == null && this.Nodes.Current != null && this.Nodes.Current.Kind == SyntaxKind.ArrayIndexExpression)
        //    {
        //        var result = WalkNewArrayInstance();

        //        return result;
        //    }

        //    var xzaarAstNode = Walk();

        //    return WalkElementAccess(xzaarAstNode);
        //}

        private AstNode WalkUnaryOperator()
        {
            AssertMinExpectedNodeCount(2);
            // |++|+|--|-|!| (expr)

            var unary = this.Nodes.Consume();

            var expr = this.Walk();

            if (expr.NodeType == NodeTypes.UNARY_OPERATOR)
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

            // var type = SyntaxFacts.GetPrefixUnaryExpression(unary.Kind);

            return AstNode.PrefixUnary(unary, expr);
        }

        private AstNode WalkEnum()
        {
            AssertMinExpectedNodeCount(3);
            // enum name { body }

            throw new System.NotImplementedException();
        }

        private AstNode WalkClass()
        {
            AssertMinExpectedNodeCount(3);
            // class name { body }

            throw new System.NotImplementedException();
        }

        private AstNode WalkStruct()
        {
            AssertMinExpectedNodeCount(3);
            // struct name { body }

            var @struct = Nodes.Consume(n => n.Kind == SyntaxKind.KeywordStruct);

            var name = WalkIdentifier();

            var fields = WalkStructFields(name.ValueText);

            var str = AstNode.Struct(name.ValueText, fields);

            this.definedStructs.Add(name.ValueText, str);

            return str;
        }

        private AstNode[] WalkStructFields(string declaringTypeName)
        {
            var fields = new List<AstNode>();
            if (PrepareScope(Nodes.Current.Children))
            {
                while (!EndOfStream())
                {
                    var possibleIdentifier = Nodes.PeekNext();
                    if (possibleIdentifier != null && possibleIdentifier.Kind == SyntaxKind.Colon)
                    {
                        // tsstyle/rust-style
                        // name : type
                        var name = WalkIdentifier();
                        var colon = Nodes.Consume();
                        var type = WalkType();
                        fields.Add(AstNode.Field(type.ValueText, name.ValueText, declaringTypeName));
                    }
                    else
                    {
                        // c#/java/cc/c++/... style
                        // type name                                                
                        var type = WalkType();
                        var name = WalkIdentifier();
                        fields.Add(AstNode.Field(type.ValueText, name.ValueText, declaringTypeName));
                    }
                    if (Nodes.Current != null &&
                        (Nodes.Current.Kind == SyntaxKind.Separator ||
                         Nodes.Current.Kind == SyntaxKind.StatementTerminator))
                    {
                        Nodes.Consume();
                    }
                }

                EndScope(SyntaxKind.Scope);
            }

            return fields.ToArray();
        }

        private AstNode[] WalkStructInitializer()
        {
            // similar to WalkStructFields, but here we expect to assign the values of the target struct and separate the expressions by a comma
            var fieldAssignments = new List<AstNode>();
            if (PrepareScope(Nodes.Current.Children))
            {
                while (!EndOfStream())
                {
                    var assignment = Walk();
                    if (assignment.NodeType != NodeTypes.ASSIGN)
                    {
                        return new[] { Error("You're suppose to assign the values here", Nodes.Current) };
                    }
                    fieldAssignments.Add(assignment);
                    Nodes.Consume(x => x.Kind == SyntaxKind.Separator);
                }

                EndScope(SyntaxKind.Scope);
            }

            return fieldAssignments.ToArray();
        }

        private AstNode WalkSwitchCase()
        {
            Debug.Assert(this.IsPossibleSwitchCase());

            // case (expr) : { body }
            // case (expr) : { body } break|continue|return


            if (Nodes.Current.Kind == SyntaxKind.KeywordCase)
            {
                AssertMinExpectedNodeCount(4);

                var @case = Nodes.Consume(x => x.Kind == SyntaxKind.KeywordCase);
                Debug.Assert(@case != null);
                var test = WalkExpressionStatement();
                var colon = Nodes.Consume(x => x.Kind == SyntaxKind.Colon);
                var switchCaseBody = WalkSwitchCaseBody();
                Debug.Assert(switchCaseBody != null);

                return AstNode.Case(test, switchCaseBody);
            }
            else
            {
                AssertMinExpectedNodeCount(3);

                var defaultCase = Nodes.Consume(x => x.Kind == SyntaxKind.KeywordDefault);
                Debug.Assert(defaultCase != null);
                var colon = Nodes.Consume(x => x.Kind == SyntaxKind.Colon);
                var switchCaseBody = WalkSwitchCaseBody();
                Debug.Assert(switchCaseBody != null);

                return AstNode.DefaultCase(switchCaseBody);
            }



            throw new System.NotImplementedException();
        }

        private AstNode WalkSwitchCaseBody()
        {
            if (Nodes.Current != null && Nodes.Current.Kind == SyntaxKind.KeywordCase)
                return Error("Multiple switch case labels are not supported at this time! Please make sure you add a 'break' or 'return' after declaring your switch case!", Nodes.Current);

            if (Nodes.Current == null)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);


            AstNode result = null;

            var childStatements = new List<AstNode>();

            if (Nodes.Current != null)
            {
                if (Nodes.Current.Kind == SyntaxKind.Scope)
                {
                    var scope = Nodes.Current;
                    if (PrepareScope(scope.Children))
                    {
                        while (!EndOfStream())
                        {
                            if (Nodes.Current.Kind == SyntaxKind.KeywordReturn ||
                                Nodes.Current.Kind == SyntaxKind.KeywordBreak ||
                                Nodes.Current.Kind == SyntaxKind.KeywordContinue)
                            {
                                childStatements.Add(Walk());
                                break;
                            }
                            childStatements.Add(WalkExpressionStatement());
                        }

                        EndScope(SyntaxKind.Scope);
                    }
                }
                else
                {
                    while (Nodes.Current != null)
                    {
                        if (Nodes.Current.Kind == SyntaxKind.KeywordReturn ||
                            Nodes.Current.Kind == SyntaxKind.KeywordBreak ||
                            Nodes.Current.Kind == SyntaxKind.KeywordContinue)
                        {
                            childStatements.Add(Walk());
                            break;
                        }
                        childStatements.Add(WalkExpressionStatement());
                    }
                }

                if (Nodes.Current != null && (Nodes.Current.Kind == SyntaxKind.KeywordReturn ||
                    Nodes.Current.Kind == SyntaxKind.KeywordBreak ||
                    Nodes.Current.Kind == SyntaxKind.KeywordContinue))
                {
                    childStatements.Add(Walk());
                }

                result = AstNode.Block(childStatements.ToArray());
                // result
            }

            if (Nodes.Current != null && (Nodes.Current.Kind != SyntaxKind.KeywordReturn &&
                Nodes.Current.Kind != SyntaxKind.KeywordBreak &&
                Nodes.Current.Kind != SyntaxKind.KeywordContinue))
            {
                if (childStatements.Count > 0)
                {
                    var lastChildStatement = childStatements[childStatements.Count - 1];
                    if (lastChildStatement.NodeType == NodeTypes.RETURN || lastChildStatement.NodeType == NodeTypes.BREAK || lastChildStatement.NodeType == NodeTypes.CONTINUE)
                        return result;
                }
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);
            }

            if (Nodes.Current == null && childStatements.Count == 0)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);

            var child = childStatements[childStatements.Count - 1];
            if (child.NodeType != NodeTypes.RETURN && child.NodeType != NodeTypes.BREAK && child.NodeType != NodeTypes.CONTINUE)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Nodes.Current);

            return result;
        }

        private AstNode WalkSwitch()
        {
            AssertMinExpectedNodeCount(3);
            // switch (expr) { body }

            var @switch = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordSwitch);

            var expr = this.WalkExpression();

            var scope = this.Nodes.Consume(x => x.Kind == SyntaxKind.Scope);

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
                EndScope(SyntaxKind.Scope);
            }


            return AstNode.Switch(expr, switchCases.ToArray());
        }

        private bool IsPossibleSwitchCase()
        {
            return this.Nodes.Current != null && ((this.Nodes.Current.Kind == SyntaxKind.KeywordCase) || (this.Nodes.Current.Kind == SyntaxKind.KeywordDefault));
        }

        private AstNode WalkForeachLoop()
        {
            AssertMinExpectedNodeCount(3);
            // foreach (expr) { body }

            var @foreach = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordForEach);

            var exprList = this.WalkForEachStatementExpressionList();

            Debug.Assert(exprList.Children.Count == 2);

            var body = this.WalkStatementOrBody();

            return AstNode.Foreach(exprList[0], exprList[1], body);
        }

        private AstNode WalkForLoop()
        {
            AssertMinExpectedNodeCount(3);
            // for (expr; expr; expr) { body }
            // while we only expect 3 items, we expect the second item to have 3 nodes
            // this wont be tested until later

            var @for = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordFor);

            var exprList = this.WalkForStatementExpressionList();

            Debug.Assert(exprList.Children.Count == 3);

            var body = this.WalkStatementOrBody();


            return AstNode.For(exprList[0], exprList[1], exprList[2], body);
        }

        private AstNode WalkForStatementExpressionList()
        {
            return WalkStatementExpressionList(SyntaxKind.StatementTerminator);
        }

        private AstNode WalkForEachStatementExpressionList()
        {
            return WalkStatementExpressionList(SyntaxKind.KeywordIn);
        }


        private AstNode WalkStatementExpressionList(SyntaxKind separator)
        {
            var forExprHolder = this.Nodes.Consume(x => x.Kind == SyntaxKind.Expression);
            if (!forExprHolder.HasChildren)
                return Error("Well, this is awkward. Are you sure that you know how to declare a for statement?", Nodes.Current);

            var expressions = SplitExpression(forExprHolder, separator);

            var exprList = new List<AstNode>();

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

                    EndScope(SyntaxKind.Expression);
                }
            }

            return AstNode.Expression(exprList.ToArray());
        }

        private List<List<SyntaxNode>> SplitExpression(SyntaxNode expr, SyntaxKind separator)
        {
            var expressions = new List<SyntaxNode>();
            var finalExpressions = new List<List<SyntaxNode>>();
            foreach (var item in expr.Children)
            {
                if (item.Kind == separator)
                {
                    finalExpressions.Add(new List<SyntaxNode>(expressions.ToArray()));
                    expressions.Clear();
                    continue;
                }
                expressions.Add(item);
            }
            if (expressions.Count > 0)
            {
                finalExpressions.Add(new List<SyntaxNode>(expressions.ToArray()));
            }
            return finalExpressions;
        }

        private AstNode WalkDoWhileLoop()
        {
            AssertMinExpectedNodeCount(4);
            // do { body } while (expr)

            AstNode result = null;

            var @do = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordDo);

            var body = this.WalkStatementOrBody();

            var @while = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordWhile);

            var condition = this.WalkExpression();

            result = AstNode.DoWhile(condition, body);

            Nodes.Consume();

            return result;
        }

        private AstNode WalkWhileLoop()
        {
            AssertMinExpectedNodeCount(3);
            // while (expr) { body }

            AstNode result = null;

            var @while = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordWhile);

            var condition = this.WalkExpression();

            var body = this.WalkStatementOrBody();

            result = AstNode.While(condition, body);

            Nodes.Consume();

            return result;
        }


        private AstNode WalkLoop()
        {
            AssertMinExpectedNodeCount(2);
            // loop { body }

            var loop = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordLoop);
            var body = this.WalkStatementOrBody();
            return AstNode.Loop(body);
        }


        private AstNode WalkIf()
        {
            AssertMinExpectedNodeCount(3);

            // if (expr) { body }         
            AstNode result = null;

            var @if = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordIf);

            var condition = this.WalkExpression(); // this.ParseExpressionCore();

            var ifTrue = this.WalkStatementOrBody();

            var ifFalse = WalkElseClause();

            if (ifFalse != null)
            {
                result = AstNode.IfElseThen(condition, ifTrue, ifFalse);
            }
            else
            {
                result = AstNode.IfThen(condition, ifTrue);
            }

            return result;
        }

        private AstNode WalkElseClause()
        {
            if (this.Nodes.Current == null || this.Nodes.Current.Kind != SyntaxKind.KeywordElse)
            {
                return null;
            }

            return WalkElse();
        }

        private AstNode WalkElse()
        {
            AssertMinExpectedNodeCount(2);
            // else <statement: { body }>
            // else <statement: if (expr) { body }>

            var @else = this.Nodes.Consume(x => x.Kind == SyntaxKind.KeywordElse);

            var statement = this.WalkStatementOrBody();

            return statement;
        }

        private AstNode WalkStatement()
        {
            return Walk();
        }


        private AstNode WalkVariable()
        {
            AssertMinExpectedNodeCount(2);
            // let|var name
            // let|var name = expr
            // let|var name : type = expr

            var varTypeExplicit = false;
            var varType = "any";
            var letVar = Nodes.Consume(x => x.Kind == SyntaxKind.KeywordVar);
            var name = WalkIdentifier();

            if (Nodes.Current != null && Nodes.Current.Kind == SyntaxKind.Colon)
            {
                var colon = Nodes.Consume();
                varTypeExplicit = true;
                varType = WalkType().ValueText;
            }

            var isAssign = Nodes.Current != null && SyntaxFacts.IsAssignmentExpression(Nodes.Current.Type);
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

                return AddVariable(AstNode.DefineVariable(varType, name.ValueText, valueAssignment));
            }
            // Nodes.Consume();
            return AddVariable(AstNode.DefineVariable(varType, name.ValueText, null));
        }

        private string GetExpressionResultType(AstNode expr)
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

        private AstNode WalkFunction()
        {
            AssertMinExpectedNodeCount(4);

            this.currentParameters.Clear();

            // fn name (expr) { body }
            // fn name (expr) -> type { body }

            var fn = Nodes.Consume(x => x.Kind == SyntaxKind.KeywordFn);
            var name = WalkIdentifier();
            var parameterList = WalkParameterList();
            var parameters = AstNode.Parameters(parameterList);

            currentParameters.AddRange(parameters.Parameters);

            var functionName = name.ValueText;
            if (Nodes.Current.Type == SyntaxKind.PointerMemberAccess || Nodes.Current.Type == SyntaxKind.Colon)
            {
                AssertMinExpectedNodeCount(3);
                var access = Nodes.Consume();

                var returnType = WalkType().ValueText;
                var function = AstNode.Function(functionName, returnType, parameters);
                definedFunctions.Add(functionName, function);

                var body = WalkBody(functionName);
                if (body != null)
                    function.SetBody(body);
                return function;
            }
            else
            {

                var function = AstNode.Function(functionName, parameters);
                if (definedFunctions.ContainsKey(functionName)) return Error($"A function with the same name '{function.Name}' already exists", Nodes.Current);
                definedFunctions.Add(functionName, function);

                var body = WalkBody(functionName);
                if (body != null)
                    function.SetBody(body);
                return function;
            }
        }

        private AstNode WalkStatementOrBody()
        {
            var stmts = new List<AstNode>();
            if (this.Nodes.Current != null && this.Nodes.Current.Kind == SyntaxKind.Scope)
            {
                return WalkBody();
            }
            else
            {
                var statement = WalkStatement();
                if (statement != null)
                    stmts.Add(statement);
            }
            return AstNode.Body(stmts.ToArray());
        }

        private AstNode WalkBody(string name = null)
        {
            if (Nodes.Current == null || Nodes.Current.Kind != SyntaxKind.Scope)
            {
                if (!string.IsNullOrEmpty(name))
                    return Error("The function '" + name + "' is clearly missing a body!!", Nodes.Current);
                return Error("We expected a new scope or body to be declared.. But it wasnt?", Nodes.Current);
            }
            var stmts = new List<AstNode>();
            if (PrepareScope())
            {
                while (!this.EndOfStream())
                {

                    var statement = WalkStatement();
                    if (statement != null)
                        stmts.Add(statement);

                    //var astNode = WalkSubExpression(Precedence.Expression);
                    //if (astNode != null && astNode.NodeType != NodeTypes.SEPARATOR || Nodes.Current == null)
                    //    args.Add(AstNode.Argument(astNode, index++));

                    //if (Nodes.Current != null && Nodes.Current.Kind == SyntaxKind.Separator)
                    //    Nodes.Consume(n => n.Kind == SyntaxKind.Separator);
                }
                EndScope(SyntaxKind.Scope);
            }
            return AstNode.Body(stmts.ToArray());
        }

        private AstNode WalkIdentifier()
        {
            AstNode item = null;

            if (Nodes.Current.Kind != SyntaxKind.Identifier
                && Nodes.PeekNext() != null && Nodes.PeekNext().Kind == SyntaxKind.Identifier)
                Nodes.Consume();

            var identifier = Nodes.Consume(x => x.Kind == SyntaxKind.Identifier);
            if (identifier == null)
                return Error("Identifier expected", Nodes.Current);

            var stringValue = identifier.StringValue;


            if (definedStructs.ContainsKey(stringValue))
            {
                var structDefinition = definedStructs[stringValue];
                if (Nodes.CurrentIs(n => n.Kind == SyntaxKind.Scope))
                {
                    var structInit = WalkStructInitializer();

                    return AstNode.CreateStruct(structDefinition, structInit);
                }
                return AstNode.CreateStruct(structDefinition);
            }


            item = AstNode.Identifier(stringValue);

            return item;
        }



        private AstNode WalkExpressionCore()
        {
            AstNode subExpr = null;
            var before = this.Nodes.Current;
            var isExpression = before.Type == SyntaxKind.Expression;
            if (isExpression)
            {
                subExpr = this.WalkExpression();
            }
            else
            {
                subExpr = this.WalkSubExpression(Precedence.Expression);
            }

            //if (isExpression && subExpr.NodeType != NodeTypes.EXPRESSION)
            //{
            //    return AstNode.Expression(subExpr);
            //}

            return subExpr;
        }


        private AstNode WalkSubExpression(Precedence precedence)
        {
            return ParseSubExpressionCore(precedence);
        }

        private AstNode WalkTerm(Precedence precedence)
        {
            AstNode expr = null;

            var tk = this.Nodes.Current.Kind;
            switch (tk)
            {
                case SyntaxKind.Identifier:
                    expr = WalkIdentifier();
                    break;

                // case SyntaxKind.ArgListKeyword:
                case SyntaxKind.KeywordFalse:
                case SyntaxKind.KeywordTrue:
                case SyntaxKind.KeywordNull:
                    expr = WalkKnownConstant();
                    break;
                case SyntaxKind.LiteralNumber:
                case SyntaxKind.LiteralString:
                case SyntaxKind.Literal:
                    expr = Walk();
                    break;
                //case SyntaxKind.KeywordNew:
                //    expr = this.ParseNewExpression();
                //    break;
                default:
                    // check for intrinsic type followed by '.'
                    if (SyntaxFacts.IsPredefinedType(tk))
                    {
                        expr = Walk();
                    }
                    break;
            }

            return this.WalkPostFixExpression(expr);
        }

        private AstNode WalkPostFixExpression(AstNode expr)
        {
            // Debug.Assert(expr != null);
            if (expr == null && this.Nodes.Current != null)
            {
                if (this.Nodes.Current.Kind == SyntaxKind.ArrayIndexExpression)
                {
                    expr = WalkNewArrayInstance();
                    // array initializer, no need for expr to have a value.
                }
            }
            if (expr == null) return null;

            while (!this.HasErrors)
            {
                if (this.Nodes.Current == null) return expr;
                SyntaxKind tk = this.Nodes.Current.Kind;
                switch (tk)
                {
                    case SyntaxKind.Expression:
                        expr = AstNode.Call(expr, this.WalkArgumentList());
                        break;

                    case SyntaxKind.AggregateObjectIndex:
                    case SyntaxKind.ArrayIndexExpression:
                        var walkArrayArgumentList = WalkArrayArgumentList();
                        if (walkArrayArgumentList == null) return errorNodes.LastOrDefault();
                        expr = AstNode.MemberAccess(expr, walkArrayArgumentList.FirstOrDefault()); // we only support one item for now                        
                        break;
                    case SyntaxKind.UnaryIncrement:
                    case SyntaxKind.UnaryDecrement:
                    case SyntaxKind.PostfixDecrement:
                    case SyntaxKind.PostfixIncrement:
                    case SyntaxKind.PlusPlus:
                    case SyntaxKind.MinusMinus:

                        var type = SyntaxFacts.GetPostfixUnaryExpression(tk);

                        expr = AstNode.PostfixUnary(Nodes.Consume(), expr, type);
                        // expr = _syntaxFactory.PostfixUnaryExpression(SyntaxFacts.GetPostfixUnaryExpression(tk), expr, this.EatToken());
                        break;

                    case SyntaxKind.MemberAccess:
                    case SyntaxKind.Dot:
                        var identifier = WalkIdentifier();
                        expr = AstNode.MemberAccessChain(expr, AstNode.MemberAccess(identifier, expr.Type, FindMemberType(expr.ValueText, expr.Type, identifier)));
                        break;
                    case SyntaxKind.PointerMemberAccess:
                    case SyntaxKind.MinusGreater:
                        // expr = _syntaxFactory.MemberAccessExpression(SyntaxKind.PointerMemberAccess, expr, this.Nodes.Next(), Walk(this.Nodes.Next()));
                        throw new Exception("pointer member access");
                        break;
                        //case SyntaxKind.Dot:
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

                        //case SyntaxKind.Question:
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

        private AstNode WalkPostfixUnary(SyntaxKind type, AstNode item)
        {
            var token = this.Nodes.Consume();
            return AstNode.PostfixUnary(token, item);
        }


        private AstNode ParseSubExpressionCore(Precedence precedence)
        {
            AstNode leftOperand = null;
            Precedence newPrecedence = 0;
            SyntaxKind opKind = SyntaxKind.None;

            // all of these are tokens that start statements and are invalid
            // to start a expression with. if we see one, then we must have
            // something like:
            //
            // return
            // if (...
            // parse out a missing name node for the expression, and keep on going
            var tk = this.Nodes.Current.Kind;
            if (SyntaxFacts.IsInvalidSubExpression(tk))
            {
                return this.Error(tk + " is not a valid sub expression", Nodes.Current);
            }


            if (tk == SyntaxKind.Expression)
            {
                leftOperand = WalkExpression();
            }


            // No left operand, so we need to parse one -- possibly preceded by a
            // unary operator.
            else if (SyntaxFacts.IsExpectedPrefixUnaryOperator(tk))
            {
                opKind = SyntaxFacts.GetPrefixUnaryExpression(tk);
                newPrecedence = SyntaxFacts.GetPrecedence(opKind);
                var opToken = this.Nodes.Consume();
                var operand = this.WalkSubExpression(newPrecedence);
                //if (SyntaxFacts.IsIncrementOrDecrementOperator(opToken.Kind))
                //{
                //    operand = CheckValidLvalue(operand);
                //}

                leftOperand =
                    AstNode.PrefixUnary(opToken, operand);
                // AstNode.Incrementor(operand);
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
                if (SyntaxFacts.IsExpectedBinaryOperator(tk) || SyntaxFacts.IsBinaryExpression(tk))
                {
                    if (SyntaxFacts.IsExpectedBinaryOperator(tk))
                        opKind = SyntaxFacts.GetBinaryExpression(tk);
                    else
                        opKind = tk;
                }
                else if (SyntaxFacts.IsExpectedAssignmentOperator(tk) || SyntaxFacts.IsAssignmentExpression(tk))
                {
                    if (SyntaxFacts.IsExpectedAssignmentOperator(tk))
                        opKind = SyntaxFacts.GetAssignmentExpression(tk);
                    else
                        opKind = tk;
                    isAssignmentOperator = true;
                }
                else
                {
                    if (Nodes.Current.Type == SyntaxKind.StatementTerminator)
                        Nodes.Consume();
                    break;
                }

                newPrecedence = SyntaxFacts.GetPrecedence(opKind);

                Debug.Assert(newPrecedence > 0);      // All binary operators must have precedence > 0!

                // check for >> or >>=
                bool doubleOp = false;
                //if (tk == SyntaxKind.GreaterEquals
                //    && (this.Nodes.PeekNext().Kind == SyntaxKind.Greater || this.Nodes.PeekNext().Kind == SyntaxKind.GreaterEquals))
                //{
                //    // check to see if they really are adjacent
                //    if (this.Nodes.Current.GetTrailingTriviaWidth() == 0 && this.Nodes.PeekNext().GetLeadingTriviaWidth() == 0)
                //    {
                //        if (this.Nodes.PeekNext().Kind == SyntaxKind.Greater)
                //        {
                //            opKind = SyntaxFacts.GetBinaryExpression(SyntaxKind.GreaterGreater);
                //        }
                //        else
                //        {
                //            opKind = SyntaxFacts.GetAssignmentExpression(SyntaxKind.GreaterGreaterEquals);
                //            isAssignmentOperator = true;
                //        }
                //        newPrecedence = SyntaxFacts.GetPrecedence(opKind);
                //        doubleOp = true;
                //    }
                //}

                // Check the precedence to see if we should "take" this operator
                if (newPrecedence < precedence)
                {
                    break;
                }

                // Same precedence, but not right-associative -- deal with this "later"
                if ((newPrecedence == precedence) && !SyntaxFacts.IsRightAssociative(opKind))
                {
                    break;
                }

                // Precedence is okay, so we'll "take" this operator.
                var opToken = Nodes.Consume(); // this.EatContextualToken(tk);
                //if (doubleOp)
                //{
                //    // combine tokens into a single token
                //    var opToken2 = this.EatToken();
                //    var kind = opToken2.Kind == SyntaxKind.GreaterThanToken ? SyntaxKind.GreaterThanGreaterThanToken : SyntaxKind.GreaterThanGreaterThanEqualsToken;
                //    opToken = SyntaxFactory.Token(opToken.GetLeadingTrivia(), kind, opToken2.GetTrailingTrivia());
                //}


                if (isAssignmentOperator)
                {
                    // leftOperand = CheckValidLvalue(leftOperand);
                    var rightOperand = this.WalkSubExpression(newPrecedence);
                    switch (opKind)
                    {
                        case SyntaxKind.Assign:
                            leftOperand = AstNode.Assign(leftOperand, rightOperand);
                            break;
                        case SyntaxKind.AssignAnd:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '&', rightOperand));
                            break;
                        case SyntaxKind.AssignOr:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '|', rightOperand));
                            break;
                        case SyntaxKind.AssignMultiply:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '*', rightOperand));
                            break;
                        case SyntaxKind.AssignDivide:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '/', rightOperand));
                            break;
                        case SyntaxKind.AssignMinus:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '-', rightOperand));
                            break;
                        case SyntaxKind.AssignPlus:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '+', rightOperand));
                            break;
                        case SyntaxKind.AssignLeftShift:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, "<<", rightOperand));
                            break;
                        case SyntaxKind.AssignRightShift:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, ">>", rightOperand));
                            break;
                        case SyntaxKind.AssignModulo:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '%', rightOperand));
                            break;
                    }

                    //  _syntaxFactory.AssignmentExpression(opKind, leftOperand, opToken,);
                }
                else
                {
                    leftOperand = AstNode.BinaryOperator((int)newPrecedence, leftOperand, opToken.StringValue, this.WalkSubExpression(newPrecedence));
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

            //if (tk == SyntaxKind.Question && precedence <= Precedence.Ternary)
            //{
            //    var questionToken = this.Nodes.Next();
            //    var colonLeft = this.ParseExpressionCore();
            //    var colon = this.Nodes.Next(); // SyntaxKind.Colon
            //    var colonRight = this.ParseExpressionCore();
            //    leftOperand = AstNode.ConditionalExpression(leftOperand, questionToken, colonLeft, colon, colonRight);
            //}

            return leftOperand;
        }


        private bool IsPossibleStatement()
        {
            var tk = this.Nodes.Current.Kind;
            switch (tk)
            {
                case SyntaxKind.KeywordBreak:
                case SyntaxKind.KeywordContinue:
                case SyntaxKind.KeywordDo:
                case SyntaxKind.KeywordFor:
                case SyntaxKind.KeywordForEach:
                case SyntaxKind.KeywordGoto:
                case SyntaxKind.KeywordLoop:
                case SyntaxKind.KeywordIf:
                case SyntaxKind.KeywordReturn:
                case SyntaxKind.KeywordSwitch:
                case SyntaxKind.KeywordWhile:
                case SyntaxKind.Scope:
                case SyntaxKind.Semicolon:
                    return true;

                case SyntaxKind.Identifier:
                    return true;

                // Accessibility modifiers are not legal in a statement,
                // but a common mistake for local functions. Parse to give a
                // better error message.

                default:
                    return SyntaxFacts.IsPredefinedType(tk)
                           || IsPossibleExpression();
            }
        }

        private bool IsPossibleExpression()
        {
            var tk = this.Nodes.Current.Kind;
            switch (tk)
            {

                case SyntaxKind.ArgList:
                case SyntaxKind.Constant:

                case SyntaxKind.Expression:
                case SyntaxKind.ArrayIndexExpression:
                case SyntaxKind.LiteralNumber:
                case SyntaxKind.LiteralString:
                case SyntaxKind.Literal:
                case SyntaxKind.KeywordNew:
                case SyntaxKind.KeywordTrue:
                case SyntaxKind.KeywordFalse:
                case SyntaxKind.KeywordNull:
                case SyntaxKind.ColonColon: // bad aliased name                
                    return true;
                case SyntaxKind.Identifier:
                    // Specifically allow the from contextual keyword, because it can always be the start of an
                    // expression (whether it is used as an identifier or a keyword).
                    return true;
                default:
                    return SyntaxFacts.IsExpectedPrefixUnaryOperator(tk)
                           || (SyntaxFacts.IsPredefinedType(tk) && tk != SyntaxKind.KeywordVoid)
                           || SyntaxFacts.IsAnyUnaryExpression(tk)
                           || SyntaxFacts.IsBinaryExpression(tk)
                           || SyntaxFacts.IsAssignmentExpressionOperatorToken(tk);
            }
        }

        private void AssertMinExpectedNodeCount(int count)
        {
            if (count > Nodes.Available)
                Error("Oh no! Unexpected end of script. We expected "
                                                    + count + " more nodes, but there are only " + Nodes.Available + " left.", Nodes.Current);
        }


        //private AstNode Error(string message)
        //{
        //    var msg = "[Error] " + message;
        //    this.errors.Add(msg);
        //    this.Nodes.Interrupted = true;
        //    return AstNode.Error(message);
        //}

        private AstNode Error(string message, SyntaxNode token = null)
        {
            var msg = "[Error] " + message;
            if (token != null)
            {
                if (token.TrailingToken != null)
                {
                    msg += ". At line " + token.TrailingToken.Line;
                }
            }
            var errorNode = AstNode.Error(message, token);

            this.errors.Add(msg);
            this.Nodes.Interrupted = true;
            this.errorNodes.Add(errorNode);
            return errorNode;
        }

        private bool EndOfStream()
        {
            return this.HasErrors || Nodes.EndOfStream();
        }

        private NodeStream Nodes => this.currentScope.Nodes;

        private void BeginScope(NodeStream scopeNodes)
        {
            this.currentScope = currentScope.BeginScope(scopeNodes);
        }

        private void EndScope(SyntaxKind endType)
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