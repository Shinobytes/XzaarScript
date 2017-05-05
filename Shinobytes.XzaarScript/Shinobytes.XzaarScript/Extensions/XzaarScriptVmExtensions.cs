using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Ast.Nodes;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.Extensions
{
    public static class XzaarScriptVmExtensions
    {
        public static IList<XzaarToken> Tokenize(this string code)
        {
            var lexer = new XzaarScriptLexer(code);
            var result = lexer.Tokenize();
            if (lexer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static IList<XzaarToken> Tokenize(this string code, out IList<string> errors)
        {
            var lexer = new XzaarScriptLexer(code);
            var result = lexer.Tokenize();
            errors = lexer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static XzaarSyntaxNode Parse(this IList<XzaarToken> tokens)
        {
            var parser = new XzaarSyntaxParser(tokens);
            var result = parser.Parse();
            if (parser.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, parser.Errors.ToArray()));
            return result;
        }

        public static XzaarSyntaxNode Parse(this IList<XzaarToken> tokens, out IList<string> errors)
        {
            var parser = new XzaarSyntaxParser(tokens);
            var result = parser.Parse();
            errors = parser.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result;
        }

        public static EntryNode Transform(this XzaarSyntaxNode tokens)
        {
            var transformer = new XzaarNodeTransformer(tokens);
            var result = transformer.Transform() as EntryNode;
            if (transformer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, transformer.Errors.ToArray()));
            return result;
        }

        public static EntryNode Transform(this XzaarSyntaxNode tokens, out IList<string> errors)
        {
            var transformer = new XzaarNodeTransformer(tokens);
            var result = transformer.Transform() as EntryNode;
            errors = transformer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result;
        }

        public static XzaarAnalyzedTree AnalyzeExpression(this EntryNode node)
        {
            IList<string> errors;
            return AnalyzeExpression(node, out errors);
        }


        public static XzaarAnalyzedTree AnalyzeExpression(this EntryNode node, out IList<string> errors)
        {
            return new XzaarExpressionAnalyzer().AnalyzeExpression(node, out errors);
        }


        public static Delegate CompileToDotNet(this XzaarAnalyzedTree tree)
        {
            return DotNetXzaarScriptCompiler.Compile(tree);
        }

        public static XzaarAssembly Compile(this XzaarAnalyzedTree tree)
        {
            return XzaarScriptCompiler.Compile(tree);
        }

        public static XzaarAssembly Compile(this XzaarAnalyzedTree tree, out List<string> errors)
        {
            return XzaarScriptCompiler.Compile(tree, out errors);
        }


        public static XzaarRuntime Run(this XzaarAssembly asm)
        {
            return XzaarVirtualMachine.Run(asm);
        }
        public static XzaarRuntime Load(this XzaarAssembly asm)
        {
            return XzaarVirtualMachine.Load(asm);
        }

        public static string AssemblyCode(this XzaarAssembly tree)
        {
            return XzaarScriptDisassembler.Disassemble(tree);
        }
    }
}