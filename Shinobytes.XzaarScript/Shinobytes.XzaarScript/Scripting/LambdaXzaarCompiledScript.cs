using System;
using System.Linq.Expressions;

namespace Shinobytes.XzaarScript.Scripting
{
    public class LambdaXzaarCompiledScript : XzaarCompiledScriptBase
    {
        private readonly Expression expression;

        private readonly Delegate program;

        public LambdaXzaarCompiledScript(Expression expression)
        {            
            this.expression = expression;
            var lambdaExpression = this.expression as LambdaExpression;
            if (lambdaExpression != null)
                program = lambdaExpression.Compile();
        }

        public LambdaExpression GetLambdaExpression() => expression as LambdaExpression;

        public override T Invoke<T>(params object[] args)
        {
            return (T)program?.DynamicInvoke(args);
        }
    }
}