using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Ast.Nodes;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.Interpreter.Tests
{
    internal static class Test_Helper_Extensions
    {
        public static IReadOnlyList<XzaarToken> Tokenize(this string code)
        {
            return new XzaarScriptLexer(code).Tokenize();
        }

        public static XzaarSyntaxNode Parse(this IReadOnlyList<XzaarToken> tokens)
        {
            return new XzaarSyntaxParser(tokens).Parse();
        }

        public static EntryNode Transform(this XzaarSyntaxNode ast)
        {
            return new XzaarNodeTransformer(ast).Transform() as EntryNode;
        }


        public static XzaarAnalyzedTree AnalyzeExpression(this EntryNode node)
        {
            return new XzaarExpressionAnalyzer().AnalyzeExpression(node);
        }

        public static XzaarAssembly Compile(this XzaarAnalyzedTree tree)
        {
            return XzaarScriptCompiler.Compile(tree);
        }

        public static XzaarRuntime Run(this XzaarAssembly asm)
        {
            return XzaarVirtualMachine.Run(asm);
        }

        public static XzaarRuntime Runtime(this XzaarAssembly asm)
        {
            return XzaarVirtualMachine.Load(asm);
        }

        public static string AssemblyCode(this XzaarAssembly tree)
        {
            return XzaarScriptDisassembler.Disassemble(tree);
        }
    }
}
