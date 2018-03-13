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
 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Transpilers;

namespace Shinobytes.XzaarScript.UnitTests
{
    [TestClass]
    public class XzaarScript_Transpiler_ES6
    {
        [TestMethod]
        public void assign_variable_singlequoute_string()
        {
            var result = Transpile("let test = 'hello world';");
            Assert.AreEqual("let test = 'hello world';", result);
        }

        [TestMethod]
        public void loop_creates_while_true()
        {
            var result = Transpile("loop { }");
            Assert.AreEqual("while (true) {\r\n}", result);
        }

        [TestMethod]
        public void while_true_creates_while_true()
        {
            var result = Transpile("while(true) { }");
            Assert.AreEqual("while (true) {\r\n}", result);
        }

        [TestMethod]
        public void empty_struct_creates_empty_class()
        {
            var result = Transpile("struct test { }");
            Assert.AreEqual("class test {\r\n}", result);
        }

        [TestMethod]
        public void struct_with_one_field_creates_class_and_constructor()
        {
            var result = Transpile("struct test { str:string }");
            Assert.AreEqual("class test {\r\n  constructor(str) {\r\n    this.str = str;\r\n  }\r\n}", result);
        }

        [TestMethod]
        public void empty_fn_creates_function()
        {
            var result = Transpile("fn test(monkey:string) -> void { }");
            Assert.AreEqual("function test(monkey) {\r\n}", result);
        }

        [TestMethod]
        public void fn_creates_function()
        {
            var result = Transpile("fn test(monkey:string) -> void { foreach (let char in monkey) { } }");
            Assert.AreEqual("function test(monkey) {\r\n  for (let char of monkey) {\r\n  }\r\n}", result);
        }


        [TestMethod]
        public void loop_array()
        {
            var result = Transpile("foreach(let item in [1, 2,3, 5]) {}");
            Assert.AreEqual("for (let item of [1, 2, 3, 5]) {\r\n}", result);
        }
        
        private string Transpile(string source)
        {
            var ast = Parser.SyntaxParser.Parse(source);
            var expression = new Compiler.ExpressionCompiler().Compile(ast);
            var result = new Transpilers.ES6.Transpiler(QuoutePreference.SingleQuoute).Transpile(expression);
            return result.TranspiledCode;
        }
    }
}
