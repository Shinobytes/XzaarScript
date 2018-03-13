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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.XzaarScript.Compiler;
using Shinobytes.XzaarScript.Parser;
using Shinobytes.XzaarScript.Tools;

namespace Shinobytes.XzaarScript.UnitTests
{
    [TestClass]
    public class XzaarScript_Parser_TypeLookup
    {
        [TestMethod]
        public void conditional_assign()
        {
            var code = ParseWithTypeLookup("let i = 1 > 0 ? true : false", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var i = 1 > 0 ? True : False", code);
        }

        [TestMethod]
        public void invoke_function_with_conditional()
        {
            var code = ParseWithTypeLookup("fn test(a:bool) -> void { } test(1 > 0 ? true : false)", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"fn test(a:bool) {
}
test(1 > 0 ? True : False)", code);
        }


        [TestMethod]
        public void variable_assigned_struct()
        {
            var code = ParseWithTypeLookup("struct hello { test : number } let j = hello", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct hello {
  number test
}
var j = hello", code);
        }


        [TestMethod]
        public void Invoke_simple_mathematical_expression()
        {
            var code = ParseWithTypeLookup("let result = 5 + (1 * 1) - 992", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var result = 5 + 1 * 1 - 992", code);
        }


        [TestMethod]
        public void variable_assigned_struct_2()
        {
            var code = ParseWithTypeLookup("struct hello { test : number } let j j = hello", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct hello {
  number test
}
var j
j = hello", code);
        }

        [TestMethod]
        public void variable_assigned_array_empty()
        {
            var code = ParseWithTypeLookup("let j = []", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var j = []", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer()
        {
            var code = ParseWithTypeLookup("let j = ['hello', 'wha']", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = [\"hello\", \"wha\"]", code);
        }


        [TestMethod]
        public void variable_assigned_array_empty_2()
        {
            var code = ParseWithTypeLookup("let j j = []", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var j
j = []", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer_2()
        {
            var code = ParseWithTypeLookup("let j j = ['hello', 'wha']", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"]", code);
        }

        [TestMethod]
        public void variable_assigned_array_initializer_3()
        {
            var code = ParseWithTypeLookup("let j j = ['hello', 'wha'].Length", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"].Length", code);
        }


        [TestMethod]
        public void variable_assigned_array_initializer_4()
        {
            var code = ParseWithTypeLookup("let j j = ['hello', 'wha'][0]", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"][0]", code);
        }


        [TestMethod]
        public void variable_assigned_array_initializer_5()
        {
            var code = ParseWithTypeLookup("let j j = ['hello', 'wha'][0].Length", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = [\"hello\", \"wha\"][0].Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_1()
        {
            var code = ParseWithTypeLookup("let j j = 'test'[0]", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = \"test\"[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_2()
        {
            var code = ParseWithTypeLookup("let j = 'test'[0]", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = \"test\"[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_3()
        {
            var code = ParseWithTypeLookup("let j j = 1[0]", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = 1[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_element_access_4()
        {
            var code = ParseWithTypeLookup("let j = 1[0]", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = 1[0]", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_1()
        {
            var code = ParseWithTypeLookup("let j j = 'test'.Length", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = \"test\".Length", code);
        }


        [TestMethod]
        public void variable_assigned_constant_property_2()
        {
            var code = ParseWithTypeLookup("let j = 'test'.Length", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = \"test\".Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_3()
        {
            var code = ParseWithTypeLookup("let j j = 0.Length", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j" + Environment.NewLine +
                            "j = 0.Length", code);
        }

        [TestMethod]
        public void variable_assigned_constant_property_4()
        {
            var code = ParseWithTypeLookup("let j = 0.Length", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = 0.Length", code);
        }

        [TestMethod]
        public void for_loop()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i] = i } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    s.hello[i] = i
  }
  return s.hello
}", code);
        }

        [TestMethod]
        public void for_break_loop()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { break } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    break
  }
  return s.hello
}", code);
        }

        [TestMethod]
        public void Declare_struct_and_create_new_instance_with_initializer()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = ParseWithTypeLookup(@"let console = $console
struct test_struct {
  number my_number
}

let a = test_struct { my_number = 1000 }
console.log(a.my_number)", out SyntaxParser parser);

            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var console = $console
struct test_struct {
  number my_number
}
var a = test_struct { my_number = 1000 }
console.log(a.my_number)", code);
        }


        [TestMethod]
        public void Declare_struct_and_create_new_instance_with_initializer_1()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = ParseWithTypeLookup(@"let console = $console
struct test_struct {
  number my_number
}

let a = test_struct { my_number: 1000 }
console.log(a.my_number)", out SyntaxParser parser);

            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var console = $console
struct test_struct {
  number my_number
}
var a = test_struct { my_number = 1000 }
console.log(a.my_number)", code);
        }

        [TestMethod]
        public void Declare_struct_and_create_new_instance_with_initializer_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = ParseWithTypeLookup(@"let console = $console
struct test_struct {
  my_number : number;
  my_string : string;
}

let a = test_struct { my_number = 1000, my_string = ""hehe"" }
console.log(a.my_number)
console.log(a.my_string)", out SyntaxParser parser);

            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var console = $console
struct test_struct {
  number my_number
  string my_string
}
var a = test_struct { my_number = 1000, my_string = ""hehe"" }
console.log(a.my_number)
console.log(a.my_string)", code);
        }


        [TestMethod]
        public void for_while_loop()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { while (true) { s.hello[i] = i } } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    while (True) {
      s.hello[i] = i
    }
  }
  return s.hello
}", code);
        }



        [TestMethod]
        public void for_while_break_loop()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { while (true) { s.hello[i] = i break } } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    while (True) {
      s.hello[i] = i
      break
    }
  }
  return s.hello
}", code);
        }


        [TestMethod]
        public void for_loop_with_if_statement_1()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff() { let s = structA for(let i = 0; i < 5; i++) { s.hello[i].apa = i } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff() -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    s.hello[i].apa = i
  }
  return s.hello
}", code);
        }


        [TestMethod]
        public void for_loop_with_if_statement_2()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA for(let i = 0; i < 5; i++) { if(s.hello[i] == i + 25) { console.log(s.hello[i+2]) } } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    if (s.hello[i] == i + 25) { 
      console.log(s.hello[i + 2])
    }
  }
  return s.hello
}", code);
        }

        [TestMethod]
        public void for_loop_with_if_statement__3()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA for(let i = 0; i < 5; i++) { if(s.hello[i] == i + 25 * 2 / 4) { console.log(s.hello[i+2] + \" hello world\") } } return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  for (var i = 0; i < 5; i++) {
    if (s.hello[i] == i + 25 * 2 / 4) { 
      console.log(s.hello[i + 2] + "" hello world"")
    }
  }
  return s.hello
}", code);
        }



        [TestMethod]
        public void switch_normal_case()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { case 9: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  switch (99 - 25 * 1) {
    case 9:
      break
  }
  console.log(s.hello[i + 2] + "" hello world"")
  return s.hello
}", code);
        }


        [TestMethod]
        public void switch_default_case()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { default: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  switch (99 - 25 * 1) {
    default:
      break
  }
  console.log(s.hello[i + 2] + "" hello world"")
  return s.hello
}", code);
        }


        [TestMethod]
        public void switch_normal_and_default_case()
        {
            var code = ParseWithTypeLookup("struct structA { number[] hello } fn do_stuff(any console) { let s = structA switch (99-25*1) { case 9: break default: break } console.log(s.hello[i+2] + \" hello world\") return s.hello } ", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"struct structA {
  number[] hello
}
fn do_stuff(console:any) -> any {
  var s = structA
  switch (99 - 25 * 1) {
    case 9:
      break
    default:
      break
  }
  console.log(s.hello[i + 2] + "" hello world"")
  return s.hello
}", code);
        }


        [TestMethod]
        public void Pre_Post_Increment_Variable_2()
        {
            // Assert.Inconclusive("Array initializers has not been implemented yet");
            var code = ParseWithTypeLookup(@"let console = $console

let y = 0
let x = y++
let z = ++y

console.log(x + ' ' + y + ' ' + z)", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var console = $console
var y = 0
var x = y++
var z = ++y
console.log(x + "" "" + y + "" "" + z)", code);
        }

        [TestMethod]
        public void simple_variable_number()
        {
            var code = ParseWithTypeLookup("let j = 0", out var parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual("var j = 0", code);
        }

        [TestMethod]
        public void simple_variable_string()
        {
            var code = ParseWithTypeLookup("let j = \"0\"", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"var j = ""0""", code);
        }

        [TestMethod]
        public void simple_fn_string()
        {
            var code = ParseWithTypeLookup("fn test() { return \"0\" }", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"fn test() -> string {
  return ""0""
}", code);
        }

        [TestMethod]
        public void simple_fn_number()
        {
            var code = ParseWithTypeLookup("fn test() { return 0 }", out SyntaxParser parser);
            Assert.AreEqual(false, parser.HasErrors);
            Assert.AreEqual(@"fn test() -> number {
  return 0
}", code);
        }

        private SyntaxParser Parser(string code)
        {
            return new SyntaxParser(new Lexer(code, false).Tokenize());
        }

        private string ParseWithTypeLookup(string code, out SyntaxParser parser)
        {
            parser = Parser(code);
            var ast = parser.Parse();
            var compiler = new ExpressionCompiler();
            var expression = compiler.Compile(ast);
            if (compiler.Errors.Count > 0) throw new Exception(string.Join(" ", compiler.Errors));
            var codeGenerator = new XzaarScriptCodeFormatter();
            return codeGenerator.Visit(expression).TrimEnd('\r', '\n');
        }
    }
}