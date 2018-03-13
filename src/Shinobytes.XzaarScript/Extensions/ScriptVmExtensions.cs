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
using Shinobytes.XzaarScript.Assembly;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Parser.Ast;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;
using Shinobytes.XzaarScript.Parser.Nodes;
using Shinobytes.XzaarScript.Tools;
using Shinobytes.XzaarScript.VM;

namespace Shinobytes.XzaarScript.Extensions
{
    public static class ScriptVmExtensions
    {
        public static IList<SyntaxToken> Tokenize(this string code)
        {
            var lexer = new Lexer(code);
            var result = lexer.Tokenize();
            if (lexer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static IList<SyntaxToken> Tokenize(this string code, out IList<string> errors)
        {
            var lexer = new Lexer(code);
            var result = lexer.Tokenize();
            errors = lexer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, lexer.Errors.ToArray()));
            return result;
        }

        public static EntryNode Parse(this IList<SyntaxToken> tokens)
        {
            var parser = new SyntaxParser(tokens);
            var result = parser.Parse() as EntryNode;
            if (parser.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, parser.Errors.ToArray()));
            return result;
        }

        public static EntryNode Parse(this IList<SyntaxToken> tokens, out IList<string> errors)
        {
            var parser = new SyntaxParser(tokens);
            var result = parser.Parse();
            errors = parser.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result as EntryNode;
        }

        public static EntryNode Transform(this IList<SyntaxToken> tokens)
        {
            var transformer = new SyntaxParser(tokens);
            var result = transformer.Parse() as EntryNode;
            if (transformer.Errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, transformer.Errors.ToArray()));
            return result;
        }

        public static EntryNode Transform(this IList<SyntaxToken> tokens, out IList<string> errors)
        {
            var transformer = new SyntaxParser(tokens);
            var result = transformer.Parse() as EntryNode;
            errors = transformer.Errors;
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return result;
        }

        public static XzaarExpression CompileExpression(this EntryNode node)
        {
            var compiler = new ExpressionCompiler();
            var expression = compiler.Compile(node);
            if (compiler.Errors.Count > 0)
            {
                throw new Exception(string.Join(Environment.NewLine, compiler.Errors.ToArray()));
            }
            return expression;
        }

        public static Delegate CompileToDotNet(this XzaarExpression expression)
        {
            return DotNetXzaarScriptCompiler.Compile(expression);
        }

        public static XzaarAssembly Compile(this XzaarExpression expression)
        {
            var val = XSVMCompiler.Compile(expression, out var errors);
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return val;
        }

        public static XzaarAssembly Compile(this XzaarExpression expression, out IList<string> errors)
        {
            return XSVMCompiler.Compile(expression, out errors);
        }

        public static Runtime Run(this XzaarAssembly asm)
        {
            return VirtualMachine.Run(asm);
        }

        public static Runtime Run(this XzaarAssembly asm, RuntimeSettings settings)
        {
            return VirtualMachine.Run(asm, settings);
        }

        public static Runtime Load(this XzaarAssembly asm)
        {
            return VirtualMachine.Load(asm, RuntimeSettings.Default);
        }

        public static Runtime Load(this XzaarAssembly asm, RuntimeSettings settings)
        {
            return VirtualMachine.Load(asm, settings);
        }

        public static string Disassemble(this XzaarAssembly tree)
        {
            return XzaarScriptDisassembler.Disassemble(tree);
        }
    }
}