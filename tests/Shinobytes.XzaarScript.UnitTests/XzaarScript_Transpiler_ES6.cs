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
