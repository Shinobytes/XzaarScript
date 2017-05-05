using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast;

namespace Shinobytes.XzaarScript.Parser.Nodes
{
    public class ErrorNode : AstNode
    {
        public SyntaxNode Token { get; set; }

        public ErrorNode(object value, int nodeIndex)
            : base(NodeTypes.ERROR, "ERROR", value, nodeIndex)
        {
        }

        public ErrorNode(object value, SyntaxNode token, int nodeIndex)
            : base(NodeTypes.ERROR, "ERROR", value, nodeIndex)
        {
            Token = token;
        }

        public override void Accept(INodeVisitor nodeVisitor)
        {
        }

        public override bool IsEmpty()
        {
            return true;
        }
    }

    public abstract class AstNode
    {
        private readonly int nodeIndex;
        private List<AstNode> children;

        protected AstNode(NodeTypes nodeType, string nodeName, object value, int nodeIndex)
        {
            this.nodeIndex = nodeIndex;
            children = new List<AstNode>();
            NodeType = nodeType;
            NodeName = nodeName;
            Value = value;
        }


        public string Type { get; set; }

        public bool IsTransformed { get; set; }

        public bool Walked { get; set; }

        public int Index => nodeIndex;

        public abstract void Accept(INodeVisitor nodeVisitor);

        public NodeTypes NodeType { get; }

        public string NodeName { get; }

        public object Value { get; protected set; }

        public string ValueText => Value + "";

        public AstNode Parent { get; set; }

        public IList<AstNode> Children => children;

        public int OperatingOrder { get; protected set; }

        public AstNode this[int childIndex]
        {
            get { return children[childIndex]; }
            set { children[childIndex] = value; }
        }
        public void SortChildren()
        {
            this.children = Children.OrderBy(c => c.Index).ToList();
        }

        public void InsertChild(int index, AstNode node)
        {
            if (node.Parent != null)
                node.Parent.RemoveChild(node);

            node.Parent = this;

            var n = node as DefineVariableNode;
            if (n != null)
            {
                n.Declare();
            }


            this.children.Insert(index, node);
        }

        public void AddChild(AstNode node)
        {
            // normally i would like to remove it from their previous parent, but since we mutate the state of the nodes rather than cloning them
            // we need to make sure that we keep our "children" from all nodes so when we walk the node tree we can expect the original parental node
            // keeps their children
            //if (node.Parent != null)
            //    node.Parent.RemoveChild(node);

            node.Parent = this;
            this.children.Add(node);
        }

        public void AddChildren(IEnumerable<AstNode> nodes)
        {
            foreach (var node in nodes)
            {
                AddChild(node);
            }
        }

        public void RemoveChild(AstNode node)
        {
            node.Parent = null;
            this.children.Remove(node);
        }

        public override string ToString()
        {
            return NodeType + " " + NodeName + " " + Value;
        }

        public abstract bool IsEmpty();
        //=>
        //    this is EmptyNode || this.NodeName == null && this.Value == null &&
        //    (Children == null || Children.Count == 0);

        #region Factory

        private static int _nodeIndex = 0;

        public static AstNode Empty()
        {
            return new EmptyNode(_nodeIndex++) { IsTransformed = true };
        }

        public static ErrorNode Error(string message)
        {
            return new ErrorNode(message, _nodeIndex++);
        }

        public static ErrorNode Error(string message, SyntaxNode token)
        {
            return new ErrorNode(message, token, _nodeIndex++);
        }

        public static ExpressionNode Expression(params AstNode[] children)
        {
            var block = new ExpressionNode(_nodeIndex++) { IsTransformed = true };
            if (children != null && children.Length > 0)
                block.AddChildren(children);
            return block;
        }

        public static BlockNode Block(params AstNode[] children)
        {
            var block = new BlockNode(_nodeIndex++) { IsTransformed = true };
            if (children != null && children.Length > 0)
                block.AddChildren(children);
            return block;
        }

        public static BodyNode Body(params AstNode[] children)
        {
            var block = new BodyNode(_nodeIndex++) { IsTransformed = true };
            if (children != null && children.Length > 0)
                block.AddChildren(children);
            return block;
        }

        public static AstNode Identifier(string name)
        {
            return new LiteralNode("NAME", name, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode StringLiteral(string value)
        {
            return new LiteralNode("STRING", value, _nodeIndex++) { IsTransformed = true };
        }
        public static AstNode NumberLiteral(object number)
        {
            return new LiteralNode("NUMBER", number, _nodeIndex++) { IsTransformed = true };
        }

        public static FunctionNode Function(string name, FunctionParametersNode argumentsExpression, AstNode bodyExpression)
        {
            return new FunctionNode(name, argumentsExpression, bodyExpression, _nodeIndex++) { IsTransformed = true };
        }

        public static FunctionNode Function(string name, FunctionParametersNode argumentsExpression)
        {
            return new FunctionNode(name, argumentsExpression, null, _nodeIndex++) { IsTransformed = true };
        }

        public static FunctionNode Function(string name, string returnType, FunctionParametersNode argumentsExpression)
        {
            return new FunctionNode(name, XzaarType.GetType(returnType), argumentsExpression, null, _nodeIndex++) { IsTransformed = true };
        }


        public static FunctionNode Function(string name, string returnType, FunctionParametersNode argumentsExpression, AstNode bodyExpression)
        {
            return new FunctionNode(name, XzaarType.GetType(returnType), argumentsExpression, bodyExpression, _nodeIndex++) { IsTransformed = true };
        }

        //public static FunctionNode Function(string name, string returnType, XzaarNode argumentsExpression, XzaarNode bodyExpression)
        //{
        //    return new FunctionNode(name, XzaarType.GetType(returnType), Parameters(argumentsExpression), bodyExpression, _nodeIndex++);
        //}

        public static FunctionNode ExternFunction(string name, FunctionParametersNode parameters)
        {
            // var functionParametersNode = Parameters(argumentsExpression);
            return new FunctionNode(name, parameters, _nodeIndex++) { IsTransformed = true };
        }

        public static FunctionNode ExternFunction(string name, string returnType, FunctionParametersNode parameters)
        {
            // var functionParametersNode = Parameters(argumentsExpression);
            return new FunctionNode(name, XzaarType.GetType(returnType), parameters, _nodeIndex++) { IsTransformed = true };
        }
        public static FunctionParametersNode Parameters(ParameterNode[] parameters)
        {
            // if (parameters)

            var result = new FunctionParametersNode(_nodeIndex++) { IsTransformed = true };
            foreach (var p in parameters) result.AddChild(p);

            return result;
        }

        public static FunctionParametersNode Parameters(AstNode parameters)
        {
            // if (parameters)
            var r = parameters as FunctionParametersNode;
            if (r != null) return r;

            var result = new FunctionParametersNode(_nodeIndex++) { IsTransformed = true };
            var count = parameters.Children.Count;
            for (int i = 0; i < count; i += 2)
            {
                var type = parameters[i];
                var isArray = parameters[i + 1].NodeType == NodeTypes.ARRAYINDEX;
                if (isArray)
                {
                    type.Value += "[]";
                    i++;
                }

                var name = parameters[i + 1];
                result.AddChild(AstNode.Parameter(name, type));

                if (count > 2 && i + 2 < count)
                {
                    var value = parameters[i + 2].Value;
                    if (value?.ToString() != ",")
                        throw new ExpressionException($"Unexpected character '{value}' found in function parameter declaration");
                    i++; // skip separator
                }
            }

            return result;
        }

        public static ParameterNode Parameter(AstNode name, AstNode type)
        {
            var n = name.Value?.ToString();
            var t = type.Value?.ToString();

            return new ParameterNode(n, t, _nodeIndex++) { IsTransformed = true };
        }

        public static DefineVariableNode DefineVariable(string type, string name, AstNode assignValue)
        {
            if (type == null) type = "any";
            return new DefineVariableNode(type, name, assignValue, _nodeIndex++) { IsTransformed = true };
        }

        public static FieldNode Field(string type, string name, string declaringType)
        {
            if (type == null) type = "any";
            return new FieldNode(type, name, declaringType, _nodeIndex++) { IsTransformed = true };
        }

        public static StructNode Struct(string name, params AstNode[] fields)
        {
            return new StructNode(name, fields, _nodeIndex++) { IsTransformed = true };
        }

        public static CreateStructNode CreateStruct(StructNode str)
        {
            return new CreateStructNode(str, _nodeIndex++) { IsTransformed = true };
        }

        public static CreateStructNode CreateStruct(StructNode str, AstNode[] structFieldInitializers)
        {
            return new CreateStructNode(str, structFieldInitializers, _nodeIndex++) { IsTransformed = true };
        }

        public static ArgumentNode Argument(AstNode item, int argIndex)
        {
            return new ArgumentNode(item, argIndex, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode MemberAccess(AstNode member)
        {
            return new MemberAccessNode(member, null, null, null, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode MemberAccess(AstNode member, string declaringType, string memberType)
        {
            return new MemberAccessNode(member, null, declaringType, memberType, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode MemberAccess(AstNode member, AstNode arrayIndexer, string declaringType, string memberType)
        {
            return new MemberAccessNode(member, arrayIndexer, declaringType, memberType, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode MemberAccess(AstNode member, AstNode arrayIndexer)
        {
            return new MemberAccessNode(member, arrayIndexer, "any", "any", _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode ParameterAccess(AstNode parameter)
        {
            return new MemberAccessNode(parameter, null, null, null, _nodeIndex++) { IsTransformed = true };
        }
        public static MemberAccessNode ParameterAccess(AstNode parameter, string parameterType)
        {
            return new MemberAccessNode(parameter, null, null, parameterType, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode VariableAccess(AstNode variable)
        {
            return new MemberAccessNode(variable, null, null, null, _nodeIndex++) { IsTransformed = true };
        }


        public static MemberAccessNode VariableAccess(AstNode variable, string variableType)
        {
            var access = variable as MemberAccessNode;
            if (access != null)
            {
                return new MemberAccessNode(access.Member, null, access.DeclaringType, variableType, _nodeIndex++) { IsTransformed = true };
            }

            return new MemberAccessNode(variable, null, null, variableType, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessNode VariableAccess(AstNode variable, AstNode arrayIndex)
        {
            var access = variable as MemberAccessNode;
            if (access != null)
            {
                if (access.ArrayIndex != null)
                {
                    return new MemberAccessNode(access, arrayIndex, access.DeclaringType, access.MemberType, _nodeIndex++) { IsTransformed = true };
                }
                else
                {
                    return new MemberAccessNode(access.Member, arrayIndex, access.DeclaringType, access.MemberType, _nodeIndex++) { IsTransformed = true };
                }
            }

            return new MemberAccessNode(variable, arrayIndex, null, null, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessChainNode MemberAccessChain(AstNode lastAccessorNode, MemberAccessNode memberAccessNode)
        {
            return new MemberAccessChainNode(lastAccessorNode, memberAccessNode, memberAccessNode.MemberType, _nodeIndex++) { IsTransformed = true };
        }

        public static MemberAccessChainNode MemberAccessChain(AstNode lastAccessorNode, AstNode memberAccessNode, string functionReturnType)
        {
            return new MemberAccessChainNode(lastAccessorNode, memberAccessNode, functionReturnType, _nodeIndex++) { IsTransformed = true };
        }

        public static AssignNode Assign(AstNode left, AstNode right)
        {
            return new AssignNode(left, right, _nodeIndex++) { IsTransformed = true };
        }

        public static ConditionalNode IfThen(AstNode condition, AstNode ifTrue)
        {
            return new ConditionalNode(condition, ifTrue, null, _nodeIndex++) { IsTransformed = true };
        }

        public static ConditionalNode IfElseThen(AstNode condition, AstNode ifTrue, AstNode ifFalse)
        {
            return new ConditionalNode(condition, ifTrue, ifFalse, _nodeIndex++) { IsTransformed = true };
        }

        public static FunctionCallNode Call(AstNode function, ArgumentNode[] argumentNodes)
        {
            return Call(null, function, argumentNodes);
        }

        public static FunctionCallNode Call(AstNode instance, AstNode function, ArgumentNode[] argumentNodes)
        {
            return new FunctionCallNode(instance, function, _nodeIndex++, argumentNodes) { IsTransformed = true };
        }

        public static ReturnNode Return()
        {
            return new ReturnNode(Empty(), _nodeIndex++) { IsTransformed = true };
        }
        public static ReturnNode Return(AstNode returnNode)
        {
            return new ReturnNode(returnNode, _nodeIndex++) { IsTransformed = true };
        }

        public static ContinueNode Continue()
        {
            return new ContinueNode(_nodeIndex++) { IsTransformed = true };
        }

        public static BreakNode Break()
        {
            return new BreakNode(_nodeIndex++) { IsTransformed = true };
        }

        public static GotoNode Goto(string labelName)
        {
            return new GotoNode(labelName, _nodeIndex++) { IsTransformed = true };
        }

        public static CaseNode DefaultCase(AstNode body)
        {
            return new CaseNode(null, body, _nodeIndex++) { IsTransformed = true };
        }

        public static CaseNode Case(AstNode test, AstNode body)
        {
            return new CaseNode(test, body, _nodeIndex++) { IsTransformed = true };
        }

        public static LabelNode Label(string labelName)
        {
            return new LabelNode(labelName, _nodeIndex++) { IsTransformed = true };
        }

        public static LoopNode Loop(AstNode body)
        {
            if (body != null) body.SortChildren();
            var loopNode = new LoopNode("LOOP", body, _nodeIndex++) { IsTransformed = true };
            loopNode.SortChildren();
            return loopNode;
        }

        public static WhileLoopNode While(AstNode test, AstNode body)
        {
            if (body != null) body.SortChildren();
            return new WhileLoopNode(test, body, _nodeIndex++) { IsTransformed = true };
        }

        public static DoWhileLoopNode DoWhile(AstNode test, AstNode body)
        {
            if (body != null) body.SortChildren();
            return new DoWhileLoopNode(test, body, _nodeIndex++) { IsTransformed = true };
        }

        public static ForeachLoopNode Foreach(AstNode variableDefinition, AstNode sourceExpression, AstNode body)
        {
            if (body != null) body.SortChildren();
            return new ForeachLoopNode(variableDefinition, sourceExpression, body, _nodeIndex++) { IsTransformed = true };
        }

        public static ForLoopNode For(AstNode initiator, AstNode test, AstNode incrementor, AstNode body)
        {
            if (body != null) body.SortChildren();
            return new ForLoopNode(initiator, test, incrementor, body, _nodeIndex++) { IsTransformed = true };
        }

        public static LogicalNotNode Negate(AstNode expression)
        {
            return new LogicalNotNode(expression, _nodeIndex++);
        }

        public static BinaryOperatorNode BinaryOperator(int order, AstNode left, char op0, AstNode right)
        {
            return new BinaryOperatorNode(order, left, op0, right, _nodeIndex++) { IsTransformed = true };
        }
        public static BinaryOperatorNode BinaryOperator(int order, AstNode left, string op0, AstNode right)
        {
            return new BinaryOperatorNode(order, left, op0, right, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode EqualityOperator(AstNode left, string toString, AstNode right)
        {
            return new EqualityOperatorNode(left, toString, right, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode ConditionalOperator(int order, AstNode node, string toString, AstNode rl)
        {
            return new ConditionalOperatorNode(order, node, toString, rl, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode Number(int i)
        {
            return new NumberNode(i, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode NewArrayInstance(params AstNode[] values)
        {
            return new ArrayNode(_nodeIndex, values.ToList()) { IsTransformed = true };
        }

        public static MatchNode Switch(AstNode valueExpression, CaseNode[] cases)
        {
            return new MatchNode(valueExpression, cases, _nodeIndex++) { IsTransformed = true };
        }

        public static UnaryNode Incrementor(AstNode item)
        {
            return new UnaryNode(false, true, item, _nodeIndex++) { IsTransformed = true };
        }

        public static UnaryNode Decrementor(AstNode item)
        {
            return new UnaryNode(false, false, item, _nodeIndex++) { IsTransformed = true };
        }

        public static UnaryNode PostIncrementor(AstNode item)
        {
            return new UnaryNode(true, true, item, _nodeIndex++) { IsTransformed = true };
        }

        public static UnaryNode PostDecrementor(AstNode item)
        {
            return new UnaryNode(true, false, item, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode PrefixUnary(SyntaxNode opToken, AstNode operand)
        {
            return new UnaryNode(false, opToken.StringValue, operand, _nodeIndex++) { IsTransformed = true };
        }

        public static AstNode PostfixUnary(SyntaxNode opToken, AstNode operand)
        {
            return new UnaryNode(true, opToken.StringValue, operand, _nodeIndex++) { IsTransformed = true };
        }
        public static AstNode PostfixUnary(SyntaxNode opToken, AstNode operand, SyntaxKind type)
        {
            return new UnaryNode(true, opToken.StringValue, operand, _nodeIndex++) { IsTransformed = true };
        }
        #endregion

        public void RemoveUntransformedVariableDeclarations()
        {
            List<AstNode> toRemove = new List<AstNode>();
            foreach (var child in this.children)
            {
                if (!child.IsTransformed && child.NodeType == NodeTypes.DECLARATION)
                {
                    toRemove.Add(child);
                }
            }
            foreach (var c in toRemove) this.RemoveChild(c);
        }

        public void SetChildren(IEnumerable<AstNode> newChildren)
        {
            this.children.Clear();
            this.AddChildren(newChildren);
        }
    }
}