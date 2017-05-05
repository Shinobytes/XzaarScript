using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Scripting.Nodes;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarExpressionTransformerContext
    {

        //private readonly Dictionary<string, List<VariableNode>> scopeVariables = new Dictionary<string, List<VariableNode>>();
        //private readonly Dictionary<string, List<ParameterNode>> scopeParameters = new Dictionary<string, List<ParameterNode>>();

        public XzaarTransformerScope GlobalScope;
        private XzaarTransformerScope CurrentScope;
        private int state;
        private bool moveNext = true;

        public XzaarExpressionTransformerContext()
        {
            Stack = new Stack<XzaarNode>();
            GlobalScope = XzaarTransformerScope.Global();
            CurrentScope = GlobalScope;
        }

        public XzaarExpressionTransformerContext(
            Stack<XzaarNode> stack,
            XzaarTransformerScope globalScope,
            XzaarTransformerScope currentScope,
            int currentNodeIndex)
        {
            Stack = stack;
            CurrentNodeIndex = currentNodeIndex;
            GlobalScope = globalScope;
            CurrentScope = currentScope;
        }

        public Stack<XzaarNode> Stack { get; set; }

        public IReadOnlyList<FunctionNode> Functions { get { return GlobalScope.Functions; } }

        public IReadOnlyList<StructNode> Structs { get { return GlobalScope.Structs; } }

        public int CurrentNodeIndex { get; set; }

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        public bool MoveNext
        {
            get { return moveNext; }
            set { moveNext = value; }
        }

        public XzaarTransformerScope In(string name = null, XzaarNode associatedNode = null)
        {
            this.CurrentScope = this.CurrentScope.In(associatedNode, name);
            return this.CurrentScope;
        }

        public XzaarTransformerScope Out()
        {
            this.CurrentScope = this.CurrentScope.Out();
            return this.CurrentScope;
        }


        public void AddGlobalFunction(FunctionNode function)
        {
            this.GlobalScope.Functions.Add(function);
        }

        public List<ParameterNode> GetParametersInScope(string scopeIdentifier)
        {
            var scope = GlobalScope.Find(scopeIdentifier);
            if (scope == null) return new List<ParameterNode>();
            return scope.Parameters;
        }

        public List<ParameterNode> GetParametersInScope()
        {
            return GetParametersInScope(CurrentScope.Name);
        }

        public List<VariableNode> GetVariablesInScope(string scopeIdentifier)
        {
            var scope = GlobalScope.Find(scopeIdentifier);
            if (scope == null)
                return new List<VariableNode>();

            var vars = new List<VariableNode>();
            if (scope.Variables.Count > 0) vars.AddRange(scope.Variables);

            var target = scope.Out();
            while (target != null)
            {
                if (target.Variables.Count > 0) vars.AddRange(target.Variables);
                target = target.Out();
            }

            return vars;
        }

        public List<VariableNode> GetVariablesInScope()
        {
            return GetVariablesInScope(CurrentScope.Name);
        }


        public XzaarExpressionTransformerContext With(
            Stack<XzaarNode> stack = null,
            XzaarTransformerScope globalScope = null,
            XzaarTransformerScope currentScope = null,
            int? currentNodeIndex = null)
        {
            return new XzaarExpressionTransformerContext(
                stack ?? Stack,
                globalScope ?? GlobalScope,
                currentScope ?? CurrentScope,
                currentNodeIndex ?? CurrentNodeIndex
            );
        }

        public void From(XzaarExpressionTransformerContext otherContext, int? nodeIndex = null)
        {
            Stack = otherContext.Stack;
            GlobalScope = otherContext.GlobalScope;
            CurrentScope = otherContext.CurrentScope;
            CurrentNodeIndex = nodeIndex ?? otherContext.CurrentNodeIndex;
        }

        public void AddVariableToScope(string scopeName, VariableNode variable)
        {
            var scope = GlobalScope.Find(scopeName);
            if (scope != null)
            {
                scope.Variables.Add(variable);
            }
        }

        public void AddVariableToScope(VariableNode parameterNode)
        {
            if (CurrentScope == null) CurrentScope = GlobalScope;
            CurrentScope.Variables.Add(parameterNode);
        }

        public void AddParameterToScope(string scopeIdentifier, ParameterNode parameterNode)
        {
            var scope = GlobalScope.Find(scopeIdentifier);
            if (scope != null)
            {
                scope.Parameters.Add(parameterNode);
            }
        }

        public void AddParameterToScope(ParameterNode parameterNode)
        {
            if (CurrentScope == null) CurrentScope = GlobalScope;
            CurrentScope.Parameters.Add(parameterNode);
        }

        public void AddGlobalStruct(StructNode structNode)
        {
            this.GlobalScope.Structs.Add(structNode);
        }
        public List<VariableNode> GetGlobalVariables()
        {
            return this.GetVariablesInScope(XzaarTransformerScope.GlobalScopeName);
        }

        public void SetScopeNode(XzaarNode result)
        {
            this.CurrentScope.SetScopeNode(result);
        }

        public void SetChildScopeNode(int childIndex, XzaarNode node)
        {
            this.CurrentScope.SetChildScopeNode(childIndex, node);
        }

        public XzaarTransformerScope GetScopeByNode(XzaarNode node)
        {
            return this.GlobalScope.Find(node);
        }
    }
}