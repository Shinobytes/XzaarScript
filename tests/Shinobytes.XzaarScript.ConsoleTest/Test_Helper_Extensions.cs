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
 
using System.Collections.Generic;
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Ast;
using Shinobytes.XzaarScript.Ast.Compilers;
using Shinobytes.XzaarScript.Ast.Nodes;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.ConsoleTest
{
    public static class Test_Helper_Extensions
    {
        public static XzaarRuntime Run(this string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .Transform()
                .AnalyzeExpression()
                .Compile()
                .Run();
        }

        public static XzaarRuntime Load(this string inputCode)
        {
            return inputCode
                .Tokenize()
                .Parse()
                .Transform()
                .AnalyzeExpression()
                .Compile()
                .Load();
        }

        public static IReadOnlyList<XzaarToken> Tokenize(this string code)
        {
            return new XzaarScriptLexer(code).Tokenize();
        }

        public static XzaarSyntaxNode Parse(this IReadOnlyList<XzaarToken> tokens)
        {
            return new XzaarSyntaxParser(tokens).Parse();
        }

        public static EntryNode Transform(this XzaarSyntaxNode tokens)
        {
            return new XzaarNodeTransformer(tokens).Transform() as EntryNode;
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
