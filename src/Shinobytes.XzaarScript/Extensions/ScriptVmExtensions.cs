/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

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
            var val = ScriptCompiler.Compile(expression, out var errors);
            if (errors.Count > 0) throw new Exception(string.Join(Environment.NewLine, errors.ToArray()));
            return val;
        }

        public static XzaarAssembly Compile(this XzaarExpression expression, out IList<string> errors)
        {
            return ScriptCompiler.Compile(expression, out errors);
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