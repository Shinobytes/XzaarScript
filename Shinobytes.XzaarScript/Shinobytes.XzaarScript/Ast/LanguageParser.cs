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
    public class LanguageParser
    {
        private readonly TokenStream tokens;

        private ParserScope currentScope;

        private readonly List<string> errors = new List<string>();
        private readonly List<ErrorNode> errorNodes = new List<ErrorNode>();

        private readonly List<LabelNode> labels = new List<LabelNode>();
        private readonly List<ParameterNode> currentParameters = new List<ParameterNode>();
        private readonly Dictionary<string, FunctionNode> definedFunctions = new Dictionary<string, FunctionNode>();
        private readonly Dictionary<string, StructNode> definedStructs = new Dictionary<string, StructNode>();
        private AstNode lastWalkedExpression;


        public LanguageParser(IList<SyntaxToken> tokens)
        {
            this.tokens = new TokenStream(tokens);
            this.currentScope = new ParserScope();
        }

        public AstNode Parse()
        {
            return new EntryNode(WalkAllNodes());
        }

        public bool HasErrors => errors.Count > 0;

        public IList<string> Errors => errors;

        private AstNode WalkAllNodes()
        {
            var nodes = new List<AstNode>();
            this.BeginScope(this.tokens);

            // var currentNode = this.Tokens.CurrentToken;
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
                    return Error(exc.Message, Tokens.Current);
                }
                // currentNode = this.Tokens.Next();
            }

            this.EndScope();
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

            var i = Tokens.Consume(n => n.Kind == SyntaxKind.Semicolon);

            if (i != null || Tokens.EndOfStream()) return null;

            var valueLower = CurrentToken.Value?.ToLower();

            switch (valueLower)
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
            }

            if (SyntaxFacts.IsPrefixUnaryExpression(CurrentToken.Kind)) return lastWalkedExpression = WalkUnaryOperator();
            if (SyntaxFacts.IsMath(CurrentToken.Kind)) return lastWalkedExpression = WalkArithmeticOperator();
            if (SyntaxFacts.IsAssignment(CurrentToken.Kind)) return lastWalkedExpression = WalkAssignmentOperator();
            switch (CurrentToken.Kind)
            {

                case SyntaxKind.Identifier:
                    {
                        if (Tokens.NextIs(SyntaxKind.Colon))
                            return lastWalkedExpression = WalkLabel();
                        return lastWalkedExpression = WalkExpressionStatement(); // return WalkIdentifier();
                    }

                case SyntaxKind.AggregateObjectIndex:
                    return lastWalkedExpression = WalkManyExpressions();

                //case SyntaxKind.ArrayIndexExpression:
                //    return WalkObjectIndex();

                //case SyntaxKind.UnaryOperator: return lastWalkedExpression = WalkUnaryOperator();
                //case SyntaxKind.LogicalConditionalOperator: return lastWalkedExpression = WalkLogicalConditionalOperator();
                case SyntaxKind.Number: return lastWalkedExpression = ParseSubExpressionCore(Precedence.Expression);
                case SyntaxKind.String: return lastWalkedExpression = ParseSubExpressionCore(Precedence.Expression);
                // case SyntaxKind.Expression: return WalkManyExpressions();
                case SyntaxKind.OpenCurly:
                    return lastWalkedExpression = WalkBody();
                default: return lastWalkedExpression = Error("Unexpected token type '" + Tokens.Current.Kind + "' found.", Tokens.Current);

            }
        }

        private AstNode WalkLabel()
        {
            var labelName = WalkIdentifier();
            var colon = Tokens.Consume(SyntaxKind.Colon);

            var label = AstNode.Label(labelName.ValueText);

            this.labels.Add(label);

            return label;
        }

        private AstNode WalkGoto()
        {
            if (Tokens.PeekNext().Kind == SyntaxKind.KeywordCase)
            {
                return Error("Goto case has not been implemented yet.", Tokens.Current);
            }
            var before = Tokens.Current;
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

            if (this.Tokens.Current != null && this.Tokens.Current.Kind == SyntaxKind.Semicolon)
            {
                this.Tokens.Consume();
            }

            return expression;

            //if (expression.Kind == SyntaxKind.EXPRESSION)
            //    return expression;

            //return AstNode.Expression(expression);


            // return _syntaxFactory.ExpressionStatement(expression, semicolon);
        }

        private AstNode WalkContinue()
        {
            try
            {
                Tokens.Consume(n => n.Kind == SyntaxKind.KeywordContinue);
                return AstNode.Continue();
            }
            finally
            {
                Tokens.Consume(SyntaxKind.Semicolon);
            }
        }

        private AstNode WalkBreak()
        {
            try
            {
                Tokens.Consume(n => n.Kind == SyntaxKind.KeywordBreak);
                return AstNode.Break();
            }
            finally
            {
                Tokens.Consume(SyntaxKind.Semicolon);
            }
        }

        private AstNode WalkKnownConstant()
        {
            var value = this.Tokens.Consume().Value;
            return AstNode.Identifier(value);
        }

        private AstNode WalkType()
        {
            var isArray = false;
            var current = Tokens.Consume();
            if (Tokens.Current != null)
            {
                if (Tokens.CurrentIs(x => SyntaxFacts.IsOpenIndexer(x.Kind)) && Tokens.NextIs(x => SyntaxFacts.IsCloseIndexer(x.Kind)))
                {
                    isArray = true;
                    Tokens.Advance(2);
                }
            }
            return AstNode.Identifier(current.Value + (isArray ? "[]" : ""));
        }

        private AstNode WalkReturn()
        {
            try
            {
                Tokens.Consume(n => n.Kind == SyntaxKind.KeywordReturn);
                var nextNode = Tokens.Current;
                if (nextNode != null && nextNode.Kind != SyntaxKind.KeywordCase)
                {
                    var result = WalkSubExpression(Precedence.Expression);
                    return AstNode.Return(result);
                }
                return AstNode.Return();
            }
            finally
            {
                Tokens.Consume(SyntaxKind.Semicolon);
            }
        }

        private AstNode WalkNumberLiteral()
        {
            return AstNode.NumberLiteral(Tokens.Consume(SyntaxKind.Number).Value);
        }

        private AstNode WalkStringLiteral()
        {
            return AstNode.StringLiteral(Tokens.Consume(SyntaxKind.String).Value);
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

        private ArgumentNode[] WalkArrayArgumentList()
        {
            return WalkArgumentList(SyntaxKind.OpenBracket, SyntaxKind.CloseBracket);
        }


        private ArgumentNode[] WalkArgumentList()
        {
            return WalkArgumentList(SyntaxKind.OpenParan, SyntaxKind.CloseParan);
        }

        private ArgumentNode[] WalkArgumentList(SyntaxKind open, SyntaxKind close)
        {
            var args = new List<ArgumentNode>();
            if (PrepareScope(open))
            {
                int index = 0;
                while (!this.EndOfStream())
                {
                    if (CurrentToken.Kind == close)
                        break;
                    if (!IsPossibleExpression() && !IsPossibleStatement())
                    {
                        var errNode = "";
                        if (Tokens.Current != null) errNode = Tokens.Current.Value;
                        Error("'" + errNode + "' is not a valid argument.", Tokens.Current);
                        return null;
                    }
                    var astNode = WalkSubExpression(Precedence.Expression);
                    if (astNode != null && astNode.Kind != SyntaxKind.Comma || Tokens.Current == null)
                        args.Add(AstNode.Argument(astNode, index++));

                    if (Tokens.Current != null && Tokens.Current.Kind == SyntaxKind.Comma)
                        Tokens.Consume(n => n.Kind == SyntaxKind.Comma);
                }
                EndScope(close);
            }
            return args.ToArray();
        }


        private AstNode WalkParameterList()
        {
            var args = new List<ParameterNode>();
            Tokens.ConsumeExpected(SyntaxKind.OpenParan);
            if (PrepareScope())
            {

                int index = 0;
                while (!this.EndOfStream())
                {
                    var before = this.Tokens.Current;

                    if (before.Kind == SyntaxKind.CloseParan)
                        break;

                    var possibleIdentifier = this.Tokens.PeekAt(1);
                    if (possibleIdentifier != null && possibleIdentifier.Kind == SyntaxKind.Colon)
                    {
                        // typescript/rust style
                        var name = WalkIdentifier();
                        var colon = Tokens.Consume(n => n.Kind == SyntaxKind.Colon);
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

                    //this.Tokens.Next(); 
                    if (before == this.Tokens.Current || (this.Tokens.Current != null && this.Tokens.Current.Kind == SyntaxKind.Comma)) Tokens.Consume();
                }
                EndScope(SyntaxKind.CloseParan);
            }
            return AstNode.Parameters(args.ToArray());
        }

        private bool PrepareScope(SyntaxKind scopeStart)
        {
            if (this.Tokens.Current != null)
            {
                Tokens.ConsumeExpected(scopeStart);

                BeginScope(Tokens);

                return true;
            }
            return false;
        }

        private bool PrepareScope()
        {
            if (this.Tokens.Current != null)
            {
                BeginScope(Tokens);

                return true;
            }
            return false;
        }

        private AstNode WalkScope()
        {
            var node = Tokens.Current;
            AstNode scopeNode = null;
            var nodes = new List<AstNode>();
            //if (node.HasChildren)
            //{
            //    BeginScope(new NodeStream(node.Children));

            //    while (!this.EndOfStream())
            //    {
            //        var astNode = Walk();
            //        if (astNode != null) nodes.Add(astNode);
            //        //this.Tokens.Next();
            //    }
            //    EndScope(SyntaxKind.Scope);
            //}


            return AstNode.Block(nodes.ToArray());
        }

        private AstNode WalkManyExpressions()
        {
            var node = Tokens.Current;
            AstNode scopeNode = null;
            var nodes = new List<AstNode>();

            if (PrepareScope())
            {
                while (!this.EndOfStream())
                {
                    var astNode = WalkExpressionCore();
                    if (astNode != null) nodes.Add(astNode);
                    //this.Tokens.Next();
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

        private AstNode WalkAssignmentOperator()
        {
            var assign = Tokens.Consume(n => SyntaxFacts.IsAssignment(n.Kind));
            // go back once if we are currently on the assignment
            // if (SyntaxFacts.IsAssignment(Tokens.CurrentToken.Kind) || SyntaxFacts.IsExpectedAssignmentOperator(Tokens.CurrentToken.Kind)) Tokens.Previous();

            return WalkExpressionCore();
        }

        private AstNode WalkNewArrayInstance()
        {
            var old = Tokens.Current;
            if (SyntaxFacts.IsOpenIndexer(Tokens.Current.Kind))
            {
                // array initializer
                var args = WalkArrayArgumentList();

                if (Tokens.Current == old) Tokens.Consume(x => SyntaxFacts.IsCloseIndexer(x.Kind));

                // Tokens.Consume(i => i.Kind == SyntaxKind.ArrayIndexExpression);

                return AstNode.NewArrayInstance(args);
            }
            return Error("Unexpected type '" + Tokens.Current?.Kind + "' found. We expected a new array 'a = []' expression.", Tokens.Current);
        }

        private AstNode WalkLogicalConditionalOperator()
        {
            // I was hoping that we never needed this one.
            // but it cannot be helped... 

            var leftSide = lastWalkedExpression;

            if (leftSide == null)
                return Error("We were unable to build the logical expression. Perhaps it is a bit too complex? Try and simplify it and avoid using paranthesis if possible!", Tokens.Current);

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

        private AstNode WalkArithmeticOperator()
        {

            return Error("Unexpected arithmetic operator found.", CurrentToken);

            AssertMinExpectedNodeCount(1);
            // get previous
            // op
            // get next

            return WalkManyExpressions();
        }

        //private AstNode WalkObjectIndex()
        //{
        //    // if previous is assignment, it is an array initializer            
        //    // otherwise a normal object index

        //    var item = this.Tokens.Previous();

        //    if (item == null && this.Tokens.CurrentToken != null && this.Tokens.CurrentToken.Kind == SyntaxKind.ArrayIndexExpression)
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

            var expr = this.WalkSubExpression(Precedence.Expression) as UnaryNode;
            if (expr == null) return Error("Unable to parse unary expression. Unknown error");
            if (expr.Item is UnaryNode unaryChild)
            {
                var unary1 = expr.IsPostUnary ? unaryChild.Item + expr.Operator : expr.Operator + unaryChild.Item;
                var unary2 = !unaryChild.IsPostUnary ? CurrentToken.Value + unaryChild.Operator : unaryChild.Operator + CurrentToken.Value;
                return Error("Invalid unary expression found. The operand of an increment or decrement operator must be a variable, property or indexer. " +
                             "Did you forget to separate the expression using a semicolon? \r\nExample: '" + expr + "' could be '" + unary1 + "; " + unary2 + "'");
            }

            return expr;
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

            var @struct = Tokens.Consume(n => n.Kind == SyntaxKind.KeywordStruct);

            var name = WalkIdentifier();

            tokens.ConsumeExpected(SyntaxKind.OpenCurly);


            var fields = WalkStructFields(name.ValueText);

            var str = AstNode.Struct(name.ValueText, fields);

            this.definedStructs.Add(name.ValueText, str);

            tokens.ConsumeExpected(SyntaxKind.CloseCurly);

            return str;
        }

        private AstNode[] WalkStructFields(string declaringTypeName)
        {
            var fields = new List<AstNode>();
            if (PrepareScope())
            {
                while (!EndOfStream())
                {
                    var possibleIdentifier = Tokens.PeekNext();

                    if (tokens.Current.Kind == SyntaxKind.CloseCurly)
                    {
                        break;
                    }

                    if (possibleIdentifier != null && possibleIdentifier.Kind == SyntaxKind.Colon)
                    {
                        // tsstyle/rust-style
                        // name : type
                        var name = WalkIdentifier();
                        var colon = Tokens.Consume();
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

                    if (Tokens.Current != null)
                    {
                        if (Tokens.Current.Kind == SyntaxKind.Comma ||
                            Tokens.Current.Kind == SyntaxKind.Semicolon)
                            Tokens.Consume();
                        else if (tokens.Current.Kind == SyntaxKind.CloseCurly)
                        {
                            break;
                        }
                    }
                }

                EndScope();
            }

            return fields.ToArray();
        }

        private AstNode[] WalkStructInitializer()
        {
            // similar to WalkStructFields, but here we expect to assign the values of the target struct and separate the expressions by a comma
            var fieldAssignments = new List<AstNode>();
            if (PrepareScope(SyntaxKind.OpenCurly))
            {
                while (!EndOfStream())
                {
                    if (SyntaxFacts.IsCloseBody(CurrentToken.Kind))
                        break;




                    var assignment = WalkAssignmentOperator();

                    // throw new NotImplementedException();


                    // var assignment = Walk();
                    //if (assignment.Kind != SyntaxKind.Equals)
                    //{
                    //    return new[] { Error("You're suppose to assign the values here", Tokens.Current) };
                    //}


                    fieldAssignments.Add(assignment);

                    Tokens.Consume(SyntaxKind.Comma);
                }

                EndScope(SyntaxKind.CloseCurly);
            }

            return fieldAssignments.ToArray();
        }

        private AstNode WalkSwitchCase()
        {
            Debug.Assert(this.IsPossibleSwitchCase());

            // case (expr) : { body }
            // case (expr) : { body } break|continue|return


            if (Tokens.Current.Kind == SyntaxKind.KeywordCase)
            {
                AssertMinExpectedNodeCount(4);

                var @case = Tokens.Consume(SyntaxKind.KeywordCase);
                Debug.Assert(@case != null);
                var test = WalkExpressionStatement();
                var colon = Tokens.Consume(SyntaxKind.Colon);
                var switchCaseBody = WalkSwitchCaseBody();
                Debug.Assert(switchCaseBody != null);

                return AstNode.Case(test, switchCaseBody);
            }
            else
            {
                AssertMinExpectedNodeCount(3);

                var defaultCase = Tokens.Consume(SyntaxKind.KeywordDefault);
                Debug.Assert(defaultCase != null);
                var colon = Tokens.Consume(SyntaxKind.Colon);
                var switchCaseBody = WalkSwitchCaseBody();
                Debug.Assert(switchCaseBody != null);

                return AstNode.DefaultCase(switchCaseBody);
            }



            throw new System.NotImplementedException();
        }

        private AstNode WalkSwitchCaseBody()
        {
            if (Tokens.Current != null && Tokens.Current.Kind == SyntaxKind.KeywordCase)
                return Error("Multiple switch case labels are not supported at this time! Please make sure you add a 'break' or 'return' after declaring your switch case!", Tokens.Current);

            if (Tokens.Current == null)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Tokens.Current);


            AstNode result = null;

            var childStatements = new List<AstNode>();

            if (Tokens.Current != null)
            {
                if (SyntaxFacts.IsOpenBody(Tokens.Current.Kind))
                {
                    var scope = Tokens.Current;
                    if (PrepareScope(SyntaxKind.OpenCurly))
                    {
                        while (!EndOfStream())
                        {
                            if (Tokens.Current.Kind == SyntaxKind.CloseCurly) break;
                            if (Tokens.Current.Kind == SyntaxKind.KeywordReturn ||
                            Tokens.Current.Kind == SyntaxKind.KeywordBreak ||
                            Tokens.Current.Kind == SyntaxKind.KeywordContinue)
                            {
                                childStatements.Add(Walk());
                                break;
                            }
                            var stmt = WalkExpressionStatement();
                            if (stmt != null)
                                childStatements.Add(stmt);
                        }

                        EndScope(SyntaxKind.CloseCurly);
                    }
                }
                else
                {
                    while (Tokens.Current != null)
                    {
                        if (Tokens.Current.Kind == SyntaxKind.KeywordReturn ||
                            Tokens.Current.Kind == SyntaxKind.KeywordBreak ||
                            Tokens.Current.Kind == SyntaxKind.KeywordContinue)
                        {
                            var breakStmt = Walk();
                            if (breakStmt != null) childStatements.Add(breakStmt);
                            break;
                        }
                        var stmt = WalkExpressionStatement();
                        if (stmt != null) childStatements.Add(stmt);
                    }
                }

                if (Tokens.Current != null && (Tokens.Current.Kind == SyntaxKind.KeywordReturn ||
                    Tokens.Current.Kind == SyntaxKind.KeywordBreak ||
                    Tokens.Current.Kind == SyntaxKind.KeywordContinue))
                {
                    var breakStmt = Walk();
                    if (breakStmt != null)
                        childStatements.Add(breakStmt);
                }

                result = AstNode.Block(childStatements.ToArray());
                // result
            }

            if (Tokens.Current != null && (Tokens.Current.Kind != SyntaxKind.KeywordReturn &&
                Tokens.Current.Kind != SyntaxKind.KeywordBreak &&
                Tokens.Current.Kind != SyntaxKind.KeywordContinue))
            {
                if (childStatements.Count > 0)
                {
                    var lastChildStatement = childStatements[childStatements.Count - 1];
                    if (lastChildStatement.Kind == SyntaxKind.KeywordReturn || lastChildStatement.Kind == SyntaxKind.KeywordBreak || lastChildStatement.Kind == SyntaxKind.KeywordContinue)
                        return result;
                }
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Tokens.Current);
            }

            if (Tokens.Current == null && childStatements.Count == 0)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Tokens.Current);

            var child = childStatements[childStatements.Count - 1];
            if (child.Kind != SyntaxKind.KeywordReturn && child.Kind != SyntaxKind.KeywordBreak && child.Kind != SyntaxKind.KeywordContinue)
                return Error("Missing expected end of switch case, please add a 'break' or 'return' after declaring your switch case!", Tokens.Current);

            return result;
        }

        private AstNode WalkSwitch()
        {
            AssertMinExpectedNodeCount(3);
            // switch (expr) { body }

            var @switch = this.Tokens.Consume(SyntaxKind.KeywordSwitch);

            Tokens.ConsumeExpected(SyntaxKind.OpenParan);

            var expr = this.WalkExpressionCore();

            Tokens.ConsumeExpected(SyntaxKind.CloseParan);

            var switchCases = new List<CaseNode>();

            if (PrepareScope(SyntaxKind.OpenCurly))
            {

                while (this.IsPossibleSwitchCase())
                {
                    var swcase = this.WalkSwitchCase() as CaseNode;
                    if (swcase != null)
                        switchCases.Add(swcase);
                }
                EndScope(SyntaxKind.CloseCurly);
            }

            return AstNode.Switch(expr, switchCases.ToArray());
        }

        private bool IsPossibleSwitchCase()
        {
            return this.Tokens.Current != null && ((this.Tokens.Current.Kind == SyntaxKind.KeywordCase) || (this.Tokens.Current.Kind == SyntaxKind.KeywordDefault));
        }

        private AstNode WalkForeachLoop()
        {
            AssertMinExpectedNodeCount(3);
            // foreach (expr) { body }

            var @foreach = this.Tokens.Consume(SyntaxKind.KeywordForEach);

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

            var @for = this.Tokens.Consume(SyntaxKind.KeywordFor);



            var exprList = this.WalkForStatementExpressionList();

            var body = this.WalkStatementOrBody();

            return AstNode.For(exprList[0], exprList[1], exprList[2], body);
        }

        private AstNode WalkForStatementExpressionList()
        {
            return WalkStatementExpressionList(SyntaxKind.OpenParan, SyntaxKind.Semicolon, SyntaxKind.CloseParan, 3);
        }

        private AstNode WalkForEachStatementExpressionList()
        {
            return WalkStatementExpressionList(SyntaxKind.OpenParan, SyntaxKind.KeywordIn, SyntaxKind.CloseParan, 2);
        }


        private AstNode WalkStatementExpressionList(SyntaxKind open, SyntaxKind separator, SyntaxKind close, int expectedExpressionCount)
        {
            this.Tokens.ConsumeExpected(open);
            try
            {
                var exprList = new List<AstNode>();

                var expr = this.Walk();
                if (expr != null) exprList.Add(expr);

                while (!EndOfStream())
                {
                    if (CurrentToken.Kind == close)
                        break;

                    if (CurrentToken.Kind != separator)
                    {
                        var tokenBefore = CurrentToken;
                        var expr2 = this.WalkExpressionCore();
                        if (expr2 != null) exprList.Add(expr2);
                        if (CurrentToken == tokenBefore)
                        {
                            return Error("Invalid expression", tokenBefore);
                        }
                    }

                    Tokens.Consume(separator);

                    if (CurrentToken.Kind == close)
                        break;
                }

                //if (!forExprHolder.HasChildren)
                //    return Error("Well, this is awkward. Are you sure that you know how to declare a for statement?", Tokens.CurrentToken);

                //var expressions = SplitExpression(forExprHolder, separator);

                //var exprList = new List<AstNode>();

                //// 0: variable declaration/instantiation
                //// 1: conditional test
                //// 2: incrementor
                //for (var i = 0; i < expressions.Count; i++)
                //{
                //    if (PrepareScope(expressions[i]))
                //    {

                //        var before = this.Tokens.CurrentToken;
                //        var astNode = Walk();
                //        if (astNode != null) exprList.Add(astNode);
                //        if (before == this.Tokens.CurrentToken) Tokens.Consume();

                //        EndScope(SyntaxKind.Expression);
                //    }
                //}

                //return AstNode.Expression(exprList.ToArray());

                return AstNode.Expression(exprList.ToArray());
            }
            finally
            {
                this.Tokens.ConsumeExpected(close);
            }
        }

        private AstNode WalkDoWhileLoop()
        {
            AssertMinExpectedNodeCount(4);
            // do { body } while (expr)

            AstNode result = null;

            var @do = this.Tokens.Consume(SyntaxKind.KeywordDo);

            var body = this.WalkStatementOrBody();

            var @while = this.Tokens.Consume(SyntaxKind.KeywordWhile);

            Tokens.ConsumeExpected(x => SyntaxFacts.IsOpenStatement(x.Kind));

            var condition = this.WalkExpressionCore();

            Tokens.ConsumeExpected(x => SyntaxFacts.IsCloseStatement(x.Kind));

            result = AstNode.DoWhile(condition, body);

            // Tokens.Consume();

            return result;
        }

        private AstNode WalkWhileLoop()
        {
            AssertMinExpectedNodeCount(3);
            // while (expr) { body }

            AstNode result = null;

            var @while = this.Tokens.Consume(SyntaxKind.KeywordWhile);


            Tokens.ConsumeExpected(x => SyntaxFacts.IsOpenStatement(x.Kind));

            var condition = this.WalkExpressionCore();

            Tokens.ConsumeExpected(x => SyntaxFacts.IsCloseStatement(x.Kind));


            var body = this.WalkStatementOrBody();

            result = AstNode.While(condition, body);

            // Tokens.Consume();

            return result;
        }


        private AstNode WalkLoop()
        {
            AssertMinExpectedNodeCount(2);
            // loop { body }

            var loop = this.Tokens.Consume(SyntaxKind.KeywordLoop);
            var body = this.WalkStatementOrBody();
            return AstNode.Loop(body);
        }


        private AstNode WalkIf()
        {
            AssertMinExpectedNodeCount(3);

            // if (expr) { body }         
            AstNode result = null;

            var @if = this.Tokens.Consume(SyntaxKind.KeywordIf);

            Tokens.ConsumeExpected(x => SyntaxFacts.IsOpenStatement(x.Kind));

            var condition = this.WalkExpressionCore(); // this.ParseExpressionCore();

            Tokens.ConsumeExpected(x => SyntaxFacts.IsCloseStatement(x.Kind));

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
            if (this.Tokens.Current == null || this.Tokens.Current.Kind != SyntaxKind.KeywordElse)
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

            var @else = this.Tokens.Consume(SyntaxKind.KeywordElse);

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
            var letVar = Tokens.Consume(SyntaxKind.KeywordVar);
            var name = WalkIdentifier();

            if (Tokens.Current != null && Tokens.Current.Kind == SyntaxKind.Colon)
            {
                var colon = Tokens.Consume();
                varTypeExplicit = true;
                varType = WalkType().ValueText;
            }

            var isAssign = Tokens.Current != null && SyntaxFacts.IsAssignment(Tokens.Current.Kind);
            if (isAssign)
            {
                var assign = Tokens.Consume();
                var before = Tokens.Current;
                AstNode valueAssignment = null;

                valueAssignment = WalkAssignmentOperator();

                if (before == Tokens.Current) Tokens.Consume();

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
            // Tokens.Consume();
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

            var fn = Tokens.Consume(SyntaxKind.KeywordFn);

            var name = WalkIdentifier();

            var parameterList = WalkParameterList();

            var parameters = AstNode.Parameters(parameterList);

            currentParameters.AddRange(parameters.Parameters);

            var functionName = name.ValueText;
            if (Tokens.Current.Kind == SyntaxKind.MinusGreater || Tokens.Current.Kind == SyntaxKind.Colon)
            {
                AssertMinExpectedNodeCount(3);
                var access = Tokens.Consume();

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
                if (definedFunctions.ContainsKey(functionName)) return Error($"A function with the same name '{function.Name}' already exists", Tokens.Current);
                definedFunctions.Add(functionName, function);

                var body = WalkBody(functionName);
                if (body != null) function.SetBody(body);
                return function;
            }
        }

        private AstNode WalkStatementOrBody()
        {
            var stmts = new List<AstNode>();
            if (this.Tokens.Current != null && SyntaxFacts.IsOpenBody(this.Tokens.Current.Kind))
            {
                return WalkBody();
            }
            else
            {
                var statement = WalkStatement();
                if (statement != null) stmts.Add(statement);

            }
            return AstNode.Body(stmts.ToArray());
        }

        private AstNode WalkBody(string name = null)
        {
            if (Tokens.Current == null || !SyntaxFacts.IsOpenBody(CurrentToken.Kind))
            {
                if (!string.IsNullOrEmpty(name))
                    return Error("The function '" + name + "' is clearly missing a body!!", Tokens.Current);
                return Error("We expected a new scope or body to be declared.. But it wasnt?", Tokens.Current);
            }

            var stmts = new List<AstNode>();
            if (PrepareScope(SyntaxKind.OpenCurly))
            {
                while (!this.EndOfStream())
                {
                    Tokens.Consume(SyntaxKind.Semicolon);

                    if (SyntaxFacts.IsCloseBody(CurrentToken.Kind))
                        break;

                    var statement = WalkStatement();
                    if (statement != null)
                        stmts.Add(statement);

                    //var astNode = WalkSubExpression(Precedence.Expression);
                    //if (astNode != null && astNode.Kind != SyntaxKind.SEPARATOR || Tokens.CurrentToken == null)
                    //    args.Add(AstNode.Argument(astNode, index++));

                    //if (Tokens.CurrentToken != null && Tokens.CurrentToken.Kind == SyntaxKind.Separator)
                    //    Tokens.Consume(n => n.Kind == SyntaxKind.Separator);
                }
                EndScope(SyntaxKind.CloseCurly);
            }
            return AstNode.Body(stmts.ToArray());
        }

        private AstNode WalkIdentifier()
        {
            AstNode item = null;

            if (Tokens.Current.Kind != SyntaxKind.Identifier
                && Tokens.PeekNext() != null && Tokens.PeekNext().Kind == SyntaxKind.Identifier)
                Tokens.Consume();

            var identifier = Tokens.Consume(SyntaxKind.Identifier);
            if (identifier == null)
                return Error("Identifier expected", Tokens.Current);

            var stringValue = identifier.Value;


            if (definedStructs.ContainsKey(stringValue))
            {
                var structDefinition = definedStructs[stringValue];
                if (Tokens.CurrentIs(n => SyntaxFacts.IsOpenBody(n.Kind)))
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
            var before = this.Tokens.Current;
            var isExpression = before.Kind == SyntaxKind.Expression;
            if (isExpression)
            {
                subExpr = this.WalkManyExpressions();
            }
            else
            {
                subExpr = this.WalkSubExpression(Precedence.Expression);
            }

            //if (isExpression && subExpr.Kind != SyntaxKind.EXPRESSION)
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

            var tk = this.Tokens.Current.Kind;
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
                case SyntaxKind.OpenParan:
                    expr = WalkCastOrParenExpressionOrLambdaOrTuple(precedence);
                    break;
                case SyntaxKind.Number:
                    expr = WalkNumberLiteral();
                    break;
                case SyntaxKind.String:
                    expr = WalkStringLiteral();
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

        private AstNode WalkCastOrParenExpressionOrLambdaOrTuple(Precedence precedence)
        {
            var openParen = Tokens.ConsumeExpected(SyntaxKind.OpenParan);
            var expression = WalkSubExpression(Precedence.Expression);
            Tokens.ConsumeExpected(SyntaxKind.CloseParan);
            return expression;
        }

        private AstNode WalkPostFixExpression(AstNode expr)
        {
            // Debug.Assert(expr != null);
            if (expr == null && this.Tokens.Current != null)
            {
                if (this.Tokens.Current.Kind == SyntaxKind.OpenBracket)
                {
                    expr = WalkNewArrayInstance();
                    // array initializer, no need for expr to have a value.
                }
            }
            if (expr == null) return null;

            while (!this.HasErrors)
            {
                if (this.Tokens.Current == null) return expr;
                SyntaxKind tk = this.Tokens.Current.Kind;
                switch (tk)
                {
                    case SyntaxKind.OpenParan:
                        expr = AstNode.Call(expr, this.WalkArgumentList());
                        break;

                    case SyntaxKind.OpenBracket:
                    case SyntaxKind.AggregateObjectIndex:
                        {
                            var walkArrayArgumentList = WalkArrayArgumentList();
                            if (walkArrayArgumentList == null) return errorNodes.LastOrDefault();
                            expr = AstNode.MemberAccess(expr, walkArrayArgumentList.FirstOrDefault()); // we only support one item for now                                                    
                        }
                        break;
                    case SyntaxKind.UnaryIncrement:
                    case SyntaxKind.UnaryDecrement:
                    case SyntaxKind.PostfixDecrement:
                    case SyntaxKind.PostfixIncrement:
                    case SyntaxKind.PlusPlus:
                    case SyntaxKind.MinusMinus:

                        var type = SyntaxFacts.GetPostfixUnaryExpression(tk);

                        expr = AstNode.PostfixUnary(Tokens.Consume(), expr, type);
                        // expr = _syntaxFactory.PostfixUnaryExpression(SyntaxFacts.GetPostfixUnaryExpression(tk), expr, this.EatToken());
                        break;


                    case SyntaxKind.MinusGreater:
                    case SyntaxKind.ColonColon:
                    case SyntaxKind.MemberAccess:
                    case SyntaxKind.Dot:
                        var identifier = WalkIdentifier();
                        expr = AstNode.MemberAccessChain(expr, AstNode.MemberAccess(identifier, expr.Type, FindMemberType(expr.ValueText, expr.Type, identifier)));
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
            var token = this.Tokens.Consume();
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
            var tk = this.Tokens.Current.Kind;
            if (SyntaxFacts.IsInvalidSubExpression(tk))
            {
                return this.Error(tk + " is not a valid sub expression", Tokens.Current);
            }


            if (tk == SyntaxKind.Expression)
            {
                leftOperand = WalkManyExpressions();
            }


            // No left operand, so we need to parse one -- possibly preceded by a
            // unary operator.
            else if (SyntaxFacts.IsExpectedPrefixUnaryOperator(tk))
            {
                opKind = SyntaxFacts.GetPrefixUnaryExpression(tk);
                newPrecedence = SyntaxFacts.GetPrecedence(opKind);
                var opToken = this.Tokens.Consume();
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

                if (CurrentToken == null)
                    break;

                tk = CurrentToken.Kind;
                bool isAssignmentOperator = false;


                if (SyntaxFacts.IsExpectedBinaryOperator(tk) || SyntaxFacts.IsBinaryExpression(tk))
                {
                    if (SyntaxFacts.IsExpectedBinaryOperator(tk))
                        opKind = SyntaxFacts.GetBinaryExpression(tk);
                    else
                        opKind = tk;
                }
                else if (SyntaxFacts.IsExpectedAssignmentOperator(tk) || SyntaxFacts.IsAssignment(tk))
                {
                    //if (SyntaxFacts.IsExpectedAssignmentOperator(tk))
                    //    opKind = SyntaxFacts.GetAssignmentExpression(tk);
                    //else
                    opKind = tk;
                    isAssignmentOperator = true;
                }
                else
                {
                    Tokens.Consume(SyntaxKind.Semicolon);
                    break;
                }

                newPrecedence = SyntaxFacts.GetPrecedence(opKind);

                Debug.Assert(newPrecedence > 0);      // All binary operators must have precedence > 0!

                // check for >> or >>=
                bool doubleOp = false;
                if (tk == SyntaxKind.GreaterEquals
                    && (this.Tokens.PeekNext().Kind == SyntaxKind.Greater || this.Tokens.PeekNext().Kind == SyntaxKind.GreaterEquals))
                {

                }

                //if (tk == SyntaxKind.GreaterEquals
                //    && (this.Tokens.PeekNext().Kind == SyntaxKind.Greater || this.Tokens.PeekNext().Kind == SyntaxKind.GreaterEquals))
                //{
                //    // check to see if they really are adjacent
                //    if (this.Tokens.CurrentToken.GetTrailingTriviaWidth() == 0 && this.Tokens.PeekNext().GetLeadingTriviaWidth() == 0)
                //    {
                //        if (this.Tokens.PeekNext().Kind == SyntaxKind.Greater)
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
                var opToken = Tokens.Consume(); // this.EatContextualToken(tk);
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
                        case SyntaxKind.Equals:
                            leftOperand = AstNode.Assign(leftOperand, rightOperand);
                            break;
                        case SyntaxKind.AndEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '&', rightOperand));
                            break;
                        case SyntaxKind.OrEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '|', rightOperand));
                            break;
                        case SyntaxKind.MultiplyEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '*', rightOperand));
                            break;
                        case SyntaxKind.DivideEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '/', rightOperand));
                            break;
                        case SyntaxKind.MinusEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '-', rightOperand));
                            break;
                        case SyntaxKind.PlusEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '+', rightOperand));
                            break;
                        case SyntaxKind.LessLessEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, "<<", rightOperand));
                            break;
                        case SyntaxKind.GreaterGreaterEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, ">>", rightOperand));
                            break;
                        case SyntaxKind.ModuloEquals:
                            leftOperand = AstNode.Assign(leftOperand, AstNode.BinaryOperator((int)newPrecedence, leftOperand, '%', rightOperand));
                            break;
                    }

                    //  _syntaxFactory.AssignmentExpression(opKind, leftOperand, opToken,);
                }
                else
                {
                    leftOperand = AstNode.BinaryOperator((int)newPrecedence, leftOperand, opToken.Value, this.WalkSubExpression(newPrecedence));
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

            if (tk == SyntaxKind.Question && precedence <= Precedence.Ternary)
            {
                var questionToken = this.Tokens.ConsumeExpected(SyntaxKind.Question);
                var colonLeft = this.WalkExpressionCore();
                var colon = this.Tokens.ConsumeExpected(SyntaxKind.Colon); // SyntaxKind.Colon
                var colonRight = this.WalkExpressionCore();
                leftOperand = AstNode.Conditional(leftOperand, questionToken, colonLeft, colon, colonRight);
            }

            return leftOperand;
        }


        private bool IsPossibleStatement()
        {
            var tk = this.Tokens.Current.Kind;
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
                case SyntaxKind.OpenParan:
                case SyntaxKind.OpenBracket:
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
            var tk = this.Tokens.Current.Kind;
            switch (tk)
            {
                case SyntaxKind.ArgList:
                case SyntaxKind.Expression:
                case SyntaxKind.ArrayIndexExpression:
                case SyntaxKind.Number:
                case SyntaxKind.String:
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
                           || SyntaxFacts.IsAssignment(tk);
            }
        }

        private void AssertMinExpectedNodeCount(int count)
        {
            if (count > Tokens.Available)
                Error("Oh no! Unexpected end of script. We expected "
                                                    + count + " more nodes, but there are only " + Tokens.Available + " left.", Tokens.Current);
        }


        //private AstNode Error(string message)
        //{
        //    var msg = "[Error] " + message;
        //    this.errors.Add(msg);
        //    this.Tokens.Interrupted = true;
        //    return AstNode.Error(message);
        //}

        private AstNode Error(string message, SyntaxToken token)
        {
            var msg = "[Error] " + message;
            if (token != null)
            {
                msg += ". At line " + token.SourceLine;
            }

            var errorNode = AstNode.Error(message);
            this.errors.Add(msg);
            this.Tokens.Interrupted = true;
            this.errorNodes.Add(errorNode);
            return errorNode;
        }

        private AstNode Error(string message)
        {
            var msg = "[Error] " + message;
            var token = this.CurrentToken ?? this.Tokens.PeekPrevious();
            if (token != null)
            {
                msg += ". At line " + token.SourceLine;
            }

            var errorNode = AstNode.Error(message);
            this.errors.Add(msg);
            this.Tokens.Interrupted = true;
            this.errorNodes.Add(errorNode);
            return errorNode;
        }

        private SyntaxToken CurrentToken => Tokens.Current;

        private bool EndOfStream()
        {
            return this.HasErrors || Tokens.EndOfStream();
        }

        private TokenStream Tokens => this.currentScope.Tokens;

        private void BeginScope(TokenStream tokens)
        {
            this.currentScope = currentScope.BeginScope(tokens);
        }

        private void EndScope()
        {
            this.currentScope = currentScope.EndScope();
        }

        private void EndScope(SyntaxKind endType)
        {
            this.currentScope = currentScope.EndScope();

            if (tokens.Current == null)
            {
                throw new ParserException("Unexpected end of scope");
            }
            if (Tokens.Current != null)
            {
                Tokens.ConsumeExpected(n => n.Kind == endType);
            }

            // Debug.Assert(node != null);
        }
    }
}