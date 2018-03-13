/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Parser.Nodes;

namespace Shinobytes.XzaarScript.Parser.Ast
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
//#if UNITY
                if (syntaxTree is DefineVariableNode node) return Process(node);
                if (syntaxTree is FunctionNode functionNode) return Process(functionNode);
                if (syntaxTree is StructNode structNode) return Process(structNode);
                if (syntaxTree is IfElseNode elseNode) return Process(elseNode);
                if (syntaxTree is LoopNode loopNode) Process(loopNode.Body);
                if (syntaxTree is BinaryOperatorNode operatorNode) return Process(operatorNode);
                if (syntaxTree is AssignNode assignNode) return Process(assignNode);
                if (syntaxTree is MemberAccessNode accessNode) return Process(accessNode);
                if (syntaxTree is MemberAccessChainNode chainNode) return Process(chainNode);
//#else 
//                return Process((dynamic)syntaxTree);
//#endif
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

                    var v = Variable(node.LastAccessor.StringValue);
                    if (v != null)
                    {
                        // NOTE: This could potentially be a class in the future and not just a struct.
                        if (IsStruct(v.Type))
                        {
                            var f = structs[v.Type].Fields.Cast<FieldNode>().FirstOrDefault(a => a.Name == node.Accessor.StringValue);
                            if (f != null) node.Accessor.Type = f.Type;
                            node.LastAccessor.Type = v.Type;
                        }
                    }
                    else
                    {
                        var p = Parameter(node.LastAccessor.StringValue);
                        if (p != null)
                        {
                            // declaring type                            
                            // NOTE: This could potentially be a class in the future and not just a struct.
                            if (IsStruct(p.Type))
                            {
                                var f = structs[p.Type].Fields.Cast<FieldNode>().FirstOrDefault(a => a.Name == node.Accessor.StringValue);
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

            if (node.AssignmentExpression is LiteralNode literalNode 
                && (literalNode.NodeName == literalNode.StringValue || literalNode.NodeName == "NAME"))
            {
                // identity
                var identity = literalNode.StringValue;
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

            if (node.Type == null)
            {
                node.Type = "any";
            }

            if (node.ReturnType != null && node.ReturnType.Name != node.Type)
            {
                node.Type = node.ReturnType.Name;
            }

            if (!functions.ContainsKey(node.Name))
            {
                functions.Add(node.Name, node);
            }

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
            return parameters.ContainsKey(name) ? parameters[name] : null;
        }
    }
}