using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shinobytes.XzaarScript.Scripting.Nodes
{
    public abstract class XzaarNode
    {
        private readonly int nodeIndex;
        private List<XzaarNode> children;

        protected XzaarNode(XzaarNodeTypes nodeType, string nodeName, object value, int nodeIndex)
        {
            this.nodeIndex = nodeIndex;
            children = new List<XzaarNode>();
            LeftParams = new List<XzaarNode>();
            RightParams = new List<XzaarNode>();
            NodeType = nodeType;
            NodeName = nodeName;
            Value = value;
        }

        public int Index { get { return nodeIndex; ; } }

        public abstract void Accept(IXzaarNodeVisitor xzaarNodeVisitor);

        public XzaarNodeTypes NodeType { get; }
        public string NodeName { get; }
        public object Value { get; protected set; }

        public XzaarNode Parent { get; set; }

        public IList<XzaarNode> Children => children;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<XzaarNode> LeftParams { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<XzaarNode> RightParams { get; }

        public int OperatingOrder { get; protected set; }

        public XzaarNode this[int childIndex]
        {
            get { return children[childIndex]; }
            set { children[childIndex] = value; }
        }
        public void SortChildren()
        {
            this.children = Children.OrderBy(c => c.Index).ToList();
        }

        public void InsertChild(int index, XzaarNode node)
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

        public void AddChild(XzaarNode node)
        {
            // normally i would like to remove it from their previous parent, but since we mutate the state of the nodes rather than cloning them
            // we need to make sure that we keep our "children" from all nodes so when we walk the node tree we can expect the original parental node
            // keeps their children
            //if (node.Parent != null)
            //    node.Parent.RemoveChild(node);

            node.Parent = this;
            this.children.Add(node);
        }

        public void AddChildren(IEnumerable<XzaarNode> nodes)
        {
            foreach (var node in nodes)
            {
                AddChild(node);
            }
        }

        public void RemoveChild(XzaarNode node)
        {
            node.Parent = null;
            this.children.Remove(node);
        }

        public override string ToString()
        {
            return NodeType + " " + NodeName + " " + Value;
        }

        public bool IsEmpty()
            =>
                this is EmptyNode || this.NodeType == null && this.NodeName == null && this.Value == null &&
                (Children == null || Children.Count == 0);

        #region Factory

        private static int _nodeIndex = 0;

        public static XzaarNode Empty()
        {
            return new EmptyNode(_nodeIndex++);
        }

        public static BlockNode Block(params XzaarNode[] children)
        {
            var block = new BlockNode(_nodeIndex++);
            if (children != null && children.Length > 0)
                block.AddChildren(children);
            return block;
        }

        public static BodyNode Body(params XzaarNode[] children)
        {
            var block = new BodyNode(_nodeIndex++);
            if (children != null && children.Length > 0)
                block.AddChildren(children);
            return block;
        }

        public static FunctionNode Function(string name, XzaarNode argumentsExpression, XzaarNode bodyExpression)
        {
            return new FunctionNode(name, Parameters(argumentsExpression), bodyExpression, _nodeIndex++);
        }

        public static FunctionNode Function(string name, string returnType, XzaarNode argumentsExpression, XzaarNode bodyExpression)
        {
            return new FunctionNode(name, XzaarType.GetType(returnType), Parameters(argumentsExpression), bodyExpression, _nodeIndex++);
        }

        public static FunctionNode ExternFunction(string name, XzaarNode argumentsExpression)
        {
            var functionParametersNode = Parameters(argumentsExpression);
            return new FunctionNode(name, functionParametersNode, _nodeIndex++);
        }

        public static FunctionNode ExternFunction(string name, string returnType, XzaarNode argumentsExpression)
        {
            var functionParametersNode = Parameters(argumentsExpression);
            return new FunctionNode(name, XzaarType.GetType(returnType), functionParametersNode, _nodeIndex++);
        }

        private static FunctionParametersNode Parameters(XzaarNode parameters)
        {
            var result = new FunctionParametersNode(_nodeIndex++);
            var count = parameters.Children.Count;
            for (int i = 0; i < count; i += 2)
            {
                var type = parameters[i];
                var name = parameters[i + 1];

                result.AddChild(XzaarNode.Parameter(name, type));

                if (count > 2 && i + 2 < count)
                {
                    var value = parameters[i + 2].Value;
                    if (value?.ToString() != ",")
                        throw new XzaarExpressionTransformerException($"Unexpected character '{value}' found in function parameter declaration");
                    i++; // skip separator
                }
            }

            return result;
        }

        public static ParameterNode Parameter(XzaarNode name, XzaarNode type)
        {
            var n = name.Value?.ToString();
            var t = type.Value?.ToString();
            return new ParameterNode(n, t, _nodeIndex++);
        }

        public static DefineVariableNode DefineVariable(string type, string name, XzaarNode assignValue)
        {
            if (type == null) type = "any";
            return new DefineVariableNode(type, name, assignValue, _nodeIndex++);
        }

        public static FieldNode Field(string type, string name, string declaringType)
        {
            if (type == null) type = "any";
            return new FieldNode(type, name, declaringType, _nodeIndex++);
        }

        public static StructNode Struct(string name, params XzaarNode[] fields)
        {
            return new StructNode(name, fields, _nodeIndex++);
        }

        public static CreateStructNode CreateStruct(StructNode str)
        {
            return new CreateStructNode(str, _nodeIndex++);
        }

        public static ArgumentNode Argument(XzaarNode item, int argIndex)
        {
            return new ArgumentNode(item, argIndex, _nodeIndex++);
        }

        public static MemberAccessNode MemberAccess(XzaarNode member)
        {
            return new MemberAccessNode(member, null, null, null, _nodeIndex++);
        }

        public static MemberAccessNode MemberAccess(XzaarNode member, string declaringType, string memberType)
        {
            return new MemberAccessNode(member, null, declaringType, memberType, _nodeIndex++);
        }

        public static MemberAccessNode MemberAccess(XzaarNode member, XzaarNode arrayIndexer, string declaringType, string memberType)
        {
            return new MemberAccessNode(member, arrayIndexer, declaringType, memberType, _nodeIndex++);
        }

        public static MemberAccessNode ParameterAccess(XzaarNode parameter)
        {
            return new MemberAccessNode(parameter, null, null, null, _nodeIndex++);
        }
        public static MemberAccessNode ParameterAccess(XzaarNode parameter, string parameterType)
        {
            return new MemberAccessNode(parameter, null, null, parameterType, _nodeIndex++);
        }

        public static MemberAccessNode VariableAccess(XzaarNode variable)
        {
            return new MemberAccessNode(variable, null, null, null, _nodeIndex++);
        }


        public static MemberAccessNode VariableAccess(XzaarNode variable, string variableType)
        {
            var access = variable as MemberAccessNode;
            if (access != null)
            {
                return new MemberAccessNode(access.Member, null, access.DeclaringType, variableType, _nodeIndex++);
            }

            return new MemberAccessNode(variable, null, null, variableType, _nodeIndex++);
        }

        public static MemberAccessNode VariableAccess(XzaarNode variable, XzaarNode arrayIndex)
        {
            var access = variable as MemberAccessNode;
            if (access != null)
            {
                return new MemberAccessNode(access.Member, arrayIndex, access.DeclaringType, access.MemberType, _nodeIndex++);
            }

            return new MemberAccessNode(variable, arrayIndex, null, null, _nodeIndex++);
        }

        public static MemberAccessChainNode MemberAccessChain(XzaarNode lastAccessorNode, MemberAccessNode memberAccessNode)
        {
            return new MemberAccessChainNode(lastAccessorNode, memberAccessNode, memberAccessNode.MemberType, _nodeIndex++);
        }

        public static MemberAccessChainNode MemberAccessChain(XzaarNode lastAccessorNode, FunctionCallNode memberAccessNode, string functionReturnType)
        {
            return new MemberAccessChainNode(lastAccessorNode, memberAccessNode, functionReturnType, _nodeIndex++);
        }

        public static AssignNode Assign(XzaarNode left, XzaarNode right)
        {
            return new AssignNode(left, right, _nodeIndex++);
        }

        public static ConditionalNode IfThen(XzaarNode condition, XzaarNode ifTrue)
        {
            return new ConditionalNode(condition, ifTrue, null, _nodeIndex++);
        }

        public static ConditionalNode IfElseThen(XzaarNode condition, XzaarNode ifTrue, XzaarNode ifFalse)
        {
            return new ConditionalNode(condition, ifTrue, ifFalse, _nodeIndex++);
        }

        public static FunctionCallNode Call(XzaarNode function, ArgumentNode[] argumentNodes)
        {
            return Call(null, function, argumentNodes);
        }

        public static FunctionCallNode Call(XzaarNode instance, XzaarNode function, ArgumentNode[] argumentNodes)
        {
            return new FunctionCallNode(instance, function, _nodeIndex++, argumentNodes);
        }

        public static ReturnNode Return()
        {
            return new ReturnNode(Empty(), _nodeIndex++);
        }
        public static ReturnNode Return(XzaarNode returnNode)
        {
            return new ReturnNode(returnNode, _nodeIndex++);
        }

        public static ContinueNode Continue()
        {
            return new ContinueNode(_nodeIndex++);
        }

        public static BreakNode Break()
        {
            return new BreakNode(_nodeIndex++);
        }

        public static GotoNode Goto(string labelName)
        {
            return new GotoNode(labelName, _nodeIndex++);
        }

        public static CaseNode DefaultCase(XzaarNode body)
        {
            return new CaseNode(null, body, _nodeIndex++);
        }

        public static CaseNode Case(XzaarNode test, XzaarNode body)
        {
            return new CaseNode(test, body, _nodeIndex++);
        }

        public static LabelNode Label(string labelName)
        {
            return new LabelNode(labelName, _nodeIndex++);
        }

        public static LoopNode Loop(XzaarNode body)
        {
            if (body != null) body.SortChildren();
            var loopNode = new LoopNode(body: body, nodeIndex: _nodeIndex++);
            loopNode.SortChildren();
            return loopNode;
        }

        public static WhileLoopNode While(XzaarNode test, XzaarNode body)
        {
            if (body != null) body.SortChildren();
            return new WhileLoopNode(test, body, _nodeIndex++);
        }

        public static DoWhileLoopNode DoWhile(XzaarNode test, XzaarNode body)
        {
            if (body != null) body.SortChildren();
            return new DoWhileLoopNode(test, body, _nodeIndex++);
        }

        public static ForeachLoopNode Foreach(XzaarNode variableDefinition, XzaarNode sourceExpression, XzaarNode body)
        {
            if (body != null) body.SortChildren();
            return new ForeachLoopNode(variableDefinition, sourceExpression, body, _nodeIndex++);
        }

        public static ForLoopNode For(XzaarNode initiator, XzaarNode test, XzaarNode incrementor, XzaarNode body)
        {
            if (body != null) body.SortChildren();
            return new ForLoopNode(initiator, test, incrementor, body, _nodeIndex++);
        }

        public static BinaryOperatorNode BinaryOperator(int order, XzaarNode left, char op0, XzaarNode right)
        {
            return new BinaryOperatorNode(order, left, op0, right, _nodeIndex++);
        }
        public static BinaryOperatorNode BinaryOperator(int order, XzaarNode left, string op0, XzaarNode right)
        {
            return new BinaryOperatorNode(order, left, op0, right, _nodeIndex++);
        }

        public static XzaarNode EqualityOperator(XzaarNode left, string toString, XzaarNode right)
        {
            return new EqualityOperatorNode(left, toString, right, _nodeIndex++);
        }

        public static XzaarNode ConditionalOperator(int order, XzaarNode xzaarNode, string toString, XzaarNode rl)
        {
            return new ConditionalOperatorNode(order, xzaarNode, toString, rl, _nodeIndex++);
        }

        public static XzaarNode Number(int i)
        {
            return new NumberNode(i, _nodeIndex++);
        }

        public static XzaarNode NewArrayInstance()
        {
            return new ArrayNode(_nodeIndex);
        }

        public static MatchNode Switch(XzaarNode valueExpression, CaseNode[] cases)
        {
            return new MatchNode(valueExpression, cases, _nodeIndex++);
        }

        public static UnaryNode Incrementor(XzaarNode item)
        {
            return new UnaryNode(false, true, item, _nodeIndex++);
        }

        public static UnaryNode Decrementor(XzaarNode item)
        {
            return new UnaryNode(false, false, item, _nodeIndex++);
        }

        public static UnaryNode PostIncrementor(XzaarNode item)
        {
            return new UnaryNode(true, true, item, _nodeIndex++);
        }

        public static UnaryNode PostDecrementor(XzaarNode item)
        {
            return new UnaryNode(true, false, item, _nodeIndex++);
        }
        #endregion     
    }
}