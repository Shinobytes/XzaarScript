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
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript.Compiler
{
    internal class ScriptDiscoveryVisitor : IReferenceBinder
    {
        private ScriptCompilerContext ctx;

        public ScriptDiscoveryVisitor(ScriptCompilerContext ctx)
        {
            this.ctx = ctx;
        }

        public object Visit(XzaarExpression expression)
        {
            // TODO(Zerratar): Dependecy Inject a stack guard validation object to do take care of this for us
            if (ctx.StackRecursionCount > 10)
                throw new CompilerException("PANIC!! Stackoverflow! " + expression.GetType() + " [" + expression.NodeType + "] not implemented");

            if (ctx.LastVisited == expression)
                ctx.StackRecursionCount++;
            else
                ctx.StackRecursionCount = 0;

            ctx.LastVisited = expression;

            if (expression.NodeType == ExpressionType.Block)
            {
                var block = expression as BlockExpression;
                if (block != null)
                {
                    foreach (var item in block.Expressions) Visit(item);
                }
                return null;
            }

            // NOTE: Blueh! If only we could have used dynamics here, that would have saved us so many darn lines :P But since we are compiling for 3.5, thats not possible
            // 

//#if UNITY
            if (expression is LambdaExpression lambda) return Visit(lambda);
            if (expression is UnaryExpression unaryExpression) return Visit(unaryExpression);
            if (expression is IfElseExpression elseExpression) return Visit(elseExpression);
            if (expression is ConditionalExpression conditionalExpression) return Visit(conditionalExpression);
            if (expression is GotoExpression gotoExpression) return Visit(gotoExpression);
            if (expression is MemberExpression memberExpression) return Visit(memberExpression);
            if (expression is BinaryExpression binaryExpression) return Visit(binaryExpression);
            if (expression is MemberAccessChainExpression chainExpression) return Visit(chainExpression);
            if (expression is ForExpression forExpression) return Visit(forExpression);
            if (expression is ForEachExpression eachExpression) return Visit(eachExpression);
            if (expression is SwitchCaseExpression caseExpression) return Visit(caseExpression);
            if (expression is DoWhileExpression whileExpression) return Visit(whileExpression);
            if (expression is LoopExpression loopExpression) return Visit(loopExpression);
            if (expression is SwitchExpression switchExpression) return Visit(switchExpression);
            if (expression is CreateStructExpression structExpression) return Visit(structExpression);
            if (expression is ConstantExpression constantExpression) return Visit(constantExpression);
            if (expression is FunctionCallExpression callExpression) return Visit(callExpression);
            if (expression is VariableDefinitionExpression definitionExpression) return Visit(definitionExpression);
            if (expression is FieldExpression fieldExpression) return Visit(fieldExpression);
            if (expression is LabelExpression labelExpression) return Visit(labelExpression);
            if (expression is ParameterExpression parameterExpression) return Visit(parameterExpression);
            if (expression is FunctionExpression functionExpression) return Visit(functionExpression);
            if (expression is StructExpression expression1) return Visit(expression1);
            return Visit(expression);
//#else
//            return Visit((dynamic)expression);
//#endif

        }


        public object Visit(LambdaExpression lambda)
        {
            return null;
        }


        public object Visit(GotoExpression @goto)
        {
            // global gotos not supported yet
            return null;
        }

        public object Visit(MemberExpression member)
        {
            return null;
        }

        public object Visit(BinaryExpression binary)
        {
            return null;
        }

        public object Visit(MemberAccessChainExpression chain)
        {
            return null;
        }

        public object Visit(ForExpression node)
        {
            if (node.Initiator != null)
            {
                Visit(node.Initiator);
            }
            return null;
        }

        public object Visit(ForEachExpression node)
        {
            if (node.Variable != null) Visit(node.Variable);
            if (node.Body != null) Visit(node.Body);
            return null;
        }

        public object Visit(SwitchCaseExpression node)
        {
            if (node.Body != null) Visit(node.Body);
            return null;
        }

        public object Visit(DoWhileExpression node)
        {
            if (node.Body != null) Visit(node.Body);
            return null;
        }

        public object Visit(LoopExpression node)
        {
            if (node.Body != null) Visit(node.Body);
            return null;
        }

        public object Visit(SwitchExpression node)
        {
            if (node.Cases != null && node.Cases.Length > 0)
            {
                foreach (var c in node.Cases) Visit(c);
            }
            return null;
        }

        public object Visit(CreateStructExpression node)
        {
            return null;
        }

        public object Visit(IfElseExpression node)
        {
            return null;
        }

        public VariableReference Visit(ConditionalExpression node)
        {
            return new VariableReference()
            {
                Name = null,
                Type = TypeLookup(node.Type, null)
            };
        }

        public VariableReference Visit(ConstantExpression constant)
        {
            return new VariableReference()
            {
                Name = null,
                Type = TypeLookup(constant.Type, null)
            };
        }

        public VariableReference Visit(FunctionCallExpression function)
        {
            if ((object)function.Type != null)
                return new VariableDefinition { Name = null, Type = TypeLookup(function.Type, null) };

            var f = ctx.Assembly.GlobalMethods.FirstOrDefault(m => m.Name == function.MethodName);
            if (f != null)
                return new VariableDefinition { Name = null, Type = f.ReturnType };
            return new VariableDefinition { Name = null, Type = TypeLookup(XzaarBaseTypes.Void, null) };
        }

        public VariableDefinition Visit(VariableDefinitionExpression definedVariable)
        {
            if (definedVariable.AssignmentExpression != null)
            {
                var typeInfo = Visit(definedVariable.AssignmentExpression) as VariableReference;
                if (typeInfo != null)
                {
                    var pdef = new VariableDefinition
                    {
                        Name = definedVariable.Name,
                        Type = typeInfo.Type
                    };
                    ctx.Assembly.GlobalVariables.Add(pdef);
                    return pdef;
                }
            }

            var propertyDefinition = new VariableDefinition
            {
                Name = definedVariable.Name,
                Type = TypeLookup(definedVariable.Type, XzaarBaseTypes.Any)
            };
            ctx.Assembly.GlobalVariables.Add(propertyDefinition);
            return propertyDefinition;
        }

        public VariableDefinition Visit(FieldExpression field)
        {
            var name = field.Name;
            return new VariableDefinition
            {
                ArrayIndex = null,
                InitialValue = null,
                Name = name,
                Type = TypeLookup(field.Type, null)
            };
        }


        public VariableReference Visit(UnaryExpression unary)
        {
            // TODO: Does this need to be implemented? (can't remember)
            return null;
        }

        public Label Visit(LabelExpression label)
        {
            // global labels not supported yet            
            throw new NotImplementedException();
        }

        public ParameterDefinition Visit(ParameterExpression parameter)
        {
            return new ParameterDefinition
            {
                Name = parameter.Name,
                Type = TypeLookup(parameter.Type, null)
            };
        }

        public MethodDefinition Visit(FunctionExpression function)
        {
            if (ctx.CurrentType == null)
            {
                // global function
                var parameters = function.Parameters.Select(Visit).ToArray();
                var method = new MethodDefinition(function.Name, parameters, function.IsExtern);

                method.SetReturnType(TypeLookup(function.ReturnType, null));

                ctx.Assembly.GlobalMethods.Add(method);

                return method;
            }
            else
            {
                // instance function :: not implemented yet
            }
            return null;
        }

        public TypeDefinition Visit(StructExpression node)
        {
            var typeName = node.Name;
            var newStruct = new TypeDefinition(typeName, true);
            var items = node.Fields.Select(Visit).Cast<VariableReference>();
            foreach (var item in items)
                newStruct.Fields.Add(item);
            ctx.KnownTypes.Add(newStruct);
            ctx.Assembly.Types.Add(newStruct);
            return newStruct;
        }


        private TypeReference TypeLookup(XzaarType target, XzaarType fallbackType)
        {

            var targetTypeName = "any";
            if ((object)target != null)
            {
                targetTypeName = target.Name;
            }
            if (targetTypeName == "any" && (object)fallbackType != null)
            {
                targetTypeName = fallbackType.Name;
            }

            var possibleType = ctx.KnownTypes.FirstOrDefault(t => t.Name == targetTypeName);
            if (possibleType != null) return possibleType;
            if (target != null)
            {
                possibleType = new TypeDefinition(target);
                ctx.KnownTypes.Add(possibleType);
                return possibleType;
            }

            if (target != null && fallbackType == null)
            {
                var typedef = new TypeDefinition(target);
                ctx.KnownTypes.Add(typedef);
                return typedef;
            }


            return TypeLookup(fallbackType, null);
        }
    }
}