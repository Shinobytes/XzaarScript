using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class ExpressionParserContext
    {

        //private readonly Dictionary<string, List<VariableNode>> scopeVariables = new Dictionary<string, List<VariableNode>>();
        //private readonly Dictionary<string, List<ParameterNode>> scopeParameters = new Dictionary<string, List<ParameterNode>>();

        public xxx_ScriptScope GlobalScope;
        private xxx_ScriptScope CurrentScope;
        private int state;
        private bool moveNext = true;
        private bool ignoreTypeRestrictions;
        private int currentNodeIndex;
        public bool NegateNext;

        public ExpressionParserContext()
        {
            Stack = new Stack<AstNode>();
            GlobalScope = xxx_ScriptScope.Global();
            CurrentScope = GlobalScope;
        }

        public ExpressionParserContext(
            Stack<AstNode> stack,
            xxx_ScriptScope globalScope,
            xxx_ScriptScope currentScope,
            int currentNodeIndex)
        {
            Stack = stack;
            CurrentNodeIndex = currentNodeIndex;
            GlobalScope = globalScope;
            CurrentScope = currentScope;
        }

        public Stack<AstNode> Stack { get; set; }

        public IList<FunctionNode> Functions => GlobalScope.Functions;

        public IList<StructNode> Structs => GlobalScope.Structs;

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

        public xxx_ScriptScope In(string name = null, AstNode associatedNode = null)
        {
            this.CurrentScope = this.CurrentScope.In(associatedNode, name);
            return this.CurrentScope;
        }

        public xxx_ScriptScope Out()
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


        public ExpressionParserContext With(
            Stack<AstNode> stack = null,
            xxx_ScriptScope globalScope = null,
            xxx_ScriptScope currentScope = null,
            int? currentNodeIndex = null)
        {
            return new ExpressionParserContext(
                stack ?? Stack,
                globalScope ?? GlobalScope,
                currentScope ?? CurrentScope,
                currentNodeIndex ?? CurrentNodeIndex
            );
        }

        public void From(ExpressionParserContext otherContext, int? nodeIndex = null)
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
            return this.GetVariablesInScope(xxx_ScriptScope.GlobalScopeName);
        }

        public void SetScopeNode(AstNode result)
        {
            this.CurrentScope.SetScopeNode(result);
        }

        public void SetChildScopeNode(int childIndex, AstNode node)
        {
            this.CurrentScope.SetChildScopeNode(childIndex, node);
        }

        public xxx_ScriptScope GetScopeByNode(AstNode node)
        {
            return this.GlobalScope.Find(node);
        }

    }
}