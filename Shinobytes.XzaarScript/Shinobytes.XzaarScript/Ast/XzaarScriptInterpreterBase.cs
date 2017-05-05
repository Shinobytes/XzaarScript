using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Extensions;

namespace Shinobytes.XzaarScript.Ast
{
    public abstract class XzaarScriptInterpreterBase
    {
        private readonly XzaarScriptLexerOld lexer;
        private readonly XzaarScriptParserOld parser;
        private readonly IXzaarScriptTransformer transformer;
        private readonly IXzaarScriptCompiler compiler;

        protected XzaarScriptInterpreterBase(
            XzaarScriptLexerOld lexer,
            XzaarScriptParserOld parser,
            IXzaarScriptTransformer transformer,
            IXzaarScriptCompiler compiler)
        {
            this.lexer = lexer;
            this.parser = parser;
            this.transformer = transformer;
            this.compiler = compiler;
        }

        public XzaarCompiledScriptBase CompileExpression(string expression)
        {
            try
            {
                var tokens = lexer.Tokenize(expression);
                var ast = parser.Parse(tokens);
                var newAst = transformer.Transform(ast);
                return compiler.Compile(newAst);
            }
            catch(Exception exc)
            {
                return null;
            }
        }

        public void RegisterVariable<T>(string variableName)
        {
            compiler.RegisterVariable<T>(variableName, XzaarScope.Global);
        }

        public void RegisterVariable<T>(string variableName, XzaarScope scope)
        {
            compiler.RegisterVariable<T>(variableName, scope);
        }

        protected void RegisterVariables(KeyValuePair<string, Type>[] variables)
        {
            variables.ForEach(p => compiler.RegisterVariable(p.Key, p.Value, XzaarScope.Global));
        }

        protected void RegisterVariables(KeyValuePair<string, Type>[] variables, XzaarScope scope)
        {
            variables.ForEach(p => compiler.RegisterVariable(p.Key, p.Value, scope));
        }
    }
}