using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Ast
{
    public class NodeTypeBinder : INodeProcessor
    {
        private readonly Dictionary<string, StructNode> structs = new Dictionary<string, StructNode>();
        private readonly Dictionary<string, DefineVariableNode> globalVariables = new Dictionary<string, DefineVariableNode>();
        private readonly Dictionary<string, DefineVariableNode> localVariables = new Dictionary<string, DefineVariableNode>();
        private readonly Dictionary<string, ParameterNode> parameters = new Dictionary<string, ParameterNode>();

        private readonly Dictionary<string, FunctionNode> functions = new Dictionary<string, FunctionNode>();
        private readonly List<string> classes = new List<string>();
        private readonly List<string> enums = new List<string>();
        private bool insideFunction;

        public AstNode Process(AstNode syntaxTree)
        {
            if (syntaxTree is EntryNode || syntaxTree is BlockNode)
            {
                var newChildren = syntaxTree.Children.Select(Process).ToList();
                syntaxTree.SetChildren(newChildren);
            }
            else if (syntaxTree is ExpressionNode)
            {
                // find return type of expression
                throw new NotImplementedException();
            }
            else
            {
#if UNITY
                if (syntaxTree is DefineVariableNode) return Process(syntaxTree as DefineVariableNode);
                if (syntaxTree is FunctionNode) return Process(syntaxTree as FunctionNode);
                if (syntaxTree is StructNode) return Process(syntaxTree as StructNode);
                if (syntaxTree is IfElseNode) return Process((syntaxTree as IfElseNode));
                if (syntaxTree is LoopNode) Process((syntaxTree as LoopNode).Body);
                if (syntaxTree is BinaryOperatorNode) return Process((syntaxTree as BinaryOperatorNode));
                if (syntaxTree is AssignNode) return Process((syntaxTree as AssignNode));
                if (syntaxTree is MemberAccessNode) return Process((syntaxTree as MemberAccessNode));
                if (syntaxTree is MemberAccessChainNode) return Process((syntaxTree as MemberAccessChainNode));
#else 
                return Process((dynamic)syntaxTree);
#endif
            }

            return syntaxTree;
        }


        private AssignNode Process(AssignNode node)
        {
            return node;
        }


        private MemberAccessNode Process(MemberAccessNode node)
        {
            if (node.Type == null || node.Type == "void") node.Type = "any";
            if (node.Type != "any" || node.Member == null) return node;

            Process(node.Member);

            node.Type = node.Member.Type;

            if (node.MemberType != node.Type)
            {
                node.MemberType = node.Type;
            }

            return node;
        }


        private MemberAccessChainNode Process(MemberAccessChainNode node)
        {
            if (node.ResultType == null || node.Type == null)
            {
                Process(node.Accessor);

                if (node.Accessor.Type == "any")
                {

                    var v = Variable(node.LastAccessor.ValueText);
                    if (v != null)
                    {
                        // NOTE: This could potentially be a class in the future and not just a struct.
                        if (IsStruct(v.Type))
                        {
                            var f = structs[v.Type].Fields.Cast<FieldNode>().FirstOrDefault(a => a.Name == node.Accessor.ValueText);
                            if (f != null) node.Accessor.Type = f.Type;
                            node.LastAccessor.Type = v.Type;
                        }
                    }
                    else
                    {
                        var p = Parameter(node.LastAccessor.ValueText);
                        if (p != null)
                        {
                            // declaring type                            
                            // NOTE: This could potentially be a class in the future and not just a struct.
                            if (IsStruct(p.Type))
                            {
                                var f = structs[p.Type].Fields.Cast<FieldNode>().FirstOrDefault(a => a.Name == node.Accessor.ValueText);
                                if (f != null) node.Accessor.Type = f.Type;
                                node.LastAccessor.Type = p.Type;
                            }
                        }
                    }
                }

                if (node.Accessor is MemberAccessNode) (node.Accessor as MemberAccessNode).MemberType = node.Accessor.Type;
                if (node.LastAccessor is MemberAccessNode) (node.LastAccessor as MemberAccessNode).MemberType = node.LastAccessor.Type;
                node.Type = node.ResultType = node.Accessor.Type;
            }
            return node;
        }


        private BinaryOperatorNode Process(BinaryOperatorNode node)
        {
            Process(node.Left);
            Process(node.Right);
            return node;
        }

        private IfElseNode Process(IfElseNode node)
        {
            Process(node.GetCondition());
            Process(node.GetTrue());
            Process(node.GetFalse());
            return node;
        }

        private DefineVariableNode Process(DefineVariableNode node)
        {
            // as fallback, always set to 'any' if for some reason it isnt already.
            if (node.Type == "void") node.SetType("any");
            if (node.Type != "any" || node.AssignmentExpression == null)
            {
                AddVariable(node);
                return node; // since we only want to check those that has been flagged as 'any', return now!
            }

            var literalNode = node.AssignmentExpression as LiteralNode;
            if (literalNode != null && (literalNode.NodeName == literalNode.ValueText || literalNode.NodeName == "NAME"))
            {
                // identity
                var identity = literalNode.ValueText;
                if (IsStruct(identity))
                {
                    node.SetValue(new CreateStructNode(structs[identity], literalNode.Index));
                    node.Type = identity;
                }

            }
            else if (node.AssignmentExpression.Type != null && node.AssignmentExpression.Type != node.Type)
                node.Type = node.AssignmentExpression.Type;

            AddVariable(node);
            return node;
        }

        private void AddVariable(DefineVariableNode node)
        {
            if (insideFunction)
            {
                localVariables.Add(node.Name, node);
                return;
            }

            globalVariables.Add(node.Name, node);
        }

        private bool IsStruct(string identity)
        {
            return structs.ContainsKey(identity);
        }

        private FunctionNode Process(FunctionNode node)
        {
            parameters.Clear();
            localVariables.Clear();
            insideFunction = true;
            if (node.Type == null) node.Type = "any";
            if (node.ReturnType != null && node.ReturnType.Name != node.Type) node.Type = node.ReturnType.Name;
            if (!functions.ContainsKey(node.Name)) functions.Add(node.Name, node);

            foreach (var p in node.Parameters.Parameters)
            {
                parameters.Add(p.Name, p);
            }

            Process(node.Body);
            insideFunction = false;
            return node;
        }

        private StructNode Process(StructNode node)
        {
            if (node.Type == null) node.Type = node.Name;
            if (!structs.ContainsKey(node.Name)) structs.Add(node.Name, node);
            return node;
        }

        private DefineVariableNode Variable(string name)
        {
            if (globalVariables.ContainsKey(name)) return globalVariables[name];
            if (localVariables.ContainsKey(name)) return localVariables[name];
            return null;
        }

        private ParameterNode Parameter(string name)
        {
            if (parameters.ContainsKey(name)) return parameters[name];
            return null;
        }
    }
}