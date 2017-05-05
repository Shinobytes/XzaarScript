using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.Compiler.Compilers;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.Extensions
{
    public static class ScriptVmExtensions
    {
        public static IList<Token> Tokenize(this string code)
        {
            var lexer = new Lexer(code);
            var result = lexer.Tokenize();
            if (lexer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static IList<Token> Tokenize(this string code, out IList<string> errors)
        {
            var lexer = new Lexer(code);
            var result = lexer.Tokenize();
            errors = lexer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static SyntaxNode Parse(this IList<Token> tokens)
        {
            var parser = new SyntaxParser(tokens);
            var result = parser.Parse();
            if (parser.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, parser.Errors.ToArray()));
            return result;
        }

        public static SyntaxNode Parse(this IList<Token> tokens, out IList<string> errors)
        {
            var parser = new SyntaxParser(tokens);
            var result = parser.Parse();
            errors = parser.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result;
        }

        public static EntryNode Transform(this SyntaxNode tokens)
        {
            var transformer = new NodeParser(tokens);
            var result = transformer.Transform() as EntryNode;
            if (transformer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, transformer.Errors.ToArray()));
            return result;
        }

        public static EntryNode Transform(this SyntaxNode tokens, out IList<string> errors)
        {
            var transformer = new NodeParser(tokens);
            var result = transformer.Transform() as EntryNode;
            errors = transformer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result;
        }

        public static AnalyzedTree AnalyzeExpression(this EntryNode node)
        {
            IList<string> errors;
            return AnalyzeExpression(node, out errors);
        }


        public static AnalyzedTree AnalyzeExpression(this EntryNode node, out IList<string> errors)
        {
            return new ExpressionAnalyzer().AnalyzeExpression(node, out errors);
        }


        public static Delegate CompileToDotNet(this AnalyzedTree tree)
        {
            return DotNetXzaarScriptCompiler.Compile(tree);
        }

        public static XzaarAssembly Compile(this AnalyzedTree tree)
        {
            return ScriptCompiler.Compile(tree);
        }

        public static XzaarAssembly Compile(this AnalyzedTree tree, out List<string> errors)
        {
            return ScriptCompiler.Compile(tree, out errors);
        }


        public static Runtime Run(this XzaarAssembly asm)
        {
            return VirtualMachine.Run(asm);
        }
        public static Runtime Load(this XzaarAssembly asm)
        {
            return VirtualMachine.Load(asm);
        }

        public static string AssemblyCode(this XzaarAssembly tree)
        {
            return ScriptDisassembler.Disassemble(tree);
        }
    }
}