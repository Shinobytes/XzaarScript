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
        public static IList<SyntaxToken> Tokenize(this string code)
        {
            var lexer = new Lexer(code, false);
            var result = lexer.Tokenize();
            if (lexer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static IList<SyntaxToken> Tokenize(this string code, out IList<string> errors)
        {
            var lexer = new Lexer(code, false);
            var result = lexer.Tokenize();
            errors = lexer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static EntryNode Parse(this IList<SyntaxToken> tokens)
        {
            var parser = new LanguageParser(tokens);
            var result = parser.Parse() as EntryNode;
            if (parser.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, parser.Errors.ToArray()));
            return result;
        }

        public static EntryNode Parse(this IList<SyntaxToken> tokens, out IList<string> errors)
        {
            var parser = new LanguageParser(tokens); // new SyntaxParser(tokens);
            var result = parser.Parse();
            errors = parser.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result as EntryNode;
        }

        public static EntryNode Transform(this IList<SyntaxToken> tokens)
        {
            var transformer = new LanguageParser(tokens);
            var result = transformer.Parse() as EntryNode;
            if (transformer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, transformer.Errors.ToArray()));
            return result;
        }

        public static EntryNode Transform(this IList<SyntaxToken> tokens, out IList<string> errors)
        {
            var transformer = new LanguageParser(tokens);
            var result = transformer.Parse() as EntryNode;
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
            var analyzeExpression = new ExpressionAnalyzer().AnalyzeExpression(node, out errors);
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return analyzeExpression;
        }


        public static Delegate CompileToDotNet(this AnalyzedTree tree)
        {
            return DotNetXzaarScriptCompiler.Compile(tree);
        }

        public static XzaarAssembly Compile(this AnalyzedTree tree)
        {
            var val = ScriptCompiler.Compile(tree, out var errors);
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return val;
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