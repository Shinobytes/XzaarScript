using System;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Assembly.Models;
using Shinobytes.XzaarScript.Ast.Expressions;

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
            // TODO: Dependecy Inject a stack guard validation object to do take care of this for us
            if (ctx.StackRecursionCount > 10)
                throw new CompilerException("PANIC!! Stackoverflow! " + expression.GetType() + " [" + expression.NodeType + "] not implemented");

            if (ctx.lastVisited == expression)
                ctx.StackRecursionCount++;
            else
                ctx.StackRecursionCount = 0;

            ctx.lastVisited = expression;

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

#if UNITY
            if (expression is UnaryExpression) return Visit(expression as UnaryExpression);
            if (expression is ConditionalExpression) return Visit(expression as ConditionalExpression);
            if (expression is GotoExpression) return Visit(expression as GotoExpression);
            //object Visit(GotoExpression @goto);
            if (expression is MemberExpression) return Visit(expression as MemberExpression);
            //object Visit(MemberExpression member);
            if (expression is BinaryExpression) return Visit(expression as BinaryExpression);
            //object Visit(BinaryExpression binary);
            if (expression is MemberAccessChainExpression) return Visit(expression as MemberAccessChainExpression);
            //object Visit(MemberAccessChainExpression chain);
            if (expression is ForExpression) return Visit(expression as ForExpression);
            //object Visit(ForExpression node);
            if (expression is ForEachExpression) return Visit(expression as ForEachExpression);
            //object Visit(ForEachExpression node);
            if (expression is SwitchCaseExpression) return Visit(expression as SwitchCaseExpression);
            //object Visit(SwitchCaseExpression node);
            if (expression is DoWhileExpression) return Visit(expression as DoWhileExpression);
            //object Visit(DoWhileExpression node);
            if (expression is LoopExpression) return Visit(expression as LoopExpression);
            //object Visit(LoopExpression node);
            if (expression is SwitchExpression) return Visit(expression as SwitchExpression);
            //object Visit(SwitchExpression node);
            if (expression is CreateStructExpression) return Visit(expression as CreateStructExpression);
            //object Visit(CreateStructExpression node);
            if (expression is ConstantExpression) return Visit(expression as ConstantExpression);
            //VariableReference Visit(ConstantExpression constant);
            if (expression is FunctionCallExpression) return Visit(expression as FunctionCallExpression);
            //VariableReference Visit(FunctionCallExpression function);
            if (expression is VariableDefinitionExpression) return Visit(expression as VariableDefinitionExpression);
            //VariableDefinition Visit(VariableDefinitionExpression definedVariable);
            if (expression is FieldExpression) return Visit(expression as FieldExpression);
            //VariableDefinition Visit(FieldExpression field);
            if (expression is LabelExpression) return Visit(expression as LabelExpression);
            //Label Visit(LabelExpression label);
            if (expression is ParameterExpression) return Visit(expression as ParameterExpression);
            //ParameterDefinition Visit(ParameterDefinitionExpression parameter);
            if (expression is FunctionExpression) return Visit(expression as FunctionExpression);
            //MethodDefinition Visit(FunctionExpression function);
            if (expression is StructExpression) return Visit(expression as StructExpression);
            //TypeDefinition Visit(StructExpression node);
            return Visit(expression);
            // return Visit((dynamic)expression);
#else
            return Visit((dynamic)expression);
#endif

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

        public object Visit(ConditionalExpression node)
        {
            return null;
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
                var parameters = function.GetParameters().Select(Visit).ToArray();
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

            return TypeLookup(fallbackType, null);
        }
    }
}