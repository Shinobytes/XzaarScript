using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Ast.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class XzaarExpressionTransformerContext
    {

        //private readonly Dictionary<string, List<VariableNode>> scopeVariables = new Dictionary<string, List<VariableNode>>();
        //private readonly Dictionary<string, List<ParameterNode>> scopeParameters = new Dictionary<string, List<ParameterNode>>();

        public XzaarTransformerScopeOld GlobalScope;
        private XzaarTransformerScopeOld CurrentScope;
        private int state;
        private bool moveNext = true;
        private bool ignoreTypeRestrictions;
        private int currentNodeIndex;
        public bool NegateNext;

        public XzaarExpressionTransformerContext()
        {
            Stack = new Stack<XzaarAstNode>();
            GlobalScope = XzaarTransformerScopeOld.Global();
            CurrentScope = GlobalScope;
        }

        public XzaarExpressionTransformerContext(
            Stack<XzaarAstNode> stack,
            XzaarTransformerScopeOld globalScope,
            XzaarTransformerScopeOld currentScope,
            int currentNodeIndex)
        {
            Stack = stack;
            CurrentNodeIndex = currentNodeIndex;
            GlobalScope = globalScope;
            CurrentScope = currentScope;
        }

        public Stack<XzaarAstNode> Stack { get; set; }

        public IList<FunctionNode> Functions { get { return GlobalScope.Functions; } }

        public IList<StructNode> Structs { get { return GlobalScope.Structs; } }

        public int CurrentNodeIndex
        {
            get { return currentNodeIndex; }
            set
            {
                var added = value - currentNodeIndex;
                GlobalNodeIndex += added;
                currentNodeIndex = value;
            }
        }

        public int GlobalNodeIndex { get; set; }

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

        public bool IgnoreTypeRestrictions
        {
            get { return ignoreTypeRestrictions; }
            set { ignoreTypeRestrictions = value; }
        }

        public XzaarTransformerScopeOld In(string name = null, XzaarAstNode associatedNode = null)
        {
            this.CurrentScope = this.CurrentScope.In(associatedNode, name);
            return this.CurrentScope;
        }

        public XzaarTransformerScopeOld Out()
        {
            this.CurrentScope = this.CurrentScope.Out();
            return this.CurrentScope;
        }


        public void AddGlobalFunction(FunctionNode function)
        {
            this.GlobalScope.Functions.Add(function);
        }

        public ParameterNode FindParameterInScope(string name)
        {
            var parameters = GetParametersInScope();
            if (name.StartsWith("$")) return new ParameterNode(name, "any", -1) { IsExtern = true };
            return parameters.FirstOrDefault(p => p.Name == name);
        }

        public VariableNode FindVariableInScope(string name)
        {
            var variables = GetVariablesInScope();
            if (name.StartsWith("$")) return new VariableNode(name, "any", null, false, -1) { IsExtern = true };
            return variables.FirstOrDefault(v => v.Name == name);
        }

        public List<ParameterNode> GetParametersInScope()
        {
            var parameters = new List<ParameterNode>();
            var current = CurrentScope;
            while (current != null)
            {
                parameters.AddRange(current.Parameters);
                current = current.Out();
            }
            return parameters;
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

        //public List<VariableNode> GetVariablesAndParametersInScope()
        //{
        //    var v1 = GetParametersInScope(CurrentScope.Name);
        //    var v2 = GetVariablesInScope(CurrentScope.Name);
        //    var outList = new List<VariableNode>();
        //    outList.AddRange(v1);
        //    outList.AddRange(v2);
        //    return outList;
        //}


        public XzaarExpressionTransformerContext With(
            Stack<XzaarAstNode> stack = null,
            XzaarTransformerScopeOld globalScope = null,
            XzaarTransformerScopeOld currentScope = null,
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
            return this.GetVariablesInScope(XzaarTransformerScopeOld.GlobalScopeName);
        }

        public void SetScopeNode(XzaarAstNode result)
        {
            this.CurrentScope.SetScopeNode(result);
        }

        public void SetChildScopeNode(int childIndex, XzaarAstNode node)
        {
            this.CurrentScope.SetChildScopeNode(childIndex, node);
        }

        public XzaarTransformerScopeOld GetScopeByNode(XzaarAstNode node)
        {
            return this.GlobalScope.Find(node);
        }

    }
}